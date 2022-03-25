using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Library.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        /// <summary>
        /// Преобразует строку в зашифрованный формат типа SHA256
        /// </summary>
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

        /// <summary>
        /// Событие нажатия на кнопку авторизации
        /// </summary>
        private void AuthorizationSubmit_click(object sender, RoutedEventArgs e)
        {
            var request = (HttpWebRequest)WebRequest.Create(App.API_URL + "Login");

            var sendData = "login=" + Uri.EscapeDataString(LoginInput.Text);
            sendData += "&password=" + Uri.EscapeDataString(ConvertToSHA256(PasswordInput.Password));
            var data = Encoding.ASCII.GetBytes(sendData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }


            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var resp = new StreamReader(response.GetResponseStream()).ReadToEnd();

                App.UserToken = resp;
                NavigationService.Navigate(new ReaderListData());
            } catch(Exception ex)
            {
                MessageBox.Show("Произошла ошибка авторизации!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
