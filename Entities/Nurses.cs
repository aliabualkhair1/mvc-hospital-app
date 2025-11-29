using Hospital_Project.Entities.Enums;
using Hospital_Project.Entities.User;

namespace Hospital_Project.Entities
{
    public class Nurses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public DateTime StartTime  { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Salary { get; set; }
        public string Phone { get; set; }
        public int Departmentid { get; set; }
        public Department Department { get; set; }
        public ICollection<DocNurse> Doctors { get; set; } = new List<DocNurse>();
  

    }
}
