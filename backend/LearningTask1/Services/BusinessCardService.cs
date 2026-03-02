using LearningTask1.Data;
using LearningTask1.Dtos;
using LearningTask1.Helpers;
using LearningTask1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LearningTask1.Services
{
    public class BusinessCardService(AppDbContext context, IWebHostEnvironment env) : IBusinessCardService
    {

        private readonly string[] _permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        public async Task<BusinessCardDto> AddBusinessCardAsync(CreateBusinessCardDto dto, IFormFile? file = null)
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

            if (file is not null && file.Length > 0)
            {
                var uploadsRoot = env.WebRootPath;
                if (string.IsNullOrEmpty(uploadsRoot))
                {
                    uploadsRoot = Path.Combine(env.ContentRootPath, "wwwroot");
                }

                var uploadsFolder = Path.Combine(uploadsRoot, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_permittedExtensions.Contains(ext))
                {
                }
                else
                {
                    var fileName = $"{Guid.NewGuid()}{ext}";
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    entity.ImageUrl = $"/uploads/{fileName}";
                }
            }

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
                , ImageUrl = entity.ImageUrl
            };
        }

        public async Task<bool> DeleteBusinessCardAsync(int id)
        {
            var existingBusinessCard = await context.BusinessCards.FindAsync(id);
            if (existingBusinessCard is null)
                return false;
            try
            {
                if (!string.IsNullOrEmpty(existingBusinessCard.ImageUrl) && existingBusinessCard.ImageUrl.StartsWith("/uploads/"))
                {
                    var uploadsRoot = env.WebRootPath;
                    if (string.IsNullOrEmpty(uploadsRoot))
                    {
                        uploadsRoot = Path.Combine(env.ContentRootPath, "wwwroot");
                    }
                    var filePath = Path.Combine(uploadsRoot, existingBusinessCard.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(filePath)) File.Delete(filePath);
                }
            }
            catch { }

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
                    , ImageUrl = x.ImageUrl
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<PagedResult<BusinessCardDto>>
    GetBusinessCardsAsync(PaginationParams pagination)
        {
            var query = context.BusinessCards
                .AsNoTracking()
                .OrderBy(x => x.Id);   

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
                    , ImageUrl = x.ImageUrl
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


        public async Task<bool> UpdateBusinessCardAsync(int id, UpdateBusinessCardDto dto, IFormFile? file = null)
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

            // if client supplied external ImageUrl, set it
            if (!string.IsNullOrEmpty(dto.ImageUrl))
            {
                existingBusinessCard.ImageUrl = dto.ImageUrl;
            }

            // handle uploaded file (replace existing)
            if (file is not null && file.Length > 0)
            {
                // delete old
                try
                {
                    if (!string.IsNullOrEmpty(existingBusinessCard.ImageUrl) && existingBusinessCard.ImageUrl.StartsWith("/uploads/"))
                    {
                        var uploadsRoot = env.WebRootPath;
                        if (string.IsNullOrEmpty(uploadsRoot))
                        {
                            uploadsRoot = Path.Combine(env.ContentRootPath, "wwwroot");
                        }
                        var oldPath = Path.Combine(uploadsRoot, existingBusinessCard.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (File.Exists(oldPath)) File.Delete(oldPath);
                    }
                }
                catch { }

                var uploadsRoot2 = env.WebRootPath;
                if (string.IsNullOrEmpty(uploadsRoot2))
                {
                    uploadsRoot2 = Path.Combine(env.ContentRootPath, "wwwroot");
                }
                var uploadsFolder = Path.Combine(uploadsRoot2, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (_permittedExtensions.Contains(ext))
                {
                    var fileName = $"{Guid.NewGuid()}{ext}";
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    existingBusinessCard.ImageUrl = $"/uploads/{fileName}";
                }
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
