using Microsoft.EntityFrameworkCore;
using SmokingCessation.Data.Entities;
using SmokingCessation.Common.Enums;

namespace SmokingCessation.Data.Context
{
    public class SmokingCessationDbContext : DbContext
    {
        public SmokingCessationDbContext(DbContextOptions<SmokingCessationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<QuitPlan> QuitPlans { get; set; }
        public DbSet<QuitPlanMilestone> QuitPlanMilestones { get; set; }
        public DbSet<SmokeLog> SmokeLogs { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Template> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
                
                entity.Property(e => e.Role)
                    .HasConversion(
                        v => v.ToString(),
                        v => (UserRole)Enum.Parse(typeof(UserRole), v));
                
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // QuitPlan entity configuration
            modelBuilder.Entity<QuitPlan>(entity =>
            {
                entity.HasKey(e => e.QuitPlanId);
                
                entity.HasOne(e => e.User)
                    .WithMany(e => e.QuitPlans)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.Property(e => e.Status)
                    .HasConversion(
                        v => v.ToString(),
                        v => (QuitPlanStatus)Enum.Parse(typeof(QuitPlanStatus), v));
                
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // QuitPlanMilestone entity configuration
            modelBuilder.Entity<QuitPlanMilestone>(entity =>
            {
                entity.HasKey(e => e.MilestoneId);
                
                entity.HasOne(e => e.QuitPlan)
                    .WithMany(e => e.Milestones)
                    .HasForeignKey(e => e.QuitPlanId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // SmokeLog entity configuration
            modelBuilder.Entity<SmokeLog>(entity =>
            {
                entity.HasKey(e => e.SmokeLogId);
                
                entity.HasOne(e => e.User)
                    .WithMany(e => e.SmokeLogs)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.Property(e => e.LogType)
                    .HasConversion(
                        v => v.ToString(),
                        v => (SmokeLogType)Enum.Parse(typeof(SmokeLogType), v));
                
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Achievement entity configuration
            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.HasKey(e => e.AchievementId);
                
                entity.Property(e => e.Type)
                    .HasConversion(
                        v => v.ToString(),
                        v => (AchievementType)Enum.Parse(typeof(AchievementType), v));
                
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // UserAchievement entity configuration
            modelBuilder.Entity<UserAchievement>(entity =>
            {
                entity.HasKey(e => e.UserAchievementId);
                
                entity.HasOne(e => e.User)
                    .WithMany(e => e.UserAchievements)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Achievement)
                    .WithMany(e => e.UserAchievements)
                    .HasForeignKey(e => e.AchievementId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasIndex(e => new { e.UserId, e.AchievementId }).IsUnique();
                
                entity.Property(e => e.EarnedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Message entity configuration
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessageId);
                
                entity.HasOne(e => e.Sender)
                    .WithMany(e => e.SentMessages)
                    .HasForeignKey(e => e.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.Receiver)
                    .WithMany(e => e.ReceivedMessages)
                    .HasForeignKey(e => e.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.ParentMessage)
                    .WithMany(e => e.Replies)
                    .HasForeignKey(e => e.ParentMessageId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.Property(e => e.Status)
                    .HasConversion(
                        v => v.ToString(),
                        v => (MessageStatus)Enum.Parse(typeof(MessageStatus), v));
                
                entity.Property(e => e.SentAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Coach entity configuration
            modelBuilder.Entity<Coach>(entity =>
            {
                entity.HasKey(e => e.CoachId);
                
                entity.HasOne(e => e.User)
                    .WithOne(e => e.Coach)
                    .HasForeignKey<Coach>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.ApprovedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.ApprovedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.Property(e => e.Status)
                    .HasConversion(
                        v => v.ToString(),
                        v => (CoachStatus)Enum.Parse(typeof(CoachStatus), v));
                
                entity.Property(e => e.AppliedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Feedback entity configuration
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId);
                
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Feedbacks)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.ResolvedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.ResolvedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.Property(e => e.Status)
                    .HasConversion(
                        v => v.ToString(),
                        v => (FeedbackStatus)Enum.Parse(typeof(FeedbackStatus), v));
                
                entity.Property(e => e.SubmittedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Template entity configuration
            modelBuilder.Entity<Template>(entity =>
            {
                entity.HasKey(e => e.TemplateId);
                
                entity.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.Property(e => e.Type)
                    .HasConversion(
                        v => v.ToString(),
                        v => (TemplateType)Enum.Parse(typeof(TemplateType), v));
                
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default achievements
            modelBuilder.Entity<Achievement>().HasData(
                new Achievement
                {
                    AchievementId = 1,
                    Title = "First Day Smoke-Free",
                    Description = "Congratulations on your first day without smoking!",
                    Type = AchievementType.TimeBasedMilestone,
                    RequiredDays = 1,
                    Points = 10,
                    CreatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    AchievementId = 2,
                    Title = "One Week Warrior",
                    Description = "You've made it through your first week smoke-free!",
                    Type = AchievementType.TimeBasedMilestone,
                    RequiredDays = 7,
                    Points = 50,
                    CreatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    AchievementId = 3,
                    Title = "One Month Master",
                    Description = "An incredible milestone - one month without smoking!",
                    Type = AchievementType.TimeBasedMilestone,
                    RequiredDays = 30,
                    Points = 200,
                    CreatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    AchievementId = 4,
                    Title = "Money Saver - $100",
                    Description = "You've saved your first $100 by not smoking!",
                    Type = AchievementType.MoneyBasedMilestone,
                    RequiredAmount = 100,
                    Points = 100,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed default admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    Email = "admin@smokingcessation.com",
                    PasswordHash = "$2a$11$v6JY5LO.D1qcY5rHW5qGCeZK8vq9U9O5KQZM6V5Q8Y9L6Y5L6Y5L6", // "admin123"
                    FirstName = "System",
                    LastName = "Administrator",
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}