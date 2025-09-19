using ZQ.Fees.Domain.Enums;

namespace ZQ.Fees.Domain.Models;

public class PaymentViewModel
{
    public int PaymentReferenceNumber { get; set; }
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }

    public PaymentViewModel(int paymentReferenceNumber, int studentId, decimal amount, PaymentMethod method)
    {
        StudentId = studentId;
        Amount = amount;
        Method = method;
        PaymentReferenceNumber = paymentReferenceNumber;
    }

    public static PaymentViewModel PopulateFrom(Payment payment)
    {
        return new PaymentViewModel(payment.Id, payment.StudentId, payment.Amount, payment.Method); 
    }
}
