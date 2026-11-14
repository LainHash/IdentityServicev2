using API.Contract.DTOs.Personnel.Departments;
using API.Domain.Common.Models;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Queries.GetAll
{
    public record GetAllDepartmentsQuery
        : PaginationQuery, IRequest<PaginationResult<IEnumerable<DepartmentResponse>>>;
}

