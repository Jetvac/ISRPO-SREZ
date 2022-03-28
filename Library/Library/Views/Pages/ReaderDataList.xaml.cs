using Library.Class;
using Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using ApiAccess = Library.Class.ApiAccessMethods;

namespace Library.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReaderDataList.xaml
    /// </summary>
    public partial class ReaderDataList : Page
    {
        public List<Reader> ReadersList = null;

        public async static Task<List<Reader>> LoadReaderData()
        {
            try
            {
                Dictionary<string, string> parameteres = new Dictionary<string, string>();
                parameteres.Add("token", App.UserToken);

                string resp = await ApiAccess.GETRequest("GetReaders", parameteres);
                List<Reader> result = JsonConvert.DeserializeObject<List<Reader>>(resp);
                return result;
            } catch (Exception ex)
            {
                MessageBox.Show("Не удалось установить соединение! Попробуйте позже.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public ReaderDataList()
        {
            InitializeComponent();
        }

        private void ContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ReaderDataWrap.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента.", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            Reader reader = ReaderDataWrap.SelectedItem as Reader;
            Dictionary<string, string> data = new Dictionary<string, string>();
            var photo = reader.Photo;
            reader.Photo = null;
            data.Add("reader", JsonConvert.SerializeObject(reader, Formatting.Indented));
            data.Add("token", App.UserToken);


            string requestData = ApiAccess.GetBooksPost("GetBooksByReader?token=" + App.UserToken, reader);
            LibraryСard libraryСards = JsonConvert.DeserializeObject<LibraryСard>(requestData);

            reader.Photo = photo;
            NavigationService.Navigate(new ReaderInfo(libraryСards));
        }

        private void ExitButton_Click(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Вы точно хотите выйти?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                App.UserToken = null;
                NavigationService.Navigate(new Authorization());
            }
        }

        private async void PageStarted(object sender, RoutedEventArgs e)
        {
            ReadersList = await LoadReaderData();
            ReaderDataWrap.ItemsSource = ReadersList;
        }

        private void ReaderSearchInput_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void ReaderSearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ReadersList != null && ReaderSearchInput.Text != "")
            {
                List<Reader> searchData = ReadersList.Where(c =>
                    c.FullName.ToLower().Contains(ReaderSearchInput.Text.ToLower())).ToList();


                if (searchData.Count != 0)
                {
                    ReaderDataWrap.ItemsSource = searchData;
                }
                else
                {
                    MessageBox.Show("Читатели не найдены!");
                }
            }
            else if (ReaderSearchInput.Text == "")
            {
                ReaderDataWrap.ItemsSource = ReadersList;
            }
        }

        private async void ReaderTicket_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ReaderDataWrap.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента.", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            Reader reader = ReaderDataWrap.SelectedItem as Reader;
            Dictionary<string, string> data = new Dictionary<string, string>();
            var photo = reader.Photo;
            reader.Photo = null;
            data.Add("reader", JsonConvert.SerializeObject(reader, Formatting.Indented));
            data.Add("token", App.UserToken);


            string requestData = ApiAccess.GetBooksPost("GetBooksByReader?token=" + App.UserToken, reader);
            LibraryСard libraryСards = JsonConvert.DeserializeObject<LibraryСard>(requestData);

            reader.Photo = photo;
            await WordCreate.Print(reader, libraryСards);
        }

        private void CSVOutput_click(object sender, MouseButtonEventArgs e)
        {
            string folderPath = $"{Directory.GetCurrentDirectory()}/readersphoto";
            string data = "Путь до фотографий читателей:\n";
            Directory.CreateDirectory(folderPath);
            Directory.Delete(folderPath, true);
            Directory.CreateDirectory(folderPath);
            int i = 0; // reader counter

            foreach (Reader reader in ReadersList)
            {
                string dataPast = $"{reader.FullName};";
                string filePath = $"{folderPath}/{reader.FirstName}_{i++}.jpeg";

                File.WriteAllBytes(filePath, reader.Photo);
                data += dataPast + filePath + "\n";
            }

            File.WriteAllText($"{Directory.GetCurrentDirectory()}/readers.csv", data, Encoding.Default);
            MessageBox.Show("Данные успешно выгружены.");
        }
    }
}
