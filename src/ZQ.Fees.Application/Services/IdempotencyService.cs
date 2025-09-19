using System.Collections.Concurrent;
using ZQ.Fees.Application.Interfaces;
using ZQ.Fees.Domain.Enums;

public class IdempotencyService: IIdempotencyService
{
    // In a real application, this would be a database or a distributed cache like Redis.
    // ConcurrentDictionary is used here to safely handle multiple requests for demonstration.
    private readonly ConcurrentDictionary<string, IdempotencyStatus> _requestCache = new ConcurrentDictionary<string, IdempotencyStatus>();

    public IdempotencyStatus GetRequestStatus(string key)
    {
        _requestCache.TryGetValue(key, out var status);
        return status;
    }

    public void MarkRequestAsInProgress(string key)
    {
        _requestCache.TryAdd(key, IdempotencyStatus.InProgress);
    }

    public void MarkRequestAsCompleted(string key)
    {
        _requestCache.TryUpdate(key, IdempotencyStatus.Completed, IdempotencyStatus.InProgress);
    }
}