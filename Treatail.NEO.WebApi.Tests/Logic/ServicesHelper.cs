using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Treatail.NEO.WebApi.Tests.Logic
{
    public enum ServiceAction
    {
        GET,
        POST,
        PUT
    }

    public static class ServicesHelper
    {
        public static async Task<string> CallService(ServiceAction action, string apiKey, string url, string jsonContent)
        {
            string responseString = null;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (!String.IsNullOrEmpty(apiKey))
                        httpClient.DefaultRequestHeaders.Add(apiKey, apiKey);
                }
                catch (Exception ex)
                {
                    var m = ex.Message;
                }

                HttpResponseMessage httpResponse = null;
                if (action == ServiceAction.GET)
                    httpResponse = await httpClient.GetAsync(url);
                else if (action == ServiceAction.POST)
                {
                    if (jsonContent == null)
                        jsonContent = String.Empty;

                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    httpResponse = await httpClient.PostAsync(url, content);
                }

                httpResponse.EnsureSuccessStatusCode();
                responseString = await httpResponse.Content.ReadAsStringAsync();
            }

            if (String.IsNullOrEmpty(responseString))
                throw new Exception("Response was empty");

            return responseString;
        }
    }
}
