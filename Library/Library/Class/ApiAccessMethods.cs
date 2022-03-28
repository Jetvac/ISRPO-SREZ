using Library.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Library.Class
{
    public class ApiAccessMethods
    {
        public static async Task<string> POSTRequest(string method, Dictionary<string, string> args)
        {
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(args);
            var response = await client.PostAsync(App.API_URL + method, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public static async Task<string> GETRequest(string method, Dictionary<string, string> args)
        {
            string data = string.Empty;
            foreach (string key in args.Keys)
            {
                data += $"{key}={args[key]}&";
            }
            data = data.Substring(0, data.Length - 1);

            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"{App.API_URL}{method}?{data}");
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public static string GetBooksPost(string method, Reader reader)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(App.API_URL + method);
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
    }
}
