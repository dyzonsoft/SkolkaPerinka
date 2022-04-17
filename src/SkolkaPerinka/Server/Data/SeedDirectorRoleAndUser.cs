using Microsoft.AspNetCore.Identity;

namespace SkolkaPerinka.Server.Data
{
    public class SeedDirectorRoleAndUser
    {
        internal async static Task Seed(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            await SeedDirectorRole(roleManager);
            await SeedDirectorUser(userManager);
        }

        private async static Task SeedDirectorRole(RoleManager<IdentityRole> roleManager)
        {
            bool DirectorRoleExists = await roleManager.RoleExistsAsync("Director");
            bool TeacherRoleExists = await roleManager.RoleExistsAsync("Teacher");
            bool ParentRoleExists = await roleManager.RoleExistsAsync("Parent");

            if (DirectorRoleExists == false)
            {
                var roleDirector = new IdentityRole
                {
                    Name = "Director"
                };
                await roleManager.CreateAsync(roleDirector);
            }

            if (TeacherRoleExists == false)
            {
                var roleTeacher = new IdentityRole
                {
                    Name = "Teacher"
                };
                await roleManager.CreateAsync(roleTeacher);
            }

            if (ParentRoleExists == false)
            {
                var roleParent = new IdentityRole
                {
                    Name = "Parent"
                };
                await roleManager.CreateAsync(roleParent);
            }
        }

        private async static Task SeedDirectorUser(UserManager<IdentityUser> userManager)
        {
            bool DirectorUserExists = await userManager.FindByEmailAsync("director@example.com") != null;

            var DirectorUser = new IdentityUser
            {
                UserName = "director@example.com",
                Email = "director@example.com"
            };

            IdentityResult identityResult = await userManager.CreateAsync(DirectorUser, "Password1!");

            if (identityResult.Succeeded)
            {
                await userManager.AddToRoleAsync(DirectorUser, "Director");
            }
        }
    }
}
