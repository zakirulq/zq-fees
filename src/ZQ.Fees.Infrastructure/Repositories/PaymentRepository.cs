using Microsoft.EntityFrameworkCore;
using ZQ.Fees.Application.Interfaces;
using ZQ.Fees.Domain.Models;
using ZQ.Fees.Infrastructure.Data;

namespace ZQ.Fees.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly FeesDbContext _context;

    public PaymentRepository(FeesDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> CreateAsync(Payment payment)
    {
        var id = _context.Payments.Max(p => (int?)p.Id) ?? 0;

        payment.Id = id + 1;
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return payment;
    }
}
