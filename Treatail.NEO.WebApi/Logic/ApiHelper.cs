using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Treatail.NEO.WebApi.Logic
{
    public static class ApiHelper
    {
        public static string ApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["TreatailNEO:ApiKey"];
            }
        }

        public static void CheckApiKey(HttpRequestBase request)
        {
            if (request.Headers[ApiKey] == null)
                throw new Exception("Invalid API Key Provided");
        }
    }
}