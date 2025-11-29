using Hospital_Project.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.DTOs
{
    public class DoctorsDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "الإسم مطلوب")]
        [RegularExpression(@"^[\u0621-\u064A\s]+$", ErrorMessage = "الاسم يجب أن يحتوي فقط على أحرف عربية ومسافات بدون رموز أو أرقام")]
        [MinLength(10, ErrorMessage = "من فضلك أدخل الإسم ثلاثى ")]
        [MaxLength(35)]
        public string Name { get; set; }
        [Required(ErrorMessage ="من فضلك أدخل العمر")]
        [Range(25,80,ErrorMessage ="العمر لا يقل عن 25 ولا يزيد عن 80 ")]
        public int? Age { get; set; }
        [MinLength(11, ErrorMessage = " من فضلك أدخل رقم هاتف صحيح ")]
        [MaxLength(11)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "رقم الهاتف غير صالح")]
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "النوع مطلوب")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "رتبة الطبيب مطلوبة")]
        public DoctorRank DoctorRank{ get; set; }
        [Required(ErrorMessage = "الراتب مطلوب")]
        [Range(2000, 25000, ErrorMessage = "من فضلك ادخل راتب صحيح")]
        public decimal Salary{ get; set; }
        [Required(ErrorMessage = " سعر الكشف مطلوب")]
        [Range(50,1000, ErrorMessage = "من فضلك ادخل راتب صحيح")]
        public decimal BookingSalary { get; set; }
        [Required(ErrorMessage = " أقصى رقم للمرضى مطلوب")]
        [Range(5,50,ErrorMessage ="الرقم لا يقل عن 5 ولا يزيد 50 مريض فى اليوم")]
        public int NumberOFPatientsonDaysofwork { get; set; }
        [Required(ErrorMessage ="اختيار التخصص مطلوب")]
        public int DepartmentId { get; set; }
    }
}
