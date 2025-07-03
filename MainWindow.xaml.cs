using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfEFProfile.EF;

namespace WpfEFProfile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string con = Properties.Settings.Default.sqlConnection;
        MyAppContext context = new MyAppContext();
        public MainWindow()
        {
            InitializeComponent();
        }


        //🔹 Function 3 (Raw SQL Execution)
        //✅ Direct SQL execution is faster for large inserts.
        //✅ Avoids EF overhead, making it lightweight.
        //❌ Manually manages SQL queries, increasing complexity.
        //❌ Lacks automatic validation & entity tracking (EF provides these).
        //❌ Does not use async, which could slow UI under heavy loads.
        public void saveImageAdonet()
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
                        cmd.Parameters.AddWithValue("@audit_user", Properties.Settings.Default.audit_user);

                        // Convert image to byte array
                        byte[] imageBytes = File.ReadAllBytes(imagePath); // Assume imagePath stores uploaded file path
                        cmd.Parameters.AddWithValue("@profile", imageBytes);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully inserted");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //Function 2 (EF with Async)
        //✅ Asynchronous operations(SaveChangesAsync()), preventing UI freezing.
        //✅ Efficient image reading(File.ReadAllBytesAsync()), improving responsiveness.
        //✅ Uses modern EF features (AddAsync()) for better performance.
        //✅ Transaction management, meaning partial inserts wont happen.
        //❌ Direct SQL execution is faster for large inserts.
        public async Task InsertProfileAsync()
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);  // Efficient async read

                Test newProfile = new Test
                {
                    Name = txtName.Text.Trim(),
                    Quantity = int.TryParse(txtQuantity.Text, out int qty) ? qty : throw new Exception("Invalid quantity."),
                    Audit_User = Properties.Settings.Default.audit_user,
                    Profile = imageBytes
                };


                await context.test.AddAsync(newProfile);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();  // Ensure transaction is committed

                MessageBox.Show("Successfully inserted!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();  // Rollback on failure
                MessageBox.Show($"Error: {ex.InnerException?.Message ?? ex.Message}");
            }
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

        //🔹 Function 1 (EF with Transactions)
        //✅ Transaction Handling ensures consistency if an error occurs.
        //✅ EF maintains ORM abstraction, simplifying code.
        //❌ Uses synchronous SaveChanges(), which can block UI.
        //❌ Reads image synchronously (File.ReadAllBytes()), causing UI lag.
        public void saveImageEF()
        {
            using (var context = new MyAppContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        byte[] imageBytes = File.ReadAllBytes(imagePath);  // Read image as byte[]
                        Test testadd = new Test
                        {
                            Name = txtName.Text,
                            Quantity = int.Parse(txtQuantity.Text),
                            Audit_User = "yes",
                            Profile = imageBytes
                        };
                        context.Add(testadd);
                        context.SaveChanges();
                        MessageBox.Show("Sucessfully Inserted");

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message, "Not updated, transaction rolled back");
                        MessageBox.Show(ex.InnerException.ToString());
                    }
                }

            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await InsertProfileAsync();
        }

        public void loadProfileImageAdonet(int id)
        {
            {
                try
                {
                    using (var context = new MyAppContext())
                    {
                        var imageBytes = context.test
                                                .Where(t => t.Id == id)
                                                .Select(t => t.Profile)
                                                .FirstOrDefault();

                        if (imageBytes != null)
                        {
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.StreamSource = ms;
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();
                                viewProfile.Source = bitmap; // Assign the image to the Image control
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            loadProfileImageAdonet(int.Parse(txtLoad.Text));
        }
    }
}