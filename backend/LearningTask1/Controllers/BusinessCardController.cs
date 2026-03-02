using LearningTask1.Dtos;
using LearningTask1.Helpers;
using LearningTask1.Models;
using LearningTask1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCardController(IBusinessCardService service) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<PagedResult<BusinessCardDto>>>GetBusinessCards([FromQuery] PaginationParams pagination)
        {
            var result = await service.GetBusinessCardsAsync(pagination);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessCardDto>> GetBusinessCard(int id)
        {
            var businessCard = await service.GetBusinessCardByIdAsync(id);

            if (businessCard is null)
            {
                return NotFound("Business Card with the given id was not found");
            }
            return Ok(businessCard);
        }

        [HttpPost]
        public async Task<ActionResult<BusinessCardDto>> AddBusinessCard(CreateBusinessCardDto dto)
        {
            var CreatedBusinessCard = await service.AddBusinessCardAsync(dto);
            return CreatedAtAction(nameof(GetBusinessCard), new { id = CreatedBusinessCard.Id }, CreatedBusinessCard);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBusinessCard(int id, UpdateBusinessCardDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID mismatch");
            }
            var UpdatedBusinessCard = await service.UpdateBusinessCardAsync(id, dto);

            if (UpdatedBusinessCard == false)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBusinessCard(int id)
        {
            var DeletedBusinessCard = await service.DeleteBusinessCardAsync(id);

            if (DeletedBusinessCard == false)
            {
                return NotFound();
            }
            return NoContent();

        }
    }
}
