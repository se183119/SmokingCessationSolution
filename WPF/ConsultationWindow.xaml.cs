using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF
{
    public partial class ConsultationWindow : Window
    {
        private int currentRating = 0;

        public ConsultationWindow()
        {
            InitializeComponent();
            InitializeChat();
        }

        private void InitializeChat()
        {
            // Set placeholder text style
            MessageTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            
            // Scroll to bottom of chat
            ChatScrollViewer.ScrollToBottom();
        }

        private void MessageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageTextBox.Text == "Nhập tin nhắn của bạn...")
            {
                MessageTextBox.Text = "";
                MessageTextBox.Foreground = (System.Windows.Media.Brush)FindResource("TextPrimaryBrush");
            }
        }

        private void MessageTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageTextBox.Text))
            {
                MessageTextBox.Text = "Nhập tin nhắn của bạn...";
                MessageTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(MessageTextBox.Text) && 
                MessageTextBox.Text != "Nhập tin nhắn của bạn...")
            {
                SendMessage();
            }
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MessageTextBox.Text) && 
                MessageTextBox.Text != "Nhập tin nhắn của bạn...")
            {
                SendMessage();
            }
        }

        private void SendMessage()
        {
            var messageText = MessageTextBox.Text;
            var currentTime = DateTime.Now.ToString("HH:mm");
            
            // Create user message bubble
            var userMessageBorder = new Border
            {
                Background = (System.Windows.Media.Brush)FindResource("PrimaryBrush"),
                CornerRadius = new CornerRadius(12, 12, 4, 12),
                Padding = new Thickness(12),
                HorizontalAlignment = HorizontalAlignment.Right,
                MaxWidth = 400,
                Margin = new Thickness(0, 5, 0, 5)
            };

            var userMessagePanel = new StackPanel { Spacing = 8 };
            
            var userMessageTextBlock = new TextBlock
            {
                Text = messageText,
                Foreground = System.Windows.Media.Brushes.White,
                Style = (Style)FindResource("BodyText"),
                TextWrapping = TextWrapping.Wrap
            };
            
            var userTimeTextBlock = new TextBlock
            {
                Text = currentTime,
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 10,
                Opacity = 0.8,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            userMessagePanel.Children.Add(userMessageTextBlock);
            userMessagePanel.Children.Add(userTimeTextBlock);
            userMessageBorder.Child = userMessagePanel;

            // Add to chat
            ChatMessagesPanel.Children.Add(userMessageBorder);

            // Clear input
            MessageTextBox.Text = "";
            MessageTextBox_LostFocus(null, null);

            // Scroll to bottom
            ChatScrollViewer.ScrollToBottom();

            // Simulate expert response after delay
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                AddExpertResponse(GetAutomaticResponse(messageText));
            };
            timer.Start();
        }

        private void AddExpertResponse(string responseText)
        {
            var currentTime = DateTime.Now.ToString("HH:mm");
            
            // Create expert message bubble
            var expertMessageBorder = new Border
            {
                Background = System.Windows.Media.Brushes.White,
                CornerRadius = new CornerRadius(12, 12, 12, 4),
                Padding = new Thickness(12),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 400,
                Margin = new Thickness(0, 5, 0, 5)
            };

            var expertMessagePanel = new StackPanel { Spacing = 8 };
            
            var expertMessageTextBlock = new TextBlock
            {
                Text = responseText,
                Style = (Style)FindResource("BodyText"),
                TextWrapping = TextWrapping.Wrap
            };
            
            var expertTimeTextBlock = new TextBlock
            {
                Text = currentTime,
                FontSize = 10,
                Opacity = 0.6,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            expertMessagePanel.Children.Add(expertMessageTextBlock);
            expertMessagePanel.Children.Add(expertTimeTextBlock);
            expertMessageBorder.Child = expertMessagePanel;

            // Add to chat
            ChatMessagesPanel.Children.Add(expertMessageBorder);

            // Scroll to bottom
            ChatScrollViewer.ScrollToBottom();
        }

        private string GetAutomaticResponse(string userMessage)
        {
            var message = userMessage.ToLower();
            
            if (message.Contains("thèm") || message.Contains("muốn hút"))
            {
                return "Tôi hiểu cảm giác này. Hãy thử:\n" +
                       "1. Thở sâu 10 lần\n" +
                       "2. Uống nước lạnh\n" +
                       "3. Đi bộ 5 phút\n" +
                       "4. Nghĩ về lý do bạn cai thuốc\n\n" +
                       "Cảm giác thèm sẽ qua đi trong 3-5 phút!";
            }
            else if (message.Contains("căng thẳng") || message.Contains("stress"))
            {
                return "Căng thẳng là nguyên nhân chính khiến người ta muốn hút thuốc lại.\n\n" +
                       "Một số cách giảm stress:\n" +
                       "• Tập yoga hoặc thiền\n" +
                       "• Nghe nhạc thư giãn\n" +
                       "• Tập thể dục nhẹ\n" +
                       "• Nói chuyện với bạn bè\n\n" +
                       "Bạn có muốn tôi hướng dẫn bài tập thở không?";
            }
            else if (message.Contains("tăng cân") || message.Contains("béo"))
            {
                return "Tăng cân nhẹ trong giai đoạn đầu cai thuốc là bình thường.\n\n" +
                       "Tips để kiểm soát cân nặng:\n" +
                       "• Ăn nhiều rau xanh và trái cây\n" +
                       "• Uống nhiều nước\n" +
                       "• Tập thể dục đều đặn\n" +
                       "• Tránh đồ ăn vặt có đường\n\n" +
                       "Cân nặng sẽ ổn định sau 3-6 tháng.";
            }
            else if (message.Contains("bao lâu") || message.Contains("khi nào"))
            {
                return "Timeline cai thuốc điển hình:\n\n" +
                       "🕐 20 phút: Nhịp tim bình thường\n" +
                       "🕐 12 giờ: CO trong máu giảm\n" +
                       "📅 2-3 ngày: Nicotine bài tiết hết\n" +
                       "📅 2-4 tuần: Tuần hoàn cải thiện\n" +
                       "📅 1-9 tháng: Phổi phục hồi\n" +
                       "📅 1 năm: Nguy cơ tim mạch giảm 50%\n\n" +
                       "Mỗi người khác nhau, nhưng bạn đang đi đúng hướng!";
            }
            else
            {
                return "Cảm ơn bạn đã chia sẻ. Tôi sẽ phân tích tình huống của bạn và đưa ra lời khuyên phù hợp.\n\n" +
                       "Trong thời gian chờ đợi, bạn có thể:\n" +
                       "• Xem lại kế hoạch cai thuốc\n" +
                       "• Thực hiện bài tập thở\n" +
                       "• Uống trà thảo mộc\n\n" +
                       "Tôi sẽ phản hồi chi tiết trong vòng 30 phút!";
            }
        }

        private void AttachFileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tính năng đính kèm file:\n\n" +
                          "📷 Hình ảnh (JPG, PNG)\n" +
                          "📄 Tài liệu (PDF, DOC)\n" +
                          "🎵 Ghi âm (MP3, WAV)\n\n" +
                          "Dung lượng tối đa: 10MB", 
                          "Đính kèm file", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void ViewCoachProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("👨‍⚕️ Dr. Nguyễn Văn Minh\n\n" +
                          "📚 Học vấn:\n" +
                          "• Bác sĩ Đa khoa - ĐH Y Hà Nội\n" +
                          "• Thạc sĩ Sức khỏe Cộng đồng\n" +
                          "• Chứng chỉ Tư vấn Cai thuốc lá Quốc tế\n\n" +
                          "💼 Kinh nghiệm:\n" +
                          "• 12 năm làm việc trong lĩnh vực\n" +
                          "• Đã hỗ trợ 2,500+ người cai thuốc thành công\n" +
                          "• Tác giả 15 bài báo khoa học\n\n" +
                          "🏆 Thành tích:\n" +
                          "• Giải thưởng Bác sĩ xuất sắc 2023\n" +
                          "• Top 10 chuyên gia cai thuốc Việt Nam", 
                          "Hồ sơ chuyên gia", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void EmergencySupportButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("🆘 HỖ TRỢ KHẨN CẤP 🆘\n\n" +
                                       "Bạn có đang trong tình huống khẩn cấp?\n\n" +
                                       "• Cảm thấy muốn hút thuốc rất mạnh\n" +
                                       "• Tâm trạng rất tệ\n" +
                                       "• Cần hỗ trợ ngay lập tức\n\n" +
                                       "Chúng tôi sẽ kết nối bạn với chuyên gia trong vòng 5 phút!", 
                                       "Hỗ trợ khẩn cấp", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Warning);
            
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("✅ Đã gửi yêu cầu hỗ trợ khẩn cấp!\n\n" +
                              "Trong khi chờ đợi:\n" +
                              "📞 Hotline 24/7: 1900 1234\n" +
                              "🌊 Thực hiện kỹ thuật thở 4-7-8\n" +
                              "💧 Uống một ly nước lạnh\n" +
                              "🚶 Đi bộ hoặc thay đổi môi trường\n\n" +
                              "Chuyên gia sẽ liên hệ trong 5 phút!", 
                              "Yêu cầu đã được gửi", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private void CallSupportButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("📞 DỊCH VỤ TƯ VẤN QUA ĐIỆN THOẠI\n\n" +
                          "🕐 Giờ làm việc: 6:00 - 22:00 hàng ngày\n" +
                          "📞 Hotline: 1900 1234\n" +
                          "⏱️ Thời gian chờ trung bình: < 3 phút\n\n" +
                          "💰 Chi phí:\n" +
                          "• Thành viên Cơ bản: 50,000đ/cuộc gọi\n" +
                          "• Thành viên Premium: MIỄN PHÍ\n" +
                          "• Thành viên VIP: MIỄN PHÍ + Ưu tiên\n\n" +
                          "Bạn có muốn đặt lịch gọi không?", 
                          "Tư vấn qua điện thoại", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void EmailSupportButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("📧 HỖ TRỢ QUA EMAIL\n\n" +
                          "📮 Địa chỉ: support@smokingcessation.com\n" +
                          "⏱️ Thời gian phản hồi: 4-8 giờ\n" +
                          "📝 Nội dung có thể gửi:\n\n" +
                          "• Câu hỏi chi tiết về cai thuốc\n" +
                          "• Chia sẻ tình huống cụ thể\n" +
                          "• Yêu cầu tài liệu hỗ trợ\n" +
                          "• Báo cáo vấn đề kỹ thuật\n\n" +
                          "💡 Tips: Càng mô tả chi tiết, câu trả lời càng hữu ích!", 
                          "Hỗ trợ qua email", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void AIChatButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🤖 TRỢ LÝ AI - SMOKINGBOT\n\n" +
                          "✨ Có thể giúp bạn:\n" +
                          "• Trả lời câu hỏi cơ bản 24/7\n" +
                          "• Đưa ra lời khuyên tức thì\n" +
                          "• Hướng dẫn kỹ thuật thở\n" +
                          "• Đánh giá mức độ thèm thuốc\n" +
                          "• Đề xuất hoạt động thay thế\n\n" +
                          "⚠️ Lưu ý: AI không thay thế tư vấn y tế chuyên nghiệp\n\n" +
                          "Tính năng sẽ được ra mắt sớm!", 
                          "Chat với AI", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void FAQ_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var topic = button?.Tag?.ToString();
            
            var answer = topic switch
            {
                "craving" => "🆘 VƯỢT QUA CƠN THÈM THUỐC\n\n" +
                           "⏰ Cơn thèm thường kéo dài 3-5 phút\n\n" +
                           "✅ Chiến lược 4D:\n" +
                           "• DELAY (Trì hoãn): Đợi 10 phút\n" +
                           "• DEEP BREATHE (Thở sâu): 4-7-8\n" +
                           "• DRINK (Uống nước): Nước lạnh\n" +
                           "• DO (Làm gì đó): Đi bộ, gọi điện\n\n" +
                           "💪 Mỗi lần vượt qua = mạnh mẽ hơn!",
                
                "weight" => "⚖️ TĂNG CÂN KHI CAI THUỐC\n\n" +
                          "📊 Thống kê:\n" +
                          "• 80% người cai thuốc tăng 2-4kg\n" +
                          "• Tăng cân nhiều nhất trong 3 tháng đầu\n" +
                          "• Cân nặng ổn định sau 6-12 tháng\n\n" +
                          "🎯 Nguyên nhân:\n" +
                          "• Trao đổi chất chậm lại\n" +
                          "• Ăn thay thế cho thuốc lá\n" +
                          "• Vị giác phục hồi → ăn ngon hơn\n\n" +
                          "✅ Giải pháp:\n" +
                          "• Tập thể dục đều đặn\n" +
                          "• Ăn nhiều rau củ quả\n" +
                          "• Uống nhiều nước\n" +
                          "• Tránh đồ ngọt",
                
                "timeline" => "⏰ TIMELINE CAI THUỐC\n\n" +
                            "🕐 20 phút: Nhịp tim + huyết áp bình thường\n" +
                            "🕐 12 giờ: CO trong máu giảm về mức bình thường\n" +
                            "📅 1 ngày: Nguy cơ đau tim giảm\n" +
                            "📅 2 ngày: Vị giác & khứu giác cải thiện\n" +
                            "📅 3 ngày: Nicotine bài tiết hết khỏi cơ thể\n" +
                            "📅 2-3 tuần: Tuần hoàn máu cải thiện\n" +
                            "📅 1-9 tháng: Ho và khó thở giảm\n" +
                            "📅 1 năm: Nguy cơ bệnh tim giảm 50%\n" +
                            "📅 5 năm: Nguy cơ đột quỵ = người không hút\n" +
                            "📅 10 năm: Nguy cơ ung thư phổi giảm 50%",
                
                "work" => "💼 CAI THUỐC VÀ CÔNG VIỆC\n\n" +
                        "⚠️ Ảnh hưởng có thể có:\n" +
                        "• Khó tập trung trong 1-2 tuần đầu\n" +
                        "• Thay đổi tâm trạng\n" +
                        "• Mệt mỏi ban đầu\n\n" +
                        "✅ Cách xử lý:\n" +
                        "• Thông báo với đồng nghiệp thân thiết\n" +
                        "• Nghỉ giải lao thường xuyên hơn\n" +
                        "• Uống nhiều nước\n" +
                        "• Tránh caffeine quá nhiều\n" +
                        "• Đi bộ thay vì hút thuốc giải lao\n\n" +
                        "💡 Sau 2-4 tuần: Tập trung tốt hơn trước!",
                
                _ => "Thông tin không có sẵn."
            };
            
            MessageBox.Show(answer, "Câu hỏi thường gặp", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ViewAllFAQButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("📚 TẤT CẢ CÂU HỎI THƯỜNG GẶP\n\n" +
                          "Danh mục chính:\n\n" +
                          "🔥 Cơn thèm thuốc (15 câu hỏi)\n" +
                          "💊 Triệu chứng cai thuốc (12 câu hỏi)\n" +
                          "⚖️ Quản lý cân nặng (8 câu hỏi)\n" +
                          "💼 Cai thuốc và công việc (6 câu hỏi)\n" +
                          "👨‍👩‍👧‍👦 Gia đình và xã hội (10 câu hỏi)\n" +
                          "🏥 Sức khỏe và y tế (20 câu hỏi)\n" +
                          "💰 Chi phí và lợi ích (5 câu hỏi)\n" +
                          "🎯 Động lực và tâm lý (12 câu hỏi)\n\n" +
                          "Tổng cộng: 88 câu hỏi với câu trả lời chi tiết!", 
                          "Tất cả FAQ", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void Resource_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var resource = button?.Tag?.ToString();
            
            var content = resource switch
            {
                "guide" => "📖 HƯỚNG DẪN CAI THUỐC TỪNG BƯỚC\n\n" +
                         "Tài liệu gồm 50 trang với:\n" +
                         "• Chuẩn bị tâm lý trước khi cai\n" +
                         "• 4 phương pháp cai thuốc phổ biến\n" +
                         "• Xử lý 20 tình huống khó khăn\n" +
                         "• Kế hoạch chi tiết cho 12 tuần đầu\n" +
                         "• Checklist và bài tập thực hành\n\n" +
                         "📥 Tải về: PDF miễn phí",
                
                "breathing" => "🧘 BÀI TẬP THỞ VÀ THIỀN\n\n" +
                             "Bộ sưu tập gồm:\n" +
                             "• 10 kỹ thuật thở giảm stress\n" +
                             "• Video hướng dẫn thiền 5-20 phút\n" +
                             "• Âm thanh thiên nhiên thư giãn\n" +
                             "• Ứng dụng nhắc nhở thở đều\n\n" +
                             "🎵 Định dạng: Video + Audio",
                
                "nutrition" => "🍎 CHẾ ĐỘ ĂN UỐNG LÀNH MẠNH\n\n" +
                             "Nội dung:\n" +
                             "• 30 thực phẩm giảm thèm nicotine\n" +
                             "• Thực đơn 4 tuần cho người cai thuốc\n" +
                             "• Công thức smoothie detox\n" +
                             "• Hướng dẫn kiểm soát cân nặng\n" +
                             "• Tips ăn uống khi stress\n\n" +
                             "👩‍🍳 Bonus: 20 công thức nấu ăn",
                
                "exercise" => "💪 BÀI TẬP THỂ DỤC PHÙ HỢP\n\n" +
                            "Chương trình tập luyện:\n" +
                            "• Bài tập cardio 10-30 phút\n" +
                            "• Yoga cho người mới bắt đầu\n" +
                            "• Strength training tại nhà\n" +
                            "• Stretching giảm căng thẳng\n" +
                            "• Kế hoạch tập 12 tuần\n\n" +
                            "📱 App đi kèm: Tracking + Reminder",
                
                _ => "Tài liệu không có sẵn."
            };
            
            MessageBox.Show(content, "Tài liệu hữu ích", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StarRating_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var rating = int.Parse(button?.Tag?.ToString() ?? "0");
            
            currentRating = rating;
            UpdateStarDisplay();
            
            MessageBox.Show($"Cảm ơn bạn đã đánh giá {rating} sao!\n\n" +
                          "Đánh giá của bạn giúp chúng tôi cải thiện chất lượng dịch vụ.", 
                          "Cảm ơn đánh giá", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void UpdateStarDisplay()
        {
            var stars = new Button[] { Star1, Star2, Star3, Star4, Star5 };
            
            for (int i = 0; i < stars.Length; i++)
            {
                if (i < currentRating)
                {
                    stars[i].Content = "⭐";
                    stars[i].Opacity = 1.0;
                }
                else
                {
                    stars[i].Content = "☆";
                    stars[i].Opacity = 0.3;
                }
            }
        }

        private void DetailedFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var feedbackWindow = new FeedbackWindow();
                feedbackWindow.Show();
            }
            catch
            {
                MessageBox.Show("Tính năng đánh giá chi tiết đang được phát triển.\n\n" +
                              "Bạn có thể:\n" +
                              "• Gửi email: feedback@smokingcessation.com\n" +
                              "• Gọi hotline: 1900 1234\n" +
                              "• Chat với chuyên gia trong ứng dụng", 
                              "Phản hồi chi tiết", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }
    }
}