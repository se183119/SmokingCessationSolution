namespace SmokingCessation.Common.Constants
{
    public static class ApplicationConstants
    {
        public const string DefaultConnectionString = "SmokingCessationConnection";
        public const string DefaultCulture = "en-US";
        
        // User-related constants
        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 100;
        public const int MaxUsernameLength = 50;
        public const int MaxEmailLength = 256;
        
        // Business logic constants
        public const int MaxDailySmokes = 100;
        public const int MaxQuitPlansPerUser = 10;
        public const int MaxMessagesPerDay = 50;
        
        // File upload constants
        public const int MaxFileSize = 5 * 1024 * 1024; // 5MB
        public const string AllowedImageTypes = ".jpg,.jpeg,.png,.gif";
        
        // Session and security
        public const int SessionTimeoutMinutes = 30;
        public const int MaxLoginAttempts = 5;
        public const int LockoutDurationMinutes = 15;
        
        // Pagination
        public const int DefaultPageSize = 20;
        public const int MaxPageSize = 100;
        
        // Cache durations (in minutes)
        public const int UserCacheDuration = 60;
        public const int StatsCacheDuration = 30;
        public const int AchievementsCacheDuration = 120;
    }

    public static class MessageConstants
    {
        public const string LoginSuccess = "Login successful";
        public const string LoginFailed = "Invalid email or password";
        public const string RegistrationSuccess = "Registration successful";
        public const string RegistrationFailed = "Registration failed";
        public const string PasswordChanged = "Password changed successfully";
        public const string ProfileUpdated = "Profile updated successfully";
        public const string QuitPlanCreated = "Quit plan created successfully";
        public const string QuitPlanUpdated = "Quit plan updated successfully";
        public const string SmokeLogged = "Smoke entry logged successfully";
        public const string MessageSent = "Message sent successfully";
        public const string AchievementUnlocked = "Congratulations! Achievement unlocked";
        public const string CoachAssigned = "Coach has been assigned to you";
        public const string FeedbackSubmitted = "Feedback submitted successfully";
        
        // Error messages
        public const string UnauthorizedAccess = "You are not authorized to access this resource";
        public const string ValidationError = "Please check the entered data";
        public const string ServerError = "An error occurred. Please try again later";
        public const string NotFound = "Requested resource not found";
        public const string DuplicateEntry = "This entry already exists";
    }

    public static class RouteConstants
    {
        // Authentication routes
        public const string Login = "/auth/login";
        public const string Register = "/auth/register";
        public const string Logout = "/auth/logout";
        
        // Member routes
        public const string Dashboard = "/member/dashboard";
        public const string Profile = "/member/profile";
        public const string QuitPlans = "/member/quit-plans";
        public const string SmokeLog = "/member/smoke-log";
        public const string Achievements = "/member/achievements";
        public const string Messages = "/member/messages";
        
        // Coach routes
        public const string CoachDashboard = "/coach/dashboard";
        public const string MemberManagement = "/coach/members";
        public const string CoachMessages = "/coach/messages";
        
        // Admin routes
        public const string AdminDashboard = "/admin/dashboard";
        public const string UserManagement = "/admin/users";
        public const string CoachApproval = "/admin/coach-approval";
        public const string Templates = "/admin/templates";
        public const string FeedbackManagement = "/admin/feedback";
    }
}