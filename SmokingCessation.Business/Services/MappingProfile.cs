using AutoMapper;
using SmokingCessation.Business.DTOs;
using SmokingCessation.Data.Entities;

namespace SmokingCessation.Business.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
            CreateMap<UpdateProfileDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // QuitPlan mappings
            CreateMap<QuitPlan, QuitPlanDto>();
            CreateMap<CreateQuitPlanDto, QuitPlan>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Common.Enums.QuitPlanStatus.Draft));

            // QuitPlanMilestone mappings
            CreateMap<QuitPlanMilestone, QuitPlanMilestoneDto>();
            CreateMap<CreateQuitPlanMilestoneDto, QuitPlanMilestone>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // SmokeLog mappings
            CreateMap<SmokeLog, SmokeLogDto>();
            CreateMap<CreateSmokeLogDto, SmokeLog>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Achievement mappings
            CreateMap<Achievement, AchievementDto>()
                .ForMember(dest => dest.IsEarned, opt => opt.Ignore())
                .ForMember(dest => dest.EarnedAt, opt => opt.Ignore());

            CreateMap<UserAchievement, AchievementDto>()
                .ForMember(dest => dest.AchievementId, opt => opt.MapFrom(src => src.Achievement.AchievementId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Achievement.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Achievement.Description))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Achievement.Type))
                .ForMember(dest => dest.RequiredDays, opt => opt.MapFrom(src => src.Achievement.RequiredDays))
                .ForMember(dest => dest.RequiredAmount, opt => opt.MapFrom(src => src.Achievement.RequiredAmount))
                .ForMember(dest => dest.RequiredCount, opt => opt.MapFrom(src => src.Achievement.RequiredCount))
                .ForMember(dest => dest.IconUrl, opt => opt.MapFrom(src => src.Achievement.IconUrl))
                .ForMember(dest => dest.BadgeUrl, opt => opt.MapFrom(src => src.Achievement.BadgeUrl))
                .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Achievement.Points))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Achievement.IsActive))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Achievement.CreatedAt))
                .ForMember(dest => dest.IsEarned, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.EarnedAt, opt => opt.MapFrom(src => src.EarnedAt));

            // Message mappings
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}".Trim()))
                .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => $"{src.Receiver.FirstName} {src.Receiver.LastName}".Trim()));
            
            CreateMap<SendMessageDto, Message>()
                .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Common.Enums.MessageStatus.Sent));

            // Coach mappings
            CreateMap<Coach, CoachDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.CurrentMemberCount, opt => opt.MapFrom(src => src.AssignedMembers.Count));
            
            CreateMap<ApplyAsCoachDto, Coach>()
                .ForMember(dest => dest.AppliedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Common.Enums.CoachStatus.Pending))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => true));

            // Feedback mappings
            CreateMap<Feedback, FeedbackDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}".Trim()))
                .ForMember(dest => dest.ResolvedByUserName, opt => opt.MapFrom(src => src.ResolvedByUser != null ? $"{src.ResolvedByUser.FirstName} {src.ResolvedByUser.LastName}".Trim() : null));
            
            CreateMap<SubmitFeedbackDto, Feedback>()
                .ForMember(dest => dest.SubmittedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Common.Enums.FeedbackStatus.Submitted));

            // Template mappings
            CreateMap<Template, TemplateDto>();
            CreateMap<CreateTemplateDto, Template>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        }
    }

    // Additional DTOs for Template
    public class TemplateDto
    {
        public int TemplateId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Common.Enums.TemplateType Type { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateTemplateDto
    {
        public string Name { get; set; } = string.Empty;
        public Common.Enums.TemplateType Type { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}