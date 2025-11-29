using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.UserDTOs
{
    public class EmailForForgetPassword
    {
        [Required(ErrorMessage ="البريد الإلكترونى مطلوب")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
