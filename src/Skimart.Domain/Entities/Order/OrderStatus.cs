using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Skimart.Domain.Entities.Order;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Pending,

    PaymentReceived,

    PaymentFailed
}