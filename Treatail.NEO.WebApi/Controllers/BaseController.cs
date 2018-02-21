using System;
using System.Configuration;
using System.Web.Mvc;
using Treatail.NEO.Core.Logic;

namespace Treatail.NEO.WebApi.Controllers
{
    public abstract class BaseController : Controller
    {
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