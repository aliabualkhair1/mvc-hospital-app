using System.ComponentModel.DataAnnotations;
namespace Hospital_Project.Entities.DTOs
{
   public class ComplaintsAndSuggestionsDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "الإسم مطلوب")]
        [RegularExpression(@"^[\p{IsArabic} ]+$", ErrorMessage = "الاسم يجب أن يحتوي على أحرف عربية أو مسافات فقط بدون رموز أو أرقام")]
        [MinLength(10, ErrorMessage = "من فضلك أدخل الإسم ثلاثى ")]
        [MaxLength(35)]
        public string Name { get; set; }
        [Required(ErrorMessage ="رقم الهاتف مطلوب")]
        [MinLength(11, ErrorMessage = " من فضلك أدخل رقم هاتف صحيح ")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "رقم الهاتف غير صالح")]
        [MaxLength(11)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "الشكوى او الإقتراح مطلوبين")]
        public string ProblemandSuggestion { get; set; }
        public DateTime ProblemandSuggestionDate { get; set; }
    }
}
