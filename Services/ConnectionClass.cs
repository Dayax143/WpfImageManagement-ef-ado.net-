using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfEFProfile.Services
{
	public class ConnectionClass
	{
		string connectionString = Properties.Settings.Default.sqlConnection;

		////this checks if the app has connection configuration (if yes go to login, else configure the connection)
		public static bool IsConnectionValid()
		{
			try
			{
				string connString = Properties.Settings.Default.sqlConnection;
				if (string.IsNullOrEmpty(connString))
					return false; // No saved connection

				using (SqlConnection con = new SqlConnection(connString))
				{
					con.Open();
					return true; // Connection successful
				}
			}
			catch (Exception)
			{
				return false; // Connection failed
			}
		}

		//  this function is for creating dynamic connection using application settings, properties
		public void ConfigureConnection(string newConnectionString)
		{
			try
			{
				// if connection is in the resources use this variable value THIS WILL SAVE IN THE RESOURCES
				//Application.Current.Resources["DBConnectionString"] = newConnectionString;

				//else if connection is in the properties.settings use this variable value
				Properties.Settings.Default.sqlConnection = newConnectionString;
				Properties.Settings.Default.Save();
				// Save the connection string to a secure location, e.g., encrypted file or settings

				MessageBox.Show("Test and GO ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void Execute_Queries(string _query)
		{
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				try
				{
					con.Open();
					SqlCommand cmd = new SqlCommand(_query, con);
					cmd.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Not executed for " + ex.Data);
				}
				finally
				{
					con.Close();
				}
			}
		}
	}
}
