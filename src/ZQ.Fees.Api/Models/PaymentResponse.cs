using ZQ.Fees.Domain.Models;

namespace ZQ.Fees.Api.Models;

public class PaymentResponse
{
    public PaymentViewModel Data { get; set; }
    public string Message { get; set; }
}