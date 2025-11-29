using Hospital_Project.Entities.DaysEnum;
using Hospital_Project.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Project.Entities
{
    [Table("DoctorAvailableDate")]
    public class DoctorsTimeTable
    {
        public int Id { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctors Doctor { get; set; }
        public Daysofweek AvailableTimes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();

    }
}
