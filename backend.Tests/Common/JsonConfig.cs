using System.Text.Json;
using System.Text.Json.Serialization;

namespace VehicleMaintenanceTracker.Tests.Common;

public static class JsonConfig
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
}