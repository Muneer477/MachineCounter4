using SMTS.DTOs;

namespace SMTS.Service.IService
{    
    public interface IContactService
    {
        Task<IEnumerable<ContactDto>> GetAllAsync();
        Task<ContactDto?> GetByIdAsync(int id);
        Task<ContactDto?> CreateAsync(ContactDto contactDto);
        Task<ContactDto?> UpdateAsync(ContactDto contactDto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();

        //Task<ResponseDto?> GetAllContactsAsync();
        //Task<ResponseDto?> GetContactByIdAsync(int id);
        //Task<ResponseDto?> CreateContactAsync(ContactDto contactDto);
        //Task<ResponseDto?> UpdateContactAsync(ContactDto contactDto);
        //Task<ResponseDto?> DeleteContactAsync(int id);
    }
}
