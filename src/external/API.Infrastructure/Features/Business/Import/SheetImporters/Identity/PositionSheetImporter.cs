using API.Application.Features.Business;
using API.Contract.DTOs.Business.Import;
using API.Contract.ImportDataRows.Identity;
using API.Domain.Features.Personnel.Departments;
using API.Domain.Features.Personnel.Positions;

namespace API.Infrastructure.Features.Business.Import.SheetImporters.Identity
{
    internal class PositionSheetImporter : BaseSheetImporter<PositionRow>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;

        private readonly Dictionary<string, Department> _departmentByCode = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Department> _departmentByName = new(StringComparer.OrdinalIgnoreCase);

        private readonly HashSet<string> _existingNames = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _existingCodes = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<Guid> _existingPublicIds = [];

        private readonly HashSet<string> _seenNames = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _seenCodes = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<Guid> _seenPublicIds = [];

        public PositionSheetImporter(
            IPositionRepository positionRepository,
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
        }

        public override string SheetName => "Positions";

        protected override async Task BeforeProcessRowsAsync(
            IReadOnlyList<PositionRow> rows,
            CancellationToken cancellationToken)
        {
            _departmentByCode.Clear();
            _departmentByName.Clear();
            _existingNames.Clear();
            _existingCodes.Clear();
            _existingPublicIds.Clear();

            _seenNames.Clear();
            _seenCodes.Clear();
            _seenPublicIds.Clear();

            var departments = await _departmentRepository.ToListAsync(cancellationToken);
            foreach (var dept in departments)
            {
                if (!string.IsNullOrWhiteSpace(dept.DepartmentCode))
                    _departmentByCode[dept.DepartmentCode] = dept;
                if (!string.IsNullOrWhiteSpace(dept.Name))
                    _departmentByName[dept.Name] = dept;
            }

            var positions = await _positionRepository.ToListAsync(cancellationToken);
            foreach (var pos in positions)
            {
                if (!string.IsNullOrWhiteSpace(pos.Name))
                    _existingNames.Add(pos.Name);
                if (!string.IsNullOrWhiteSpace(pos.PositionCode))
                    _existingCodes.Add(pos.PositionCode);
                if (pos.PublicId != Guid.Empty)
                    _existingPublicIds.Add(pos.PublicId);
            }
        }

        protected override Task ProcessRowAsync(
            PositionRow row,
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

            var code = row.PositionCode?.Trim();
            if (string.IsNullOrWhiteSpace(code))
            {
                result.AddError(rowIndex, "PositionCode is required and must not be empty.");
                return Task.CompletedTask;
            }

            if (_existingNames.Contains(name))
            {
                result.AddError(rowIndex, $"Position with name '{name}' already exists.");
                return Task.CompletedTask;
            }

            if (!_seenNames.Add(name))
            {
                result.AddError(rowIndex, $"Position with name '{name}' is duplicated in the file.");
                return Task.CompletedTask;
            }

            if (_existingCodes.Contains(code))
            {
                result.AddError(rowIndex, $"Position with code '{code}' already exists.");
                return Task.CompletedTask;
            }

            if (!_seenCodes.Add(code))
            {
                result.AddError(rowIndex, $"Position with code '{code}' is duplicated in the file.");
                return Task.CompletedTask;
            }

            // Resolve Department
            Department? department = null;
            var deptCode = row.DepartmentCode?.Trim();
            var deptName = row.DepartmentName?.Trim();

            if (!string.IsNullOrWhiteSpace(deptCode) && _departmentByCode.TryGetValue(deptCode, out var dByCode))
            {
                department = dByCode;
            }
            else if (!string.IsNullOrWhiteSpace(deptName) && _departmentByName.TryGetValue(deptName, out var dByName))
            {
                department = dByName;
            }

            if (department is null)
            {
                result.AddError(rowIndex, $"Department '{deptCode ?? deptName}' does not exist. Please import Departments first.");
                return Task.CompletedTask;
            }

            if (row.PublicId != Guid.Empty)
            {
                if (_existingPublicIds.Contains(row.PublicId))
                {
                    result.AddError(rowIndex, $"Position with public id '{row.PublicId}' already exists.");
                    return Task.CompletedTask;
                }

                if (!_seenPublicIds.Add(row.PublicId))
                {
                    result.AddError(rowIndex, $"Position with public id '{row.PublicId}' is duplicated in the file.");
                    return Task.CompletedTask;
                }
            }

            var position = new Position(name, code, department.Id, row.Description?.Trim());

            if (row.PublicId != Guid.Empty)
            {
                typeof(API.Domain.Common.Entities.Entity)
                    .GetProperty(nameof(position.PublicId))?
                    .SetValue(position, row.PublicId);
            }

            _positionRepository.Add(position);
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

