using ZQ.Fees.Domain.Enums;

namespace ZQ.Fees.Domain.Models;

public class Payment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Payment(int studentId, decimal amount, PaymentMethod method)
    {
        StudentId = studentId;
        Amount = amount;
        Method = method;
        CreatedAt = DateTime.UtcNow;
    }

    public static Payment CreatePayment(int studentId, decimal amount, PaymentMethod method)
    {
        return new Payment(studentId, amount, method); 
    }
}
