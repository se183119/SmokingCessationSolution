using WPF.Configuration;
using WPF.ViewModels;
using WPF.Services;
using DataAccessObjects;

namespace WPF;

public class ApplicationManager
{
    private readonly ServiceContainer _serviceContainer;

    public ApplicationManager()
    {
        _serviceContainer = ServiceConfiguration.ConfigureServices();
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Initialize database and seed data
            using var context = _serviceContainer.GetService<SmokingWPFContext>();
            await Task.Run(() => DataSeeder.SeedData(context));
            
            Console.WriteLine("Application initialized successfully!");
            Console.WriteLine("Database seeded with sample data.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing application: {ex.Message}");
            throw;
        }
    }

    public T GetService<T>() where T : class
    {
        return _serviceContainer.GetService<T>();
    }

    public async Task RunDemoAsync()
    {
        Console.WriteLine("\n=== Smoking Cessation Support Platform Demo ===\n");

        // Demo authentication
        await DemoAuthenticationAsync();
        
        // Demo dashboard functionality
        await DemoDashboardAsync();
        
        // Demo progress tracking
        await DemoProgressTrackingAsync();
    }

    private async Task DemoAuthenticationAsync()
    {
        Console.WriteLine("1. Authentication Demo");
        Console.WriteLine("=====================");

        var loginViewModel = GetService<LoginViewModel>();
        
        // Simulate login attempt
        loginViewModel.Username = "member1";
        loginViewModel.Password = "member123";
        
        Console.WriteLine($"Attempting login with username: {loginViewModel.Username}");
        
        try
        {
            // Simulate command execution - fix the await issue
            var loginTask = Task.Run(() => {
                // Simulate login logic here since Execute is void
                var authService = _serviceContainer.GetService<IAuthenticationService>();
                return authService.AuthenticateAsync(loginViewModel.Username, loginViewModel.Password);
            });
            
            var user = await loginTask;
            if (user != null)
            {
                Console.WriteLine("Login successful!");
            }
            else
            {
                Console.WriteLine("Login failed - invalid credentials");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login failed: {ex.Message}");
        }
        
        Console.WriteLine();
    }

    private async Task DemoDashboardAsync()
    {
        Console.WriteLine("2. Dashboard Demo");
        Console.WriteLine("=================");

        var dashboardViewModel = GetService<MemberDashboardViewModel>();
        
        try
        {
            await dashboardViewModel.InitializeAsync();
            
            var currentUser = dashboardViewModel.CurrentUser;
            if (currentUser != null)
            {
                Console.WriteLine($"Welcome, {currentUser.FirstName} {currentUser.LastName}!");
                Console.WriteLine($"Role: {currentUser.Role}");
                Console.WriteLine($"Member since: {currentUser.CreatedDate:yyyy-MM-dd}");
            }

            var dashboardData = dashboardViewModel.DashboardData;
            if (dashboardData != null)
            {
                Console.WriteLine($"Days smoke-free: {dashboardData.DaysSmokeFree}");
                Console.WriteLine($"Money saved: ${dashboardData.MoneySaved:F2}");
                Console.WriteLine($"Cigarettes not smoked: {dashboardData.CigarettesNotSmoked}");
                Console.WriteLine($"Total achievements: {dashboardData.TotalAchievements}");
                Console.WriteLine($"Total points: {dashboardData.TotalPoints}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Dashboard error: {ex.Message}");
        }
        
        Console.WriteLine();
    }

    private async Task DemoProgressTrackingAsync()
    {
        Console.WriteLine("3. Progress Tracking Demo");
        Console.WriteLine("=========================");

        var progressViewModel = GetService<ProgressTrackingViewModel>();
        
        try
        {
            await progressViewModel.InitializeAsync();
            
            // Simulate logging today's progress
            progressViewModel.TodayProgress.CigarettesSmoked = 0;
            progressViewModel.TodayProgress.MoodLevel = "Good";
            progressViewModel.TodayProgress.EnergyLevel = "High";
            progressViewModel.TodayProgress.SleepQuality = "Good";
            progressViewModel.TodayProgress.CravingsCount = 2;
            progressViewModel.TodayProgress.Notes = "Feeling strong today, avoided smoking triggers";
            
            Console.WriteLine("Logging today's progress:");
            Console.WriteLine($"Cigarettes smoked: {progressViewModel.TodayProgress.CigarettesSmoked}");
            Console.WriteLine($"Mood level: {progressViewModel.TodayProgress.MoodLevel}");
            Console.WriteLine($"Energy level: {progressViewModel.TodayProgress.EnergyLevel}");
            Console.WriteLine($"Sleep quality: {progressViewModel.TodayProgress.SleepQuality}");
            Console.WriteLine($"Cravings count: {progressViewModel.TodayProgress.CravingsCount}");
            Console.WriteLine($"Notes: {progressViewModel.TodayProgress.Notes}");
            
            // Simulate saving progress - fix the await issue
            var saveTask = Task.Run(async () => {
                var progressService = _serviceContainer.GetService<IProgressService>();
                var authService = _serviceContainer.GetService<IAuthenticationService>();
                var currentUser = await authService.GetCurrentUserAsync();
                if (currentUser != null)
                {
                    progressViewModel.TodayProgress.UserId = currentUser.Id;
                    await progressService.LogProgressAsync(progressViewModel.TodayProgress);
                }
            });
            
            await saveTask;
            Console.WriteLine("Progress saved successfully!");

            if (progressViewModel.RecentProgress.Any())
            {
                Console.WriteLine($"\nRecent progress entries: {progressViewModel.RecentProgress.Count}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Progress tracking error: {ex.Message}");
        }
        
        Console.WriteLine();
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var app = new ApplicationManager();
            
            await app.InitializeAsync();
            await app.RunDemoAsync();
            
            Console.WriteLine("Demo completed successfully!");
            Console.WriteLine("\nThis demonstrates the core functionality of the Smoking Cessation Support Platform:");
            Console.WriteLine("- Complete business object model with EF Core");
            Console.WriteLine("- Repository pattern with Unit of Work");
            Console.WriteLine("- Service layer with business logic");
            Console.WriteLine("- MVVM ViewModels ready for WPF binding");
            Console.WriteLine("- Role-based authentication system");
            Console.WriteLine("- Progress tracking and achievement system");
            Console.WriteLine("- Database seeding with sample data");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Application error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
}