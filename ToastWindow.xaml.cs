using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace PushBee
{
    public partial class ToastWindow : Window
    {
        /// <summary>
        /// 自动关闭定时器
        /// </summary>
        private DispatcherTimer? _autoCloseTimer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">通知消息</param>
        /// <param name="type">通知类型</param>
        public ToastWindow(string message, NotificationType type)
        {
            InitializeComponent();
            // 修改UI元素
            MessageText.Text = message;
            ApplyTheme(type);
            // 
            Setup();
        }

        /// <summary>
        /// 设置通知窗口的消息和主题
        /// </summary>
        private void Setup()
        {
            StartAutoCloseTimer();
        }

        /// <summary>
        /// 根据通知类型应用主题样式
        /// </summary>
        /// <param name="type">通知类型</param>
        private void ApplyTheme(NotificationType type)
        {
            string bgColor = "";
            string borderColor = "";
            string textColor = "";
            string iconColor = "";
            string iconData = "";

            switch (type)
            {
                case NotificationType.Success:
                    bgColor = "#ECFDF5";
                    borderColor = "#A7F3D0";
                    textColor = "#065F46";
                    iconColor = "#10B981";
                    iconData = "M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z";
                    break;
                case NotificationType.Error:
                    bgColor = "#FEF2F2";
                    borderColor = "#FECACA";
                    textColor = "#991B1B";
                    iconColor = "#EF4444";
                    iconData = "M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z";
                    break;
                case NotificationType.Info:
                    bgColor = "#EFF6FF";
                    borderColor = "#BFDBFE";
                    textColor = "#1E40AF";
                    iconColor = "#3B82F6";
                    iconData = "M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z";
                    break;
                case NotificationType.Warning:
                    bgColor = "#FFFBEB";
                    borderColor = "#FDE68A";
                    textColor = "#92400E";
                    iconColor = "#F59E0B";
                    iconData = "M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z";
                    break;
            }

            var converter = new BrushConverter();
            MainBorder.Background = (SolidColorBrush)converter.ConvertFrom(bgColor)!;
            MainBorder.BorderBrush = (SolidColorBrush)converter.ConvertFrom(borderColor)!;
            MessageText.Foreground = (SolidColorBrush)converter.ConvertFrom(textColor)!;
            IconPath.Fill = (SolidColorBrush)converter.ConvertFrom(iconColor)!;
            IconPath.Data = Geometry.Parse(iconData);
        }

        /// <summary>
        /// 启动自动关闭定时器
        /// </summary>
        private void StartAutoCloseTimer()
        {
            _autoCloseTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5) // 5秒后自动关闭
            };
            _autoCloseTimer.Tick += (s, e) => CloseToast();
            _autoCloseTimer.Start();
        }

        /// <summary>
        /// 窗口加载事件处理程序
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取并开始滑入动画
            var anim = FindResource("SlideIn") as Storyboard;
            anim?.Begin(this);
        }

        /// <summary>
        /// 关闭按钮点击事件处理程序
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseToast();
        }

        /// <summary>
        /// 关闭通知窗口的方法
        /// </summary>
        private void CloseToast()
        {
            // 停止自动关闭定时器
            _autoCloseTimer?.Stop();
            _autoCloseTimer = null;
            // 获取并开始淡出动画
            var anim = FindResource("FadeOut") as Storyboard;
            anim?.Begin(this);
        }

        /// <summary>
        /// 淡出动画完成事件处理程序
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void FadeOut_Completed(object sender, EventArgs e)
        {
            // 关闭窗口，Closed事件会自动调用ToastManager.Remove(this)
            this.Close();
        }
    }
}
