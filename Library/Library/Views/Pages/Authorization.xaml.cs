using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ApiAccess = Library.Class.ApiAccessMethods;

namespace Library.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public static string StringCrypt(string inputString)
        {
            SHA256Managed cryptObject = new SHA256Managed();
            string hash = "";
            byte[] cryptoArray = cryptObject.ComputeHash(Encoding.ASCII.GetBytes(inputString));
            foreach (byte theByte in cryptoArray)
                hash += theByte.ToString("x2");
            return hash;
        }

        public Authorization()
        {
            InitializeComponent();
        }

        private async void AuthorizationSubmit_click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("login", LoginInput.Text);
            parameters.Add("password", StringCrypt(PasswordInput.Password));

            try
            {
                string response = await ApiAccess.POSTRequest("Login", parameters);
                switch (response)
                {
                    case "Неверный логин или пароль":
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;

                    case "Пользователь не найден":
                        MessageBox.Show("Пользователь не найден!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                }

                App.UserToken = response;
                NavigationService.Navigate(new ReaderDataList());
            }
            catch
            {
                MessageBox.Show("Не удалось установить соединение! Попробуйте позже.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ShowPassword_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PassShow.Visibility = Visibility.Visible;
            PasswordInput.Visibility = Visibility.Collapsed;
            PassShow.Text = PasswordInput.Password;
        }

        private void HidePassword_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PassShow.Visibility = Visibility.Collapsed;
            PasswordInput.Visibility = Visibility.Visible;
        }
    }
}
