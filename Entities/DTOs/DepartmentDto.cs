using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "إسم القسم مطلوب")]

        public Departments Name { get; set; }

    }
}
