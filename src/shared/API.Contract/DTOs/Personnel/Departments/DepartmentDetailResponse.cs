using API.Contract.DTOs.Personnel.Positions;

namespace API.Contract.DTOs.Personnel.Departments
{
    public class DepartmentDetailResponse : DepartmentResponse
    {
        public IEnumerable<PositionResponse> Positions { get; set; } = [];
    }
}
