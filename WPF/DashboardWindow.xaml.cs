using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            // Get user info and load dashboard data
            var username = Application.Current.Properties["Username"]?.ToString() ?? "User";
            WelcomeText.Text = $"Chào mừng trở lại, {username}!";
            
            // Load user progress data (simulated)
            LoadProgressData();
        }

        private void LoadProgressData()
        {
            // Simulate loading user progress data
            DaysSmokeFreeText.Text = "15";
            MoneySavedText.Text = "450,000";
            HealthScoreText.Text = "78%";
            StreakText.Text = "15";
            
            // Update chart based on selected type
            UpdateChart("Ngày không thuốc");
        }

        private void UpdateChart(string chartType)
        {
            // This would normally update a real chart control
            // For now, we'll just update the sample chart representation
            // In a real implementation, you would use a charting library like OxyPlot or LiveCharts
        }

        private void ChartTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedItem = comboBox?.SelectedItem as ComboBoxItem;
            
            if (selectedItem != null)
            {
                UpdateChart(selectedItem.Content.ToString());
            }
        }

        private void UpdateProgressButton_Click(object sender, RoutedEventArgs e)
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

        private void LogProgressButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var progressWindow = new ProgressTrackingWindow();
                progressWindow.Show();
            }
            catch
            {
                MessageBox.Show("Tính năng ghi nhận tiến độ đang được phát triển.", 
                              "Thông báo", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private void ViewPlanButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var quitPlanWindow = new QuitPlanWindow();
                quitPlanWindow.Show();
            }
            catch
            {
                MessageBox.Show("Tính năng xem kế hoạch đang được phát triển.", 
                              "Thông báo", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private void ConsultationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var consultationWindow = new ConsultationWindow();
                consultationWindow.Show();
            }
            catch
            {
                MessageBox.Show("Tính năng tư vấn đang được phát triển.", 
                              "Thông báo", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private void AchievementsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var achievementWindow = new AchievementWindow();
                achievementWindow.Show();
            }
            catch
            {
                MessageBox.Show("Tính năng thành tích đang được phát triển.", 
                              "Thông báo", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private void ViewAllActivitiesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tính năng xem tất cả hoạt động đang được phát triển.", 
                          "Thông báo", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void EmergencyHelpButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có cần hỗ trợ khẩn cấp không?\n\nChúng tôi sẽ kết nối bạn với chuyên gia ngay lập tức.", 
                                       "Hỗ trợ khẩn cấp", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                // Show emergency support options
                MessageBox.Show("Hotline hỗ trợ khẩn cấp: 1900 1234\n\nBạn cũng có thể:\n" +
                              "1. Thực hiện bài tập thở sâu (4-7-8)\n" +
                              "2. Uống một ly nước lạnh\n" +
                              "3. Gọi điện cho người thân\n" +
                              "4. Đi bộ 5-10 phút\n" +
                              "5. Nhắc nhở bản thân lý do cai thuốc", 
                              "Hỗ trợ khẩn cấp", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }
    }
}