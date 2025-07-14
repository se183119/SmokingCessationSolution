namespace SmokingCessation.Common.Enums
{
    public enum UserRole
    {
        Member = 1,
        Coach = 2,
        Admin = 3
    }

    public enum QuitPlanStatus
    {
        Draft = 1,
        Active = 2,
        Completed = 3,
        Paused = 4,
        Cancelled = 5
    }

    public enum MessageStatus
    {
        Sent = 1,
        Delivered = 2,
        Read = 3
    }

    public enum AchievementType
    {
        TimeBasedMilestone = 1,
        MoneyBasedMilestone = 2,
        HealthBasedMilestone = 3,
        BehaviorBasedMilestone = 4
    }

    public enum CoachStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Suspended = 4
    }

    public enum SmokeLogType
    {
        Smoked = 1,
        Craving = 2,
        Alternative = 3
    }

    public enum FeedbackStatus
    {
        Submitted = 1,
        InReview = 2,
        Resolved = 3,
        Closed = 4
    }

    public enum TemplateType
    {
        QuitPlan = 1,
        Message = 2,
        Achievement = 3,
        Feedback = 4
    }
}