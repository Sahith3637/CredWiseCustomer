using AutoMapper;
using CredWiseCustomer.Application.DTOs;
using CredWiseCustomer.Core.Entities;

namespace CredWiseCustomer.Application.Mappings
{
    public class FdProfile : Profile
    {
        public FdProfile()
        {
            CreateMap<ApplyFdDto, Fdapplication>();
            CreateMap<Fdapplication, FdStatusDto>();
            CreateMap<Fdtransaction, FdPaymentScheduleDto>()
                .ForMember(dest => dest.FDTransactionId, opt => opt.MapFrom(src => src.FdtransactionId))
                .ForMember(dest => dest.FDApplicationId, opt => opt.MapFrom(src => src.FdapplicationId));
        }
    }
} 