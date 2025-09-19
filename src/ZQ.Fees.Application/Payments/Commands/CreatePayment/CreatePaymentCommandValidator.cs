using FluentValidation;

namespace ZQ.Fees.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        //Adding some basic, complex customize validation can be done to provide strong protection
        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than 0");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Method)
            .IsInEnum()
            .WithMessage("Payment method must be a valid payment method");
    }
}
