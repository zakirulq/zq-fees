using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZQ.Fees.Application.Interfaces;

namespace ZQ.Fees.Test.Integration;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly IFixture _fixture;

    public CustomWebApplicationFactory()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
    }

    internal IPaymentRepository MockPaymentRepository { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Normally for outbound services, we should mock it in integration test. So, the test will not have
            // any hard dependencies on external resources. There are many ways, tools, emulator available to inject and replace 
            // real outbound calls. 
            MockPaymentRepository = _fixture.Freeze<IPaymentRepository>();
            services.Replace(new ServiceDescriptor(typeof(IPaymentRepository), MockPaymentRepository));
        });
    }
}
