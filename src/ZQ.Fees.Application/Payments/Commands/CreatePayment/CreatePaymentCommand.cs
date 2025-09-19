using MediatR;
using ZQ.Fees.Domain.Enums;
using ZQ.Fees.Domain.Models;

namespace ZQ.Fees.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommand : IRequest<Payment>
{
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
}