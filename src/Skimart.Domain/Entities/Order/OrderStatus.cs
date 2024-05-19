using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Skimart.Domain.Entities.Order;

// [JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    [EnumMember(Value = "Pending")]
    Pending,

    [EnumMember(Value = "Payment Received")]
    PaymentReceived,

    [EnumMember(Value = "Payment Failed")]
    PaymentFailed
}