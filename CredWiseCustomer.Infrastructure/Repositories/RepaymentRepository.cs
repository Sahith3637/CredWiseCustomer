using CredWiseCustomer.Application.Interfaces;
using CredWiseCustomer.Core.Entities;
using CredWiseCustomer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CredWiseCustomer.Infrastructure.Repositories
{
    public class RepaymentRepository : IRepaymentRepository
    {
        private readonly AppDbContext _context;
        public RepaymentRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<LoanRepaymentSchedule>> GetRepaymentScheduleAsync(int loanApplicationId)
        {
            return await _context.LoanRepaymentSchedules
                .Where(r => r.LoanApplicationId == loanApplicationId)
                .ToListAsync();
        }

        public async Task<LoanRepaymentSchedule?> GetRepaymentByIdAsync(int repaymentId)
        {
            return await _context.LoanRepaymentSchedules.FindAsync(repaymentId);
        }

        public async Task UpdateRepaymentAsync(LoanRepaymentSchedule repayment)
        {
            _context.LoanRepaymentSchedules.Update(repayment);
            await _context.SaveChangesAsync();
        }
    }
} 