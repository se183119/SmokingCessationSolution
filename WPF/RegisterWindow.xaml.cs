using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            SmokingStatusComboBox.SelectedIndex = 0; // Default to "Đang hút thuốc"
            MaleRadioButton.IsChecked = true; // Default gender
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                // Show loading
                LoadingPanel.Visibility = Visibility.Visible;
                RegisterButton.IsEnabled = false;

                // Simulate registration process
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(2);
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    LoadingPanel.Visibility = Visibility.Collapsed;
                    RegisterButton.IsEnabled = true;

                    // Show success message and navigate to login
                    MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập để tiếp tục.", 
                                  "Thành công", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Information);

                    BackToLogin();
                };
                timer.Start();
            }
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            BackToLogin();
        }

        private void BackToLogin()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            ClearErrors();

            // Validate Full Name
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                ShowError(FullNameError, "Vui lòng nhập họ và tên.");
                isValid = false;
            }
            else if (FullNameTextBox.Text.Length < 2)
            {
                ShowError(FullNameError, "Họ và tên phải có ít nhất 2 ký tự.");
                isValid = false;
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                ShowError(EmailError, "Vui lòng nhập email.");
                isValid = false;
            }
            else if (!IsValidEmail(EmailTextBox.Text))
            {
                ShowError(EmailError, "Email không hợp lệ.");
                isValid = false;
            }

            // Validate Phone
            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                ShowError(PhoneError, "Vui lòng nhập số điện thoại.");
                isValid = false;
            }
            else if (!IsValidPhone(PhoneTextBox.Text))
            {
                ShowError(PhoneError, "Số điện thoại không hợp lệ.");
                isValid = false;
            }

            // Validate Date of Birth
            if (DateOfBirthPicker.SelectedDate == null)
            {
                ShowError(DateOfBirthError, "Vui lòng chọn ngày sinh.");
                isValid = false;
            }
            else if (DateOfBirthPicker.SelectedDate > DateTime.Now.AddYears(-13))
            {
                ShowError(DateOfBirthError, "Bạn phải ít nhất 13 tuổi để đăng ký.");
                isValid = false;
            }

            // Validate Username
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                ShowError(UsernameError, "Vui lòng nhập tên đăng nhập.");
                isValid = false;
            }
            else if (!IsValidUsername(UsernameTextBox.Text))
            {
                ShowError(UsernameError, "Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới, ít nhất 3 ký tự.");
                isValid = false;
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ShowError(PasswordError, "Vui lòng nhập mật khẩu.");
                isValid = false;
            }
            else if (PasswordBox.Password.Length < 8)
            {
                ShowError(PasswordError, "Mật khẩu phải có ít nhất 8 ký tự.");
                isValid = false;
            }

            // Validate Confirm Password
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                ShowError(ConfirmPasswordError, "Mật khẩu xác nhận không khớp.");
                isValid = false;
            }

            // Validate Terms and Conditions
            if (TermsCheckBox.IsChecked != true)
            {
                ShowGeneralError("Vui lòng đồng ý với điều khoản sử dụng và chính sách bảo mật.");
                isValid = false;
            }

            // Validate Smoking Information (optional but show warnings)
            if (!string.IsNullOrWhiteSpace(YearsSmokingTextBox.Text) && !int.TryParse(YearsSmokingTextBox.Text, out int yearsSmoki))
            {
                ShowGeneralError("Số năm hút thuốc phải là một số nguyên.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(CigarettesPerDayTextBox.Text) && !int.TryParse(CigarettesPerDayTextBox.Text, out int cigarettesPerDay))
            {
                ShowGeneralError("Số điếu thuốc mỗi ngày phải là một số nguyên.");
                isValid = false;
            }

            return isValid;
        }

        private void ClearErrors()
        {
            FullNameError.Visibility = Visibility.Collapsed;
            EmailError.Visibility = Visibility.Collapsed;
            PhoneError.Visibility = Visibility.Collapsed;
            DateOfBirthError.Visibility = Visibility.Collapsed;
            UsernameError.Visibility = Visibility.Collapsed;
            PasswordError.Visibility = Visibility.Collapsed;
            ConfirmPasswordError.Visibility = Visibility.Collapsed;
            GeneralErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void ShowError(TextBlock errorTextBlock, string message)
        {
            errorTextBlock.Text = message;
            errorTextBlock.Visibility = Visibility.Visible;
        }

        private void ShowGeneralError(string message)
        {
            GeneralErrorTextBlock.Text = message;
            GeneralErrorTextBlock.Visibility = Visibility.Visible;
        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsValidPhone(string phone)
        {
            var phonePattern = @"^[\d\s\-\+\(\)]{10,15}$";
            return Regex.IsMatch(phone, phonePattern);
        }

        private bool IsValidUsername(string username)
        {
            var usernamePattern = @"^[a-zA-Z0-9_]{3,20}$";
            return Regex.IsMatch(username, usernamePattern);
        }
    }
}