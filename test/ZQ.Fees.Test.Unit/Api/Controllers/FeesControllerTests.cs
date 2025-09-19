using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using ZQ.Fees.Api.Controllers;
using ZQ.Fees.Api.Models;
using ZQ.Fees.Application.Payments.Commands.CreatePayment;
using ZQ.Fees.Domain.Enums;
using ZQ.Fees.Domain.Models;

namespace ZQ.Fees.Test.Unit.Api.Controllers;

[TestFixture]
public class FeesControllerTests
{
    private IMediator _mediatorMock;
    private FeesController _controller;
    private IFixture _fixture;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _mediatorMock = _fixture.Create<IMediator>();
        _controller = new FeesController(_mediatorMock);
    }

    [Test]
    public async Task CreatePayment_Should_Return_Ok_With_PaymentId()
    {
        // Arrange
        var studentId = 123;
        var request = _fixture.Build<CreatePaymentRequest>()
            .With(r => r.Amount, 100.00m)
            .With(r => r.Method, PaymentMethod.CreditCard)
            .Create();

        var payment = _fixture.Create<Payment>();
        _mediatorMock.Send(Arg.Any<CreatePaymentCommand>()).ReturnsForAnyArgs(payment);

        // Act
        var response = await _controller.CreatePayment(studentId, request) as OkObjectResult;

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(200);

        var paymentResponse = response.Value as PaymentResponse;
        paymentResponse.Should().NotBeNull();
        paymentResponse.Message.Should().Be("Payment created successfully");
        paymentResponse.Data.Should().NotBeNull();

        await _mediatorMock.Received(1).Send(Arg.Any<CreatePaymentCommand>());
    }
}