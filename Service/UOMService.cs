using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMTS.DTOs;
using SMTS.Entities;
using SMTS.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMTS.Utility;

namespace SMTS.Services
{
    public class UOMService : IUOMService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;       

        public UOMService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }                

        public async Task<IEnumerable<UOMsDto>> GetAllAsync()
        {
            var uomss = await _context.UOM.ToListAsync();

            // This assumes that the UOMs class has nullable properties.
            // Adjust them or set default values before mapping if needed.
            

            return _mapper.Map<IEnumerable<UOMsDto>>(uomss);
        }


        public async Task<UOMsDto?> GetByIdAsync(int id)
        {
            var uoms = await _context.UOM.FindAsync(id);
            return _mapper.Map<UOMsDto>(uoms);
        }

        public async Task<UOMsDto?> CreateAsync(UOMsDto UOMsDto)
        {
            var uoms = _mapper.Map<UOMs>(UOMsDto);
            _context.UOM.Add(uoms);
            await _context.SaveChangesAsync();
            return _mapper.Map<UOMsDto>(uoms);
        }

        public async Task<UOMsDto> UpdateAsync(UOMsDto UOMsDto)
        {
            try
            {
                var uoms = await _context.UOM.FindAsync(UOMsDto.Id);
                if (uoms == null)
                {
                    // You need to decide what to do if the UOM isn't found
                    // Throw an exception or return null or some default value
                    throw new KeyNotFoundException("UOM not found.");
                }

                _mapper.Map(UOMsDto, uoms);
                _context.UOM.Update(uoms);
                await _context.SaveChangesAsync();
                return _mapper.Map<UOMsDto>(uoms); // Map and return the updated UOM
            }
            catch (DbUpdateException ex)
            {
                if (IsForeignKeyViolation(ex))
                {
                    // Handle the foreign key violation
                    throw new CustomException("Foreign key constraint violated.");
                }
                else
                {
                    // Handle other types of DbUpdateException or rethrow
                    // Depending on your use case, you might want to return a default value, null, or throw
                    throw; // Rethrows the current exception
                }
            }
            // If there are other potential exceptions that should be caught and handled differently,
            // add additional catch blocks here
        }        

        private bool IsForeignKeyViolation(DbUpdateException ex)
        {
            var sqlException = ex.GetBaseException() as SqlException;

            if (sqlException != null)
            {
                foreach (SqlError error in sqlException.Errors)
                {
                    // In SQL Server, the number for a foreign key violation is 547
                    if (error.Number == 547)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var uoms = await _context.UOM.FindAsync(id);
            if (uoms == null) return false;

            _context.UOM.Remove(uoms);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            int count = await _context.UOM.CountAsync();
            return count;
        }
    }
}

//using Mango.Web.Models;
//using Mango.Web.Service.IService;
//using Mango.Web.Utility;

//namespace Mango.Web.Service
//{
//    public class CouponService : ICouponService
//    {
//        private readonly IBaseService _baseService;
//        public CouponService(IBaseService baseService)
//        {
//            _baseService = baseService;
//        }

//        public async Task<ResponseDto?> CreateCouponsAsync(CouponDto couponDto)
//        {
//            return await _baseService.SendAsync(new RequestDto()
//            {
//                ApiType = SD.ApiType.POST,
//                Data = couponDto,
//                Url = SD.CouponAPIBase + "/api/coupon"
//            });
//        }

//        public async Task<ResponseDto?> DeleteCouponsAsync(int id)
//        {
//            return await _baseService.SendAsync(new RequestDto()
//            {
//                ApiType = SD.ApiType.DELETE,
//                Url = SD.CouponAPIBase + "/api/coupon/" + id
//            });
//        }

//        public async Task<ResponseDto?> GetAllCouponsAsync()
//        {
//            return await _baseService.SendAsync(new RequestDto()
//            {
//                ApiType = SD.ApiType.GET,
//                Url = SD.CouponAPIBase + "/api/coupon"
//            });
//        }

//        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
//        {
//            return await _baseService.SendAsync(new RequestDto()
//            {
//                ApiType = SD.ApiType.GET,
//                Url = SD.CouponAPIBase + "/api/coupon/GetByCode/" + couponCode
//            });
//        }

//        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
//        {
//            return await _baseService.SendAsync(new RequestDto()
//            {
//                ApiType = SD.ApiType.GET,
//                Url = SD.CouponAPIBase + "/api/coupon/" + id
//            });
//        }

//        public async Task<ResponseDto?> UpdateCouponsAsync(CouponDto couponDto)
//        {
//            return await _baseService.SendAsync(new RequestDto()
//            {
//                ApiType = SD.ApiType.PUT,
//                Data = couponDto,
//                Url = SD.CouponAPIBase + "/api/coupon"
//            });
//        }
//    }
//}
