using AutoMapper;
using Store_Sys.DTOs;
using Store_Sys.Modules;
using WebApplication1.Modules;

namespace Store_Sys.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
        }
    }
}