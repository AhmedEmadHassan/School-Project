using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Infrustructure.Seeders
{
    public class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> _userManager)
        {
            var usersCount = await _userManager.Users.CountAsync();
            if (usersCount == 0)
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@project.com",
                    FullName = "Admin User",
                    Country = "Egypt",
                    PhoneNumber = "123456789",
                    Address = "Egypt",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                await _userManager.CreateAsync(user, "P@$$w0rd");
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
