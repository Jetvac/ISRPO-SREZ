using System.Windows;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// URL-адрес подклбючения к API
        /// </summary>
        public const string API_URL = "https://localhost:7256/";
        /// <summary>
        /// Токен текущего авторизованного пользователя
        /// </summary>
        public static string UserToken = null;
    }
}
