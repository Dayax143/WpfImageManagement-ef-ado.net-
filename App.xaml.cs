using System.Configuration;
using System.Data;
using System.Windows;
using WpfEFProfile.Services;
using WpfEFProfile.Wins;

namespace WpfEFProfile
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (ConnectionClass.IsConnectionValid())
            {
                var loginWindow = new winLogin();
                loginWindow.Show();
            }
            else
            {
                var configWindow = new winConfig();
                configWindow.Show();
            }

            //var configWindow = new winAdmin();
            //configWindow.Show();
        }
    }
}
