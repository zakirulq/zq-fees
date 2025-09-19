using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZQ.Fees.Application.Interfaces;
using ZQ.Fees.Infrastructure.Data;
using ZQ.Fees.Infrastructure.Repositories;

namespace ZQ.Fees.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<FeesDbContext>(options => options.UseInMemoryDatabase("FeesDb"));

        services.AddScoped<IPaymentRepository, PaymentRepository>();

        return services;
    }
}
