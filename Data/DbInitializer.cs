using RestaurantSystem.Web.Models;
using RestaurantSystem.Web.Security;

namespace RestaurantSystem.Web.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // creeaza admin DOAR daca nu exista deja
            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                context.Users.Add(new User
                {
                    Email = "admin@restaurant.local",
                    PasswordHash = PasswordHasher.Hash("Admin123!"),
                    Role = "Admin"
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
