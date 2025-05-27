using System;

namespace CredWiseCustomer.Application.DTOs
{
    public class FdPaymentScheduleDto
    {
        public int FDTransactionId { get; set; }
        public int FDApplicationId { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionStatus { get; set; }
        public string? TransactionReference { get; set; }
    }
} 