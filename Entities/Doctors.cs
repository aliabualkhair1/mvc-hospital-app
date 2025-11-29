using Hospital_Project.Entities;
using Hospital_Project.Entities.Enums;
using Hospital_Project.Entities.User;
using System.ComponentModel.DataAnnotations;

public class Doctors
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Age { get; set; }
    [MaxLength(11)]
    public string Phone { get; set; }
    public string Gender { get; set; }
    public decimal Salary { get; set; }
    public DoctorRank DoctorRank { get; set; }
    public decimal BookingSalary { get; set; }
    public ICollection<Appointments> Appointments { get; set; }=new List<Appointments>();
    public ICollection<DoctorsTimeTable> AvailableDate { get; set; } = new List<DoctorsTimeTable>();
    public int NumberOFPatientsonDaysofwork { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public ICollection<DocNurse> Nurses { get; set; } = new List<DocNurse>();

}
