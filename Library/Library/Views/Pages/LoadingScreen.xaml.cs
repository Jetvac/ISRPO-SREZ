using System.Windows;
using System.Windows.Controls;

namespace Library.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen : Page
    {
        public LoadingScreen()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ReaderListData mainPage = new ReaderListData();
            int result = await mainPage.LoadReaderData();
            if (result == 1)
            {
                NavigationService.Navigate(mainPage);
            }
        }
    }
}
