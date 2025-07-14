using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmokingCessation.Business.DTOs;
using SmokingCessation.Business.Interfaces;
using SmokingCessation.Data.Repositories;

namespace SmokingCessation.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(Common.Enums.UserRole role)
        {
            var users = await _unitOfWork.Users.GetUsersByRoleAsync(role);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateProfileAsync(int userId, UpdateProfileDto updateProfileDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return (false, "User not found");
                }

                _mapper.Map(updateProfileDto, user);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return new DashboardStatsDto();

            var daysSmokeFree = await _unitOfWork.SmokeLogs.GetDaysSmokeFreeAsync(userId);
            var moneySaved = await _unitOfWork.SmokeLogs.GetMoneySavedAsync(userId, 
                user.CostPerPack ?? 0, user.CigarettesPerPack ?? 20);

            var cigarettesAvoided = daysSmokeFree * (user.CigarettesPerDay ?? 0);
            var timeSaved = TimeSpan.FromMinutes(cigarettesAvoided * 5); // Assuming 5 minutes per cigarette

            var totalLogs = await _unitOfWork.SmokeLogs.CountAsync(sl => sl.UserId == userId);
            var achievements = await _unitOfWork.UserAchievements.CountAsync(ua => ua.UserId == userId);

            return new DashboardStatsDto
            {
                DaysSmokeFree = daysSmokeFree,
                MoneySaved = moneySaved,
                CigarettesAvoided = cigarettesAvoided,
                TimeSaved = timeSaved,
                TotalLogs = totalLogs,
                AchievementsEarned = achievements,
                CurrentStreak = daysSmokeFree, // Simplified for now
                LongestStreak = daysSmokeFree // Simplified for now
            };
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                user.IsActive = false;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null) return false;

                user.IsActive = true;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm)
        {
            var users = await _unitOfWork.Users.FindAsync(u => 
                u.Username.Contains(searchTerm) || 
                u.Email.Contains(searchTerm) ||
                (u.FirstName != null && u.FirstName.Contains(searchTerm)) ||
                (u.LastName != null && u.LastName.Contains(searchTerm)));
            
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserWithDetailsAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetUserWithAchievementsAsync(userId);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }
    }

    public class SmokeLogService : ISmokeLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SmokeLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SmokeLogDto>> GetUserSmokeLogsAsync(int userId)
        {
            var smokeLogs = await _unitOfWork.SmokeLogs.GetUserSmokeLogsAsync(userId);
            return _mapper.Map<IEnumerable<SmokeLogDto>>(smokeLogs);
        }

        public async Task<IEnumerable<SmokeLogDto>> GetUserSmokeLogsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var smokeLogs = await _unitOfWork.SmokeLogs.GetUserSmokeLogsByDateRangeAsync(userId, startDate, endDate);
            return _mapper.Map<IEnumerable<SmokeLogDto>>(smokeLogs);
        }

        public async Task<SmokeLogDto?> GetSmokeLogByIdAsync(int smokeLogId)
        {
            var smokeLog = await _unitOfWork.SmokeLogs.GetByIdAsync(smokeLogId);
            return smokeLog != null ? _mapper.Map<SmokeLogDto>(smokeLog) : null;
        }

        public async Task<(bool Success, int? SmokeLogId, string? ErrorMessage)> CreateSmokeLogAsync(int userId, CreateSmokeLogDto createSmokeLogDto)
        {
            try
            {
                var smokeLog = _mapper.Map<Data.Entities.SmokeLog>(createSmokeLogDto);
                smokeLog.UserId = userId;

                await _unitOfWork.SmokeLogs.AddAsync(smokeLog);
                await _unitOfWork.SaveChangesAsync();

                return (true, smokeLog.SmokeLogId, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateSmokeLogAsync(int smokeLogId, CreateSmokeLogDto updateSmokeLogDto)
        {
            try
            {
                var smokeLog = await _unitOfWork.SmokeLogs.GetByIdAsync(smokeLogId);
                if (smokeLog == null)
                {
                    return (false, "Smoke log not found");
                }

                _mapper.Map(updateSmokeLogDto, smokeLog);
                _unitOfWork.SmokeLogs.Update(smokeLog);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteSmokeLogAsync(int smokeLogId)
        {
            try
            {
                var smokeLog = await _unitOfWork.SmokeLogs.GetByIdAsync(smokeLogId);
                if (smokeLog == null)
                {
                    return (false, "Smoke log not found");
                }

                _unitOfWork.SmokeLogs.Remove(smokeLog);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<DashboardStatsDto> GetUserStatsAsync(int userId)
        {
            var userService = new UserService(_unitOfWork, _mapper);
            return await userService.GetDashboardStatsAsync(userId);
        }

        public async Task<ProgressChartDto> GetProgressChartDataAsync(int userId, int days = 30)
        {
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-days);

            var smokeLogs = await _unitOfWork.SmokeLogs.GetUserSmokeLogsByDateRangeAsync(userId, startDate, endDate);
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            var dailyProgress = new List<DailyProgressDto>();
            
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dayLogs = smokeLogs.Where(sl => sl.LogDate.Date == date).ToList();
                var cigarettesSmoked = dayLogs.Where(sl => sl.LogType == Common.Enums.SmokeLogType.Smoked)
                    .Sum(sl => sl.CigarettesSmoked ?? 0);
                var cravingsLogged = dayLogs.Count(sl => sl.LogType == Common.Enums.SmokeLogType.Craving);
                
                var wasSmokeFree = cigarettesSmoked == 0;
                var dailyMoneySaved = wasSmokeFree && user?.CostPerPack != null && user?.CigarettesPerPack != null
                    ? (decimal)(user.CigarettesPerDay ?? 0) / user.CigarettesPerPack.Value * user.CostPerPack.Value
                    : 0;

                dailyProgress.Add(new DailyProgressDto
                {
                    Date = date,
                    CigarettesSmoked = cigarettesSmoked,
                    CravingsLogged = cravingsLogged,
                    WasSmokeFree = wasSmokeFree,
                    DailyMoneySaved = dailyMoneySaved
                });
            }

            return new ProgressChartDto
            {
                DailyProgress = dailyProgress
            };
        }

        public async Task<IEnumerable<SmokeLogDto>> GetTodaySmokeLogsAsync(int userId)
        {
            var smokeLogs = await _unitOfWork.SmokeLogs.GetTodaySmokeLogsAsync(userId);
            return _mapper.Map<IEnumerable<SmokeLogDto>>(smokeLogs);
        }

        public async Task<int> GetStreakAsync(int userId)
        {
            return await _unitOfWork.SmokeLogs.GetDaysSmokeFreeAsync(userId);
        }
    }

    public class QuitPlanService : IQuitPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuitPlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuitPlanDto>> GetUserQuitPlansAsync(int userId)
        {
            var quitPlans = await _unitOfWork.QuitPlans.GetUserQuitPlansAsync(userId);
            return _mapper.Map<IEnumerable<QuitPlanDto>>(quitPlans);
        }

        public async Task<QuitPlanDto?> GetQuitPlanByIdAsync(int quitPlanId)
        {
            var quitPlan = await _unitOfWork.QuitPlans.GetQuitPlanWithMilestonesAsync(quitPlanId);
            return quitPlan != null ? _mapper.Map<QuitPlanDto>(quitPlan) : null;
        }

        public async Task<QuitPlanDto?> GetActiveQuitPlanAsync(int userId)
        {
            var quitPlan = await _unitOfWork.QuitPlans.GetActiveQuitPlanAsync(userId);
            return quitPlan != null ? _mapper.Map<QuitPlanDto>(quitPlan) : null;
        }

        public async Task<(bool Success, int? QuitPlanId, string? ErrorMessage)> CreateQuitPlanAsync(int userId, CreateQuitPlanDto createQuitPlanDto)
        {
            try
            {
                var quitPlan = _mapper.Map<Data.Entities.QuitPlan>(createQuitPlanDto);
                quitPlan.UserId = userId;

                await _unitOfWork.QuitPlans.AddAsync(quitPlan);
                await _unitOfWork.SaveChangesAsync();

                return (true, quitPlan.QuitPlanId, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateQuitPlanAsync(int quitPlanId, CreateQuitPlanDto updateQuitPlanDto)
        {
            try
            {
                var quitPlan = await _unitOfWork.QuitPlans.GetByIdAsync(quitPlanId);
                if (quitPlan == null)
                {
                    return (false, "Quit plan not found");
                }

                _mapper.Map(updateQuitPlanDto, quitPlan);
                quitPlan.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.QuitPlans.Update(quitPlan);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteQuitPlanAsync(int quitPlanId)
        {
            try
            {
                var quitPlan = await _unitOfWork.QuitPlans.GetByIdAsync(quitPlanId);
                if (quitPlan == null)
                {
                    return (false, "Quit plan not found");
                }

                _unitOfWork.QuitPlans.Remove(quitPlan);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> ActivateQuitPlanAsync(int quitPlanId)
        {
            try
            {
                var quitPlan = await _unitOfWork.QuitPlans.GetByIdAsync(quitPlanId);
                if (quitPlan == null)
                {
                    return (false, "Quit plan not found");
                }

                // Deactivate other active plans for this user
                var userPlans = await _unitOfWork.QuitPlans.GetUserQuitPlansAsync(quitPlan.UserId);
                foreach (var plan in userPlans.Where(p => p.Status == Common.Enums.QuitPlanStatus.Active))
                {
                    plan.Status = Common.Enums.QuitPlanStatus.Paused;
                    _unitOfWork.QuitPlans.Update(plan);
                }

                quitPlan.Status = Common.Enums.QuitPlanStatus.Active;
                quitPlan.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.QuitPlans.Update(quitPlan);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> CompleteQuitPlanAsync(int quitPlanId)
        {
            try
            {
                var quitPlan = await _unitOfWork.QuitPlans.GetByIdAsync(quitPlanId);
                if (quitPlan == null)
                {
                    return (false, "Quit plan not found");
                }

                quitPlan.Status = Common.Enums.QuitPlanStatus.Completed;
                quitPlan.EndDate = DateTime.UtcNow;
                quitPlan.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.QuitPlans.Update(quitPlan);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> CompleteMilestoneAsync(int milestoneId)
        {
            try
            {
                var milestone = await _unitOfWork.QuitPlans.Query()
                    .SelectMany(qp => qp.Milestones)
                    .FirstOrDefaultAsync(m => m.MilestoneId == milestoneId);

                if (milestone == null)
                {
                    return (false, "Milestone not found");
                }

                milestone.IsCompleted = true;
                milestone.CompletedAt = DateTime.UtcNow;
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<QuitPlanDto?> GetQuitPlanWithMilestonesAsync(int quitPlanId)
        {
            var quitPlan = await _unitOfWork.QuitPlans.GetQuitPlanWithMilestonesAsync(quitPlanId);
            return quitPlan != null ? _mapper.Map<QuitPlanDto>(quitPlan) : null;
        }
    }

    public class AchievementService : IAchievementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AchievementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AchievementDto>> GetUserAchievementsAsync(int userId)
        {
            var userAchievements = await _unitOfWork.UserAchievements.GetUserAchievementsAsync(userId);
            return _mapper.Map<IEnumerable<AchievementDto>>(userAchievements);
        }

        public async Task<IEnumerable<AchievementDto>> GetAvailableAchievementsAsync(int userId)
        {
            var allAchievements = await _unitOfWork.Achievements.GetActiveAchievementsAsync();
            var userAchievements = await _unitOfWork.UserAchievements.GetUserAchievementsAsync(userId);
            var earnedIds = userAchievements.Select(ua => ua.AchievementId).ToHashSet();

            var availableAchievements = allAchievements.Where(a => !earnedIds.Contains(a.AchievementId));
            var result = _mapper.Map<IEnumerable<AchievementDto>>(availableAchievements);
            
            foreach (var achievement in result)
            {
                achievement.IsEarned = false;
            }

            return result;
        }

        public async Task<IEnumerable<AchievementDto>> GetUnnotifiedAchievementsAsync(int userId)
        {
            var userAchievements = await _unitOfWork.UserAchievements.GetUnnotifiedAchievementsAsync(userId);
            return _mapper.Map<IEnumerable<AchievementDto>>(userAchievements);
        }

        public async Task MarkAchievementsAsNotifiedAsync(int userId, IEnumerable<int> achievementIds)
        {
            var userAchievements = await _unitOfWork.UserAchievements.GetUserAchievementsAsync(userId);
            var toUpdate = userAchievements.Where(ua => achievementIds.Contains(ua.AchievementId));

            foreach (var achievement in toUpdate)
            {
                achievement.IsNotified = true;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CheckAndAwardAchievementsAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return;

            var userStats = new UserService(_unitOfWork, _mapper);
            var stats = await userStats.GetDashboardStatsAsync(userId);

            var allAchievements = await _unitOfWork.Achievements.GetActiveAchievementsAsync();
            var userAchievements = await _unitOfWork.UserAchievements.GetUserAchievementsAsync(userId);
            var earnedIds = userAchievements.Select(ua => ua.AchievementId).ToHashSet();

            var newAchievements = new List<Data.Entities.UserAchievement>();

            foreach (var achievement in allAchievements.Where(a => !earnedIds.Contains(a.AchievementId)))
            {
                bool shouldEarn = achievement.Type switch
                {
                    Common.Enums.AchievementType.TimeBasedMilestone => 
                        achievement.RequiredDays.HasValue && stats.DaysSmokeFree >= achievement.RequiredDays.Value,
                    Common.Enums.AchievementType.MoneyBasedMilestone => 
                        achievement.RequiredAmount.HasValue && stats.MoneySaved >= achievement.RequiredAmount.Value,
                    _ => false
                };

                if (shouldEarn)
                {
                    newAchievements.Add(new Data.Entities.UserAchievement
                    {
                        UserId = userId,
                        AchievementId = achievement.AchievementId,
                        EarnedAt = DateTime.UtcNow,
                        IsNotified = false
                    });
                }
            }

            if (newAchievements.Any())
            {
                await _unitOfWork.UserAchievements.AddRangeAsync(newAchievements);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AchievementDto>> GetAllAchievementsAsync()
        {
            var achievements = await _unitOfWork.Achievements.GetActiveAchievementsAsync();
            return _mapper.Map<IEnumerable<AchievementDto>>(achievements);
        }

        public async Task<(bool Success, string? ErrorMessage)> CreateAchievementAsync(AchievementDto achievementDto)
        {
            try
            {
                var achievement = _mapper.Map<Data.Entities.Achievement>(achievementDto);
                await _unitOfWork.Achievements.AddAsync(achievement);
                await _unitOfWork.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateAchievementAsync(int achievementId, AchievementDto achievementDto)
        {
            try
            {
                var achievement = await _unitOfWork.Achievements.GetByIdAsync(achievementId);
                if (achievement == null)
                {
                    return (false, "Achievement not found");
                }

                _mapper.Map(achievementDto, achievement);
                _unitOfWork.Achievements.Update(achievement);
                await _unitOfWork.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteAchievementAsync(int achievementId)
        {
            try
            {
                var achievement = await _unitOfWork.Achievements.GetByIdAsync(achievementId);
                if (achievement == null)
                {
                    return (false, "Achievement not found");
                }

                achievement.IsActive = false;
                _unitOfWork.Achievements.Update(achievement);
                await _unitOfWork.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }

    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageDto>> GetUserMessagesAsync(int userId)
        {
            var messages = await _unitOfWork.Messages.GetUserMessagesAsync(userId);
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<IEnumerable<MessageDto>> GetConversationAsync(int senderId, int receiverId)
        {
            var messages = await _unitOfWork.Messages.GetConversationAsync(senderId, receiverId);
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<MessageDto?> GetMessageByIdAsync(int messageId)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(messageId);
            return message != null ? _mapper.Map<MessageDto>(message) : null;
        }

        public async Task<(bool Success, int? MessageId, string? ErrorMessage)> SendMessageAsync(int senderId, SendMessageDto sendMessageDto)
        {
            try
            {
                var message = _mapper.Map<Data.Entities.Message>(sendMessageDto);
                message.SenderId = senderId;

                await _unitOfWork.Messages.AddAsync(message);
                await _unitOfWork.SaveChangesAsync();

                return (true, message.MessageId, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> MarkAsReadAsync(int messageId, int userId)
        {
            try
            {
                var message = await _unitOfWork.Messages.GetByIdAsync(messageId);
                if (message == null || message.ReceiverId != userId)
                {
                    return (false, "Message not found or access denied");
                }

                await _unitOfWork.Messages.MarkAsReadAsync(messageId);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteMessageAsync(int messageId, int userId)
        {
            try
            {
                var message = await _unitOfWork.Messages.GetByIdAsync(messageId);
                if (message == null || (message.SenderId != userId && message.ReceiverId != userId))
                {
                    return (false, "Message not found or access denied");
                }

                message.IsDeleted = true;
                _unitOfWork.Messages.Update(message);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<int> GetUnreadMessageCountAsync(int userId)
        {
            return await _unitOfWork.Messages.GetUnreadMessageCountAsync(userId);
        }

        public async Task<IEnumerable<MessageDto>> GetUnreadMessagesAsync(int userId)
        {
            var messages = await _unitOfWork.Messages.GetUnreadMessagesAsync(userId);
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }
    }

    public class CoachService : ICoachService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CoachService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CoachDto>> GetAvailableCoachesAsync()
        {
            var coaches = await _unitOfWork.Coaches.GetAvailableCoachesAsync();
            return _mapper.Map<IEnumerable<CoachDto>>(coaches);
        }

        public async Task<CoachDto?> GetCoachByUserIdAsync(int userId)
        {
            var coach = await _unitOfWork.Coaches.GetCoachByUserIdAsync(userId);
            return coach != null ? _mapper.Map<CoachDto>(coach) : null;
        }

        public async Task<CoachDto?> GetCoachByIdAsync(int coachId)
        {
            var coach = await _unitOfWork.Coaches.GetCoachWithMembersAsync(coachId);
            return coach != null ? _mapper.Map<CoachDto>(coach) : null;
        }

        public async Task<IEnumerable<CoachDto>> GetCoachesByStatusAsync(Common.Enums.CoachStatus status)
        {
            var coaches = await _unitOfWork.Coaches.GetCoachesByStatusAsync(status);
            return _mapper.Map<IEnumerable<CoachDto>>(coaches);
        }

        public async Task<(bool Success, string? ErrorMessage)> ApplyAsCoachAsync(int userId, ApplyAsCoachDto applyAsCoachDto)
        {
            try
            {
                var existingCoach = await _unitOfWork.Coaches.GetCoachByUserIdAsync(userId);
                if (existingCoach != null)
                {
                    return (false, "User has already applied as a coach");
                }

                var coach = _mapper.Map<Data.Entities.Coach>(applyAsCoachDto);
                coach.UserId = userId;

                await _unitOfWork.Coaches.AddAsync(coach);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> ApproveCoachAsync(int coachId, int approvedByUserId)
        {
            try
            {
                var coach = await _unitOfWork.Coaches.GetByIdAsync(coachId);
                if (coach == null)
                {
                    return (false, "Coach not found");
                }

                coach.Status = Common.Enums.CoachStatus.Approved;
                coach.ApprovedAt = DateTime.UtcNow;
                coach.ApprovedByUserId = approvedByUserId;
                _unitOfWork.Coaches.Update(coach);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> RejectCoachAsync(int coachId, int rejectedByUserId)
        {
            try
            {
                var coach = await _unitOfWork.Coaches.GetByIdAsync(coachId);
                if (coach == null)
                {
                    return (false, "Coach not found");
                }

                coach.Status = Common.Enums.CoachStatus.Rejected;
                coach.ApprovedByUserId = rejectedByUserId;
                _unitOfWork.Coaches.Update(coach);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> SuspendCoachAsync(int coachId, int suspendedByUserId)
        {
            try
            {
                var coach = await _unitOfWork.Coaches.GetByIdAsync(coachId);
                if (coach == null)
                {
                    return (false, "Coach not found");
                }

                coach.Status = Common.Enums.CoachStatus.Suspended;
                coach.IsAvailable = false;
                _unitOfWork.Coaches.Update(coach);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> AssignMemberToCoachAsync(int memberId, int coachId)
        {
            try
            {
                var coach = await _unitOfWork.Coaches.GetCoachWithMembersAsync(coachId);
                if (coach == null)
                {
                    return (false, "Coach not found");
                }

                if (coach.AssignedMembers.Count >= coach.MaxMembers)
                {
                    return (false, "Coach has reached maximum member capacity");
                }

                var member = await _unitOfWork.Users.GetByIdAsync(memberId);
                if (member == null)
                {
                    return (false, "Member not found");
                }

                coach.AssignedMembers.Add(member);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> RemoveMemberFromCoachAsync(int memberId)
        {
            try
            {
                var member = await _unitOfWork.Users.GetByIdAsync(memberId);
                if (member == null)
                {
                    return (false, "Member not found");
                }

                // Logic to remove member from coach would depend on your relationship setup
                // This is a simplified implementation
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<UserDto>> GetCoachMembersAsync(int coachId)
        {
            var coach = await _unitOfWork.Coaches.GetCoachWithMembersAsync(coachId);
            if (coach == null)
                return Enumerable.Empty<UserDto>();

            return _mapper.Map<IEnumerable<UserDto>>(coach.AssignedMembers);
        }
    }

    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FeedbackDto>> GetUserFeedbackAsync(int userId)
        {
            var feedback = await _unitOfWork.Feedbacks.GetUserFeedbackAsync(userId);
            return _mapper.Map<IEnumerable<FeedbackDto>>(feedback);
        }

        public async Task<IEnumerable<FeedbackDto>> GetPendingFeedbackAsync()
        {
            var feedback = await _unitOfWork.Feedbacks.GetPendingFeedbackAsync();
            return _mapper.Map<IEnumerable<FeedbackDto>>(feedback);
        }

        public async Task<FeedbackDto?> GetFeedbackByIdAsync(int feedbackId)
        {
            var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(feedbackId);
            return feedback != null ? _mapper.Map<FeedbackDto>(feedback) : null;
        }

        public async Task<(bool Success, int? FeedbackId, string? ErrorMessage)> SubmitFeedbackAsync(int userId, SubmitFeedbackDto submitFeedbackDto)
        {
            try
            {
                var feedback = _mapper.Map<Data.Entities.Feedback>(submitFeedbackDto);
                feedback.UserId = userId;

                await _unitOfWork.Feedbacks.AddAsync(feedback);
                await _unitOfWork.SaveChangesAsync();

                return (true, feedback.FeedbackId, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> ResolveFeedbackAsync(int feedbackId, int resolvedByUserId, string response)
        {
            try
            {
                var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(feedbackId);
                if (feedback == null)
                {
                    return (false, "Feedback not found");
                }

                feedback.Status = Common.Enums.FeedbackStatus.Resolved;
                feedback.ResolvedAt = DateTime.UtcNow;
                feedback.ResolvedByUserId = resolvedByUserId;
                feedback.Response = response;
                _unitOfWork.Feedbacks.Update(feedback);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> CloseFeedbackAsync(int feedbackId)
        {
            try
            {
                var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(feedbackId);
                if (feedback == null)
                {
                    return (false, "Feedback not found");
                }

                feedback.Status = Common.Enums.FeedbackStatus.Closed;
                _unitOfWork.Feedbacks.Update(feedback);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<IEnumerable<FeedbackDto>> GetFeedbackByStatusAsync(Common.Enums.FeedbackStatus status)
        {
            var feedback = await _unitOfWork.Feedbacks.GetFeedbackByStatusAsync(status);
            return _mapper.Map<IEnumerable<FeedbackDto>>(feedback);
        }
    }
}