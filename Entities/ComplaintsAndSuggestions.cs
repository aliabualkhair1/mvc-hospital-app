using Hospital_Project.Entities.User;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities
{
    public class ComplaintsAndSuggestions
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(11)]
        public string Phone { get; set; }
        [Required]
        public string ProblemandSuggestion { get; set; }
        public DateTime ProblemandSuggestionDate { get; set; }
        public string UserId { get; set; }
        public UserInfo User { get; set; }
    }
}
