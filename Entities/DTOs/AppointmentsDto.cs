using Hospital_Project.Entities.DaysEnum;
using Hospital_Project.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.DTOs
{
    public class AppointmentsDto
    {
        public int Id { get; set; }
        public DateTime? ConsultationDateOfBooking { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? ConsultationDate { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = " تاريخ إنتهاء الإٍستشارة مطلوب")]
        public DateOnly EndOfConsultationDate { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "تاريخ الكشف مطلوب")]
        public DateOnly DoctorDate { get; set; }
        [Required(ErrorMessage ="اختيار المريض مطلوب")]
        public int PatientId { get; set; }
        [Required(ErrorMessage = "اختيار الطبيب مطلوب")]
        public int DoctorId { get; set; }
        public int? EmployeeId { get; set; }
        [Required(ErrorMessage = "المواعيد المتاحة للطبيب مطلوبة ")]
        public int DoctorAvailableTimeId { get; set; }
        public Daysofweek DoctorAvailableTime { get; set; }
        public int NumberOfBooking { get; set; }
        public int? NumberOfConsultation { get; set; }
        [Required(ErrorMessage ="اختيار القسم مطلوب")]
        public int DepartmentId { get; set; }
    }
}
