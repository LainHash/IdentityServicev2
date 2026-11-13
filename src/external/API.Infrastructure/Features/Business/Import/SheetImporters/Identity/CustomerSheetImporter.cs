using API.Application.Features.Business;
using API.Contract.DTOs.Business.Import;
using API.Contract.ImportDataRows.Identity;
using API.Domain.Features.Guest.Customers;
using API.Domain.Features.Identity.Users;

namespace API.Infrastructure.Features.Business.Import.SheetImporters.Identity
{
    internal class CustomerSheetImporter : BaseSheetImporter<CustomerRow>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;

        private readonly Dictionary<string, User> _userByEmail = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<long> _existingCustomerUserIds = [];
        private readonly HashSet<Guid> _existingPublicIds = [];

        private readonly HashSet<string> _seenUserEmails = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<Guid> _seenPublicIds = [];

        public CustomerSheetImporter(
            ICustomerRepository customerRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }

        public override string SheetName => "Customers";

        protected override async Task BeforeProcessRowsAsync(
            IReadOnlyList<CustomerRow> rows,
            CancellationToken cancellationToken)
        {
            _userByEmail.Clear();
            _existingCustomerUserIds.Clear();
            _existingPublicIds.Clear();

            _seenUserEmails.Clear();
            _seenPublicIds.Clear();

            var users = await _userRepository.ToListAsync(cancellationToken);
            foreach (var user in users)
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                    _userByEmail[user.Email] = user;
            }

            var customers = await _customerRepository.ToListAsync(cancellationToken);
            foreach (var customer in customers)
            {
                _existingCustomerUserIds.Add(customer.UserId);
                if (customer.PublicId != Guid.Empty)
                    _existingPublicIds.Add(customer.PublicId);
            }
        }

        protected override Task ProcessRowAsync(
            CustomerRow row,
            int rowIndex,
            ImportResultResponse result,
            CancellationToken cancellationToken)
        {
            var userEmail = row.UserEmail?.Trim();
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                result.AddError(rowIndex, "UserEmail is required.");
                return Task.CompletedTask;
            }

            if (!_userByEmail.TryGetValue(userEmail, out var user))
            {
                result.AddError(rowIndex, $"User with email '{userEmail}' does not exist.");
                return Task.CompletedTask;
            }

            if (_existingCustomerUserIds.Contains(user.Id))
            {
                result.AddError(rowIndex, $"User with email '{userEmail}' already has a Customer record.");
                return Task.CompletedTask;
            }

            if (!_seenUserEmails.Add(userEmail))
            {
                result.AddError(rowIndex, $"User with email '{userEmail}' is duplicated in the file.");
                return Task.CompletedTask;
            }

            if (row.LoyaltyPoints < 0)
            {
                result.AddError(rowIndex, "LoyaltyPoints must not be negative.");
                return Task.CompletedTask;
            }

            if (row.PublicId != Guid.Empty)
            {
                if (_existingPublicIds.Contains(row.PublicId))
                {
                    result.AddError(rowIndex, $"Customer with public id '{row.PublicId}' already exists.");
                    return Task.CompletedTask;
                }

                if (!_seenPublicIds.Add(row.PublicId))
                {
                    result.AddError(rowIndex, $"Customer with public id '{row.PublicId}' is duplicated in the file.");
                    return Task.CompletedTask;
                }
            }

            var customer = new Customer(user.Id, row.LoyaltyPoints);

            if (row.PublicId != Guid.Empty)
            {
                typeof(API.Domain.Common.Entities.Entity)
                    .GetProperty(nameof(customer.PublicId))?
                    .SetValue(customer, row.PublicId);
            }

            _customerRepository.Add(customer);
            _existingCustomerUserIds.Add(user.Id);

            result.SuccessCount++;
            return Task.CompletedTask;
        }
    }
}

