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
        private List<TblLora> loraList;
        private int currentIndex = 0;
        private string imagePath = string.Empty;

        public winAdmin()
        {
            InitializeComponent();
            loraList = new List<TblLora>();
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
    }
}
