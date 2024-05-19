using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Skimart.Contracts.Orders.Responses;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatusResponse
{
    [EnumMember(Value = "Pending")]
    Pending,

    [EnumMember(Value = "Payment Received")]
    PaymentReceived,

    [EnumMember(Value = "Payment Failed")]
    PaymentFailed
}