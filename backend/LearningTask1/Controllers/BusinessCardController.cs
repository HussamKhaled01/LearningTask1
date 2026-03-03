using LearningTask1.Dtos;
using LearningTask1.Helpers;
using LearningTask1.Services;
using Microsoft.AspNetCore.Mvc;


namespace LearningTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCardController(IBusinessCardService service) : ControllerBase
    {
        private static readonly string[] _permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSizeBytes = 1 * 1024 * 1024;

        [HttpGet]
        public async Task<ActionResult<PagedResult<BusinessCardDto>>>GetBusinessCards([FromQuery] PaginationParams pagination)
        {
            var result = await service.GetBusinessCardsAsync(pagination);
            return Ok(result);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportData([FromQuery] PaginationParams pagination, [FromQuery] string format = "csv")
        {
            var fileContent = await service.ExportBusinessCardsAsync(pagination, format);
            var contentType = format.ToLower() == "xml" ? "application/xml" : "text/csv";
            var fileExtension = format.ToLower() == "xml" ? "xml" : "csv";
            return File(fileContent, contentType, $"BusinessCardsExport.{fileExtension}");
        }

        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportData(IFormFile file)
        {
            var count = await service.ImportBusinessCardsAsync(file);
            return Ok(new { Message = $"Successfully imported {count} business cards." });
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

            var fileError = ValidateImage(file);
            if (fileError != null) return BadRequest(fileError);

            var createdCard = await service.AddBusinessCardAsync(dto, file);
            return CreatedAtAction(nameof(GetBusinessCard), new { id = createdCard.Id }, createdCard);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateBusinessCard(int id, [FromForm] UpdateBusinessCardDto dto, IFormFile? file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var fileError = ValidateImage(file);
            if (fileError != null) return BadRequest(fileError);

            var isUpdated = await service.UpdateBusinessCardAsync(id, dto, file);
            if (!isUpdated) 
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBusinessCard(int id)
        {
            var isDeleted = await service.DeleteBusinessCardAsync(id);
            if (!isDeleted) 
                return NotFound();

            return NoContent();
        }

        private static string? ValidateImage(IFormFile? file)
        {
            if (file is null) return null;

            if (file.Length > MaxFileSizeBytes)
                return "File too large. Max 1 MB.";

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_permittedExtensions.Contains(ext))
                return "Invalid file type. Allowed: .jpg, .jpeg, .png, .gif";

            return null;
        }
    }
}
