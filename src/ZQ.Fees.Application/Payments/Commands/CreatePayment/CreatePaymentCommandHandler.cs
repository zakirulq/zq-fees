using MediatR;
using ZQ.Fees.Application.Interfaces;
using ZQ.Fees.Domain.Models;

namespace ZQ.Fees.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Payment>
{
    private readonly IPaymentRepository _paymentRepository;

    public CreatePaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Payment> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = Payment.CreatePayment(request.StudentId, request.Amount, request.Method);

        // Other action can be done here: Publish some domain event to trigger other actions like notification, real payment, push analytic data, etc
        // Also outbox can be used to persist further action and decouple it for further choreography
        return await _paymentRepository.CreateAsync(payment);
    }
}