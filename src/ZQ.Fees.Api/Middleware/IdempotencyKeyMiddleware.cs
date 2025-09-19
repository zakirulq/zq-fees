using System.Net;
using ZQ.Fees.Application.Interfaces;
using ZQ.Fees.Domain.Enums;

namespace ZQ.Fees.Api.Middleware;

public class IdempotencyKeyMiddleware
{
    private readonly RequestDelegate _next;

    public IdempotencyKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IIdempotencyService idempotencyService)
    {
        // Only apply for POST requests, we can add any other types we need
        if (context.Request.Method != "POST")
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Idempotency-Key", out var key))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Idempotency-Key header is required for this request.");
            return;
        }

        var idempotencyKey = key.ToString();

        var status = idempotencyService.GetRequestStatus(idempotencyKey);
        if (status == IdempotencyStatus.Completed)
        {
            // Request was already completed, return the cached result
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync("Duplicate request. Payment was already processed.");
            return;
        }

        if (status == IdempotencyStatus.InProgress)
        {
            // Request is in progress, return a conflict status
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            await context.Response.WriteAsync("Request is already being processed.");
            return;
        }

        idempotencyService.MarkRequestAsInProgress(idempotencyKey);
        
        // Continue to the next middleware and controller logic
        await _next(context);

        // Once the next middleware is done, update the status
        idempotencyService.MarkRequestAsCompleted(idempotencyKey);
    }
}