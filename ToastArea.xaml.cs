using System;
using System.Windows;
using System.Windows.Media;

namespace PushBee
{
    public partial class ToastArea : Window
    {
        public ToastArea()
        {
            InitializeComponent();
            Loaded += ToastArea_Loaded;
        }

        private void ToastArea_Loaded(object sender, RoutedEventArgs e)
        {
            Reposition();
        }

        public void Reposition()
        {
            var workArea = SystemParameters.WorkArea;
            this.Left = workArea.Right - this.Width;
            this.Top = workArea.Top;
        }

        public void AddToast(string message, NotificationType type)
        {
            var toast = new Toast
            {
                Message = message,
                NotificationType = type,
                Margin = new Thickness(0, 0, 0, 10), // Spacing between toasts
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            toast.Closed += Toast_Closed;
            ToastContainer.Children.Add(toast);
            
            // Ensure window is visible and positioned
            this.Show();
            Reposition();
        }

        private void Toast_Closed(object sender, RoutedEventArgs e)
        {
            if (sender is Toast toast)
            {
                toast.Closed -= Toast_Closed;
                ToastContainer.Children.Remove(toast);

                if (ToastContainer.Children.Count == 0)
                {
                    this.Hide();
                }
            }
        }
    }
}
