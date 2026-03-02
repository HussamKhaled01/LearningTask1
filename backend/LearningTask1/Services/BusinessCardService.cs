using LearningTask1.Data;
using LearningTask1.Dtos;
using LearningTask1.Helpers;
using LearningTask1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LearningTask1.Services
{
    public class BusinessCardService(AppDbContext context) : IBusinessCardService
    {

        public async Task<BusinessCardDto> AddBusinessCardAsync(CreateBusinessCardDto dto)
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

            return new BusinessCardDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Gender = entity.Gender,
                DOB = entity.DOB,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Address = entity.Address
            };
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

        public async Task<BusinessCardDto?> GetBusinessCardByIdAsync(int id)
        {
            var result = await context.BusinessCards
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new BusinessCardDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Gender = x.Gender,
                    DOB = x.DOB,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<PagedResult<BusinessCardDto>>
    GetBusinessCardsAsync(PaginationParams pagination)
        {
            var query = context.BusinessCards
                .AsNoTracking()
                .OrderBy(x => x.Id);   // VERY IMPORTANT for pagination

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(x => new BusinessCardDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Gender = x.Gender,
                    DOB = x.DOB,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address
                })
                .ToListAsync();

            return new PagedResult<BusinessCardDto>
            {
                Data = items,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = totalCount
            };
        }


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
