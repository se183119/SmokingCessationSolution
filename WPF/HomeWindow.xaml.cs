using System.Windows;

namespace WPF
{
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        private void GetStartedButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to registration or login
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private void LearnMoreButton_Click(object sender, RoutedEventArgs e)
        {
            // Scroll to features section or show more info
            MessageBox.Show("Thông tin chi tiết về platform sẽ được hiển thị ở đây.", 
                          "Tìm hiểu thêm", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void ViewAllLeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bảng xếp hạng đầy đủ sẽ được hiển thị ở đây.", 
                          "Bảng xếp hạng", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void RegisterNowButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
        }

        private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Liên hệ tư vấn:\nEmail: support@smokingcessation.com\nHotline: 1900 1234\nGiờ làm việc: 8:00 - 22:00 hàng ngày", 
                          "Liên hệ tư vấn", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }
    }
}