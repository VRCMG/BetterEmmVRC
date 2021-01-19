using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TinyJSON;

namespace emmVRC.Network
{
    public class HTTPRequest
    {
        public static string get_sync(string url)
        {
            return HTTPRequest.request(HttpMethod.Get, url, null).Result;
        }

        public static string post_sync(string url, object obj)
        {
            return HTTPRequest.request(HttpMethod.Post, url, obj).Result;
        }

        public static string put_sync(string url, object obj)
        {
            return HTTPRequest.request(HttpMethod.Put, url, obj).Result;
        }

        public static string patch_sync(string url, object obj)
        {
            return HTTPRequest.request(new HttpMethod("PATCH"), url, obj).Result;
        }

        public static string delete_sync(string url, object obj)
        {
            return HTTPRequest.request(HttpMethod.Delete, url, obj).Result;
        }

        public static async Task<string> get(string url)
        {
            return await HTTPRequest.request(HttpMethod.Get, url, null);
        }

        public static async Task<string> post(string url, object obj)
        {
            return await HTTPRequest.request(HttpMethod.Post, url, obj);
        }

        public static async Task<string> put(string url, object obj)
        {
            return await HTTPRequest.request(HttpMethod.Put, url, obj);
        }

        public static async Task<string> patch(string url, object obj)
        {
            return await HTTPRequest.request(new HttpMethod("PATCH"), url, obj);
        }

        public static async Task<string> delete(string url, object obj)
        {
            return await HTTPRequest.request(HttpMethod.Delete, url, obj);
        }

        private static async Task<string> request(HttpMethod method, string url, object obj = null)
        {
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(method, url);
                if (obj != null)
                {
                    string content = TinyJSON.Encoder.Encode(obj);
                    requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                }

                Task<string> result3;
                using (HttpResponseMessage result2 = BetterEmmVRC.BetterEmmVRC.NetworkLib.EmmVRCNetworkClient.SendAsync(requestMessage).Result)
                {
                    if (result2.IsSuccessStatusCode)
                    {
                        result3 = result2.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        result3 = Task.FromResult<string>(result2.ReasonPhrase);
                    }
                }
                return result3.Result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
