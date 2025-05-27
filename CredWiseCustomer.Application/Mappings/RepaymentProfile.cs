using AutoMapper;
using CredWiseCustomer.Application.DTOs;
using CredWiseCustomer.Core.Entities;

namespace CredWiseCustomer.Application.Mappings
{
    public class RepaymentProfile : Profile
    {
        public RepaymentProfile()
        {
            CreateMap<LoanRepaymentSchedule, RepaymentScheduleDto>();
        }
    }
} 