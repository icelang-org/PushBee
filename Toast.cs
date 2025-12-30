using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace PushBee
{
    public class Toast : ContentControl
    {
        private DispatcherTimer? _autoCloseTimer;

        static Toast()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Toast), new FrameworkPropertyMetadata(typeof(Toast)));
        }

        public Toast()
        {
            Loaded += Toast_Loaded;
        }

        #region Events

        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(
            nameof(Closed), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Toast));

        public event RoutedEventHandler Closed
        {
            add { AddHandler(ClosedEvent, value); }
            remove { RemoveHandler(ClosedEvent, value); }
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty NotificationTypeProperty = DependencyProperty.Register(
            nameof(NotificationType), typeof(NotificationType), typeof(Toast), 
            new PropertyMetadata(NotificationType.Info, OnNotificationTypeChanged));

        public NotificationType NotificationType
        {
            get { return (NotificationType)GetValue(NotificationTypeProperty); }
            set { SetValue(NotificationTypeProperty, value); }
        }

        public static readonly DependencyProperty IconDataProperty = DependencyProperty.Register(
            nameof(IconData), typeof(Geometry), typeof(Toast), new PropertyMetadata(null));

        public Geometry IconData
        {
            get { return (Geometry)GetValue(IconDataProperty); }
            private set { SetValue(IconDataProperty, value); }
        }

        public static readonly DependencyProperty IconFillProperty = DependencyProperty.Register(
            nameof(IconFill), typeof(Brush), typeof(Toast), new PropertyMetadata(Brushes.Blue));

        public Brush IconFill
        {
            get { return (Brush)GetValue(IconFillProperty); }
            private set { SetValue(IconFillProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(Toast), new PropertyMetadata(new CornerRadius(6)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_CloseButton") is Button closeBtn)
            {
                closeBtn.Click += (s, e) => Close();
            }
            
            UpdateTheme(NotificationType);
        }

        private void Toast_Loaded(object sender, RoutedEventArgs e)
        {
            // Start SlideIn animation
            if (TryFindResource("SlideIn") is Storyboard anim)
            {
                anim.Begin(this);
            }

            StartAutoCloseTimer();
        }

        private static void OnNotificationTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Toast toast)
            {
                toast.UpdateTheme((NotificationType)e.NewValue);
            }
        }

        private void UpdateTheme(NotificationType type)
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
            Background = (SolidColorBrush)converter.ConvertFrom(bgColor)!;
            BorderBrush = (SolidColorBrush)converter.ConvertFrom(borderColor)!;
            Foreground = (SolidColorBrush)converter.ConvertFrom(textColor)!;
            IconFill = (SolidColorBrush)converter.ConvertFrom(iconColor)!;
            
            try 
            {
                IconData = Geometry.Parse(iconData);
            }
            catch 
            { 
                // Ignore parse errors 
            }
        }

        private void StartAutoCloseTimer()
        {
            _autoCloseTimer?.Stop();
            _autoCloseTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _autoCloseTimer.Tick += (s, e) => Close();
            _autoCloseTimer.Start();
        }

        public void Close()
        {
            _autoCloseTimer?.Stop();
            _autoCloseTimer = null;

            if (TryFindResource("FadeOut") is Storyboard anim)
            {
                if (anim.IsFrozen)
                {
                    anim = anim.Clone();
                    anim.Completed += (s, e) => RaiseEvent(new RoutedEventArgs(ClosedEvent));
                } 
                anim.Begin(this);
            }
            else
            {
                RaiseEvent(new RoutedEventArgs(ClosedEvent));
            }
        }
    }
}
