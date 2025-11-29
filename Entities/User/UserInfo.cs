using Hospital_Project.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace Hospital_Project.Entities.User
{
    public class UserInfo:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string? VerificationCode { get; set; }
        public bool IsVerified { get; set; } = false;
        //public int CodeRequestCount { get; set; } = 0;
        //public DateTime? CodeRequestTimeAgain { get; set; }
    }
}
