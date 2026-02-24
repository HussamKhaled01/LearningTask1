using LearningTask1.Data;
using LearningTask1.Dtos;
using LearningTask1.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningTask1.Services
{
    public class BusinessCardService(AppDbContext context) : IBusinessCardService
    {

        public async Task<BusinessCard> AddBusinessCardAsync(CreateBusinessCardDto dto)
        {
            var entity = new BusinessCard
            {
                Name = dto.Name,
                Gender = dto.Gender,
                DOB = dto.DOB,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };

            context.BusinessCards.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteBusinessCardAsync(int id)
        {
            var existingBusinessCard = await context.BusinessCards.FindAsync(id);
            if (existingBusinessCard is null)
                return false;

            context.BusinessCards.Remove(existingBusinessCard);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessCard?> GetBusinessCardByIdAsync(int id)
        {
            var result = await context.BusinessCards.FindAsync(id);
            return result;
        }

        public async Task<List<BusinessCard>> GetAllBusinessCardsAsync()
                    => await context.BusinessCards.ToListAsync();


        public async Task<bool> UpdateBusinessCardAsync(int id, UpdateBusinessCardDto dto)
        {
            var existingBusinessCard = await context.BusinessCards.FindAsync(id);
            if (existingBusinessCard is null)
            {
                return false;
            }

            existingBusinessCard.Name = dto.Name;
            existingBusinessCard.Gender = dto.Gender;
            existingBusinessCard.DOB = dto.DOB;
            existingBusinessCard.Email = dto.Email;
            existingBusinessCard.PhoneNumber = dto.PhoneNumber;
            existingBusinessCard.Address = dto.Address;

            await context.SaveChangesAsync();

            return true;
        }
    }
}
