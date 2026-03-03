using LearningTask1.Models;
using LearningTask1.Services;
using LearningTask1.Helpers;
using LearningTask1.Data;
using LearningTask1.Dtos;
using LearningTask1.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LearningTask1.Tests.Services
{
    public class BusinessCardServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString()) 
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddBusinessCardAsync_SavesCardToDatabase()
        {
            using var context = GetInMemoryDbContext();
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment.Setup(m => m.WebRootPath).Returns("wwwroot");
            
            var service = new BusinessCardService(context, mockEnvironment.Object);
            
            var newCardDto = new CreateBusinessCardDto 
            { 
                Name = "Unit Test Entry",
                Email = "test@example.com",
                PhoneNumber = "123456789",
                DOB = System.DateOnly.FromDateTime(System.DateTime.Now),
                Gender = "Male",
                Address = "123 Test Street"
            };

            var createdCard = await service.AddBusinessCardAsync(newCardDto, null);

            Assert.NotNull(createdCard);
            Assert.Equal(1, context.BusinessCards.Count());
            Assert.Equal("Unit Test Entry", context.BusinessCards.First().Name);
        }

        [Fact]
        public async Task GetBusinessCardByIdAsync_ReturnsCorrectCard()
        {
            using var context = GetInMemoryDbContext();
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var service = new BusinessCardService(context, mockEnvironment.Object);

            var card = new BusinessCard { Name = "Find Me", PhoneNumber = "111", Email="findme@mail.com", Gender="Male", Address="a" };
            context.BusinessCards.Add(card);
            await context.SaveChangesAsync();

            var result = await service.GetBusinessCardByIdAsync(card.Id);

            Assert.NotNull(result);
            Assert.Equal("Find Me", result.Name);
        }

        [Fact]
        public async Task GetBusinessCardsAsync_SortsAndPaginatesCorrectly()
        {
            using var context = GetInMemoryDbContext();
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var service = new BusinessCardService(context, mockEnvironment.Object);

            for (int i = 0; i < 15; i++)
            {
                context.BusinessCards.Add(new BusinessCard { Name = $"Card {i}", Email="a@b.c", PhoneNumber="123", Gender="M", Address="A" });
            }
            await context.SaveChangesAsync();

            var paginationParams = new PaginationParams { PageNumber = 2, PageSize = 5 };

            var pagedList = await service.GetBusinessCardsAsync(paginationParams);

            Assert.Equal(5, pagedList.Data.Count);
            Assert.Equal(15, pagedList.TotalCount);
            Assert.Equal(3, pagedList.TotalPages);
        }
    }
}
