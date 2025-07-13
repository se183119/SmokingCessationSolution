using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF
{
    public partial class QuitPlanWindow : Window
    {
        private string selectedPlanType = "";

        public QuitPlanWindow()
        {
            InitializeComponent();
        }

        private void PlanTemplate_Click(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                selectedPlanType = border.Tag.ToString();
                ShowPlanDetails(selectedPlanType);
            }
        }

        private void ShowPlanDetails(string planType)
        {
            DefaultContent.Visibility = Visibility.Collapsed;
            PlanContent.Visibility = Visibility.Visible;

            switch (planType)
            {
                case "Gradual":
                    SetupGradualPlan();
                    break;
                case "ColdTurkey":
                    SetupColdTurkeyPlan();
                    break;
                case "Replacement":
                    SetupReplacementPlan();
                    break;
                case "Custom":
                    SetupCustomPlan();
                    break;
            }
        }

        private void SetupGradualPlan()
        {
            PlanIcon.Text = "📉";
            PlanTitle.Text = "Kế hoạch cai từ từ";
            PlanDescription.Text = "Giảm dần số điếu thuốc mỗi ngày trong 4-6 tuần";
            
            // Set default values for gradual plan
            CurrentCigarettesTextBox.Text = "20";
            DurationComboBox.SelectedIndex = 1; // 4 weeks
        }

        private void SetupColdTurkeyPlan()
        {
            PlanIcon.Text = "🛑";
            PlanTitle.Text = "Kế hoạch dừng ngay";
            PlanDescription.Text = "Ngừng hút thuốc hoàn toàn ngay từ ngày đầu";
            
            CurrentCigarettesTextBox.Text = "0";
            DurationComboBox.SelectedIndex = 0; // 2 weeks
        }

        private void SetupReplacementPlan()
        {
            PlanIcon.Text = "🔄";
            PlanTitle.Text = "Kế hoạch thay thế thói quen";
            PlanDescription.Text = "Thay thế thuốc lá bằng hoạt động tích cực khác";
            
            CurrentCigarettesTextBox.Text = "10";
            DurationComboBox.SelectedIndex = 2; // 6 weeks
        }

        private void SetupCustomPlan()
        {
            PlanIcon.Text = "⚙️";
            PlanTitle.Text = "Kế hoạch tùy chỉnh";
            PlanDescription.Text = "Tạo kế hoạch riêng phù hợp với tình huống cá nhân";
            
            CurrentCigarettesTextBox.Text = "";
            DurationComboBox.SelectedIndex = 1; // 4 weeks
        }

        private void GenerateScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            SchedulePanel.Children.Clear();

            if (string.IsNullOrEmpty(selectedPlanType))
                return;

            var startDate = StartDatePicker.SelectedDate ?? DateTime.Today;
            var durationItem = DurationComboBox.SelectedItem as ComboBoxItem;
            var totalDays = int.Parse(durationItem?.Tag?.ToString() ?? "28");
            
            if (!int.TryParse(CurrentCigarettesTextBox.Text, out int currentCigarettes))
                currentCigarettes = 20;

            GenerateScheduleForPlan(selectedPlanType, startDate, totalDays, currentCigarettes);
        }

        private void GenerateScheduleForPlan(string planType, DateTime startDate, int totalDays, int currentCigarettes)
        {
            var weeks = (totalDays + 6) / 7; // Round up to nearest week

            for (int week = 1; week <= weeks; ++week)
            {
                var weekCard = CreateWeekCard(planType, week, weeks, currentCigarettes, startDate);
                SchedulePanel.Children.Add(weekCard);
            }
        }

        private Border CreateWeekCard(string planType, int week, int totalWeeks, int initialCigarettes, DateTime startDate)
        {
            var border = new Border
            {
                Style = (Style)FindResource("Card"),
                Margin = new Thickness(0, 5, 0, 5),
                Background = week == 1 ? 
                    (System.Windows.Media.Brush)FindResource("PrimaryLightBrush") : 
                    (System.Windows.Media.Brush)FindResource("SurfaceBrush")
            };

            var stackPanel = new StackPanel { Spacing = 10 };

            // Week header
            var headerPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.SpaceBetween
            };

            var weekText = new TextBlock
            {
                Text = $"Tuần {week}",
                FontWeight = FontWeights.SemiBold,
                Foreground = week == 1 ? System.Windows.Media.Brushes.White : 
                           (System.Windows.Media.Brush)FindResource("TextPrimaryBrush")
            };

            var dateText = new TextBlock
            {
                Text = $"{startDate.AddDays((week - 1) * 7):dd/MM} - {startDate.AddDays(week * 7 - 1):dd/MM}",
                FontSize = 11,
                Foreground = week == 1 ? System.Windows.Media.Brushes.White : 
                           (System.Windows.Media.Brush)FindResource("TextSecondaryBrush")
            };

            headerPanel.Children.Add(weekText);
            headerPanel.Children.Add(dateText);
            stackPanel.Children.Add(headerPanel);

            // Week goals based on plan type
            var goalText = GetWeekGoal(planType, week, totalWeeks, initialCigarettes);
            var goalTextBlock = new TextBlock
            {
                Text = goalText,
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                Foreground = week == 1 ? System.Windows.Media.Brushes.White : 
                           (System.Windows.Media.Brush)FindResource("TextPrimaryBrush")
            };
            stackPanel.Children.Add(goalTextBlock);

            // Week activities
            var activitiesText = GetWeekActivities(planType, week);
            if (!string.IsNullOrEmpty(activitiesText))
            {
                var activitiesTextBlock = new TextBlock
                {
                    Text = activitiesText,
                    FontSize = 11,
                    TextWrapping = TextWrapping.Wrap,
                    Opacity = 0.8,
                    Foreground = week == 1 ? System.Windows.Media.Brushes.White : 
                               (System.Windows.Media.Brush)FindResource("TextSecondaryBrush")
                };
                stackPanel.Children.Add(activitiesTextBlock);
            }

            border.Child = stackPanel;
            return border;
        }

        private string GetWeekGoal(string planType, int week, int totalWeeks, int initialCigarettes)
        {
            return planType switch
            {
                "Gradual" => GetGradualPlanGoal(week, totalWeeks, initialCigarettes),
                "ColdTurkey" => week == 1 ? "Mục tiêu: 0 điếu thuốc/ngày" : "Duy trì: 0 điếu thuốc/ngày",
                "Replacement" => $"Tuần {week}: Thay thế {Math.Max(1, initialCigarettes / totalWeeks * week)} lần hút thuốc",
                "Custom" => $"Tuần {week}: Mục tiêu tùy chỉnh theo nhu cầu",
                _ => ""
            };
        }

        private string GetGradualPlanGoal(int week, int totalWeeks, int initialCigarettes)
        {
            var targetCigarettes = Math.Max(0, initialCigarettes - (initialCigarettes * week / totalWeeks));
            var previousTarget = week == 1 ? initialCigarettes : 
                               Math.Max(0, initialCigarettes - (initialCigarettes * (week - 1) / totalWeeks));
            
            return $"Mục tiêu: Giảm từ {previousTarget} xuống {targetCigarettes} điếu/ngày";
        }

        private string GetWeekActivities(string planType, int week)
        {
            return week switch
            {
                1 => "• Bắt đầu ghi nhận tiến độ hàng ngày\n• Thiết lập thói quen thay thế\n• Tìm hiểu về lợi ích của việc cai thuốc",
                2 => "• Tăng cường hoạt động thể chất\n• Thực hành kỹ thuật thở sâu\n• Tránh các yếu tố kích thích",
                3 => "• Đánh giá và điều chỉnh kế hoạch\n• Tham gia hoạt động xã hội\n• Tập trung vào động lực dài hạn",
                4 => "• Củng cố thói quen mới\n• Chuẩn bị cho giai đoạn duy trì\n• Chia sẻ kinh nghiệm với người khác",
                _ => ""
            };
        }

        private void SavePlanButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidatePlan())
            {
                MessageBox.Show("Kế hoạch đã được lưu thành công!\n\n" +
                              $"Loại kế hoạch: {PlanTitle.Text}\n" +
                              $"Ngày bắt đầu: {StartDatePicker.SelectedDate:dd/MM/yyyy}\n" +
                              $"Thời gian dự kiến: {((ComboBoxItem)DurationComboBox.SelectedItem).Content}", 
                              "Lưu thành công", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private void StartPlanButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidatePlan())
            {
                var result = MessageBox.Show("Bạn có sẵn sàng bắt đầu thực hiện kế hoạch này không?\n\n" +
                                           "Hệ thống sẽ bắt đầu theo dõi tiến độ của bạn từ hôm nay.", 
                                           "Bắt đầu kế hoạch", 
                                           MessageBoxButton.YesNo, 
                                           MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Kế hoạch đã được kích hoạt!\n\n" +
                                  "Bạn có thể theo dõi tiến độ trong phần 'Theo dõi tiến độ' và 'Bảng điều khiển'.", 
                                  "Kế hoạch đã kích hoạt", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Information);
                }
            }
        }

        private void ShareWithCoachButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidatePlan())
            {
                MessageBox.Show("Kế hoạch đã được chia sẻ với coach của bạn!\n\n" +
                              "Coach sẽ nhận được thông báo và có thể đưa ra lời khuyên phù hợp.", 
                              "Chia sẻ thành công", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private bool ValidatePlan()
        {
            if (string.IsNullOrEmpty(selectedPlanType))
            {
                MessageBox.Show("Vui lòng chọn loại kế hoạch.", "Thiếu thông tin", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (StartDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu.", "Thiếu thông tin", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(CurrentCigarettesTextBox.Text, out int cigarettes) || cigarettes < 0)
            {
                MessageBox.Show("Vui lòng nhập số điếu thuốc hiện tại hợp lệ.", "Thông tin không hợp lệ", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void AssessmentButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tính năng đánh giá để tìm kế hoạch phù hợp đang được phát triển.\n\n" +
                          "Bài đánh giá sẽ bao gồm:\n" +
                          "• Thói quen hút thuốc hiện tại\n" +
                          "• Mức độ nghiện nicotine\n" +
                          "• Động lực cai thuốc\n" +
                          "• Tình trạng sức khỏe\n" +
                          "• Hỗ trợ xã hội", 
                          "Tính năng đang phát triển", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void UpdateCurrentProgressButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var progressWindow = new ProgressTrackingWindow();
                progressWindow.Show();
            }
            catch
            {
                MessageBox.Show("Tính năng cập nhật tiến độ đang được phát triển.", 
                              "Thông báo", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private void ViewDetailedProgressButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tính năng xem tiến độ chi tiết đang được phát triển.\n\n" +
                          "Sẽ hiển thị:\n" +
                          "• Biểu đồ tiến độ theo ngày\n" +
                          "• So sánh với kế hoạch đề ra\n" +
                          "• Các cột mốc đã đạt được\n" +
                          "• Dự báo thành công", 
                          "Tính năng đang phát triển", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void EmergencyContactButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn liên hệ với coach để được hỗ trợ điều chỉnh kế hoạch?\n\n" +
                                       "Chúng tôi sẽ sắp xếp cuộc gọi trong vòng 24 giờ.", 
                                       "Hỗ trợ khẩn cấp", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Yêu cầu hỗ trợ đã được gửi!\n\n" +
                              "Coach sẽ liên hệ với bạn trong vòng 24 giờ.\n" +
                              "Hotline khẩn cấp: 1900 1234", 
                              "Đã gửi yêu cầu", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }
    }
}