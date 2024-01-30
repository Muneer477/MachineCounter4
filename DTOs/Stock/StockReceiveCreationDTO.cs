namespace SMTS.DTOs.Stock
{
    public class StockReceiveCreationDTO
    {
        public StockOutInCreationDTO StockOutInCreationDTO { get; set; }
        public List<StockOutInDTLDTO> stockOutInDTLDTOs { get; set; }

        public int OperationId { get; set; }
    }
}
