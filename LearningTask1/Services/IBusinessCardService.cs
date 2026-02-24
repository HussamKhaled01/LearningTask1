using LearningTask1.Dtos;
using LearningTask1.Models;

namespace LearningTask1.Services
{
    public interface IBusinessCardService
    {
        Task<List<BusinessCard>> GetAllBusinessCardsAsync();
        Task<BusinessCard?> GetBusinessCardByIdAsync(int id);
        Task<BusinessCard> AddBusinessCardAsync(CreateBusinessCardDto dto);
        Task<bool> UpdateBusinessCardAsync(int id, UpdateBusinessCardDto dto);
        Task<bool> DeleteBusinessCardAsync(int id);
    }
}
