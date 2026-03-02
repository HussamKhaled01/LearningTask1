using System.ComponentModel.DataAnnotations;

namespace LearningTask1.Dtos
{
    public class CreateBusinessCardDto
    {
        [Required]
        public string Name { get; set; }
        public string? Gender { get; set; }
        public DateOnly DOB { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
    }
}
