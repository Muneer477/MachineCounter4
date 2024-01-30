using AutoMapper;
using SMTS.DTOs;
using SMTS.DTOs.Location;
using SMTS.DTOs.Stock;
using SMTS.DTOs.Types;
using SMTS.Entities;
using SMTS.Service.IService;

namespace SMTS
{
    public class MappingConfig
    {
       


    public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                
                

                config.CreateMap<ContactDto, Contact>();
                config.CreateMap<Contact, ContactDto>();
                config.CreateMap<WorkCenterDto, WorkCenter>();
                config.CreateMap<WorkCenter, WorkCenterDto>();
                config.CreateMap<UOMsDto, UOMs>();
                config.CreateMap<UOMs, UOMsDto>();
                config.CreateMap<EmployeeDto, Employee>();
                config.CreateMap<Employee, EmployeeDto>();
                config.CreateMap<DepartmentDto, Departments>();
                config.CreateMap<Departments, DepartmentDto>();
                config.CreateMap<PartDto, PartDTL>();
                config.CreateMap<PartDTL, PartDto>();
                config.CreateMap<PartUOMDto, PartUOM>();
                config.CreateMap<PartUOM, PartUOMDto>();
                config.CreateMap<PartDTLPrefixBatchSNDto, PartDTLPrefixBatchSN>();
                config.CreateMap<PartDTLPrefixBatchSN, PartDTLPrefixBatchSNDto>();
                config.CreateMap<PIOT_Counter, PIOTCounterDTO>();
                config.CreateMap<PIOTMaintenance, PIOTMaintainanceDTO>();
                config.CreateMap<PIOTRunning, PIOTRunningDTO>();


                config.CreateMap<PlanningJODTO, PlanningJO>();
                config.CreateMap<PlanningJO, PlanningJODTO>();

                config.CreateMap<StockOutInCreationDTO, StockOutIn>();
                //config.CreateMap<StockOutInCreationDTO, StockOutInCreationDTO>();

                config.CreateMap<StockOutInDTO, StockOutIn>()
                    .ForMember(dest => dest.DocKey, opt => opt.MapFrom(src => src.Id)); 
                config.CreateMap<StockOutIn, StockOutInDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DocKey));

                config.CreateMap<StockOutInDTLDTO, StockOutInDTL>();
                config.CreateMap<StockOutInDTL, StockOutInDTLDTO>();

                config.CreateMap<StockLocationDTO, StockLocation>();
                config.CreateMap<StockLocation, StockLocationDTO>();

                config.CreateMap<StockJobOperationRelationDTO, StockJobOperationRelation>().ReverseMap();
                config.CreateMap<StockOutInPropertyDTO, StockOutInProperty>().ReverseMap();

                config.CreateMap<TypesDTO, Types>().ReverseMap();
                

                config.CreateMap<WeightReadingsDto, WeightReadings>().ReverseMap();
                


                config.CreateMap<JobOperationStatusDTO, JobOperationStatus>().ReverseMap();
                config.CreateMap<JobOperationStatusDTO, JobOrderWithOperationStatusDTO>().ReverseMap();            
                

                






                config.CreateMap<JobOrderDto, JobOrder>();
                config.CreateMap<JobOrder, JobOrderDto>();
                config.CreateMap<JobOrderOperationDTO, JobOrderOperation>();                
                config.CreateMap<JobOrderOperation, JobOrderOperationDTO>();

                
                

                config.CreateMap<JoBOMDTO, JoBOM>();
                config.CreateMap<JoBOM, JoBOMDTO> ();




            });
            return mappingConfig;
        }
    }
}
