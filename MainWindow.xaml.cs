using System.Windows;

namespace PushBee
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnSuccessClick(object sender, RoutedEventArgs e)
        {
            ToastManager.Show("操作成功！数据已保存到服务器操作成功！数据已保存到服务器操作成功！数据已保存到服务器", NotificationType.Success);
        }

        private async void OnErrorClick(object sender, RoutedEventArgs e)
        {
            ToastManager.Show("操作失败！请检查网络连接", NotificationType.Error);
        }

        private async void OnInfoClick(object sender, RoutedEventArgs e)
        {
            ToastManager.Show("系统将在今晚进行维护，预计持续2小时", NotificationType.Info);
        }

        private async void OnWarningClick(object sender, RoutedEventArgs e)
        {
            ToastManager.Show("您的密码即将过期，请及时修改", NotificationType.Warning);
        }
    }
}
