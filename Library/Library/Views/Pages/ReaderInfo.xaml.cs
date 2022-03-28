using Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Library.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReaderInfo.xaml
    /// </summary>
    public partial class ReaderInfo : Page
    {
        public ReaderInfo(LibraryСard data)
        {
            InitializeComponent();

            DgReaderBook.ItemsSource = data.Records;
            List<string> genres = new List<string>();
            genres.Add("Отобразить всё");
            genres.AddRange(data.Records.Select(c => c.Book.Genre).Distinct());

            List<string> Authors = new List<string>();
            Authors.Add("Отобразить всё");
            Authors.AddRange(data.Records.Select(c => c.Book.Author).Distinct());

            List<string> publ = new List<string>();
            publ.Add("Отобразить всё");
            publ.AddRange(data.Records.Select(c => c.Book.Publisher).Distinct());

            AuthorCB.ItemsSource = Authors;
            GenresCB.ItemsSource = genres;
            PublCB.ItemsSource = publ;
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
