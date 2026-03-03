using LearningTask1.Data;
using LearningTask1.Dtos;
using LearningTask1.Helpers;
using LearningTask1.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
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

            entity.ImageUrl = await SaveFileAndGetUrlAsync(file);

            context.BusinessCards.Add(entity);
            await context.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<bool> DeleteBusinessCardAsync(int id)
        {
            var existingBusinessCard = await context.BusinessCards.FindAsync(id);
            if (existingBusinessCard is null)
                return false;

            DeleteFileIfExists(existingBusinessCard.ImageUrl);

            context.BusinessCards.Remove(existingBusinessCard);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<byte[]> ExportBusinessCardsAsync(PaginationParams pagination, string format)
        {
            var query = context.BusinessCards.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
            {
                query = query.Where(c => 
                    c.Name.Contains(pagination.SearchTerm) ||
                    c.Email.Contains(pagination.SearchTerm) ||
                    (c.PhoneNumber != null && c.PhoneNumber.Contains(pagination.SearchTerm))
                );
            }

            if (!string.IsNullOrWhiteSpace(pagination.Gender))
                query = query.Where(c => c.Gender == pagination.Gender);

            if (pagination.DobFrom.HasValue)
                query = query.Where(c => c.DOB >= pagination.DobFrom.Value);

            if (pagination.DobTo.HasValue)
                query = query.Where(c => c.DOB <= pagination.DobTo.Value);

            query = query.OrderBy(x => x.Id);
            var items = await query.Select(x => MapToDto(x)).ToListAsync();

            using var memoryStream = new MemoryStream();

            if (format.Equals("csv", StringComparison.OrdinalIgnoreCase))
            {
                using var writer = new StreamWriter(memoryStream, leaveOpen: true);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                await csv.WriteRecordsAsync(items);
                await writer.FlushAsync();
            }
            else if (format.Equals("xml", StringComparison.OrdinalIgnoreCase))
            {
                var serializer = new XmlSerializer(typeof(System.Collections.Generic.List<BusinessCardDto>));
                serializer.Serialize(memoryStream, items);
            }
            else
            {
                throw new ArgumentException("Invalid export format. Supported formats are 'csv' and 'xml'.");
            }

            memoryStream.Position = 0;
            return memoryStream.ToArray();
        }

        public async Task<int> ImportBusinessCardsAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return 0;

            var ext = Path.GetExtension(file.FileName).ToLower();
            var importedCards = new System.Collections.Generic.List<BusinessCard>();

            using var stream = file.OpenReadStream();

            if (ext == ".csv")
            {
                using var reader = new StreamReader(stream);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HeaderValidated = null, MissingFieldFound = null };
                using var csv = new CsvReader(reader, config);
                var dtos = csv.GetRecords<BusinessCardDto>().ToList();
                foreach(var dto in dtos) {
                    importedCards.Add(new BusinessCard {
                        Name = dto.Name, Gender = dto.Gender, DOB = dto.DOB, Email = dto.Email, PhoneNumber = dto.PhoneNumber, Address = dto.Address, ImageUrl = dto.ImageUrl
                    });
                }
            }
            else if (ext == ".xml")
            {
                var serializer = new XmlSerializer(typeof(System.Collections.Generic.List<BusinessCardDto>));
                if (serializer.Deserialize(stream) is System.Collections.Generic.List<BusinessCardDto> dtos)
                {
                    foreach(var dto in dtos) {
                        importedCards.Add(new BusinessCard {
                            Name = dto.Name, Gender = dto.Gender, DOB = dto.DOB, Email = dto.Email, PhoneNumber = dto.PhoneNumber, Address = dto.Address, ImageUrl = dto.ImageUrl
                        });
                    }
                }
            }
            else
            {
                throw new ArgumentException("Invalid file type. Only .csv and .xml are supported.");
            }

            if (importedCards.Any())
            {
                await context.BusinessCards.AddRangeAsync(importedCards);
                await context.SaveChangesAsync();
            }

            return importedCards.Count;
        }

        public async Task<BusinessCardDto?> GetBusinessCardByIdAsync(int id)
        {
            var result = await context.BusinessCards
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => MapToDto(x))
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<PagedResult<BusinessCardDto>> GetBusinessCardsAsync(PaginationParams pagination)
        {
            var query = context.BusinessCards.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
            {
                query = query.Where(c => 
                    c.Name.Contains(pagination.SearchTerm) ||
                    c.Email.Contains(pagination.SearchTerm) ||
                    (c.PhoneNumber != null && c.PhoneNumber.Contains(pagination.SearchTerm))
                );
            }

            if (!string.IsNullOrWhiteSpace(pagination.Gender))
            {
                query = query.Where(c => c.Gender == pagination.Gender);
            }

            if (pagination.DobFrom.HasValue)
            {
                query = query.Where(c => c.DOB >= pagination.DobFrom.Value);
            }

            if (pagination.DobTo.HasValue)
            {
                query = query.Where(c => c.DOB <= pagination.DobTo.Value);
            }

            query = query.OrderBy(x => x.Id);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(x => MapToDto(x))
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

            if (!string.IsNullOrEmpty(dto.ImageUrl))
            {
                existingBusinessCard.ImageUrl = dto.ImageUrl;
            }

            if (file is not null && file.Length > 0)
            {
                DeleteFileIfExists(existingBusinessCard.ImageUrl);
                existingBusinessCard.ImageUrl = await SaveFileAndGetUrlAsync(file);
            }

            await context.SaveChangesAsync();
            return true;
        }



        private static BusinessCardDto MapToDto(BusinessCard entity)
        {
            return new BusinessCardDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Gender = entity.Gender,
                DOB = entity.DOB,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Address = entity.Address,
                ImageUrl = entity.ImageUrl
            };
        }

        private async Task<string?> SaveFileAndGetUrlAsync(IFormFile? file)
        {
            if (file is null || file.Length == 0)
                return null;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_permittedExtensions.Contains(ext))
                return null; 

            var uploadsFolder = GetUploadsFolderPath();
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        private void DeleteFileIfExists(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl) || !imageUrl.StartsWith("/uploads/"))
                return;

            var uploadsRoot = GetUploadsFolderPath();
            var specificFileName = imageUrl.Replace("/uploads/", "");
            var filePath = Path.Combine(uploadsRoot, specificFileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string GetUploadsFolderPath()
        {
            var uploadsRoot = env.WebRootPath;
            if (string.IsNullOrEmpty(uploadsRoot))
            {
                uploadsRoot = Path.Combine(env.ContentRootPath, "wwwroot");
            }
            return Path.Combine(uploadsRoot, "uploads");
        }
    }
}
