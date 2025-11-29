using Hospital_Project.Entities.DaysEnum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Project.Entities.DTOs
{
    [Table("DoctorAvailableDate")]
    public class DoctorAvailableTimeDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="الطبيب مطلوب")]
        public int DoctorId { get; set; }
        [Required(ErrorMessage = "أيام العمل مطلوبة")]
        public Daysofweek AvailableTimes { get; set; }
        [Required(ErrorMessage = " موعد البدء مطلوب")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "موعد الإنتهاء مطلوب")]
        public DateTime EndTime { get; set; }
    }
}
