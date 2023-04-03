using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DatabaseInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public DatabaseInitializer(UserManager<User> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task InitializeDatabase()
    {
        try
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                await _dbContext.Database.MigrateAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        var user = new User()
        {
            Email = "test@test.com",
            UserName = "test@test.com",
            EmailConfirmed = true,
            FirstName = "Ime",
            LastName = "Prezime"
        };

        if (_userManager.Users.All(u => u.Email != user.Email))
            await _userManager.CreateAsync(user, "ASDqwe123");
    }
}