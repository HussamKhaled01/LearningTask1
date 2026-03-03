
namespace LearningTask1.Models
{
    public class BusinessCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateOnly DOB { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? ImageUrl { get; set; }
    }
}
