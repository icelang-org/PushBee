using System.Windows;

namespace PushBee
{
    public partial class ToastWindow : Window
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">通知消息</param>
        /// <param name="type">通知类型</param>
        public ToastWindow(string message, NotificationType type)
        {
            InitializeComponent();
            
            // 将逻辑委托给 Toast 控件
            ToastControl.Content = message;
            ToastControl.NotificationType = type;
            
            // 当 Toast 控件关闭时，关闭窗口
            ToastControl.Closed += (s, e) => this.Close();
        }
    }
}
