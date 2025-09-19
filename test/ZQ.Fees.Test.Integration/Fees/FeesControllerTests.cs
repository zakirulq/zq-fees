using FluentAssertions;
using NSubstitute;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZQ.Fees.Api.Models;
using ZQ.Fees.Domain.Enums;
using ZQ.Fees.Domain.Models;

namespace ZQ.Fees.Test.Integration.Fees;

[TestFixture]
public class FeesControllerTests
{
    private CustomWebApplicationFactory _factory;
    private HttpClient _client;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task CreatePayment_Should_Return_Ok_When_Request_Is_Valid()
    {
        // Arrange
        var studentId = 123;
        var requestBody = new
        {
            Amount = 100.00,
            Method = "CreditCard"
        };

        var mockPayment = Payment.CreatePayment(studentId, 100.00m, PaymentMethod.CreditCard);
        _factory.MockPaymentRepository.CreateAsync(Arg.Any<Payment>()).Returns(mockPayment);

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Fees/{studentId}/payments")
        {
            Content = jsonContent
        };
        request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString());

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBodyString = await response.Content.ReadAsStringAsync();
        var paymentResponse = JsonSerializer.Deserialize<PaymentResponse>(
            responseBodyString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } }
        );

        paymentResponse.Should().NotBeNull();
        paymentResponse.Message.Should().Be("Payment created successfully");
        paymentResponse.Data.Should().NotBeNull();
    }
}
