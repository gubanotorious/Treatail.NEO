using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Treatail.NEO.WebApi.Tests.Logic
{
    public enum ServiceAction
    {
        GET,
        POST
    }

    public static class ServicesHelper
    {
        public static string CallService(ServiceAction action, string apiKey, string url, string jsonContent)
        {
            using (var client = new WebClient { Encoding = System.Text.Encoding.UTF8 })
            {
                //Set the content type header
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                if (!String.IsNullOrEmpty(apiKey))
                    client.Headers.Add(apiKey, apiKey);

                if (action == ServiceAction.GET)
                    return client.DownloadString(url);
                else if (action == ServiceAction.POST)
                {
                    if (jsonContent == null)
                        jsonContent = "{}";

                    return client.UploadString(url, jsonContent);
                }
            }
            return null;
        }
    }
}
