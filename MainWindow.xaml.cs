using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Data.SqlClient;
using System.IO;

namespace WpfEFProfile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private string imagePath = string.Empty;

        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    imagePath = openFileDialog.FileName;
                    imgProfile.Source = new BitmapImage(new Uri(imagePath));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        string con = Properties.Settings.Default.sqlConnection;

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            using (SqlConnection conn = new SqlConnection(con))
            {
                try
                {
                    string query = "INSERT INTO test (name, quantity, audit_user, profile) VALUES (@name, @quantity, @audit_user, @profile)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@quantity", int.Parse(txtQuantity.Text));
                        cmd.Parameters.AddWithValue("@audit_user", txtAuditUser.Text);

                        // Convert image to byte array
                        byte[] imageBytes = File.ReadAllBytes(imagePath); // Assume imagePath stores uploaded file path
                        cmd.Parameters.AddWithValue("@profile", imageBytes);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully inserted");
                    }
                }


                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void LoadProfileImage(int id)
        {
            byte[] imageBytes = null;

            using (SqlConnection conn = new SqlConnection(con))
            {
                string query = "SELECT profile FROM test WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        imageBytes = (byte[])result;
                    }
                }
            }

            if (imageBytes != null)
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    viewProfile.Source = bitmap;  // Assign the image to your Image control
                }
            }
        }


        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            LoadProfileImage(int.Parse(txtLoad.Text));
        }
    }
}