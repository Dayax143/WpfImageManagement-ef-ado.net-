using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfEFProfile.EF;

namespace WpfEFProfile
{
    /// <summary>  
    /// Interaction logic for winAdmin.xaml  
    /// </summary>  
    public partial class winAdmin : Window
    {
        private List<Test> test;
        private int currentIndex = 0;
        private string imagePath = string.Empty;

        public winAdmin()
        {
            InitializeComponent();
            test = new List<Test>();
            dpDate.SelectedDate = DateTime.Now; // Set default date  
        }

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

        public async Task InsertProfileAsync()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name cannot be empty.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Invalid quantity. Please enter a positive number.");
                return;
            }

            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                MessageBox.Show("Please upload a valid image.");
                return;
            }

            try
            {
                byte[] imageBytes = await File.ReadAllBytesAsync(imagePath); // Efficient async read  

                using var context = new MyAppContext();
                using var transaction = await context.Database.BeginTransactionAsync();

                try
                {
                    Test newProfile = new Test
                    {
                        Name = txtName.Text.Trim(),
                        Quantity = qty,
                        Audit_User = Properties.Settings.Default.audit_user,
                        Profile = imageBytes
                    };

                    await context.test.AddAsync(newProfile);
                    await context.SaveChangesAsync();

                    await transaction.CommitAsync(); // Ensure transaction is committed  

                    MessageBox.Show("Successfully inserted!");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); // Rollback on failure  
                    MessageBox.Show($"Error: {ex.InnerException?.Message ?? ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while processing the image: {ex.Message}");
            }
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await InsertProfileAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        public void loadProfileImageAdonet(int id)
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
                            imgTestProfile.Source = bitmap; // Corrected to use 'imgProfile' instead of 'viewProfile'  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtSearch.Text, out int id))
                {
                    loadProfileImageAdonet(id);
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a valid numeric ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtSearch.Text, out int id))
                {
                    loadProfileAndDetails(id);
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a valid numeric ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void loadProfileAndDetails(int id)
        {
            try
            {
                using (var context = new MyAppContext())
                {
                    var profileData = context.test
                                             .Where(t => t.Id == id)
                                             .Select(t => new
                                             {
                                                 t.Name,
                                                 t.Quantity,
                                                 t.Profile
                                             })
                                             .FirstOrDefault();

                    if (profileData != null)
                    {
                        txtName.Text = profileData.Name;
                        txtQuantity.Text = profileData.Quantity.ToString();

                        if (profileData.Profile != null)
                        {
                            using (MemoryStream ms = new MemoryStream(profileData.Profile))
                            {
                                BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.StreamSource = ms;
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();
                                imgTestProfile.Source = bitmap;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No record found for the given ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        // function database
        public void BackupDatabase(string backupFile)
        {
            try
            {
                string backupQuery = $"BACKUP DATABASE [testDB] TO DISK = '{backupFile}' WITH FORMAT;";

                using var context = new MyAppContext();
                context.Database.ExecuteSqlRaw(backupQuery);

                MessageBox.Show($"Backup completed! File saved at: {backupFile}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // browse button
        private void btnBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = DateTime.Now;
            string formattedDate = date.ToString("yyyy_MM_dd_HH_mm_ss"); // Ensures a clean file name
            string defaultFolder = Properties.Settings.Default.BackupFolderPath; // Retrieve previous path

            // Ask the user whether to save in the default folder or choose a new path
            MessageBoxResult result = MessageBox.Show(
                $"Do you want to save the backup in the default folder?\n(Default: {defaultFolder})",
                "Backup Confirmation",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question
            );

            string backupFilePath;

            if (result == MessageBoxResult.Yes)
            {
                if (!string.IsNullOrEmpty(defaultFolder) && Directory.Exists(defaultFolder))
                {
                    // Save backup automatically in default folder
                    backupFilePath = Path.Combine(defaultFolder, $"testDB_{formattedDate}.bak");
                }
                else
                {
                    MessageBox.Show("The default backup folder is not set or does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else if (result == MessageBoxResult.No)
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "Backup Files (*.bak)|*.bak",
                    FileName = $"testDB_{formattedDate}.bak"
                };

                if (dialog.ShowDialog() == true)
                {
                    backupFilePath = dialog.FileName;
                    Properties.Settings.Default.BackupFolderPath = Path.GetDirectoryName(dialog.FileName);
                    Properties.Settings.Default.Save(); // Save the updated path

                    //lblDefaultPath.Text = backupFilePath;
                }
                else
                {
                    return; // Exit if the user cancels the dialog
                }
            }
            else
            {
                return; // Exit if the user cancels the confirmation dialog
            }

            // Proceed with the backup
            BackupDatabase(backupFilePath);

        }


        // this is restore function
        private void RestoreDatabase(string backupFile)
        {
            try
            {
                string connectionString = "server=.; database=master; user id=sa; password=123; trustservercertificate=true;";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if the database exists
                    string checkDbQuery = "IF DB_ID('testDB') IS NOT NULL SELECT 1 ELSE SELECT 0;";
                    using (SqlCommand checkCmd = new SqlCommand(checkDbQuery, con))
                    {
                        int dbExists = (int)checkCmd.ExecuteScalar();

                        string restoreQuery;
                        if (dbExists == 1)
                        {
                            // Database exists, replace it
                            restoreQuery = @$"
                            ALTER DATABASE [testDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                            RESTORE DATABASE [testDB] FROM DISK = '{backupFile}' WITH REPLACE;
                            ALTER DATABASE [testDB] SET MULTI_USER;";
                        }
                        else
                        {
                            // Database does not exist, restore directly
                            restoreQuery = @$"
                            RESTORE DATABASE [testDB] FROM DISK = '{backupFile}' WITH REPLACE;";
                        }

                        using (SqlCommand restoreCmd = new SqlCommand(restoreQuery, con))
                        {
                            restoreCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show($"Database restoration completed successfully! from\\ {backupFile}");
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: {ex.Message}");
            }
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Backup files (*.bak)|*.bak",
                };

                if (dialog.ShowDialog() == true)
                {
                    var path = dialog.FileName;
                    RestoreDatabase(path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new MyAppContext())
                {
                    if (!int.TryParse(txtSearch.Text, out currentIndex))
                    {
                        MessageBox.Show("Invalid ID.");
                        return;
                    }

                    var nextProfile = context.test
                                             .Where(t => t.Id > currentIndex)
                                             .OrderBy(t => t.Id)
                                             .Select(t => new { t.Id, t.Name, t.Quantity, t.Profile })
                                             .FirstOrDefault();

                    if (nextProfile != null)
                    {
                        txtSearch.Text = nextProfile.Id.ToString();
                        txtName.Text = nextProfile.Name;
                        txtQuantity.Text = nextProfile.Quantity.ToString();

                        if (nextProfile.Profile != null)
                        {
                            using var ms = new MemoryStream(nextProfile.Profile);
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = ms;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            imgTestProfile.Source = bitmap;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No more records found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new MyAppContext())
                {
                    if (!int.TryParse(txtSearch.Text, out currentIndex))
                    {
                        MessageBox.Show("Invalid ID.");
                        return;
                    }

                    var previousProfile = context.test
                                                 .Where(t => t.Id < currentIndex)
                                                 .OrderByDescending(t => t.Id)
                                                 .Select(t => new { t.Id, t.Name, t.Quantity, t.Profile })
                                                 .FirstOrDefault();

                    if (previousProfile != null)
                    {
                        txtSearch.Text = previousProfile.Id.ToString();
                        txtName.Text = previousProfile.Name;
                        txtQuantity.Text = previousProfile.Quantity.ToString();

                        if (previousProfile.Profile != null)
                        {
                            using var ms = new MemoryStream(previousProfile.Profile);
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = ms;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            imgTestProfile.Source = bitmap;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No previous records found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
