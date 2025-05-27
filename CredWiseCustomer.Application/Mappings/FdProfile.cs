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
        }
    }
}