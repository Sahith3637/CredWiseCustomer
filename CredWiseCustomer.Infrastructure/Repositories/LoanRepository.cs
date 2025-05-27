using CredWiseCustomer.Application.Interfaces;
using CredWiseCustomer.Core.Entities;
using CredWiseCustomer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CredWiseCustomer.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly AppDbContext _context;
        public LoanRepository(AppDbContext context) { _context = context; }

        public async Task<int> AddLoanApplicationAsync(LoanApplication loan)
        {
            _context.LoanApplications.Add(loan);
            await _context.SaveChangesAsync();
            return loan.LoanApplicationId;
        }

        public async Task<LoanApplication?> GetLoanApplicationByIdAsync(int loanApplicationId)
        {
            return await _context.LoanApplications.FindAsync(loanApplicationId);
        }

        public async Task<IEnumerable<LoanApplication>> GetLoansByUserIdAsync(int userId)
        {
            return await _context.LoanApplications
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }
    }
} 