using Moq;
using Microsoft.AspNetCore.Mvc;
using LearningTask1.Controllers;
using LearningTask1.Services;
using LearningTask1.Models;
using LearningTask1.Dtos;
using LearningTask1.Helpers;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningTask1.Tests.Controllers
{
    public class BusinessCardControllerTests
    {
        private readonly Mock<IBusinessCardService> _mockService;
        private readonly BusinessCardController _controller;

        public BusinessCardControllerTests()
        {
            _mockService = new Mock<IBusinessCardService>();
            _controller = new BusinessCardController(_mockService.Object);
        }

        [Fact]
        public async Task GetBusinessCards_ReturnsOkResult()
        {
            var pageParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var mockCards = new List<BusinessCardDto> 
            { 
                new BusinessCardDto { Id = 1, Name = "Test" } 
            };
            var mockPagedList = new PagedResult<BusinessCardDto>
            {
                Data = mockCards,
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };
            
            _mockService.Setup(s => s.GetBusinessCardsAsync(pageParams))
                .ReturnsAsync(mockPagedList);

            var result = await _controller.GetBusinessCards(pageParams);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetBusinessCard_ExistingId_ReturnsOkResult()
        {
            var mockCard = new BusinessCardDto { Id = 1, Name = "Test" };
            _mockService.Setup(s => s.GetBusinessCardByIdAsync(1))
                .ReturnsAsync(mockCard);

            var result = await _controller.GetBusinessCard(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(mockCard, okResult.Value);
        }

        [Fact]
        public async Task GetBusinessCard_NonExistingId_ReturnsNotFoundResult()
        {
            _mockService.Setup(s => s.GetBusinessCardByIdAsync(99))
                .ReturnsAsync((BusinessCardDto?)null);

            // Act
            var result = await _controller.GetBusinessCard(99);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Business Card with the given id was not found", notFoundResult.Value);
        }
    }
}
