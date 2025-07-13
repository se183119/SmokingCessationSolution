using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class ProgressTrackingWindow : Window
    {
        private int selectedMood = 0;

        public ProgressTrackingWindow()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Set default values
            TrackingDatePicker.SelectedDate = DateTime.Today;
            StressLevelText.Text = $"{StressLevelSlider.Value}/10";
            CravingLevelText.Text = $"{CravingLevelSlider.Value}/10";
            SelectedMoodText.Text = "Chưa chọn";
        }

        private void SmokingStatusChanged(object sender, RoutedEventArgs e)
        {
            // Show/hide cigarette count panel based on smoking status
            if (NoSmokingRadio.IsChecked == true)
            {
                CigaretteCountPanel.Visibility = Visibility.Collapsed;
                CigaretteCountTextBox.Text = "0";
            }
            else
            {
                CigaretteCountPanel.Visibility = Visibility.Visible;
            }
        }

        private void MoodButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset all mood button styles
            ResetMoodButtons();

            var button = sender as Button;
            if (button != null)
            {
                selectedMood = int.Parse(button.Tag.ToString());
                
                // Highlight selected mood
                button.BorderBrush = (System.Windows.Media.Brush)FindResource("PrimaryBrush");
                button.BorderThickness = new Thickness(3);
                
                // Update mood text
                SelectedMoodText.Text = selectedMood switch
                {
                    1 => "Rất tệ",
                    2 => "Tệ",
                    3 => "Bình thường",
                    4 => "Tốt", 
                    5 => "Rất tốt",
                    _ => "Chưa chọn"
                };
            }
        }

        private void ResetMoodButtons()
        {
            var moodButtons = new Button[] { MoodVeryBad, MoodBad, MoodNeutral, MoodGood, MoodVeryGood };
            foreach (var button in moodButtons)
            {
                button.BorderBrush = (System.Windows.Media.Brush)FindResource("DividerBrush");
                button.BorderThickness = new Thickness(2);
            }
        }

        private void StressLevelSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (StressLevelText != null)
            {
                StressLevelText.Text = $"{(int)e.NewValue}/10";
            }
        }

        private void CravingLevelSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CravingLevelText != null)
            {
                CravingLevelText.Text = $"{(int)e.NewValue}/10";
            }
        }

        private void SaveProgressButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                // Collect form data
                var progressData = CollectFormData();
                
                // Simulate saving progress
                MessageBox.Show("Tiến độ đã được lưu thành công!\n\n" +
                              $"Ngày: {progressData.Date:dd/MM/yyyy}\n" +
                              $"Tình trạng: {progressData.SmokingStatus}\n" +
                              $"Tâm trạng: {progressData.MoodText}\n" +
                              $"Mức căng thẳng: {progressData.StressLevel}/10\n" +
                              $"Mức thèm thuốc: {progressData.CravingLevel}/10", 
                              "Lưu thành công", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
                
                // Reset form for next entry
                ResetForm();
            }
        }

        private bool ValidateForm()
        {
            if (TrackingDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày ghi nhận.", "Thiếu thông tin", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (TrackingDatePicker.SelectedDate > DateTime.Today)
            {
                MessageBox.Show("Không thể ghi nhận tiến độ cho ngày trong tương lai.", "Ngày không hợp lệ", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (selectedMood == 0)
            {
                MessageBox.Show("Vui lòng chọn tâm trạng của bạn.", "Thiếu thông tin", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validate cigarette count if smoking
            if (!NoSmokingRadio.IsChecked == true && CigaretteCountPanel.Visibility == Visibility.Visible)
            {
                if (!int.TryParse(CigaretteCountTextBox.Text, out int cigaretteCount) || cigaretteCount < 0)
                {
                    MessageBox.Show("Vui lòng nhập số điếu thuốc hợp lệ (số nguyên >= 0).", "Thông tin không hợp lệ", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            // Validate activity duration
            if (!int.TryParse(ActivityDurationTextBox.Text, out int duration) || duration < 0)
            {
                MessageBox.Show("Vui lòng nhập thời gian hoạt động hợp lệ (số nguyên >= 0).", "Thông tin không hợp lệ", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validate water intake
            if (!int.TryParse(WaterIntakeTextBox.Text, out int waterIntake) || waterIntake < 0)
            {
                MessageBox.Show("Vui lòng nhập lượng nước uống hợp lệ (số nguyên >= 0).", "Thông tin không hợp lệ", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private ProgressData CollectFormData()
        {
            return new ProgressData
            {
                Date = TrackingDatePicker.SelectedDate.Value,
                SmokingStatus = GetSmokingStatus(),
                CigaretteCount = int.TryParse(CigaretteCountTextBox.Text, out int count) ? count : 0,
                MoodScore = selectedMood,
                MoodText = SelectedMoodText.Text,
                StressLevel = (int)StressLevelSlider.Value,
                CravingLevel = (int)CravingLevelSlider.Value,
                HasExercise = ExerciseCheckBox.IsChecked == true,
                HasWalking = WalkingCheckBox.IsChecked == true,
                HasSports = SportsCheckBox.IsChecked == true,
                HasYoga = YogaCheckBox.IsChecked == true,
                ActivityDuration = int.TryParse(ActivityDurationTextBox.Text, out int duration) ? duration : 0,
                WaterIntake = int.TryParse(WaterIntakeTextBox.Text, out int water) ? water : 0,
                Notes = NotesTextBox.Text
            };
        }

        private string GetSmokingStatus()
        {
            if (NoSmokingRadio.IsChecked == true) return "Không hút thuốc";
            if (LightSmokingRadio.IsChecked == true) return "Hút ít hơn bình thường";
            if (NormalSmokingRadio.IsChecked == true) return "Hút bình thường";
            if (HeavySmokingRadio.IsChecked == true) return "Hút nhiều hơn bình thường";
            return "Không xác định";
        }

        private void ResetFormButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đặt lại form? Tất cả dữ liệu đã nhập sẽ bị mất.", 
                                       "Xác nhận đặt lại", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                ResetForm();
            }
        }

        private void ResetForm()
        {
            // Reset to default values
            TrackingDatePicker.SelectedDate = DateTime.Today;
            NoSmokingRadio.IsChecked = true;
            CigaretteCountTextBox.Text = "0";
            CigaretteCountPanel.Visibility = Visibility.Collapsed;
            
            // Reset mood
            selectedMood = 0;
            ResetMoodButtons();
            SelectedMoodText.Text = "Chưa chọn";
            
            // Reset sliders
            StressLevelSlider.Value = 5;
            CravingLevelSlider.Value = 3;
            
            // Reset checkboxes
            ExerciseCheckBox.IsChecked = false;
            WalkingCheckBox.IsChecked = false;
            SportsCheckBox.IsChecked = false;
            YogaCheckBox.IsChecked = false;
            
            // Reset text inputs
            ActivityDurationTextBox.Text = "0";
            WaterIntakeTextBox.Text = "0";
            NotesTextBox.Text = "Ví dụ: Cảm thấy khó khăn vào buổi sáng, nhưng đã vượt qua bằng cách uống trà xanh...";
        }

        private void ViewAllEntriesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tính năng xem tất cả ghi nhận đang được phát triển.\n\n" +
                          "Sẽ hiển thị lịch sử tất cả các lần ghi nhận tiến độ với biểu đồ và thống kê chi tiết.", 
                          "Tính năng đang phát triển", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }
    }

    // Data model for progress tracking
    public class ProgressData
    {
        public DateTime Date { get; set; }
        public string SmokingStatus { get; set; } = "";
        public int CigaretteCount { get; set; }
        public int MoodScore { get; set; }
        public string MoodText { get; set; } = "";
        public int StressLevel { get; set; }
        public int CravingLevel { get; set; }
        public bool HasExercise { get; set; }
        public bool HasWalking { get; set; }
        public bool HasSports { get; set; }
        public bool HasYoga { get; set; }
        public int ActivityDuration { get; set; }
        public int WaterIntake { get; set; }
        public string Notes { get; set; } = "";
    }
}