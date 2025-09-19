using FluentValidation.TestHelper;
using NUnit.Framework;
using ZQ.Fees.Application.Payments.Commands.CreatePayment;
using ZQ.Fees.Domain.Enums;

namespace ZQ.Fees.Test.Unit.Application.Payments.CreatePayment;

[TestFixture]
public class CreatePaymentCommandValidatorTests
{
    private CreatePaymentCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreatePaymentCommandValidator();
    }

    [Test]
    public void Should_Have_Error_When_StudentId_Is_Zero()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            StudentId = 0,
            Amount = 100.00m,
            Method = PaymentMethod.CreditCard
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StudentId)
            .WithErrorMessage("Student ID must be greater than 0");
    }

    [Test]
    public void Should_Have_Error_When_StudentId_Is_Negative()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            StudentId = -1,
            Amount = 100.00m,
            Method = PaymentMethod.CreditCard
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StudentId)
            .WithErrorMessage("Student ID must be greater than 0");
    }

    [Test]
    public void Should_Have_Error_When_Amount_Is_Zero()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            StudentId = 1,
            Amount = 0,
            Method = PaymentMethod.CreditCard
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage("Amount must be greater than 0");
    }

    [Test]
    public void Should_Have_Error_When_Amount_Is_Negative()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            StudentId = 1,
            Amount = -50.00m,
            Method = PaymentMethod.CreditCard
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage("Amount must be greater than 0");
    }

    [Test]
    public void Should_Have_Error_When_Method_Is_Invalid()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            StudentId = 1,
            Amount = 100.00m,
            Method = (PaymentMethod)999 // Invalid enum value
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Method)
            .WithErrorMessage("Payment method must be a valid payment method");
    }

    [TestCase(PaymentMethod.CreditCard)]
    [TestCase(PaymentMethod.DebitCard)]
    [TestCase(PaymentMethod.BankTransfer)]
    [TestCase(PaymentMethod.Cash)]
    [TestCase(PaymentMethod.Check)]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid(PaymentMethod method)
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            StudentId = 1,
            Amount = 100.00m,
            Method = method
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid_With_Positive_Values()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            StudentId = 123,
            Amount = 250.75m,
            Method = PaymentMethod.BankTransfer
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}