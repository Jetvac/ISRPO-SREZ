using LibraryWEBAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Library.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReaderListData.xaml
    /// </summary>
    public partial class ReaderListData : Page
    {
        /// <summary>
        /// Текущий список читателей
        /// </summary>
        public List<Reader> Readers = null;
        public ReaderListData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Выгрузка читателей в список
        /// </summary>
        private void Grid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            WebClient getClient = new WebClient();
            getClient.Encoding = Encoding.UTF8;
            string data = getClient.DownloadString(App.API_URL + "GetReaders?token=" + App.UserToken);
            Readers = JsonConvert.DeserializeObject <List<Reader>>(data);

            ReaderDataList.ItemsSource = Readers;
        }

        /// <summary>
        /// Отсеивает пользователей по ФИО
        /// </summary>
        private void ReaderSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Readers != null && ReaderSearchInput.Text != "")
            {
                List<Reader> searchData = Readers.Where(c =>
                    c.FirstName.Contains(ReaderSearchInput.Text) ||
                    c.LastName.Contains(ReaderSearchInput.Text) ||
                    c.MiddleName.Contains(ReaderSearchInput.Text)).ToList();


                if (searchData.Count != 0)
                {
                    ReaderDataList.ItemsSource = searchData;
                } else
                {
                    MessageBox.Show("Читатели не найдены!");
                }
            } else if (ReaderSearchInput.Text == "")
            {
                ReaderDataList.ItemsSource = Readers;
            }
        }

        /// <summary>
        /// Завершает текущую сессию и возвращает на окно авторизации
        /// </summary>
        private void ExitButton_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            App.UserToken = null;
            NavigationService.Navigate(new Authorization());
            Readers = null;
        }

        /// <summary>
        /// Вывод данных читателей в CSV
        /// </summary>
        private void CSVOutput_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string data = "ФИО\n";

            foreach (Reader reader in Readers)
            {
                data += $"{reader.FullName}\n";
            }

            File.WriteAllText($"{Directory.GetCurrentDirectory()}/readers.csv", data, Encoding.Default);
            MessageBox.Show("Данные успешно выгружены.");
        }
    }
}
