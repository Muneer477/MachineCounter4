using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMTS.DTOs.Common;
using SMTS.DTOs.Stock;
using SMTS.DTOs.Types;
using SMTS.Entities;
using SMTS.Service.IService;
using System.Net.Sockets;

namespace SMTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly MESDbContext _context;
        private readonly IMapper mapper;
        private readonly IStockOutInPropertyService _stockOutInPropertyService;
        private readonly IStockJobOperationRelationService _stockJobOperationRelationService;

        public StockController(MESDbContext mESDbContext, IMapper mapper, IStockOutInPropertyService stockOutInPropertyService, IStockJobOperationRelationService stockJobOperationRelationService)
        {
            this._context = mESDbContext;
            this.mapper = mapper;
            this._stockOutInPropertyService = stockOutInPropertyService;
            _stockJobOperationRelationService = stockJobOperationRelationService;
        }

        [HttpGet("stockOutIn-list")]
        public async Task<List<StockOutInDTO>> getList(string type)
        {
            var data = await _context.StockOutIn.Where(row => row.Type == type)
            .ToListAsync();

            return mapper.Map<List<StockOutInDTO>>(data);
        }

        /*{
        "stockOutInCreationDTO": {
          "docNo": "string",
          "desc": "Blowing",
          "date": "2023-11-19T03:49:56.418Z",
          "type": "SR"
        },
        "stockOutInDTLDTOs": [
          {
            "dtlKey": 0,
            "seq": 1,
            "partId": 142,
            "locationId": 1,
            "quantity": 339,
            "uomId": 1,
            "date": "2023-11-19T03:49:56.418Z",
            "batch": "string",
            "serialNumber": "string",
            "type": "SR",
            "remark": "string",
            "typeOf": "string",
            "docKey": 0,
            "partCode": "string",
            "description": "string"
          }
        ],
        "operationId": 2362
      }*/
        [HttpPost("stockOutIn-operation-receive")]
        public async Task<ActionResult<ResponseDTO>> createStockReceiveOperation([FromBody] StockReceiveCreationDTO dto) //combine 2 dto (stockoutin, stockoutindtl)
        {
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //initialise StockOutIn 
                    //if can select difference type (SR/SI/ST/SA)
                    //get running number according to type
                    //assign StockOutIn New DocNo
                    //add
                    //save

                    var stockOutIn = mapper.Map<StockOutIn>(dto.StockOutInCreationDTO); 
                    var runningNumber = await GetNextRunningNumberAsync(dto.StockOutInCreationDTO.Type);
                    stockOutIn.DocNo = runningNumber.NewDocNo;
                    _context.StockOutIn.Add(stockOutIn);
                    await _context.SaveChangesAsync();

                    //for loop
                        //initialise StockOutInDTL
                        //transfer StockOutIn DocKey to DTL
                        //add
                        //await add - joboperationrelation (operationid dtlkey)
                        //if stockoutin description is blowing, await add property (3, seq, dtlKey)
                        //save all max 3

                    foreach (var detailDto in dto.stockOutInDTLDTOs)
                    {
                        var stockOutInDTL = mapper.Map<StockOutInDTL>(detailDto);
                        stockOutInDTL.DocKey = stockOutIn.DocKey;
                        _context.StockOutInDTL.Add(stockOutInDTL);
                        await _context.SaveChangesAsync();

                        await _stockJobOperationRelationService.CreateAsyncWithOutReturn(dto.OperationId, stockOutInDTL.DtlKey);

                        if (dto.StockOutInCreationDTO.Desc == "Blowing")
                        {
                            await _stockOutInPropertyService.CreateWithoutReturn(3, stockOutInDTL.Seq.ToString(), stockOutInDTL.DtlKey);
                        }

                    }

                    await SaveRunningNumberAsync(runningNumber.Number, runningNumber.Prefix);
                    await transaction.CommitAsync();

                    var successResponseDto = new ResponseDTO
                    {
                        StatusCode = 200,
                        Message = "Stock receive operation successful.",
                        Error = new List<string>()
                    };

                    return Ok(successResponseDto);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Log the exception...
                    return StatusCode(500, ex.Message);
                }
            };
               

        }

      
        [HttpPost("stockOutIn-create")]
        public async Task<ActionResult<ResponseDTO>> createStockReceive([FromBody] StockReceiveCreationDTO dto)
        {
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var stockOutIn = mapper.Map<StockOutIn>(dto.StockOutInCreationDTO);
                    var runningNumber = await GetNextRunningNumberAsync(dto.StockOutInCreationDTO.Type);
                    stockOutIn.DocNo = runningNumber.NewDocNo;
                    _context.StockOutIn.Add(stockOutIn);
                    await _context.SaveChangesAsync();

                    foreach (var detailDto in dto.stockOutInDTLDTOs)
                    {
                        var stockOutInDTL = mapper.Map<StockOutInDTL>(detailDto);
                        stockOutInDTL.DocKey = stockOutIn.DocKey;


                        _context.StockOutInDTL.Add(stockOutInDTL);
                    }
                    await _context.SaveChangesAsync();

                    await SaveRunningNumberAsync(runningNumber.Number, runningNumber.Prefix);
                    await transaction.CommitAsync();

                    var successResponseDto = new ResponseDTO
                    {
                        StatusCode = 200,
                        Message = "Stock receive operation successful.",
                        Error = new List<string>()
                    };

                    return Ok(successResponseDto);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Log the exception...
                    return StatusCode(500, ex.Message);
                }
            };
               

        }







        // Running Number
        [HttpGet("get-RunningNumber")]
        public async Task<string> getRunningNumber(string Prefix)
        {
            var RunningNumber = await GetNextRunningNumberAsync(Prefix);

            return RunningNumber.NewDocNo;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<(string NewDocNo, int? Number, string Prefix)> GetNextRunningNumberAsync(string prefix)
        {
            // Get the current running number
            var runningNumber = await _context.RunningNumbers
                .Where(r => r.Prefix == prefix)
                .OrderByDescending(r => r.Number)
                .Select(r => r.Number)
                .FirstOrDefaultAsync();
            //Console.WriteLine(runningNumber);
            // If no existing numbers are found, start with 1
            if (runningNumber == 0)
                runningNumber = 1;
            else
                runningNumber++;

            // Format the running number with leading zeros
            string formattedRunningNumber = $"{prefix}-{runningNumber:D6}";

            return (formattedRunningNumber, runningNumber, prefix);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task SaveRunningNumberAsync(int? Number, string Prefix)
        {
            // Check if the specified prefix and number combination exists
            var existingRecord = await _context.RunningNumbers
                .FirstOrDefaultAsync(r => r.Prefix == Prefix && r.Number == Number);

            if (existingRecord == null)
            {
                // Insert the new record
                var newRecord = new RunningNumbers
                {
                    Prefix = Prefix,
                    Number = Number
                };

                _context.RunningNumbers.Add(newRecord);
                await _context.SaveChangesAsync();
            }
        }



    }
}
