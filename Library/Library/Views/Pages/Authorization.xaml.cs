using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Library.Class;

namespace Library.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public static string ConvertToSHA256(string inputString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(inputString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        public Authorization()
        {
            InitializeComponent();
        }

        private async void AuthorizationSubmit_click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("login", LoginInput.Text);
            args.Add("password", ConvertToSHA256(PasswordInput.Password));

            try
            {
                string response = await ApiControl.POSTRequest("Login", args);

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
                NavigationService.Navigate(new LoadingScreen());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void ShowPassword_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PassShow.Text = PasswordInput.Password;
            PassShow.Visibility = Visibility.Visible;
            PasswordInput.Visibility = Visibility.Collapsed;
        }

        private void HidePassword_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PassShow.Visibility = Visibility.Collapsed;
            PasswordInput.Visibility = Visibility.Visible;
        }
    }
}
