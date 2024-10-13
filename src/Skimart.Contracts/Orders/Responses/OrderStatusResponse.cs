using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Skimart.Contracts.Orders.Responses;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatusResponse
{
    Pending,

    PaymentReceived,

    PaymentFailed
}