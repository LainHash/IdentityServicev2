using API.Application.Features.Business;
using API.Contract.DTOs.Business.Import;
using API.Contract.ImportDataRows.Identity;
using API.Domain.Features.Identity.Users;
using API.Domain.Features.Personnel.Employees;
using API.Domain.Features.Personnel.Employees.Enums;
using API.Domain.Features.Personnel.Positions;
using System.Globalization;

namespace API.Infrastructure.Features.Business.Import.SheetImporters.Identity
{
    internal class EmployeeSheetImporter : BaseSheetImporter<EmployeeRow>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPositionRepository _positionRepository;

        private readonly Dictionary<string, User> _userByEmail = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<long> _existingEmployeeUserIds = [];
        private readonly HashSet<Guid> _existingPublicIds = [];

        private readonly Dictionary<string, Position> _positionByCode = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Position> _positionByName = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Guid, Position> _positionByPublicId = [];

        private readonly HashSet<string> _seenUserEmails = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<Guid> _seenPublicIds = [];

        public EmployeeSheetImporter(
            IEmployeeRepository employeeRepository,
            IUserRepository userRepository,
            IPositionRepository positionRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _positionRepository = positionRepository;
        }

        public override string SheetName => "Employees";

        protected override async Task BeforeProcessRowsAsync(
            IReadOnlyList<EmployeeRow> rows,
            CancellationToken cancellationToken)
        {
            _userByEmail.Clear();
            _existingEmployeeUserIds.Clear();
            _existingPublicIds.Clear();
            _positionByCode.Clear();
            _positionByName.Clear();
            _positionByPublicId.Clear();

            _seenUserEmails.Clear();
            _seenPublicIds.Clear();

            var users = await _userRepository.ToListAsync(cancellationToken);
            foreach (var user in users)
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                    _userByEmail[user.Email] = user;
            }

            var employees = await _employeeRepository.ToListAsync(cancellationToken);
            foreach (var employee in employees)
            {
                _existingEmployeeUserIds.Add(employee.UserId);
                if (employee.PublicId != Guid.Empty)
                    _existingPublicIds.Add(employee.PublicId);
            }

            var positions = await _positionRepository.ToListAsync(cancellationToken);
            foreach (var pos in positions)
            {
                if (!string.IsNullOrWhiteSpace(pos.PositionCode))
                    _positionByCode[pos.PositionCode] = pos;
                if (!string.IsNullOrWhiteSpace(pos.Name))
                    _positionByName[pos.Name] = pos;
                if (pos.PublicId != Guid.Empty)
                    _positionByPublicId[pos.PublicId] = pos;
            }
        }

        protected override Task ProcessRowAsync(
            EmployeeRow row,
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

            if (_existingEmployeeUserIds.Contains(user.Id))
            {
                result.AddError(rowIndex, $"User with email '{userEmail}' already has an Employee record.");
                return Task.CompletedTask;
            }

            if (!_seenUserEmails.Add(userEmail))
            {
                result.AddError(rowIndex, $"User with email '{userEmail}' is duplicated in the file.");
                return Task.CompletedTask;
            }

            // Resolve Position strictly from imported positions
            Position? position = null;
            if (row.PositionPublicId.HasValue && row.PositionPublicId.Value != Guid.Empty)
            {
                _positionByPublicId.TryGetValue(row.PositionPublicId.Value, out position);
            }

            if (position is null && !string.IsNullOrWhiteSpace(row.PositionCode))
            {
                _positionByCode.TryGetValue(row.PositionCode.Trim(), out position);
            }

            if (position is null && !string.IsNullOrWhiteSpace(row.PositionName))
            {
                _positionByName.TryGetValue(row.PositionName.Trim(), out position);
            }

            if (position is null)
            {
                var identifier = row.PositionCode ?? row.PositionName ?? row.PositionPublicId?.ToString() ?? "unspecified";
                result.AddError(rowIndex, $"Position '{identifier}' does not exist. Please import Positions first.");
                return Task.CompletedTask;
            }

            var hireDate = ParseHireDate(row.HireDate);
            var status = ParseStatus(row.Status);

            if (row.PublicId != Guid.Empty)
            {
                if (_existingPublicIds.Contains(row.PublicId))
                {
                    result.AddError(rowIndex, $"Employee with public id '{row.PublicId}' already exists.");
                    return Task.CompletedTask;
                }

                if (!_seenPublicIds.Add(row.PublicId))
                {
                    result.AddError(rowIndex, $"Employee with public id '{row.PublicId}' is duplicated in the file.");
                    return Task.CompletedTask;
                }
            }

            var branchId = row.BranchPublicId.HasValue && row.BranchPublicId.Value != Guid.Empty
                ? row.BranchPublicId
                : null;

            var employee = new Employee(user.Id, position.Id, branchId, hireDate, status);

            if (row.PublicId != Guid.Empty)
            {
                typeof(API.Domain.Common.Entities.Entity)
                    .GetProperty(nameof(employee.PublicId))?
                    .SetValue(employee, row.PublicId);
            }

            _employeeRepository.Add(employee);
            _existingEmployeeUserIds.Add(user.Id);
            if (row.PublicId != Guid.Empty)
            {
                _existingPublicIds.Add(row.PublicId);
            }

            result.SuccessCount++;
            return Task.CompletedTask;
        }

        private static DateTime ParseHireDate(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return DateTime.UtcNow;

            var formats = new[] { "yyyy-MM-dd", "dd/MM/yyyy", "d/M/yyyy", "MM/dd/yyyy", "yyyy/MM/dd", "yyyy-MM-ddTHH:mm:ss" };
            if (DateTime.TryParseExact(input.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            }

            if (DateTime.TryParse(input.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            }

            return DateTime.UtcNow;
        }

        private static EmployeeStatus ParseStatus(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return EmployeeStatus.Active;

            if (Enum.TryParse<EmployeeStatus>(input.Trim(), true, out var status))
            {
                return status;
            }

            return EmployeeStatus.Active;
        }
    }
}
