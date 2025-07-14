using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmokingCessation.WPF.Views;
using SmokingCessation.WPF.ViewModels;
using SmokingCessation.WPF.Services;
using SmokingCessation.Business.Interfaces;
using SmokingCessation.Business.Services;
using SmokingCessation.Data.Context;
using SmokingCessation.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using AutoMapper;

namespace SmokingCessation.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
                .Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Database
            services.AddDbContext<SmokingCessationDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SmokingCessationDb;Trusted_Connection=true;MultipleActiveResultSets=true"));

            // Repository pattern
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Business services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuitPlanService, QuitPlanService>();
            services.AddScoped<ISmokeLogService, SmokeLogService>();
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<ICoachService, CoachService>();
            services.AddScoped<IFeedbackService, FeedbackService>();

            // Application services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<QuitPlanViewModel>();
            services.AddTransient<SmokeLogViewModel>();
            services.AddTransient<AchievementsViewModel>();
            services.AddTransient<MessagesViewModel>();
            services.AddTransient<CoachDashboardViewModel>();
            services.AddTransient<AdminDashboardViewModel>();

            // Views
            services.AddTransient<LoginWindow>();
            services.AddTransient<RegisterWindow>();
            services.AddTransient<MainWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            // Ensure database is created
            using (var scope = _host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SmokingCessationDbContext>();
                await context.Database.EnsureCreatedAsync();
            }

            var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
            loginWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }

        public static T GetService<T>() where T : class
        {
            return ((App)Current)._host.Services.GetRequiredService<T>();
        }
    }
}
