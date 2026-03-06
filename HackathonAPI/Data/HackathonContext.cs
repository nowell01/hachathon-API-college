using HackathonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Data;

public class HackathonContext : DbContext
{
    public HackathonContext(DbContextOptions<HackathonContext> options) : base(options) { }

    public DbSet<Member> Members { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Challenge> Challenges { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Member>()
            .HasOne(m => m.Region)
            .WithMany(r => r.Members)
            .HasForeignKey(m => m.RegionID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Member>()
            .HasOne(m => m.Challenge)
            .WithMany(c => c.Members)
            .HasForeignKey(m => m.ChallengeID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Member>()
            .HasIndex(m => m.MemberCode)
            .IsUnique();

        modelBuilder.Entity<Region>()
            .HasIndex(r => r.Code)
            .IsUnique();

        modelBuilder.Entity<Challenge>()
            .HasIndex(c => c.Code)
            .IsUnique();

        modelBuilder.Entity<Region>()
            .Property(r => r.RowVersion)
            .IsRowVersion();
    }

    public override int SaveChanges()
    {
        ApplyAuditing();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditing();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditing()
    {
        var now = DateTime.Now;
        const string user = "System";

        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOn = now;
                entry.Entity.CreatedBy = user;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedOn = now;
                entry.Entity.UpdatedBy = user;
            }
        }
    }
}