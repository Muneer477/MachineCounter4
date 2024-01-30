using AutoMapper;
using SMTS.DTOs.Location;
using SMTS.DTOs.Stock;
using SMTS.DTOs.Types;
using SMTS.Entities;

namespace SMTS.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
 
            CreateMap<TypesDTO, Types>().ReverseMap();
            CreateMap<StockOutInCreationDTO, StockOutIn>().ReverseMap();
            CreateMap<StockOutInDTL, StockOutInDTLDTO>().ReverseMap();
            CreateMap<StockOutInDTO, StockOutIn>().ReverseMap();
            CreateMap<StockLocationDTO, StockLocation>().ReverseMap(); ;

        }

    }
}
