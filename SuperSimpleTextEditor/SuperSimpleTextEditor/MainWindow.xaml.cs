using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace SuperSimpleTextEditor
{
    public partial class MainWindow : Window
    {
        private string fileDialogName = ""; // Път до текущия файл
        private bool isTextChanged = false; // Индикатор за незапазени промени

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                DefaultExt = "txt",
                Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*",
                Multiselect = false
            };

            if (fileDialog.ShowDialog() == true)
            {
                fileDialogName = fileDialog.FileName;
                try
                {
                    fileSpaceBox.Text = File.ReadAllText(fileDialogName);
                    fileNameBlock.Text = Path.GetFileName(fileDialogName);
                    isTextChanged = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Грешка при отваряне на файла: {ex.Message}", "Грешка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(fileSpaceBox.Text))
            {
                MessageBox.Show("Текстовият прозорец е празен. Няма какво да запазите!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(fileDialogName))
            {
                SaveFileAs_Click(sender, e);
                return;
            }

            SaveToFile(fileDialogName);
        }

        private void SaveFileAs_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "txt",
                Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                fileDialogName = saveFileDialog.FileName;
                SaveToFile(fileDialogName);
            }
        }

        private void SaveToFile(string path)
        {
            try
            {
                File.WriteAllText(path, fileSpaceBox.Text);
                fileNameBlock.Text = Path.GetFileName(path);
                isTextChanged = false;
                MessageBox.Show("Файлът е запазен успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка при запазване на файла: {ex.Message}", "Грешка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void fileSpaceBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isTextChanged = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isTextChanged)
            {
                var result = MessageBox.Show("Имате незапазени промени. Искате ли да запазите преди изход?", "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile_Click(sender, new RoutedEventArgs());
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
