using Library.Class;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Library.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReaderProfile.xaml
    /// </summary>
    public partial class ReaderProfile : Page
    {
        public Reader SelectedClient { get; set; }
        LibraryСard LibraryCardItem;

        public ReaderProfile(Reader selectedClient, LibraryСard libraryCard)
        {
            InitializeComponent();

            SelectedClient = selectedClient;
            LibraryCardItem = libraryCard;
            DgReaderBook.ItemsSource = libraryCard.Records;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> genres = new List<string>();
            genres.Add("Отобразить всё");
            genres.AddRange((DgReaderBook.ItemsSource as List<RecordBook>).Select(c => c.Book.Genre).Distinct().ToList());
            CBGenre.ItemsSource = genres;

            List<string> authors = new List<string>();
            authors.Add("Отобразить всё");
            authors.AddRange((DgReaderBook.ItemsSource as List<RecordBook>).Select(c => c.Book.Author).Distinct().ToList());
            CBAuthor.ItemsSource = authors;

            List<string> publishers = new List<string>();
            publishers.Add("Отобразить всё");
            publishers.AddRange((DgReaderBook.ItemsSource as List<RecordBook>).Select(c => c.Book.PublisherEx).Distinct().ToList());
            CBPublisher.ItemsSource = publishers;

            CBGenre.SelectedIndex = 0;
            CBAuthor.SelectedIndex = 0;
            CBPublisher.SelectedIndex = 0;

            CBGenre.SelectionChanged += CBFilter_SelectionChanged;
            CBAuthor.SelectionChanged += CBFilter_SelectionChanged;
            CBPublisher.SelectionChanged += CBFilter_SelectionChanged;
        }
        public void FilterList(string FilterGenre = "Отобразить всё", string AuthorFilter = "Отобразить всё", string PublisherFilter = "Отобразить всё")
        {
            var FilteredData = LibraryCardItem.Records;

            if (FilterGenre != "Отобразить всё")
            {
                FilteredData = FilteredData.Where(c => c.Book.Genre == FilterGenre).ToList();
            }
            if (AuthorFilter != "Отобразить всё")
            {
                FilteredData = FilteredData.Where(c => c.Book.Author == AuthorFilter).ToList();
            }
            if (PublisherFilter != "Отобразить всё")
            {
                FilteredData = FilteredData.Where(c => c.Book.PublisherEx == PublisherFilter).ToList();
            }
            DgReaderBook.ItemsSource = FilteredData;
        }
        private void back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void CBFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string GenreFilter = CBGenre.SelectedItem is null ? "" : CBGenre.SelectedItem.ToString();
            string AuthorFilter = CBAuthor.SelectedItem is null ? "" : CBAuthor.SelectedItem.ToString();
            string PublisherFilter = CBPublisher.SelectedItem is null ? "" : CBPublisher.SelectedItem.ToString();

            FilterList(GenreFilter, AuthorFilter, PublisherFilter);
        }


        private async void WordReport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await ReportWorker.GenerateReport(LibraryCardItem, FileType.doc);
        }

        private async void PDFReport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await ReportWorker.GenerateReport(LibraryCardItem, FileType.pdf);
        }

        private async void ExcelReport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await ReportWorker.GenerateExcelReport(LibraryCardItem);
        }
    }
}
