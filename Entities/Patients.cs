using Hospital_Project.Entities;
using Hospital_Project.Entities.Enums;
using Hospital_Project.Entities.User;
using System.ComponentModel.DataAnnotations;
namespace Hospital_Project.Entities;

public class Patients
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Age { get; set; }
    public string Gender { get; set; }
    [MaxLength(11)]
    public string Phone { get; set; }
    public ICollection<Appointments> Appointments { get; set; }=new List<Appointments>();
    public string UserId { get; set; }
    public UserInfo User { get; set; }
}
