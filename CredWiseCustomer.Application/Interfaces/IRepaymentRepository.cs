using CredWiseCustomer.Core.Entities;

namespace CredWiseCustomer.Application.Interfaces
{
    public interface IRepaymentRepository
    {
        Task<IEnumerable<LoanRepaymentSchedule>> GetRepaymentScheduleAsync(int loanApplicationId);
        Task<LoanRepaymentSchedule?> GetRepaymentByIdAsync(int repaymentId);
        Task UpdateRepaymentAsync(LoanRepaymentSchedule repayment);
    }
} 