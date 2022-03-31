using Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Library.Class
{
    class ApiControl
    {
        public static async Task<string> GetRequest(string method, Dictionary<string, string> args)
        {
            return await Task.Run(() =>
            {
                WebClient getClient = new WebClient();
                getClient.Encoding = Encoding.UTF8;

                string functionArgs = String.Empty;

                foreach (string key in args.Keys)
                {
                    functionArgs += $"{key}={args[key]}&";
                }
                functionArgs = functionArgs.Substring(0, functionArgs.Length - 1);

                string data = getClient.DownloadString($"{App.URL}{method}?{functionArgs}");
                return data;
            });
        }
        public static async Task<string> POSTRequest(string method, Dictionary<string, string> args)
        {
            HttpClient client = new HttpClient();

            var content = new FormUrlEncodedContent(args);

            var response = await client.PostAsync(App.URL + method, content);

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
        public static string GetBooksPost(string method, Reader reader)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(App.URL + method);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(reader);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }
        public static LibraryСard GetReaderLibraryCard(Reader reader)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            var photo = reader.Photo;
            reader.Photo = null;
            data.Add("reader", JsonConvert.SerializeObject(reader, Formatting.Indented));
            data.Add("token", App.UserToken);


            string requestData = GetBooksPost("GetBooksByReader?token=" + App.UserToken, reader);
            LibraryСard result = JsonConvert.DeserializeObject<LibraryСard>(requestData);

            reader.Photo = photo;
            return result;
        }
    }
}
