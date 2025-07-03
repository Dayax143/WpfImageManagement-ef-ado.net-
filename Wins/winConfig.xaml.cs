using System.Windows;
using WpfEFProfile.Services;

namespace WpfEFProfile.Wins
{
	/// <summary>
	/// Interaction logic for winConfig.xaml
	/// </summary>
	public partial class winConfig : Window
	{
		// This holds everything about connections, settings and methods
		ConnectionClass connectionClass = new ConnectionClass();
		public winConfig()
		{
			InitializeComponent();
		}

		//  this function is for creating dynamic connection using application settings, properties
		private void btnSaveConfig_Click(object sender, RoutedEventArgs e)
		{
			string newConnectionString = $"Server={txtServer.Text};Database={txtDatabase.Text};User ID={txtUserID.Text};Password={txtPassword.Password};TrustServerCertificate=True;";
			connectionClass.ConfigureConnection(newConnectionString);
		}

		//  this function works with both is for creating dynamic connection using application resources(x:key)
		//  from the properties.settings.default.sqlconnection
		//  and from <!--<sys:String x:Key="DBConnectionString">Real Connection String</sys:String>-->
		private void btnContest_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// if connection is in the resources use this variable value
				//string savedConnString = (string)Application.Current.Resources["DBConnectionString"]; // Or get from App.config

				//else if connection is in the properties.settings use this variable value
				//string savedConnString = Properties.Settings.Default.SqlConnection;

				//This calls a bool method named(IsConnectionValid) in ConnectionClass which ensures if connection is established returns true, false
				if (ConnectionClass.IsConnectionValid())
				{
					MessageBox.Show("Connection Established sucessfully");
					winLogin login = new winLogin();
					login.Show();
					Close();
				}
				else
				{
					MessageBox.Show("Failed to connect");
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
