using SmokingCessation.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SmokingCessation.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
            // Bind password box to view model
            PasswordBox.PasswordChanged += (s, e) =>
            {
                if (DataContext is LoginViewModel vm)
                {
                    vm.Password = PasswordBox.Password;
                }
            };
        }
    }
}