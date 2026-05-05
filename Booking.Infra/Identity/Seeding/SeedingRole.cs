using Microsoft.AspNetCore.Identity;

namespace Restaurant.Infra.Identity.Seeding
{
    public static class SeedingRole
    {
        public static async Task GetRole(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] { "Staff", "User", "SuperAdmin", "Manager" };
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
