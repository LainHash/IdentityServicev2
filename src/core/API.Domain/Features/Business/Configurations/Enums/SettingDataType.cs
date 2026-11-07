using System.Text.Json.Serialization;

namespace API.Domain.Features.Business.Configurations.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SettingDataType
    {
        String = 1,
        Integer = 2,
        Decimal = 3,
        Boolean = 4,
        Json = 5,
        TimeSpan = 6
    }
}
