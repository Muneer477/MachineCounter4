namespace SMTS.DTOs.Common
{
    public class ResponseDTO
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<string> Error { get; set; } // Include details about the error if applicable

        public bool IsSuccess { get; set; } = true;
    }
}
