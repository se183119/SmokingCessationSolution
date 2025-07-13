using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class SmokingStatusWindow : Window
    {
        public SmokingStatusWindow()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            MotivationLevelText.Text = $"{MotivationLevelSlider.Value}/10";
            UpdateCostCalculator();
        }

        private void SmokingStatusChanged(object sender, RoutedEventArgs e)
        {
            if (CurrentlySmokingRadio.IsChecked == true)
            {
                CurrentSmokingPanel.Visibility = Visibility.Visible;
                QuitHistoryPanel.Visibility = Visibility.Collapsed;
            }
            else if (QuitSmokingRadio.IsChecked == true)
            {
                CurrentSmokingPanel.Visibility = Visibility.Collapsed;
                QuitHistoryPanel.Visibility = Visibility.Visible;
            }
            else if (NeverSmokedRadio.IsChecked == true)
            {
                CurrentSmokingPanel.Visibility = Visibility.Collapsed;
                QuitHistoryPanel.Visibility = Visibility.Collapsed;
            }

            UpdateSummary();
            UpdateCostCalculator();
        }

        private void MotivationLevelSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MotivationLevelText != null)
            {
                MotivationLevelText.Text = $"{(int)e.NewValue}/10";
            }
        }

        private void UpdateSummary()
        {
            SummaryPanel.Children.Clear();

            if (CurrentlySmokingRadio.IsChecked == true)
            {
                AddSummaryItem("Tình trạng", "Đang hút thuốc", "🚬");
                
                if (!string.IsNullOrEmpty(DailyCigarettesTextBox.Text))
                {
                    AddSummaryItem("Số điếu/ngày", $"{DailyCigarettesTextBox.Text} điếu", "📊");
                }
            }
            else if (QuitSmokingRadio.IsChecked == true)
            {
                AddSummaryItem("Tình trạng", "Đã bỏ thuốc", "✅");
                
                if (QuitDatePicker.SelectedDate.HasValue)
                {
                    var daysSinceQuit = (DateTime.Today - QuitDatePicker.SelectedDate.Value).Days;
                    AddSummaryItem("Đã bỏ thuốc", $"{daysSinceQuit} ngày", "🎯");
                }
            }
            else if (NeverSmokedRadio.IsChecked == true)
            {
                AddSummaryItem("Tình trạng", "Không bao giờ hút", "🌟");
            }

            if (SummaryPanel.Children.Count == 0)
            {
                var noInfoText = new TextBlock
                {
                    Text = "Chưa có thông tin",
                    Style = (Style)FindResource("SecondaryText"),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                SummaryPanel.Children.Add(noInfoText);
            }
        }

        private void AddSummaryItem(string label, string value, string icon)
        {
            var border = new Border
            {
                Background = (System.Windows.Media.Brush)FindResource("BackgroundBrush"),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 2, 0, 2)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            var iconText = new TextBlock
            {
                Text = icon,
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(iconText, 0);

            var stackPanel = new StackPanel();
            
            var labelText = new TextBlock
            {
                Text = label,
                FontSize = 11,
                Opacity = 0.7
            };
            
            var valueText = new TextBlock
            {
                Text = value,
                FontWeight = FontWeights.SemiBold,
                FontSize = 12
            };

            stackPanel.Children.Add(labelText);
            stackPanel.Children.Add(valueText);
            Grid.SetColumn(stackPanel, 1);

            grid.Children.Add(iconText);
            grid.Children.Add(stackPanel);
            border.Child = grid;

            SummaryPanel.Children.Add(border);
        }

        private void UpdateCostCalculator()
        {
            if (CurrentlySmokingRadio.IsChecked == true)
            {
                if (int.TryParse(DailyCigarettesTextBox.Text, out int dailyCigarettes) &&
                    int.TryParse(CostPerPackTextBox.Text, out int costPerPack) &&
                    CigarettesPerPackComboBox.SelectedItem != null)
                {
                    var cigarettesPerPack = GetCigarettesPerPack();
                    var costPerCigarette = (double)costPerPack / cigarettesPerPack;
                    
                    var dailyCost = dailyCigarettes * costPerCigarette;
                    var monthlyCost = dailyCost * 30;
                    var yearlyCost = dailyCost * 365;

                    DailyCostText.Text = $"{dailyCost:N0} VNĐ";
                    MonthlyCostText.Text = $"{monthlyCost:N0} VNĐ";
                    YearlyCostText.Text = $"{yearlyCost:N0} VNĐ";

                    // Update saving tip
                    if (yearlyCost > 1000000) // More than 1 million VND per year
                    {
                        SavingTipText.Text = $"Nếu bỏ thuốc, bạn có thể tiết kiệm {yearlyCost:N0} VNĐ mỗi năm! " +
                                           "Số tiền này có thể dùng để mua xe máy mới hoặc đi du lịch.";
                    }
                    else if (yearlyCost > 500000)
                    {
                        SavingTipText.Text = $"Bỏ thuốc giúp bạn tiết kiệm {yearlyCost:N0} VNĐ mỗi năm. " +
                                           "Đủ để mua nhiều món đồ hữu ích cho gia đình.";
                    }
                    else
                    {
                        SavingTipText.Text = "Mỗi điếu thuốc không hút là một khoản tiết kiệm cho tương lai!";
                    }
                }
                else
                {
                    DailyCostText.Text = "0 VNĐ";
                    MonthlyCostText.Text = "0 VNĐ";
                    YearlyCostText.Text = "0 VNĐ";
                    SavingTipText.Text = "Nhập đầy đủ thông tin để tính toán chi phí";
                }
            }
            else
            {
                DailyCostText.Text = "0 VNĐ";
                MonthlyCostText.Text = "0 VNĐ";
                YearlyCostText.Text = "0 VNĐ";
                SavingTipText.Text = "Chúc mừng! Bạn đang tiết kiệm tiền bằng cách không hút thuốc.";
            }
        }

        private int GetCigarettesPerPack()
        {
            var selectedItem = CigarettesPerPackComboBox.SelectedItem as ComboBoxItem;
            return selectedItem?.Content.ToString() switch
            {
                "10 điếu" => 10,
                "20 điếu" => 20,
                "25 điếu" => 25,
                _ => 20
            };
        }

        private void SaveInformationButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                var smokingData = CollectFormData();
                
                MessageBox.Show("Thông tin tình trạng hút thuốc đã được lưu thành công!\n\n" +
                              $"Tình trạng: {smokingData.Status}\n" +
                              $"Mức độ quyết tâm: {smokingData.MotivationLevel}/10\n" +
                              "Dữ liệu này sẽ giúp tạo kế hoạch cai thuốc phù hợp với bạn.", 
                              "Lưu thành công", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
                
                UpdateSummary();
                UpdateCostCalculator();
            }
        }

        private bool ValidateForm()
        {
            if (CurrentlySmokingRadio.IsChecked != true && 
                QuitSmokingRadio.IsChecked != true && 
                NeverSmokedRadio.IsChecked != true)
            {
                MessageBox.Show("Vui lòng chọn tình trạng hút thuốc của bạn.", "Thiếu thông tin", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (CurrentlySmokingRadio.IsChecked == true)
            {
                if (!int.TryParse(DailyCigarettesTextBox.Text, out int cigarettes) || cigarettes < 0)
                {
                    MessageBox.Show("Vui lòng nhập số điếu thuốc hợp lệ.", "Thông tin không hợp lệ", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!int.TryParse(CostPerPackTextBox.Text, out int cost) || cost < 0)
                {
                    MessageBox.Show("Vui lòng nhập giá tiền hợp lệ.", "Thông tin không hợp lệ", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            if (QuitSmokingRadio.IsChecked == true)
            {
                if (QuitDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày bỏ thuốc.", "Thiếu thông tin", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (QuitDatePicker.SelectedDate > DateTime.Today)
                {
                    MessageBox.Show("Ngày bỏ thuốc không thể ở tương lai.", "Ngày không hợp lệ", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }

        private SmokingStatusData CollectFormData()
        {
            var data = new SmokingStatusData
            {
                Status = GetSmokingStatus(),
                MotivationLevel = (int)MotivationLevelSlider.Value
            };

            if (CurrentlySmokingRadio.IsChecked == true)
            {
                data.DailyCigarettes = int.TryParse(DailyCigarettesTextBox.Text, out int daily) ? daily : 0;
                data.CostPerPack = int.TryParse(CostPerPackTextBox.Text, out int cost) ? cost : 0;
                data.Brand = (BrandComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
                data.FirstCigaretteTime = (FirstCigaretteTimeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
                
                // Collect triggers
                data.Triggers = CollectTriggers();
            }

            if (QuitSmokingRadio.IsChecked == true)
            {
                data.QuitDate = QuitDatePicker.SelectedDate;
                data.PreviousAttempts = int.TryParse(PreviousAttemptsTextBox.Text, out int attempts) ? attempts : 0;
                data.QuitMethod = (QuitMethodComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            }

            // Collect health issues and motivations
            data.HealthIssues = CollectHealthIssues();
            data.Motivations = CollectMotivations();

            return data;
        }

        private string GetSmokingStatus()
        {
            if (CurrentlySmokingRadio.IsChecked == true) return "Đang hút thuốc";
            if (QuitSmokingRadio.IsChecked == true) return "Đã bỏ thuốc";
            if (NeverSmokedRadio.IsChecked == true) return "Không bao giờ hút";
            return "Không xác định";
        }

        private List<string> CollectTriggers()
        {
            var triggers = new List<string>();
            if (TriggerStressCheckBox.IsChecked == true) triggers.Add("Căng thẳng");
            if (TriggerSocialCheckBox.IsChecked == true) triggers.Add("Xã hội");
            if (TriggerWorkCheckBox.IsChecked == true) triggers.Add("Giờ nghỉ");
            if (TriggerAfterMealCheckBox.IsChecked == true) triggers.Add("Sau bữa ăn");
            if (TriggerDrivingCheckBox.IsChecked == true) triggers.Add("Lái xe");
            if (TriggerAlcoholCheckBox.IsChecked == true) triggers.Add("Rượu bia");
            if (TriggerBoredCheckBox.IsChecked == true) triggers.Add("Buồn chán");
            return triggers;
        }

        private List<string> CollectHealthIssues()
        {
            var issues = new List<string>();
            if (HealthCoughCheckBox.IsChecked == true) issues.Add("Ho kéo dài");
            if (HealthBreathingCheckBox.IsChecked == true) issues.Add("Khó thở");
            if (HealthChestPainCheckBox.IsChecked == true) issues.Add("Đau ngực");
            if (HealthTasteCheckBox.IsChecked == true) issues.Add("Mất vị giác/khứu giác");
            if (HealthSleepCheckBox.IsChecked == true) issues.Add("Khó ngủ");
            if (HealthFatigueCheckBox.IsChecked == true) issues.Add("Mệt mỏi");
            if (HealthNoneCheckBox.IsChecked == true) issues.Add("Không có vấn đề");
            return issues;
        }

        private List<string> CollectMotivations()
        {
            var motivations = new List<string>();
            if (MotivationHealthCheckBox.IsChecked == true) motivations.Add("Cải thiện sức khỏe");
            if (MotivationMoneyCheckBox.IsChecked == true) motivations.Add("Tiết kiệm tiền");
            if (MotivationFamilyCheckBox.IsChecked == true) motivations.Add("Vì gia đình");
            if (MotivationAppearanceCheckBox.IsChecked == true) motivations.Add("Cải thiện ngoại hình");
            if (MotivationSocialCheckBox.IsChecked == true) motivations.Add("Áp lực xã hội");
            if (MotivationPregnancyCheckBox.IsChecked == true) motivations.Add("Mang thai/sinh con");
            return motivations;
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
            // Reset radio buttons
            CurrentlySmokingRadio.IsChecked = false;
            QuitSmokingRadio.IsChecked = false;
            NeverSmokedRadio.IsChecked = false;
            
            // Reset panels
            CurrentSmokingPanel.Visibility = Visibility.Collapsed;
            QuitHistoryPanel.Visibility = Visibility.Collapsed;
            
            // Reset text inputs
            DailyCigarettesTextBox.Text = "0";
            CostPerPackTextBox.Text = "0";
            PreviousAttemptsTextBox.Text = "0";
            
            // Reset comboboxes
            BrandComboBox.SelectedIndex = -1;
            FirstCigaretteTimeComboBox.SelectedIndex = -1;
            CigarettesPerPackComboBox.SelectedIndex = 1;
            QuitMethodComboBox.SelectedIndex = -1;
            
            // Reset date picker
            QuitDatePicker.SelectedDate = null;
            
            // Reset sliders
            MotivationLevelSlider.Value = 7;
            
            // Reset all checkboxes
            var checkboxes = new CheckBox[] 
            {
                TriggerStressCheckBox, TriggerSocialCheckBox, TriggerWorkCheckBox, TriggerAfterMealCheckBox,
                TriggerDrivingCheckBox, TriggerAlcoholCheckBox, TriggerBoredCheckBox,
                HealthCoughCheckBox, HealthBreathingCheckBox, HealthChestPainCheckBox, HealthTasteCheckBox,
                HealthSleepCheckBox, HealthFatigueCheckBox, HealthNoneCheckBox,
                MotivationHealthCheckBox, MotivationMoneyCheckBox, MotivationFamilyCheckBox,
                MotivationAppearanceCheckBox, MotivationSocialCheckBox, MotivationPregnancyCheckBox
            };
            
            foreach (var checkbox in checkboxes)
            {
                checkbox.IsChecked = false;
            }
            
            UpdateSummary();
            UpdateCostCalculator();
        }

        private void CreateQuitPlanButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var quitPlanWindow = new QuitPlanWindow();
                quitPlanWindow.Show();
            }
            catch
            {
                MessageBox.Show("Tính năng tạo kế hoạch cai thuốc đang được phát triển.", 
                              "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TrackProgressButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var progressWindow = new ProgressTrackingWindow();
                progressWindow.Show();
            }
            catch
            {
                MessageBox.Show("Tính năng theo dõi tiến độ đang được phát triển.", 
                              "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ConsultExpertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var consultationWindow = new ConsultationWindow();
                consultationWindow.Show();
            }
            catch
            {
                MessageBox.Show("Tính năng tư vấn chuyên gia đang được phát triển.", 
                              "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GetSupportButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hỗ trợ 24/7:\n\n" +
                          "📞 Hotline: 1900 1234\n" +
                          "💬 Chat online: Có sẵn trong app\n" +
                          "📧 Email: support@smokingcessation.com\n" +
                          "🏥 Tư vấn y tế: Đặt lịch với bác sĩ\n\n" +
                          "Chúng tôi luôn sẵn sàng hỗ trợ bạn!", 
                          "Hỗ trợ", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    // Data model for smoking status
    public class SmokingStatusData
    {
        public string Status { get; set; } = "";
        public int DailyCigarettes { get; set; }
        public int CostPerPack { get; set; }
        public string Brand { get; set; } = "";
        public string FirstCigaretteTime { get; set; } = "";
        public List<string> Triggers { get; set; } = new List<string>();
        public DateTime? QuitDate { get; set; }
        public int PreviousAttempts { get; set; }
        public string QuitMethod { get; set; } = "";
        public List<string> HealthIssues { get; set; } = new List<string>();
        public List<string> Motivations { get; set; } = new List<string>();
        public int MotivationLevel { get; set; }
    }
}