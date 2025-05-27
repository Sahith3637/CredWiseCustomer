using CredWiseCustomer.Core.Entities;

namespace CredWiseCustomer.Application.Interfaces
{
    public interface ILoanRepository
    {
        Task<int> AddLoanApplicationAsync(LoanApplication loan);
        Task<LoanApplication?> GetLoanApplicationByIdAsync(int loanApplicationId);
        Task<IEnumerable<LoanApplication>> GetLoansByUserIdAsync(int userId);
    }
} 