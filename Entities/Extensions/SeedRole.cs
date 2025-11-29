using Hospital_Project.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace Hospital_Project.Entities.Extensions
{
    public class SeedRole
    {
     
            public static async Task InitializeAsync(IServiceProvider serviceProvider)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<UserInfo>>();
                string[] roles = { "Employee", "Doctor", "Patient","Nurse","Admin" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
     
            
   
            }
    }                 
}
