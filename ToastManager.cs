using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace PushBee
{
    public static class ToastManager
    {
        private static List<ToastWindow> _openWindows = [];

        public static void Show(string message, NotificationType type)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var toast = new ToastWindow(message, type);
                
                // Add to list before showing
                _openWindows.Add(toast);

                toast.Closed += (s, e) => Remove(toast);
                toast.SizeChanged += (s, e) => RepositionToasts();
                
                toast.Show();
                RepositionToasts();
            });
        }

        public static void Remove(ToastWindow toast)
        {
            if (Application.Current == null) 
            {
                return;
            }

            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (_openWindows.Contains(toast))
                {
                    _openWindows.Remove(toast);
                    RepositionToasts();
                }
            });
        }

        private static void RepositionToasts()
        {
            if (Application.Current == null) return;

            var workArea = SystemParameters.WorkArea;
            double rightMargin = 20;
            double topMargin = 20;
            double spacing = 5;
            
            double currentTop = workArea.Top + topMargin;

            foreach (var window in _openWindows.ToList()) // Use ToList to avoid modification errors if any
            {
                if (window.IsLoaded && window.Visibility == Visibility.Visible)
                {
                    window.Left = workArea.Right - window.Width - rightMargin;
                    window.Top = currentTop;
                    
                    currentTop += window.ActualHeight + spacing;
                }
            }
        }
    }
}
