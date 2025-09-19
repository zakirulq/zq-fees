using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZQ.Fees.Api.Models;
using ZQ.Fees.Application.Payments.Commands.CreatePayment;
using ZQ.Fees.Domain.Models;

namespace ZQ.Fees.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{studentId}/payments")]
    public async Task<IActionResult> CreatePayment(int studentId, [FromBody] CreatePaymentRequest request)
    {
        var command = new CreatePaymentCommand
        {
            StudentId = studentId,
            Amount = request.Amount,
            Method = request.Method
        };

        var payment = await _mediator.Send(command);
        var response = PaymentViewModel.PopulateFrom(payment);

        return Ok(new PaymentResponse{ Data = response, Message = "Payment created successfully" });
    }
}
