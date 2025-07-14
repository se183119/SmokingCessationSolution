using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace SmokingCessation.WPF.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        private bool _isBusy;
        private string _title = string.Empty;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        protected async Task ExecuteAsync(Func<Task> operation, string? busyMessage = null)
        {
            IsBusy = true;
            try
            {
                await operation();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected async Task<T?> ExecuteAsync<T>(Func<Task<T>> operation, string? busyMessage = null)
        {
            IsBusy = true;
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                return default(T);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected virtual Task HandleErrorAsync(Exception exception)
        {
            // Override in derived classes for specific error handling
            System.Diagnostics.Debug.WriteLine($"Error: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}