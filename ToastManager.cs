using System;
using System.Windows;
using System.Windows.Threading;

namespace PushBee
{
    public static class ToastManager
    {
        private static ToastArea? _toastArea;

        public static void Show(string message, NotificationType type)
        {
            if (Application.Current == null) return;

            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                EnsureToastArea();
                _toastArea!.AddToast(message, type);
            });
        }

        private static void EnsureToastArea()
        {
            if (_toastArea == null)
            {
                _toastArea = new ToastArea();
                _toastArea.Closed += (s, e) => _toastArea = null;
            }
        }
    }
}
