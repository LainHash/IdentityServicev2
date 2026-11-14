using API.Contract.DTOs.Personnel.Departments;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Queries.GetById
{
    public record GetDepartmentByIdQuery(Guid Id)
        : IRequest<Result<DepartmentDetailResponse>>;
}

