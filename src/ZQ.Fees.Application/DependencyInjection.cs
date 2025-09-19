using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ZQ.Fees.Application.Common.Behaviors;
using ZQ.Fees.Application.Interfaces;
using ZQ.Fees.Application.Payments.Commands.CreatePayment;

namespace ZQ.Fees.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePaymentCommand).Assembly));
        services.AddValidatorsFromAssemblyContaining<CreatePaymentCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddSingleton<IIdempotencyService, IdempotencyService>();
        return services;
    }
}
