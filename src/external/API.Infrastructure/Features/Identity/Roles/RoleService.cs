using API.Application.Common.Services;
using API.Application.Features.Identity.Roles;
using API.Application.Features.Identity.Roles.Commands.Create;
using API.Application.Features.Identity.Roles.Commands.Delete;
using API.Application.Features.Identity.Roles.Commands.Restore;
using API.Application.Features.Identity.Roles.Commands.Update;
using API.Application.Features.Identity.Roles.Queries.GetAll;
using API.Application.Features.Identity.Roles.Queries.GetById;
using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Models.Messages;
using API.Domain.Common.Models.Results;
using API.Domain.Features.Identity.Roles;
using AutoMapper;
using System.Net;

namespace API.Infrastructure.Features.Identity.Roles
{
    internal class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(
            IRoleRepository roleRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResponse<IEnumerable<RoleResponse>>> GetAllAsync(
            GetAllRolesSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var roles = await _roleRepository
                .ToListAsync(specification, cancellationToken);

            var totalItems = await _roleRepository
                .CountAsync(specification, cancellationToken);

            var skip = specification.Skip;
            var take = specification.Take;

            var response = _mapper.Map<IEnumerable<RoleResponse>>(roles);
            return PaginationResponse<IEnumerable<RoleResponse>>
                .Succeed(response, Success.Retrieved("Roles"), totalItems, skip, take);
        }

        public async Task<APIResponse<RoleDetailResponse>> GetByIdAsync(
            GetRoleByIdSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository
                .FindAsync(specification, cancellationToken);
            if (role is null)
            {
                return APIResponse<RoleDetailResponse>
                    .Fail(Error.NotFound("Role"), HttpStatusCode.NotFound);
            }

            var response = _mapper.Map<RoleDetailResponse>(role);
            return APIResponse<RoleDetailResponse>
                .Succeed(response, Success.Retrieved("Role"));
        }

        public async Task<APIResponse<RoleResponse>> CreateAsync(
            CreateRoleCommand command,
            CancellationToken cancellationToken = default)
        {
            var isExisting = await _roleRepository
                .IsExistingNameAsync(null, command.Body.Name, cancellationToken);
            if (isExisting)
            {
                return APIResponse<RoleResponse>
                    .Fail(Error.ExistedName("Role"), HttpStatusCode.Conflict);
            }

            var role = Role.Create(command.Body.Name, command.Body.Description);
            _roleRepository.Add(role);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<RoleResponse>(role);
            return APIResponse<RoleResponse>
                .Succeed(response, Success.Created("Role"), HttpStatusCode.Created);
        }

        public async Task<APIResponse<RoleResponse>> UpdateAsync(
            UpdateRoleCommand command,
            CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository
                .FindByIdAsync(command.Id, cancellationToken);
            if (role is null || role.IsDeleted)
            {
                return APIResponse<RoleResponse>
                    .Fail(Error.NotFound("Role"), HttpStatusCode.NotFound);
            }

            var isExisting = await _roleRepository
                .IsExistingNameAsync(role.PublicId, command.Body.Name, cancellationToken);
            if (isExisting)
            {
                return APIResponse<RoleResponse>
                    .Fail(Error.ExistedName("Role"), HttpStatusCode.Conflict);
            }

            role.UpdateDetails(command.Body.Name, command.Body.Description);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<RoleResponse>(role);
            return APIResponse<RoleResponse>
                .Succeed(response, Success.Updated("Role"));
        }

        public async Task<APIResponse> DeleteAsync(
            DeleteRoleCommand command,
            CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository
                .FindByIdAsync(command.Id, cancellationToken);
            if (role is null)
            {
                return APIResponse
                    .Fail(Error.NotFound("Role"), HttpStatusCode.NotFound);
            }

            if (role.IsDeleted)
            {
                return APIResponse
                    .Fail(Error.AlreadyDeleted("Role"), HttpStatusCode.Conflict);
            }

            role.SoftDelete();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return APIResponse
                .Succeed(Success.Deleted("Role"));
        }

        public async Task<APIResponse> RestoreAsync(
            RestoreRoleCommand command,
            CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository
                .FindByIdAsync(command.Id, cancellationToken);
            if (role is null)
            {
                return APIResponse
                    .Fail(Error.NotFound("Role"), HttpStatusCode.NotFound);
            }

            if (!role.IsDeleted)
            {
                return APIResponse
                    .Fail(Error.NotYetDeleted("Role"), HttpStatusCode.Conflict);
            }

            role.Restore();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return APIResponse
                .Succeed(Success.Restored("Role"));
        }
    }
}

