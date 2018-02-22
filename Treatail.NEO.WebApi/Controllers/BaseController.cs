using System;
using System.Configuration;
using System.Web.Mvc;
using Treatail.NEO.Core.Logic;
using Treatail.NEO.WebApi.Logic;

namespace Treatail.NEO.WebApi.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController()
        {
            //We need to enable API Key checking for production
            //ApiHelper.CheckApiKey(Request);
        }

        /// <summary>
        /// Reads configuration to find the NEO network to operate on
        /// </summary>
        protected NetworkType CurrentNetwork
        {
            get
            {
                string value = ConfigurationManager.AppSettings["NEONetwork"];
                return (NetworkType)Enum.Parse(typeof(NetworkType), value);
            }
        }
    }
}