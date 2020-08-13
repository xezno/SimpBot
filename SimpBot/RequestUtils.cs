using RSG;
using System;
using System.Net;

namespace SimpBot
{
    public static class RequestUtils
    {
        public static Promise<string> GetString(string url)
        {
            var promise = new Promise<string>();

            using (var client = new WebClient())
            {
                client.DownloadStringCompleted += (s, ev) =>
                {
                    if (ev.Error != null)
                    {
                        promise.Reject(ev.Error);
                    }
                    else
                    {
                        promise.Resolve(ev.Result);
                    }
                };

                client.DownloadStringAsync(new Uri(url), null);
            }

            return promise;
        }

        public static async void Post(string url)
        {
            // await httpClient.PostAsync(url, null);
            throw new NotImplementedException();
        }
    }
}
