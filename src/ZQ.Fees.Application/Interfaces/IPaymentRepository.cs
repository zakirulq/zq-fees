using ZQ.Fees.Domain.Models;

namespace ZQ.Fees.Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> CreateAsync(Payment payment);
}
