using LearningTask1.Dtos;
using LearningTask1.Helpers;

namespace LearningTask1.Services
{
    public interface IBusinessCardService
    {
        Task<PagedResult<BusinessCardDto>> GetBusinessCardsAsync(PaginationParams pagination);
        Task<BusinessCardDto?> GetBusinessCardByIdAsync(int id);
        Task<BusinessCardDto> AddBusinessCardAsync(CreateBusinessCardDto dto, IFormFile? file = null);
        Task<bool> UpdateBusinessCardAsync(int id, UpdateBusinessCardDto dto, IFormFile? file = null);
        Task<bool> DeleteBusinessCardAsync(int id);
        Task<byte[]> ExportBusinessCardsAsync(PaginationParams pagination, string format);
        Task<int> ImportBusinessCardsAsync(IFormFile file);
    }
}
