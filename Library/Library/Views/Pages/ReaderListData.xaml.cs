using Library.Class;
using Library.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Library.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReaderListData.xaml
    /// </summary>
    public partial class ReaderListData : Page
    {
        public List<Reader> Readers = null;

        public ReaderListData()
        {
            InitializeComponent();
        }

        public async Task<int> LoadReaderData()
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("token", App.UserToken);

            string data = await ApiControl.GetRequest("GetReaders", args);

            Readers = JsonConvert.DeserializeObject<List<Reader>>(data);
            ReaderDataList.ItemsSource = Readers;
            return 1;
        }

        private void AccountExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Вы точно хотите выйти?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                App.UserToken = null;
                NavigationService.Navigate(new Authorization());
            }
        }

        private void ReaderSearch_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void CSV_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string photoSavePATH = $"{Directory.GetCurrentDirectory()}/readersphoto";
            string data = "ФИО; Путь до фотографий читателей:\n";
            Directory.CreateDirectory(photoSavePATH);
            Directory.Delete(photoSavePATH, true);
            Directory.CreateDirectory(photoSavePATH);
            int i = 0;

            foreach (Reader reader in Readers)
            {
                string filePath = $"{photoSavePATH}/{reader.FirstName}_{i++}.jpeg";

                File.WriteAllBytes(filePath, reader.Photo);
                data += $"{reader.FullName};{filePath}\n";
            }

            File.WriteAllText($"{Directory.GetCurrentDirectory()}/readers.csv", data, Encoding.Default);
            MessageBox.Show("Данные успешно выгружены.");
        }

        private void ReaderSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Readers != null && ReaderSearchInput.Text != "")
            {
                List<Reader> searchData = Readers.Where(c =>
                    c.FullName.Contains(ReaderSearchInput.Text)).ToList();


                if (searchData.Count != 0)
                {
                    ReaderDataList.ItemsSource = searchData;
                }
                else
                {
                    MessageBox.Show("Читатели не найдены!");
                }
            }
            else if (ReaderSearchInput.Text == "")
            {
                ReaderDataList.ItemsSource = Readers;
            }
        }

        private async void ReaderTicket_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Reader reader = ReaderDataList.SelectedItem as Reader;
            if (reader == null) { MessageBox.Show("Выберите пользователя!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            LibraryСard ReaderCard = ApiControl.GetReaderLibraryCard(reader);
            await ReportWorker.ReaderWordGenerate(reader, ReaderCard);
        }

        private void LoadReader_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Reader reader = ReaderDataList.SelectedItem as Reader;

            LibraryСard ReaderCard = ApiControl.GetReaderLibraryCard(reader);
            NavigationService.Navigate(new ReaderProfile(reader, ReaderCard));
        }
    }
}
