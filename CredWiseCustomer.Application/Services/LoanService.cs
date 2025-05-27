using AutoMapper;
using CredWiseCustomer.Application.DTOs;
using CredWiseCustomer.Application.Interfaces;
using CredWiseCustomer.Core.Entities;

namespace CredWiseCustomer.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _repo;
        private readonly IMapper _mapper;

        public LoanService(ILoanRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<int> ApplyForLoanAsync(ApplyLoanDto dto)
        {
            var loan = _mapper.Map<LoanApplication>(dto);
            loan.Status = "Initial Review";
            loan.CreatedAt = DateTime.UtcNow;
            loan.IsActive = true;
            return await _repo.AddLoanApplicationAsync(loan);
        }

        public async Task<LoanStatusDto?> GetLoanStatusAsync(int loanApplicationId)
        {
            var loan = await _repo.GetLoanApplicationByIdAsync(loanApplicationId);
            return loan != null ? _mapper.Map<LoanStatusDto>(loan) : null;
        }

        public async Task<IEnumerable<LoanStatusDto>> GetAllLoansForUserAsync(int userId)
        {
            var loans = await _repo.GetLoansByUserIdAsync(userId);
            return loans.Select(_mapper.Map<LoanStatusDto>);
        }
    }
} 