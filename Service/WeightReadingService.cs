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

namespace SMTS.Service
{
    public class WeightReadingService : IWeightReadingService
    {
        private readonly MESDbContext _context;
        private readonly IMapper _mapper;
        public WeightReadingService(MESDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<WeightReadingsDto?> CreateAsync(WeightReadingsDto weightReadingsDto)
        {
            var WeightReadings = _mapper.Map<WeightReadings>(weightReadingsDto);
            _context.WeightReadings.Add(WeightReadings);
            await _context.SaveChangesAsync();
            return _mapper.Map<WeightReadingsDto>(WeightReadings);
        }

        public async Task<WeightReadingsDto?> GetByIdAsync(int id)
        {
            var uoms = await _context.WeightReadings.FindAsync(id);
            return _mapper.Map<WeightReadingsDto>(uoms);
        }


    }
}
