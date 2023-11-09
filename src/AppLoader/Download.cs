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
                string message = "�� ������ ���� ������������, ��������� ������� AppLoader.json";
                CloseByException(message);
            }
            catch (JsonException)
            {
                string message = "������ ������ ����� ������������, ��������� ���� AppLoader.json";
                CloseByException(message);
            }
            catch (Win32Exception)
            {
                string message = $"������ ������� ���������� ����������.";
                CloseByException(message);
            }
            catch
            {
                string message = "�������������� ����������, ���������� ����� �������";
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