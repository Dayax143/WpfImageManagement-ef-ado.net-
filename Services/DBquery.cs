using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfEFProfile.Services
{
	public class DBquery
	{
		private readonly string connectionString = "Server=rt\\rtser;Database=testDB; user id='sa'; password=sa@123; trustservercertificate=true;";

		// this holds me the the execution function
		public bool ExecuteQuery(string query)
		{
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				try
				{
					con.Open();
					using (SqlCommand cmd = new SqlCommand(query, con))
					{
						cmd.ExecuteNonQuery();
						return true; // Success
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error: {ex.Message}", "Query Execution Failed");
					return false; // Failure
				}
			}
		}

		// this holds me the the datagridveiw load function
		public DataTable GetData(string query)
		{
			DataTable dt = new DataTable();
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				try
				{
					con.Open();
					using (SqlCommand cmd = new SqlCommand(query, con))
					{
						SqlDataAdapter adapter = new SqlDataAdapter(cmd);
						adapter.Fill(dt);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error: {ex.Message}", "Data Fetch Failed");
				}
			}
			return dt;
		}

		public static void OpenEditWindow(DataGrid dataGrid, Window editWindow)
		{
			if (dataGrid.SelectedItem is DataRowView row)
			{
				foreach (var control in editWindow.GetType().GetProperties())
				{
					if (control.Name.StartsWith("txt") && row.Row.Table.Columns.Contains(control.Name.Substring(3)))
					{
						var textBox = control.GetValue(editWindow) as TextBox;
						if (textBox != null)
						{
							textBox.Text = row[control.Name.Substring(3)].ToString();
						}
					}
				}
				editWindow.ShowDialog();
			}
			else
			{
				MessageBox.Show("No item selected!", "Error");
			}
		}

	}
}
