using Hospital_Project.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.DTOs
{
    public class EmployeesDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "الإسم مطلوب")]
        [RegularExpression(@"^[\u0621-\u064A\s]+$", ErrorMessage = "الاسم يجب أن يحتوي فقط على أحرف عربية ومسافات بدون رموز أو أرقام")]
        [MinLength(10, ErrorMessage = "من فضلك أدخل الإسم ثلاثى ")]
        [MaxLength(35)]
        public string Name { get; set; }

        [Required(ErrorMessage = "العمر مطلوب")]
        [Range(25,60,ErrorMessage ="العمر لا يقل عن 25 ولا يزيد 60")]
        public int? Age { get; set; }
        [Required(ErrorMessage = "النوع مطلوب")]
        public string Gender { get; set; }
        [MinLength(11, ErrorMessage = " من فضلك أدخل رقم هاتف صحيح ")]
        [MaxLength(11)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "رقم الهاتف غير صالح")]
        [Required(ErrorMessage ="رقم الهاتف مطلوب")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "طبيعة العمل مطلوبة")]
        public NatureOfWork NatureOfWork { get; set; }
        [Required(ErrorMessage = "الراتب مطلوب")]
        [Range(2000, 20000, ErrorMessage = "من فضلك ادخل راتب صحيح")]
        public decimal Salary { get; set; }
        public ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();

    }
}
