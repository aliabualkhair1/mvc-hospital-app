using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.UserDTOs
{
    public class SignIn
    {
        [EmailAddress]
        [Required(ErrorMessage = "البريد الإلكترونى مطلوب")]
        public string Email { get; set; }
        [PasswordPropertyText]
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; }
    }
}
