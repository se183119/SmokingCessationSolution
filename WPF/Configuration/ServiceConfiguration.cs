using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
using Repositories;
using WPF.Services;
using WPF.ViewModels;

namespace WPF.Configuration;

public class ServiceContainer
{
    private readonly Dictionary<Type, object> _services = new();
    private readonly Dictionary<Type, Func<object>> _serviceFactories = new();

    public void RegisterSingleton<T>(T instance) where T : class
    {
        _services[typeof(T)] = instance;
    }

    public void RegisterTransient<T>(Func<T> factory) where T : class
    {
        _serviceFactories[typeof(T)] = () => factory();
    }

    public void RegisterSingleton<TInterface, TImplementation>(Func<TImplementation> factory)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        var instance = factory();
        _services[typeof(TInterface)] = instance;
    }

    public T GetService<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }

        if (_serviceFactories.TryGetValue(typeof(T), out var factory))
        {
            return (T)factory();
        }

        // Try to create instance
        return CreateInstance<T>();
    }

    private T CreateInstance<T>() where T : class
    {
        var type = typeof(T);
        var constructors = type.GetConstructors();
        var constructor = constructors.OrderByDescending(c => c.GetParameters().Length).First();
        
        var parameters = constructor.GetParameters();
        var args = new object[parameters.Length];
        
        for (int i = 0; i < parameters.Length; i++)
        {
            args[i] = GetService(parameters[i].ParameterType);
        }
        
        return (T)Activator.CreateInstance(type, args)!;
    }

    private object GetService(Type type)
    {
        if (_services.TryGetValue(type, out var service))
        {
            return service;
        }

        if (_serviceFactories.TryGetValue(type, out var factory))
        {
            return factory();
        }

        throw new InvalidOperationException($"Service of type {type.Name} is not registered.");
    }
}

public static class ServiceConfiguration
{
    public static ServiceContainer ConfigureServices()
    {
        var container = new ServiceContainer();

        // Database Context
        var connectionString = "Data Source=SmokingWPF.db";
        var options = new DbContextOptionsBuilder<SmokingWPFContext>()
            .UseSqlite(connectionString)
            .Options;

        container.RegisterTransient(() => new SmokingWPFContext(options));

        // Unit of Work
        container.RegisterTransient<IUnitOfWork>(() => 
            new UnitOfWork(container.GetService<SmokingWPFContext>()));

        // Services
        container.RegisterSingleton<IAuthenticationService, AuthenticationService>(() => 
            new AuthenticationService(container.GetService<IUnitOfWork>()));
        container.RegisterSingleton<INavigationService, NavigationService>(() => 
            new NavigationService());
        container.RegisterTransient<IUserService>(() => 
            new UserService(container.GetService<IUnitOfWork>()));
        container.RegisterTransient<ISmokingStatusService>(() => 
            new SmokingStatusService(container.GetService<IUnitOfWork>()));
        container.RegisterTransient<IProgressService>(() => 
            new ProgressService(container.GetService<IUnitOfWork>()));
        container.RegisterTransient<IAchievementService>(() => 
            new AchievementService(container.GetService<IUnitOfWork>()));
        container.RegisterTransient<IQuitPlanService>(() => 
            new QuitPlanService(container.GetService<IUnitOfWork>()));

        // ViewModels
        container.RegisterTransient(() => new LoginViewModel(
            container.GetService<IAuthenticationService>(),
            container.GetService<INavigationService>()));
        container.RegisterTransient(() => new RegisterViewModel(
            container.GetService<IUserService>(),
            container.GetService<INavigationService>(),
            container.GetService<IAuthenticationService>()));
        container.RegisterTransient(() => new DashboardViewModel(
            container.GetService<IUserService>(),
            container.GetService<IProgressService>(),
            container.GetService<IAuthenticationService>()));
        container.RegisterTransient(() => new MemberDashboardViewModel(
            container.GetService<IUserService>(),
            container.GetService<IProgressService>(),
            container.GetService<IAuthenticationService>(),
            container.GetService<ISmokingStatusService>(),
            container.GetService<IQuitPlanService>(),
            container.GetService<IAchievementService>()));
        container.RegisterTransient(() => new ProgressTrackingViewModel(
            container.GetService<IProgressService>(),
            container.GetService<IAuthenticationService>(),
            container.GetService<IAchievementService>()));

        return container;
    }
}

public class NavigationService : INavigationService
{
    private readonly Stack<string> _navigationHistory = new();
    private string? _currentView;

    public bool CanGoBack => _navigationHistory.Count > 0;

    public void NavigateTo(string viewName)
    {
        NavigateTo(viewName, null);
    }

    public void NavigateTo(string viewName, object? parameter)
    {
        if (!string.IsNullOrEmpty(_currentView))
        {
            _navigationHistory.Push(_currentView);
        }
        
        _currentView = viewName;
        
        // In a real WPF application, this would trigger navigation
        Console.WriteLine($"Navigating to: {viewName}");
        if (parameter != null)
        {
            Console.WriteLine($"With parameter: {parameter}");
        }
    }

    public void GoBack()
    {
        if (CanGoBack)
        {
            _currentView = _navigationHistory.Pop();
            Console.WriteLine($"Navigating back to: {_currentView}");
        }
    }
}