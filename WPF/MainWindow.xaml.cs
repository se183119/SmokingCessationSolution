using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class MainWindow : Window
    {
        private string currentUserRole = "Member"; // Default role

        public MainWindow()
        {
            InitializeComponent();
            LoadUserInfo();
            SetupNavigation();
        }

        private void LoadUserInfo()
        {
            // Get user info from application properties (set during login)
            var username = Application.Current.Properties["Username"]?.ToString() ?? "User";
            var userRole = Application.Current.Properties["UserRole"]?.ToString() ?? "Member";
            
            currentUserRole = userRole;
            WelcomeTextBlock.Text = $"Chào mừng, {username}!";
        }

        private void SetupNavigation()
        {
            NavigationPanel.Children.Clear();

            // Add role-based navigation items
            switch (currentUserRole)
            {
                case "Member":
                    SetupMemberNavigation();
                    break;
                case "Coach":
                    SetupCoachNavigation();
                    break;
                case "Admin":
                    SetupAdminNavigation();
                    break;
            }
        }

        private void SetupMemberNavigation()
        {
            AddNavigationItem("🏠 Trang chủ", "Home");
            AddNavigationItem("📊 Bảng điều khiển", "Dashboard");
            AddNavigationItem("📈 Theo dõi tiến độ", "ProgressTracking");
            AddNavigationItem("📝 Kế hoạch cai thuốc", "QuitPlan");
            AddNavigationItem("🚭 Tình trạng hút thuốc", "SmokingStatus");
            AddNavigationItem("🏆 Thành tích", "Achievement");
            AddNavigationItem("💬 Tư vấn", "Consultation");
            AddNavigationItem("👤 Hồ sơ cá nhân", "Profile");
            AddNavigationItem("💎 Gói thành viên", "Membership");
            AddNavigationItem("💭 Phản hồi", "Feedback");
        }

        private void SetupCoachNavigation()
        {
            AddNavigationItem("🏠 Trang chủ", "Home");
            AddNavigationItem("📊 Bảng điều khiển", "CoachDashboard");
            AddNavigationItem("💬 Quản lý tư vấn", "ConsultationManagement");
            AddNavigationItem("👥 Tiến độ thành viên", "MemberProgress");
            AddNavigationItem("👤 Hồ sơ cá nhân", "Profile");
        }

        private void SetupAdminNavigation()
        {
            AddNavigationItem("🏠 Trang chủ", "Home");
            AddNavigationItem("📊 Bảng điều khiển Admin", "AdminDashboard");
            AddNavigationItem("👥 Quản lý người dùng", "UserManagement");
            AddNavigationItem("💎 Quản lý gói thành viên", "MembershipManagement");
            AddNavigationItem("📈 Báo cáo hệ thống", "SystemReports");
            AddNavigationItem("💭 Quản lý phản hồi", "FeedbackManagement");
            AddNavigationItem("👤 Hồ sơ cá nhân", "Profile");
        }

        private void AddNavigationItem(string text, string tag)
        {
            var button = new Button
            {
                Content = text,
                Style = (Style)FindResource("NavMenuItem"),
                Tag = tag,
                Margin = new Thickness(0, 2, 0, 2)
            };
            button.Click += NavigationButton_Click;
            NavigationPanel.Children.Add(button);
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tag = button?.Tag?.ToString();

            if (tag == "Home")
            {
                // Already showing home content, no action needed
                return;
            }

            try
            {
                // Navigate to specific window based on tag
                Window targetWindow = tag switch
                {
                    "Dashboard" => new DashboardWindow(),
                    "ProgressTracking" => new ProgressTrackingWindow(),
                    "QuitPlan" => new QuitPlanWindow(),
                    "SmokingStatus" => new SmokingStatusWindow(),
                    "Achievement" => new AchievementWindow(),
                    "Consultation" => new ConsultationWindow(),
                    "Profile" => new ProfileWindow(),
                    "Membership" => new MembershipWindow(),
                    "Feedback" => new FeedbackWindow(),
                    "CoachDashboard" => new CoachDashboardWindow(),
                    "ConsultationManagement" => new ConsultationManagementWindow(),
                    "MemberProgress" => new MemberProgressWindow(),
                    "AdminDashboard" => new AdminDashboardWindow(),
                    "UserManagement" => new UserManagementWindow(),
                    "MembershipManagement" => new MembershipManagementWindow(),
                    "SystemReports" => new SystemReportsWindow(),
                    "FeedbackManagement" => new FeedbackManagementWindow(),
                    _ => null
                };

                if (targetWindow != null)
                {
                    targetWindow.Show();
                }
                else
                {
                    MessageBox.Show($"Tính năng '{button.Content}' đang được phát triển.", 
                                  "Thông báo", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Tính năng '{button.Content}' đang được phát triển.", 
                              "Thông báo", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private void UserMenuComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedItem = comboBox?.SelectedItem as ComboBoxItem;
            
            if (selectedItem != null)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "Hồ sơ cá nhân":
                        try
                        {
                            var profileWindow = new ProfileWindow();
                            profileWindow.Show();
                        }
                        catch
                        {
                            MessageBox.Show("Tính năng hồ sơ cá nhân đang được phát triển.", 
                                          "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case "Cài đặt":
                        MessageBox.Show("Tính năng cài đặt đang được phát triển.", 
                                      "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Trợ giúp":
                        MessageBox.Show("Liên hệ hỗ trợ: support@smokingcessation.com", 
                                      "Trợ giúp", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Đăng xuất":
                        Logout();
                        break;
                }
                
                // Reset selection
                comboBox.SelectedItem = null;
            }
        }

        private void GetStartedButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentUserRole == "Member")
            {
                try
                {
                    var quitPlanWindow = new QuitPlanWindow();
                    quitPlanWindow.Show();
                }
                catch
                {
                    MessageBox.Show("Tính năng kế hoạch cai thuốc đang được phát triển.", 
                                  "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void QuickAction_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var action = button?.Tag?.ToString();

            Window targetWindow = action switch
            {
                "Progress" => new ProgressTrackingWindow(),
                "Plan" => new QuitPlanWindow(),
                "Consultation" => new ConsultationWindow(),
                _ => null
            };

            try
            {
                if (targetWindow != null)
                {
                    targetWindow.Show();
                }
            }
            catch
            {
                MessageBox.Show("Tính năng đang được phát triển.", 
                              "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Logout()
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", 
                                       "Xác nhận đăng xuất", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                // Clear session data
                Application.Current.Properties.Clear();
                
                // Show login window
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                
                // Close this window
                this.Close();
            }
        }
    }
}