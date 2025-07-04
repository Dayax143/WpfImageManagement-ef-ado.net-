using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfEFProfile.EF;

namespace WpfEFProfile
{
	/// <summary>
	/// Interaction logic for winAdmin.xaml
	/// </summary>
	public partial class winAdmin : Window
	{
		private List<TblLora> loraList;
		private int currentIndex = 0;

		public winAdmin()
		{
			InitializeComponent();
			loraList = new List<TblLora>();
			dpDate.SelectedDate = DateTime.Now; // Set default date
		}

		private void btnCreate_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				using var context = new MyAppContext();
				var newLora = new TblLora
				{
					CorSupply = txtCorSupply.Text,
					LoraSerial = txtSerial.Text,
					ReceiptRv = txtReceipt.Text,
					Status = txtStatus.Text,
					Refference = txtReference.Text,
					Date = dpDate.SelectedDate ?? DateTime.Now
				};

				if (int.TryParse(txtLoraId.Text, out int loraId))
				{
					newLora.LoraId = loraId;
				}
				else
				{
					MessageBox.Show("Invalid LoraId. Please enter a valid number.");
					return;
				}

				context.Set<TblLora>().Add(newLora); // safer for unmapped types
				context.SaveChanges();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnRead_Click(object sender, RoutedEventArgs e)
		{
			using var context = new MyAppContext();
			loraList = context.Set<TblLora>().Take(200).ToList();
			currentIndex = 0;
			DisplayCurrent();
		}

		private void btnUpdate_Click(object sender, RoutedEventArgs e)
		{
			using var context = new MyAppContext();
			var id = int.Parse(txtLoraId.Text);
			var lora = context.Set<TblLora>().Find(id);
			if (lora != null)
			{
				lora.CorSupply = txtCorSupply.Text;
				lora.LoraSerial = txtSerial.Text;
				lora.ReceiptRv = txtReceipt.Text;
				lora.Status = txtStatus.Text;
				lora.Refference = txtReference.Text;
				lora.Date = dpDate.SelectedDate ?? lora.Date;

				context.SaveChanges();
			}
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			using var context = new MyAppContext();
			var id = int.Parse(txtLoraId.Text);
			var lora = context.Set<TblLora>().Find(id);
			if (lora != null)
			{
				context.Set<TblLora>().Remove(lora);
				context.SaveChanges();
			}
		}

		private void DisplayCurrent()
		{
			if (loraList == null || !loraList.Any()) return;

			var item = loraList[currentIndex];
			txtLoraId.Text = item.LoraId.ToString();
			txtCorSupply.Text = item.CorSupply;
			txtSerial.Text = item.LoraSerial;
			txtReceipt.Text = item.ReceiptRv;
			txtStatus.Text = item.Status;
			txtReference.Text = item.Refference;
			dpDate.SelectedDate = item.Date;
		}

		private void btnNext_Click(object sender, RoutedEventArgs e)
		{
			if (currentIndex < loraList.Count - 1)
			{
				currentIndex++;
				DisplayCurrent();
			}
		}

		private void btnPrevious_Click(object sender, RoutedEventArgs e)
		{
			if (currentIndex > 0)
			{
				currentIndex--;
				DisplayCurrent();
			}
		}
	}
}
