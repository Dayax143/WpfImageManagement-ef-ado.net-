using System.IO;
using System.Windows;
using WpfEFProfile.EF;
using Microsoft.Win32;
using System.Media;
using System.Windows.Media;
using System.Diagnostics;

namespace WpfEFProfile
{
    /// <summary>
    /// Interaction logic for winMedia.xaml
    /// </summary>
    public partial class winMedia : Window
    {
        public winMedia()
        {
            InitializeComponent();
        }
        public async Task SavePdfToDatabase(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var pdf = new tblMedia
            {
                FileName = fileInfo.Name,
                MimeType = "application/pdf",
                FileSize = fileInfo.Length,
                FileData = await File.ReadAllBytesAsync(filePath),
                UploadedAt = DateTime.Now,
                Description = "Uploaded PDF document"
            };

            using var context = new MyAppContext();
            context.tblMedia.Add(pdf);
            await context.SaveChangesAsync();
        }

        private byte[] _fileData;
        private long _fileSize;

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Audio Files (*.mp3;*.wav)|*.mp3;*.wav|All Files (*.*)|*.*"
                };

                if (dialog.ShowDialog() == true)
                {
                    txtFileName.Text = System.IO.Path.GetFileName(dialog.FileName);
                    txtMimeType.Text = GetMimeType(dialog.FileName);
                    _fileData = File.ReadAllBytes(dialog.FileName);
                    _fileSize = _fileData.Length;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var media = new tblMedia
                {
                    FileName = txtFileName.Text,
                    MimeType = txtMimeType.Text,
                    FileSize = _fileSize,
                    FileData = _fileData,
                    UploadedAt = DateTime.Now,
                    Description = txtDescription.Text
                };

                using (var context = new MyAppContext())
                {
                    context.tblMedia.Add(media);
                    context.SaveChanges();
                }

                MessageBox.Show("File uploaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private string GetMimeType(string filePath)
        {
            var ext = System.IO.Path.GetExtension(filePath).ToLower();
            return ext switch
            {
                ".wav" => "audio/wav",
                ".mp3" => "audio/mpeg",
                _ => "application/octet-stream"
            };
        }

        private void LoadMedia_Click(object sender, RoutedEventArgs e)
        {
            using var context = new MyAppContext();
            var mediaFiles = context.tblMedia.ToList();
            lstMedia.ItemsSource = mediaFiles;
        }

        private MediaPlayer mediaPlayer = new MediaPlayer(); // Declare at class level

        private async void PlaySelected_Click(object sender, RoutedEventArgs e)
        {
            if (lstMedia.SelectedItem is tblMedia selected && selected.MimeType.StartsWith("audio"))
            {
                string tempPath = Path.Combine(Path.GetTempPath(), selected.FileName);
                await File.WriteAllBytesAsync(tempPath, selected.FileData);

                mediaPlayer.Open(new Uri(tempPath, UriKind.Absolute));
                mediaPlayer.Play();
            }
            else
            {
                MessageBox.Show("Selected file is not playable audio.");
            }
        }
        private async void SaveSelected_Click(object sender, RoutedEventArgs e)
        {
            if (lstMedia.SelectedItem is tblMedia selected)
            {
                var dialog = new SaveFileDialog
                {
                    FileName = selected.FileName,
                    Filter = "All Files (*.*)|*.*"
                };
                if (dialog.ShowDialog() == true)
                {
                    await File.WriteAllBytesAsync(dialog.FileName, selected.FileData);
                    MessageBox.Show("Saved successfully.");
                }
            }
        }
        private bool IsVideo(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            return ext is ".mp4" or ".wmv" or ".avi" or ".mov";
        }

        private async void PlayVideo_Click(object sender, RoutedEventArgs e)
        {
            if (lstMedia.SelectedItem is tblMedia selected && IsVideo(selected.FileName))
            {
                string tempPath = Path.Combine(Path.GetTempPath(), selected.FileName);
                await File.WriteAllBytesAsync(tempPath, selected.FileData);

                videoPlayer.Source = new Uri(tempPath, UriKind.Absolute);
                videoPlayer.Play();
            }
            else
            {
                MessageBox.Show("Selected file is not a recognized video format.");
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Stop();
        }

        private void btnRewind_Click(object sender, RoutedEventArgs e)
        {
            if (videoPlayer.NaturalDuration.HasTimeSpan)
            {
                videoPlayer.Position -= TimeSpan.FromSeconds(10);
            }
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (videoPlayer.NaturalDuration.HasTimeSpan)
            {
                videoPlayer.Position += TimeSpan.FromSeconds(10);
            }
        }

        private async void UploadPdf_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                var fileInfo = new FileInfo(filePath);

                var pdf = new tblMedia
                {
                    FileName = fileInfo.Name,
                    MimeType = "application/pdf",
                    FileSize = fileInfo.Length,
                    FileData = await File.ReadAllBytesAsync(filePath),
                    UploadedAt = DateTime.Now,
                    Description = "Uploaded via Upload PDF button"
                };

                using var context = new MyAppContext();
                context.tblMedia.Add(pdf);
                await context.SaveChangesAsync();

                MessageBox.Show("PDF uploaded successfully.");
            }
        }

        private async void ViewPdf_Click(object sender, RoutedEventArgs e)
        {
            // Example: Get the latest PDF entry
            using var context = new MyAppContext();
            var pdf = context.tblMedia.OrderByDescending(p => p.UploadedAt).FirstOrDefault();

            if (pdf?.FileData != null)
            {
                string tempPath = Path.Combine(Path.GetTempPath(), pdf.FileName);
                await File.WriteAllBytesAsync(tempPath, pdf.FileData);

                Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("No PDF available to view.");
            }
        }
    }
}
