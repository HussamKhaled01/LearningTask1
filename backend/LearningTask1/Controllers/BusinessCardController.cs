using LearningTask1.Dtos;
using LearningTask1.Helpers;
using LearningTask1.Models;
using LearningTask1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace LearningTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCardController(IBusinessCardService service) : ControllerBase
    {
        private static readonly string[] _permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSizeBytes = 1 * 1024 * 1024; // 1 MB

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
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<BusinessCardDto>> AddBusinessCard([FromForm] CreateBusinessCardDto dto, IFormFile? file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (file is not null)
            {
                if (file.Length > MaxFileSizeBytes)
                    return BadRequest("File too large. Max 1 MB.");

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_permittedExtensions.Contains(ext))
                    return BadRequest("Invalid file type. Allowed: .jpg, .jpeg, .png, .gif");
            }

            var CreatedBusinessCard = await service.AddBusinessCardAsync(dto, file);
            return CreatedAtAction(nameof(GetBusinessCard), new { id = CreatedBusinessCard.Id }, CreatedBusinessCard);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateBusinessCard(int id, [FromForm] UpdateBusinessCardDto dto, IFormFile? file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
            {
                return BadRequest("ID mismatch");
            }
            if (file is not null)
            {
                if (file.Length > MaxFileSizeBytes)
                    return BadRequest("File too large. Max 1 MB.");

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_permittedExtensions.Contains(ext))
                    return BadRequest("Invalid file type. Allowed: .jpg, .jpeg, .png, .gif");
            }

            var UpdatedBusinessCard = await service.UpdateBusinessCardAsync(id, dto, file);

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
