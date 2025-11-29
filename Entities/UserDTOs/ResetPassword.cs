using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.UserDTOs
{
    public class ResetPassword
    {
        [EmailAddress]
[Required(ErrorMessage ="البريد الإلكترونى مطلوب")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [PasswordPropertyText]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و15 حرف")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9\s\u0600-\u06FF]).{6,15}$",
ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير، صغير، رقم، ورمز، ويجب ألا تحتوي على حروف عربية، ويكون طولها من 6 إلى 15 حرفًا.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [PasswordPropertyText]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و15 حرف")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9\s\u0600-\u06FF]).{6,15}$",
ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير، صغير، رقم، ورمز، ويجب ألا تحتوي على حروف عربية، ويكون طولها من 6 إلى 15 حرفًا.")]
        public string ConfirmNewPassword{ get; set; }
    }
}
