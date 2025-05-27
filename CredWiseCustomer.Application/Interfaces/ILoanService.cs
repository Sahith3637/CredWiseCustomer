using CredWiseCustomer.Application.DTOs;

namespace CredWiseCustomer.Application.Interfaces
{
    public interface ILoanService
    {
        Task<int> ApplyForLoanAsync(ApplyLoanDto dto);
        Task<LoanStatusDto?> GetLoanStatusAsync(int loanApplicationId);
        Task<IEnumerable<LoanStatusDto>> GetAllLoansForUserAsync(int userId);
    }
} 