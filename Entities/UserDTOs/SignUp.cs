using Hospital_Project.Entities.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.UserDTOs
{
    public class SignUp
    {
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [DisplayName("الاسم الأول")]
        [RegularExpression(@"^[\p{IsArabic} ]+$", ErrorMessage = "الاسم يجب أن يحتوي على أحرف عربية أو مسافات فقط بدون رموز أو أرقام")]
        [MinLength(2,ErrorMessage ="أعد الكتابة بشكل صحيح")]
        [MaxLength(10,ErrorMessage ="أعد الكتابة بشكل صحيح")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        [DisplayName("اسم العائلة")]
        [RegularExpression(@"^[\p{IsArabic} ]+$", ErrorMessage = "الاسم يجب أن يحتوي على أحرف عربية أو مسافات فقط بدون رموز أو أرقام")]
        [MinLength(2, ErrorMessage = "أعد الكتابة بشكل صحيح")]
        [MaxLength(10, ErrorMessage = "أعد الكتابة بشكل صحيح")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "النوع مطلوب")]
        [DisplayName("النوع")]
        public Gender? Gender { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [MinLength(11, ErrorMessage = " من فضلك أدخل رقم هاتف صحيح ")]
        [MaxLength(11)]
        [DisplayName("رقم الهاتف")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "رقم الهاتف غير صالح")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "الإيميل غير صالح")]
        [DisplayName("البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [DisplayName("كلمة المرور")]
        [PasswordPropertyText]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و15 حرف")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9\s\u0600-\u06FF]).{6,15}$",
ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير، صغير، رقم، ورمز، ويجب ألا تحتوي على حروف عربية، ويكون طولها من 6 إلى 15 حرفًا.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "كلمة المرور غير متطابقة")]
        [DisplayName("تأكيد كلمة المرور")]
        [PasswordPropertyText]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و15 حرف")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9\s\u0600-\u06FF]).{6,15}$",
ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير، صغير، رقم، ورمز، ويجب ألا تحتوي على حروف عربية، ويكون طولها من 6 إلى 15 حرفًا.")]
        public string ConfirmPassword { get; set; }

    }
}
