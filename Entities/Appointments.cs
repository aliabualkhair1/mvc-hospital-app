using Hospital_Project.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Project.Entities;
public class Appointments
{
    public int Id { get; set; }
    public DateTime DateOfBooking{ get; set; }
    public DateTime? ConsultationDateOfBooking{ get; set; }
    public DateOnly? ConsultationDate { get; set; }
    public DateOnly EndOfConsultationDate { get; set; }
    public DateOnly DoctorDate{ get; set; }
    public int PatientId { get; set; }
    public Patients Patient { get; set; }
    public int DoctorId { get; set; }
    public Doctors Doctor { get; set; }
    public int? EmployeeId { get; set; }
    public Employees Employee { get; set; }
    public int DoctorAvailableTimeId { get; set; }
    public DoctorsTimeTable DoctorAvailableTime { get; set; }
    public int NumberOfBooking { get; set; }
    public int? NumberOfConsultation { get; set; }


}