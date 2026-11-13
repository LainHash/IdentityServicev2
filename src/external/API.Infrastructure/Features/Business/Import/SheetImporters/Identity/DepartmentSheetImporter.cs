using API.Application.Features.Business;
using API.Contract.DTOs.Business.Import;
using API.Contract.ImportDataRows.Identity;
using API.Domain.Features.Personnel.Departments;

namespace API.Infrastructure.Features.Business.Import.SheetImporters.Identity
{
    internal class DepartmentSheetImporter : BaseSheetImporter<DepartmentRow>
    {
        private readonly IDepartmentRepository _departmentRepository;

        private readonly HashSet<string> _existingNames = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _existingCodes = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<Guid> _existingPublicIds = [];

        private readonly HashSet<string> _seenNames = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _seenCodes = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<Guid> _seenPublicIds = [];

        public DepartmentSheetImporter(
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _departmentRepository = departmentRepository;
        }

        public override string SheetName => "Departments";

        protected override async Task BeforeProcessRowsAsync(
            IReadOnlyList<DepartmentRow> rows,
            CancellationToken cancellationToken)
        {
            _existingNames.Clear();
            _existingCodes.Clear();
            _existingPublicIds.Clear();

            _seenNames.Clear();
            _seenCodes.Clear();
            _seenPublicIds.Clear();

            var departments = await _departmentRepository.ToListAsync(cancellationToken);
            foreach (var dept in departments)
            {
                if (!string.IsNullOrWhiteSpace(dept.Name))
                    _existingNames.Add(dept.Name);
                if (!string.IsNullOrWhiteSpace(dept.DepartmentCode))
                    _existingCodes.Add(dept.DepartmentCode);
                if (dept.PublicId != Guid.Empty)
                    _existingPublicIds.Add(dept.PublicId);
            }
        }

        protected override Task ProcessRowAsync(
            DepartmentRow row,
            int rowIndex,
            ImportResultResponse result,
            CancellationToken cancellationToken)
        {
            var name = row.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                result.AddError(rowIndex, "Name is required and must not be empty.");
                return Task.CompletedTask;
            }

            if (name.Length > 50)
            {
                result.AddError(rowIndex, "Name must not exceed 50 characters.");
                return Task.CompletedTask;
            }

            var code = row.DepartmentCode?.Trim();
            if (string.IsNullOrWhiteSpace(code))
            {
                result.AddError(rowIndex, "DepartmentCode is required and must not be empty.");
                return Task.CompletedTask;
            }

            if (_existingNames.Contains(name))
            {
                result.AddError(rowIndex, $"Department with name '{name}' already exists.");
                return Task.CompletedTask;
            }

            if (!_seenNames.Add(name))
            {
                result.AddError(rowIndex, $"Department with name '{name}' is duplicated in the file.");
                return Task.CompletedTask;
            }

            if (_existingCodes.Contains(code))
            {
                result.AddError(rowIndex, $"Department with code '{code}' already exists.");
                return Task.CompletedTask;
            }

            if (!_seenCodes.Add(code))
            {
                result.AddError(rowIndex, $"Department with code '{code}' is duplicated in the file.");
                return Task.CompletedTask;
            }

            if (row.PublicId != Guid.Empty)
            {
                if (_existingPublicIds.Contains(row.PublicId))
                {
                    result.AddError(rowIndex, $"Department with public id '{row.PublicId}' already exists.");
                    return Task.CompletedTask;
                }

                if (!_seenPublicIds.Add(row.PublicId))
                {
                    result.AddError(rowIndex, $"Department with public id '{row.PublicId}' is duplicated in the file.");
                    return Task.CompletedTask;
                }
            }

            var department = new Department(name, code, row.Description?.Trim());

            if (row.PublicId != Guid.Empty)
            {
                typeof(API.Domain.Common.Entities.Entity)
                    .GetProperty(nameof(department.PublicId))?
                    .SetValue(department, row.PublicId);
            }

            _departmentRepository.Add(department);
            _existingNames.Add(name);
            _existingCodes.Add(code);
            if (row.PublicId != Guid.Empty)
            {
                _existingPublicIds.Add(row.PublicId);
            }

            result.SuccessCount++;
            return Task.CompletedTask;
        }
    }
}

