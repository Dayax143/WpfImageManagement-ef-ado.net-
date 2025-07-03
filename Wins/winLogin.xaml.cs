using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using WpfEFProfile.Services;
using WpfEFProfile;
using WpfEFProfile.EF;

namespace WpfEFProfile.Wins
{
	/// <summary>
	/// Interaction logic for winLogin.xaml
	/// </summary>
	public partial class winLogin : Window
	{
		//this for getting connections from connection class
		//ConnectionClass conClass = new ConnectionClass();

		//this for configuring the connection locally
		//private readonly string connectionString = "Server=172.16.168.212;Database=ArfiPlusPerfurmes; user id='sa'; password=123; trustservercertificate=true;";

		//loading connection from configuration file in app.config
		//string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;


		//from the app.xaml connection
		//string connectionString = Properties.Settings.Default.sqlConnection;
		string connectionString = "Server=.; Database=wbh_minisystem; user id=sa; password=123; trustservercertificate=true";


		//this for loading the methods for database operations
		//DBquery DBquery = new DBquery();

		public winLogin()
		{
			InitializeComponent();
		}

		public void saveSession(string ses_status)
		{
			using (var context = new MyAppContext())
			{
				try
				{
					Sessions session = new Sessions
					{
						ses_user = Properties.Settings.Default.audit_user,
						ses_time = DateTime.Now,
						ses_status = ses_status
					};

					context.sessions.Add(session);
					context.SaveChanges();
					MessageBox.Show("Session registered");
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		public void loginFunction(string username, string password)
		{
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				try
				{
					string query = "SELECT COUNT(*) FROM tblUser WHERE Username = @username AND Password = @password";

					SqlCommand cmd = new SqlCommand(query, con);
					cmd.Parameters.AddWithValue("@username", username);
					cmd.Parameters.AddWithValue("@password", password); // Consider hashing passwords!
					con.Open();
					int userExists = (int)cmd.ExecuteScalar();

					if (userExists > 0)
					{
						MessageBox.Show("Login Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
						MainWindow winDashboard = new MainWindow();
						//saveSession("Success");
						Hide();
						winDashboard.Show();
						Close();
					}
					else
					{
						MessageBox.Show("Invalid credentials. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						//saveSession("Failed");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void btnLogin_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default.audit_user = UsernameTextBox.Text;
			loginFunction(UsernameTextBox.Text, PasswordBox.Password);

		}
	}
}
