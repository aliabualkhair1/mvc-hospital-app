using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.UserDTOs
{
    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage ="البريد الإلكترونى مطلوب")]
        public string Email { get; set; }

        [Required(ErrorMessage = "من فضلك أدخل كود التفعيل")]
        [Display(Name = "كود التفعيل")]
        public string Code { get; set; }
    }
}
