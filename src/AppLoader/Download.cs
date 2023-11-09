using AppLoader.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace AppLoader
{
    public partial class Download : Form
    {

        public Download()
        {
            InitializeComponent();
        }

        private void Download_Load(object sender, EventArgs e)
        {
        }

        private async void Download_Shown(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;            
            progressBar1.Value = 0;
            progressBar1.Step = 1;
         
            try
            {
                var currenDirectory = Directory.GetCurrentDirectory();
                ReadConfig read = new();
                var config = read.readFile();
                FilesCopy copy = new();
                progressBar1.Maximum = copy.GetCountFiles(currenDirectory);
                
                var progress = new Progress<int>(p => progressBar1.Value = p);

                await Task.Run(() =>
                {                    
                    copy.CopyFileAsync(progress, config.TargetPath, currenDirectory);
                });

                Process.Start(config.TargetPath + "\\" + config.ProcessName);
                Application.Exit();
            }
            catch (FileNotFoundException)
            {
                string message = "Не найден файл конфигурации, проверьте наличие AppLoader.json";
                CloseByException(message);
            }
            catch (JsonException)
            {
                string message = "Ошибка чтения файла конфигурации, проверьте файл AppLoader.json";
                CloseByException(message);
            }
            catch (Win32Exception)
            {
                string message = $"Ошибка запуска финального приложения.";
                CloseByException(message);
            }
            catch
            {
                string message = "Необработанное исключение, приложение будет закрыто";
                CloseByException(message);
            }
        }

        private void CloseByException(string message)
        {
            var result = MessageBox.Show(message);
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
        }        
    }
}