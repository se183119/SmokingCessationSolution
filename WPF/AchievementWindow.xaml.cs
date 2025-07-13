using System.Windows;
using System.Windows.Input;

namespace WPF
{
    public partial class AchievementWindow : Window
    {
        public AchievementWindow()
        {
            InitializeComponent();
        }

        private void Achievement_Click(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                var achievementTag = border.Tag.ToString();
                ShowAchievementDetails(achievementTag);
            }
        }

        private void ShowAchievementDetails(string achievementTag)
        {
            var achievementInfo = GetAchievementInfo(achievementTag);
            
            MessageBox.Show($"🏆 {achievementInfo.Title}\n\n" +
                          $"📝 Mô tả: {achievementInfo.Description}\n" +
                          $"📊 Tiến độ: {achievementInfo.Progress}\n" +
                          $"🎯 Yêu cầu: {achievementInfo.Requirements}\n" +
                          $"🎁 Phần thưởng: {achievementInfo.Reward}", 
                          "Chi tiết thành tích", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private AchievementInfo GetAchievementInfo(string tag)
        {
            return tag switch
            {
                "day1" => new AchievementInfo
                {
                    Title = "Ngày đầu tiên",
                    Description = "Bước đầu tiên trong hành trình cai thuốc lá",
                    Progress = "✅ Hoàn thành",
                    Requirements = "Không hút thuốc trong 24 giờ đầu tiên",
                    Reward = "50 điểm thành tích + Huy hiệu Khởi đầu"
                },
                "week1" => new AchievementInfo
                {
                    Title = "Tuần đầu tiên",
                    Description = "Vượt qua giai đoạn khó khăn nhất",
                    Progress = "✅ Hoàn thành",
                    Requirements = "Không hút thuốc trong 7 ngày liên tiếp",
                    Reward = "200 điểm thành tích + Huy hiệu Kiên trì"
                },
                "month1" => new AchievementInfo
                {
                    Title = "Một tháng không thuốc",
                    Description = "Thói quen mới đã bắt đầu hình thành",
                    Progress = "🔥 Đang thực hiện (50%)",
                    Requirements = "Không hút thuốc trong 30 ngày liên tiếp",
                    Reward = "500 điểm thành tích + Huy hiệu Quyết tâm"
                },
                "month3" => new AchievementInfo
                {
                    Title = "Ba tháng không thuốc",
                    Description = "Sức khỏe và tâm lý đã cải thiện đáng kể",
                    Progress = "⏳ Chưa đạt",
                    Requirements = "Không hút thuốc trong 90 ngày liên tiếp",
                    Reward = "1000 điểm thành tích + Huy hiệu Chiến thắng"
                },
                "month6" => new AchievementInfo
                {
                    Title = "Sáu tháng không thuốc",
                    Description = "Nguy cơ sức khỏe đã giảm đáng kể",
                    Progress = "⏳ Chưa đạt",
                    Requirements = "Không hút thuốc trong 180 ngày liên tiếp",
                    Reward = "1500 điểm thành tích + Huy hiệu Thành công"
                },
                "year1" => new AchievementInfo
                {
                    Title = "Một năm không thuốc",
                    Description = "Cột mốc quan trọng trong hành trình cai thuốc",
                    Progress = "⏳ Chưa đạt",
                    Requirements = "Không hút thuốc trong 365 ngày liên tiếp",
                    Reward = "3000 điểm thành tích + Huy hiệu Bậc thầy"
                },
                "lifetime" => new AchievementInfo
                {
                    Title = "Cả đời không thuốc",
                    Description = "Thành tích cao quý nhất - cuộc sống khỏe mạnh",
                    Progress = "⏳ Chưa đạt",
                    Requirements = "Duy trì không hút thuốc suốt cuộc đời",
                    Reward = "10000 điểm thành tích + Huy hiệu Huyền thoại"
                },
                "streak" => new AchievementInfo
                {
                    Title = "Chuỗi 15 ngày",
                    Description = "Duy trì chuỗi ngày không hút thuốc",
                    Progress = "✅ Hoàn thành",
                    Requirements = "15 ngày liên tiếp không hút thuốc",
                    Reward = "300 điểm thành tích + Huy hiệu Ổn định"
                },
                "breathing" => new AchievementInfo
                {
                    Title = "Hơi thở khỏe mạnh",
                    Description = "Chức năng phổi đã cải thiện rõ rệt",
                    Progress = "✅ Hoàn thành",
                    Requirements = "Cải thiện hơi thở sau 2 tuần không hút thuốc",
                    Reward = "400 điểm thành tích + Huy hiệu Sức khỏe"
                },
                "taste" => new AchievementInfo
                {
                    Title = "Vị giác phục hồi",
                    Description = "Khả năng nếm vị đã trở lại bình thường",
                    Progress = "🔥 Đang cải thiện (65%)",
                    Requirements = "Phục hồi 80% khả năng nếm vị",
                    Reward = "350 điểm thành tích + Huy hiệu Phục hồi"
                },
                "energy" => new AchievementInfo
                {
                    Title = "Năng lượng dồi dào",
                    Description = "Cảm thấy tràn đầy năng lượng và sức sống",
                    Progress = "⏳ Chưa đạt",
                    Requirements = "Duy trì mức năng lượng cao trong 2 tuần",
                    Reward = "450 điểm thành tích + Huy hiệu Sinh lực"
                },
                "sleep" => new AchievementInfo
                {
                    Title = "Giấc ngủ ngon",
                    Description = "Chất lượng giấc ngủ đã cải thiện đáng kể",
                    Progress = "⏳ Chưa đạt",
                    Requirements = "7 giờ ngủ chất lượng mỗi đêm trong 2 tuần",
                    Reward = "400 điểm thành tích + Huy hiệu Nghỉ ngơi"
                },
                "logging" => new AchievementInfo
                {
                    Title = "Ghi nhận đều đặn",
                    Description = "Thói quen ghi nhận tiến độ hàng ngày",
                    Progress = "✅ 7 ngày liên tiếp",
                    Requirements = "Ghi nhận tiến độ 7 ngày liên tiếp",
                    Reward = "250 điểm thành tích + Huy hiệu Kỷ luật"
                },
                "exercise" => new AchievementInfo
                {
                    Title = "Thói quen tập luyện",
                    Description = "Xây dựng thói quen tập thể dục thường xuyên",
                    Progress = "🔥 5/7 ngày tuần này",
                    Requirements = "Tập thể dục ít nhất 5 ngày/tuần",
                    Reward = "300 điểm thành tích + Huy hiệu Thể thao"
                },
                "social" => new AchievementInfo
                {
                    Title = "Hỗ trợ cộng đồng",
                    Description = "Tham gia tích cực vào cộng đồng cai thuốc",
                    Progress = "✅ Hoàn thành",
                    Requirements = "Tham gia thảo luận và hỗ trợ thành viên khác",
                    Reward = "200 điểm thành tích + Huy hiệu Cộng đồng"
                },
                "mentor" => new AchievementInfo
                {
                    Title = "Người cố vấn",
                    Description = "Trở thành người hướng dẫn cho thành viên mới",
                    Progress = "⏳ Chưa đạt",
                    Requirements = "Hướng dẫn thành công 5 thành viên mới",
                    Reward = "800 điểm thành tích + Huy hiệu Cố vấn"
                },
                "savings1" => new AchievementInfo
                {
                    Title = "100K tiết kiệm",
                    Description = "Mốc tiết kiệm đầu tiên từ việc không mua thuốc",
                    Progress = "✅ Hoàn thành",
                    Requirements = "Tiết kiệm được 100,000 VNĐ",
                    Reward = "150 điểm thành tích + Huy hiệu Tiết kiệm"
                },
                "savings2" => new AchievementInfo
                {
                    Title = "500K tiết kiệm",
                    Description = "Số tiền đáng kể đã được tiết kiệm",
                    Progress = "🔥 Sắp đạt (400K/500K)",
                    Requirements = "Tiết kiệm được 500,000 VNĐ",
                    Reward = "400 điểm thành tích + Huy hiệu Thông thái"
                },
                "savings3" => new AchievementInfo
                {
                    Title = "1 triệu tiết kiệm",
                    Description = "Mốc tiết kiệm lớn - có thể mua được nhiều thứ hay",
                    Progress = "⏳ Chưa đạt",
                    Requirements = "Tiết kiệm được 1,000,000 VNĐ",
                    Reward = "600 điểm thành tích + Huy hiệu Giàu có"
                },
                "spender" => new AchievementInfo
                {
                    Title = "Chủ tiết kiệm",
                    Description = "Bậc thầy về tiết kiệm tiền từ việc cai thuốc",
                    Progress = "⏳ Chưa đạt",
                    Requirements = "Tiết kiệm được 5,000,000 VNĐ",
                    Reward = "1200 điểm thành tích + Huy hiệu Tài chính"
                },
                _ => new AchievementInfo
                {
                    Title = "Thành tích không xác định",
                    Description = "Thông tin không có sẵn",
                    Progress = "N/A",
                    Requirements = "N/A",
                    Reward = "N/A"
                }
            };
        }

        private void ShareAchievementButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Chia sẻ thành tích 'Chuỗi 15 ngày' lên:\n\n" +
                                       "📱 Facebook\n" +
                                       "🐦 Twitter\n" +
                                       "💬 Cộng đồng nội bộ\n" +
                                       "📧 Email cho bạn bè\n\n" +
                                       "Bạn có muốn chia sẻ không?", 
                                       "Chia sẻ thành tích", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Thành tích đã được chia sẻ thành công!\n\n" +
                              "Việc chia sẻ thành công sẽ giúp:\n" +
                              "• Tăng động lực cho bản thân\n" +
                              "• Truyền cảm hứng cho người khác\n" +
                              "• Nhận được sự ủng hộ từ cộng đồng\n" +
                              "• Xây dựng mạng lưới hỗ trợ", 
                              "Chia sẻ thành công", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private void ViewLeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🏅 Bảng xếp hạng tháng này:\n\n" +
                          "🥇 #1: Nguyễn Văn A - 2,450 điểm\n" +
                          "🥈 #2: Trần Thị B - 2,200 điểm\n" +
                          "🥉 #3: Lê Văn C - 1,980 điểm\n" +
                          "...\n" +
                          "🏃 #10: Phạm Thị D - 900 điểm\n" +
                          "...\n" +
                          "📍 #12: Bạn - 850 điểm\n\n" +
                          "💡 Mẹo: Ghi nhận tiến độ hàng ngày và tham gia cộng đồng để tăng điểm!", 
                          "Bảng xếp hạng", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }
    }

    // Data model for achievement information
    public class AchievementInfo
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Progress { get; set; } = "";
        public string Requirements { get; set; } = "";
        public string Reward { get; set; } = "";
    }
}