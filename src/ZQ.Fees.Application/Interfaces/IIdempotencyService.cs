using ZQ.Fees.Domain.Enums;

namespace ZQ.Fees.Application.Interfaces;

public interface IIdempotencyService
{
    public IdempotencyStatus GetRequestStatus(string key);
    public void MarkRequestAsInProgress(string key);
    public void MarkRequestAsCompleted(string key);
}
