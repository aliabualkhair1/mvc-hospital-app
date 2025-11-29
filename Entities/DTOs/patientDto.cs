using Hospital_Project.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.DTOs
{
    public class patientDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="الإسم مطلوب")]
        [RegularExpression(@"^[\p{IsArabic} ]+$", ErrorMessage = "الاسم يجب أن يحتوي على أحرف عربية أو مسافات فقط بدون رموز أو أرقام")]
        [MinLength(10, ErrorMessage = "من فضلك أدخل الإسم ثلاثى ")]
        [MaxLength(35)]
        public string Name { get; set; }
        [Required(ErrorMessage ="العمر مطلوب")]
        [Range(1,90,ErrorMessage ="اعد كتابة العمر بشكل صحيح ")]
        public int? Age { get; set; }
        [Required(ErrorMessage ="النوع مطلوب")]
        public string Gender { get; set; }
        [MinLength(11, ErrorMessage = " من فضلك أدخل رقم هاتف صحيح ")]
        [MaxLength(11)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "رقم الهاتف غير صالح")]
        [Required(ErrorMessage ="رقم الهاتف مطلوب")]
        public string Phone { get; set; }
        public ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();

    }
}
