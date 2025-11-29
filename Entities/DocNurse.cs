using Hospital_Project.Entities.User;

namespace Hospital_Project.Entities
{
    public class DocNurse
    {
        public int DoctorId { get; set; }
        public Doctors Doctor { get; set; }
        public int NurseId { get; set; }
        public Nurses Nurse { get; set; }

    }
}
