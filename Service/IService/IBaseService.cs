using SMTS.DTOs.Common;

namespace SMTS.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDTO?> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
