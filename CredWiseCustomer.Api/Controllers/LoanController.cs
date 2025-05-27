using CredWiseCustomer.Application.DTOs;
using CredWiseCustomer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CredWiseCustomer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        // POST: api/Loan
        [HttpPost]
        public async Task<ActionResult<int>> ApplyForLoan([FromBody] ApplyLoanDto dto)
        {
            var loanId = await _loanService.ApplyForLoanAsync(dto);
            return CreatedAtAction(nameof(GetLoanStatus), new { loanApplicationId = loanId }, loanId);
        }

        // GET: api/Loan/{loanApplicationId}
        [HttpGet("{loanApplicationId:int}")]
        public async Task<ActionResult<LoanStatusDto>> GetLoanStatus(int loanApplicationId)
        {
            var status = await _loanService.GetLoanStatusAsync(loanApplicationId);
            if (status == null)
                return NotFound();
            return Ok(status);
        }

        // GET: api/Loan/user/{userId}
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<LoanStatusDto>>> GetAllLoansForUser(int userId)
        {
            var loans = await _loanService.GetAllLoansForUserAsync(userId);
            return Ok(loans);
        }
    }
} 