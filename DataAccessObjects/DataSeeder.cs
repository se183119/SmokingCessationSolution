using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects;

public static class DataSeeder
{
    public static void SeedData(SmokingWPFContext context)
    {
        context.Database.EnsureCreated();

        // Seed Users if none exist
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    Email = "admin@smokingcessation.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    FirstName = "System",
                    LastName = "Administrator",
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new User
                {
                    Username = "coach1",
                    Email = "coach1@smokingcessation.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("coach123"),
                    FirstName = "Sarah",
                    LastName = "Johnson",
                    Role = UserRole.Coach,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new User
                {
                    Username = "member1",
                    Email = "member1@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("member123"),
                    FirstName = "John",
                    LastName = "Doe",
                    Role = UserRole.Member,
                    IsActive = true,
                    DateOfBirth = new DateTime(1985, 5, 15),
                    Gender = "Male",
                    CreatedDate = DateTime.UtcNow
                },
                new User
                {
                    Username = "member2",
                    Email = "member2@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("member123"),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Role = UserRole.Member,
                    IsActive = true,
                    DateOfBirth = new DateTime(1990, 3, 22),
                    Gender = "Female",
                    CreatedDate = DateTime.UtcNow
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        // Seed Membership Packages if none exist
        if (!context.MembershipPackages.Any())
        {
            var packages = new List<MembershipPackage>
            {
                new MembershipPackage
                {
                    Name = "Basic Plan",
                    Description = "Essential smoking cessation tools and tracking",
                    Price = 9.99m,
                    DurationInMonths = 1,
                    Features = "Daily tracking, Basic progress reports, Community access",
                    IsActive = true,
                    SortOrder = 1
                },
                new MembershipPackage
                {
                    Name = "Premium Plan",
                    Description = "Enhanced support with coaching",
                    Price = 19.99m,
                    DurationInMonths = 1,
                    Features = "All Basic features, Coach consultations, Advanced analytics, Personalized plans",
                    IsActive = true,
                    SortOrder = 2
                },
                new MembershipPackage
                {
                    Name = "Annual Plan",
                    Description = "Complete yearly support program",
                    Price = 199.99m,
                    DurationInMonths = 12,
                    Features = "All Premium features, Priority support, Exclusive content, Money-back guarantee",
                    IsActive = true,
                    SortOrder = 3
                }
            };

            context.MembershipPackages.AddRange(packages);
            context.SaveChanges();
        }

        // Seed some sample smoking status data
        if (!context.SmokingStatuses.Any())
        {
            var member1 = context.Users.FirstOrDefault(u => u.Username == "member1");
            var member2 = context.Users.FirstOrDefault(u => u.Username == "member2");

            if (member1 != null && member2 != null)
            {
                var smokingStatuses = new List<SmokingStatus>
                {
                    new SmokingStatus
                    {
                        UserId = member1.Id,
                        CigarettesPerDay = 20,
                        CostPerPack = 12.50m,
                        CigarettesPerPack = 20,
                        SmokingFrequency = "Daily",
                        YearsSmoked = 10,
                        PreferredBrand = "Marlboro",
                        Notes = "Smoking heavily, especially during stress"
                    },
                    new SmokingStatus
                    {
                        UserId = member2.Id,
                        CigarettesPerDay = 10,
                        CostPerPack = 10.00m,
                        CigarettesPerPack = 20,
                        SmokingFrequency = "Daily",
                        YearsSmoked = 5,
                        PreferredBrand = "Camel",
                        Notes = "Social smoker, mainly weekends"
                    }
                };

                context.SmokingStatuses.AddRange(smokingStatuses);
                context.SaveChanges();
            }
        }

        // Seed some sample quit plans
        if (!context.QuitPlans.Any())
        {
            var member1 = context.Users.FirstOrDefault(u => u.Username == "member1");
            if (member1 != null)
            {
                var quitPlan = new QuitPlan
                {
                    UserId = member1.Id,
                    Title = "30-Day Gradual Reduction Plan",
                    Description = "Gradually reduce cigarettes over 30 days",
                    QuitDate = DateTime.UtcNow.AddDays(30),
                    PlanType = "Gradual",
                    Motivations = "Health concerns, save money, family pressure",
                    Triggers = "Stress, coffee, alcohol, driving",
                    CopingStrategies = "Exercise, meditation, chewing gum, avoid triggers"
                };

                context.QuitPlans.Add(quitPlan);
                context.SaveChanges();

                // Add milestones for the quit plan
                var milestones = new List<QuitPlanMilestone>
                {
                    new QuitPlanMilestone
                    {
                        QuitPlanId = quitPlan.Id,
                        Title = "Reduce to 15 cigarettes/day",
                        Description = "Cut down from 20 to 15 cigarettes per day",
                        DaysFromQuit = -20,
                        Reward = "Buy a new book"
                    },
                    new QuitPlanMilestone
                    {
                        QuitPlanId = quitPlan.Id,
                        Title = "Reduce to 10 cigarettes/day",
                        Description = "Cut down from 15 to 10 cigarettes per day",
                        DaysFromQuit = -10,
                        Reward = "Movie night out"
                    },
                    new QuitPlanMilestone
                    {
                        QuitPlanId = quitPlan.Id,
                        Title = "Quit Day",
                        Description = "Complete cessation of smoking",
                        DaysFromQuit = 0,
                        Reward = "Celebrate with family dinner"
                    },
                    new QuitPlanMilestone
                    {
                        QuitPlanId = quitPlan.Id,
                        Title = "One Week Smoke-Free",
                        Description = "Successfully completed one week without smoking",
                        DaysFromQuit = 7,
                        Reward = "Buy new clothes with money saved"
                    },
                    new QuitPlanMilestone
                    {
                        QuitPlanId = quitPlan.Id,
                        Title = "One Month Smoke-Free",
                        Description = "Successfully completed one month without smoking",
                        DaysFromQuit = 30,
                        Reward = "Weekend getaway"
                    }
                };

                context.QuitPlanMilestones.AddRange(milestones);
                context.SaveChanges();
            }
        }

        // Seed some notifications
        if (!context.Notifications.Any())
        {
            var member1 = context.Users.FirstOrDefault(u => u.Username == "member1");
            var member2 = context.Users.FirstOrDefault(u => u.Username == "member2");

            if (member1 != null && member2 != null)
            {
                var notifications = new List<Notification>
                {
                    new Notification
                    {
                        UserId = member1.Id,
                        Title = "Welcome to Smoking Cessation!",
                        Message = "Congratulations on taking the first step towards a smoke-free life!",
                        Type = "System"
                    },
                    new Notification
                    {
                        UserId = member2.Id,
                        Title = "Daily Motivation",
                        Message = "Remember: Every cigarette you don't smoke is a victory!",
                        Type = "Motivational"
                    }
                };

                context.Notifications.AddRange(notifications);
                context.SaveChanges();
            }
        }
    }
}