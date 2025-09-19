using ZQ.Fees.Domain.Enums;

namespace ZQ.Fees.Api.Models;

public class CreatePaymentRequest
{
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
}
