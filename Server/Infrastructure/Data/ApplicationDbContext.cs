using Application.Common.Interfaces;
using Domain.Common;
using Infrastructure.Identity;
using Infrastructure.Identity.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>, IApplicationDbContext
{
    private readonly IDateTimeService _dateTimeService;
    public DbSet<GymUser> GymUsers { get; set; }
    public DbSet<GymUserView> GymUserView { get; set; }
    public DbSet<CheckInHistory> CheckIns { get; set; }
    public DbSet<GymWorkerView> GymWorkers { get; set; }
    public DbSet<CheckInHistoryView> CheckInHistoryView { get; set; }
    public DbSet<DailyTraining> DailyTraining { get; set; }
    public DbSet<DailyHistory> DailyHistory { get; set; }
    public DbSet<DailyHistoryView> DailyHistoryView { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTimeService)
        : base(options) => _dateTimeService = dateTimeService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");
        builder.Entity<IdentityRole<int>>().ToTable("Roles");
        builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        builder.Entity<CheckInHistory>().ToTable("CheckInHistory");
        builder.Entity<GymUser>().ToTable("GymUser");
        builder.Entity<GymWorker>().ToTable("GymWorker");
        builder.Entity<GymWorkerView>().ToView("GymWorkerView");
        builder.Entity<GymUserView>().ToView("GymUserView");
        builder.Entity<CheckInHistoryView>().ToView("CheckInHistoryView");
        builder.Entity<DailyTraining>().ToTable("DailyTraining");
        builder.Entity<DailyHistory>().ToTable("DailyHistory");
        builder.Entity<DailyHistoryView>().ToView("DailyHistoryView");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.DateCreated = _dateTimeService.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.DateModified = _dateTimeService.Now;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}