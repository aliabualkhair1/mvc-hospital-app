using Hospital_Project.Entities;
using Hospital_Project.Entities.Enums;
using Hospital_Project.Entities.User;
using System.ComponentModel.DataAnnotations;

public class Employees
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Age { get; set; }
    public string Gender { get; set; }
    [MaxLength(11)]
    public string Phone { get; set; }
    public NatureOfWork NatureOfWork { get; set; }
    public decimal Salary { get; set; }
    public ICollection<Appointments> Appointments { get; set; }=new List<Appointments>();


}

