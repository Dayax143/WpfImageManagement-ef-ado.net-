using System.IO;
using System.Windows;
using WpfEFProfile.EF;
using Microsoft.Win32;
using System.Media;
using System.Windows.Media;

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
    }
}
