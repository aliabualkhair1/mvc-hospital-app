using Hospital_Project.Entities.User;

namespace Hospital_Project.Entities.DTOs
{
    public class AccountDetailsViewModel
    {
            public UserInfo User { get; set; }
            public List<string> Roles { get; set; }
       
    }
}
