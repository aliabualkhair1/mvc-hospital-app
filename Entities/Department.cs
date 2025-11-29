using Hospital_Project.Entities.User;

namespace Hospital_Project.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public Departments Name { get; set; }
        public ICollection<Doctors> Doctors { get; set; }=new List<Doctors>();
        public ICollection<Nurses> Nurses { get; set; } = new List<Nurses>();


    }
}
