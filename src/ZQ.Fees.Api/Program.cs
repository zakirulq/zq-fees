using System.Text.Json.Serialization;
using ZQ.Fees.Api.Filters;
using ZQ.Fees.Api.Middleware;
using ZQ.Fees.Application;
using ZQ.Fees.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ApiExceptionFilterAttribute>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseMiddleware<IdempotencyKeyMiddleware>();
app.Run();
