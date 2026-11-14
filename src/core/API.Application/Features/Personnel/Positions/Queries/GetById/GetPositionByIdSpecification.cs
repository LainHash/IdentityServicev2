using API.Domain.Common.Specifications;
using API.Domain.Features.Personnel.Positions;

namespace API.Application.Features.Personnel.Positions.Queries.GetById
{
    public class GetPositionByIdSpecification : BaseSpecification<Position>
    {
        public GetPositionByIdSpecification(GetPositionByIdQuery query)
        {
            EnableSoftDeleteFilter();
            AddInclude(x => x.Department);
            AddCriteria(x => x.PublicId == query.Id);
        }
    }
}

