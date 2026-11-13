using System.Text.Json.Serialization;

namespace API.Domain.Features.Personnel.Employees.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EmployeeStatus
    {
        Active,
        OnLeave,
        Suspended,
        Terminated
    }
}
