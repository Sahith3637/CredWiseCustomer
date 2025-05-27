using AutoMapper;
using CredWiseCustomer.Application.DTOs;
using CredWiseCustomer.Core.Entities;

namespace CredWiseCustomer.Application.Mappings
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<ApplyLoanDto, LoanApplication>();
            CreateMap<LoanApplication, LoanStatusDto>();
        }
    }
} 