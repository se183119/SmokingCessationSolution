using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace DataAccessObjects;

public class SmokingWPFContext : DbContext
{
    public SmokingWPFContext(DbContextOptions<SmokingWPFContext> options) : base(options)
    {
    }

    // DbSets for all entities
    public DbSet<User> Users { get; set; }
    public DbSet<SmokingStatus> SmokingStatuses { get; set; }
    public DbSet<QuitPlan> QuitPlans { get; set; }
    public DbSet<QuitPlanMilestone> QuitPlanMilestones { get; set; }
    public DbSet<ProgressTracking> ProgressTrackings { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<MembershipPackage> MembershipPackages { get; set; }
    public DbSet<MembershipSubscription> MembershipSubscriptions { get; set; }
    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=SmokingWPF.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Role).HasConversion<int>();
        });

        // SmokingStatus configuration
        modelBuilder.Entity<SmokingStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.SmokingStatuses)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.CostPerPack).HasPrecision(10, 2);
        });

        // QuitPlan configuration
        modelBuilder.Entity<QuitPlan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.QuitPlans)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // QuitPlanMilestone configuration
        modelBuilder.Entity<QuitPlanMilestone>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.QuitPlan)
                  .WithMany(q => q.Milestones)
                  .HasForeignKey(e => e.QuitPlanId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ProgressTracking configuration
        modelBuilder.Entity<ProgressTracking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.ProgressTrackings)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.MoneySaved).HasPrecision(10, 2);
            entity.Property(e => e.MoneySpent).HasPrecision(10, 2);
            entity.Property(e => e.Weight).HasPrecision(5, 2);
            entity.HasIndex(e => new { e.UserId, e.RecordDate }).IsUnique();
        });

        // Achievement configuration
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Achievements)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Notification configuration
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Notifications)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // MembershipPackage configuration
        modelBuilder.Entity<MembershipPackage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(10, 2);
        });

        // MembershipSubscription configuration
        modelBuilder.Entity<MembershipSubscription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithOne(u => u.MembershipSubscription)
                  .HasForeignKey<MembershipSubscription>(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.MembershipPackage)
                  .WithMany(m => m.Subscriptions)
                  .HasForeignKey(e => e.MembershipPackageId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.AmountPaid).HasPrecision(10, 2);
        });

        // Consultation configuration
        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Member)
                  .WithMany(u => u.ConsultationsAsMember)
                  .HasForeignKey(e => e.MemberId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Coach)
                  .WithMany(u => u.ConsultationsAsCoach)
                  .HasForeignKey(e => e.CoachId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Feedback configuration
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Feedbacks)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
