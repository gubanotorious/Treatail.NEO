using System.Web.Mvc;
using Treatail.NEO.Core.Logic;
using Treatail.NEO.WebApi.Logic;

namespace Treatail.NEO.WebApi.Controllers
{
    /// <summary>
    /// Handles any wallet related interaction
    /// </summary>
    public class WalletController : BaseController
    {
        /// <summary>
        /// Creates the wallet and user account for the hosted user wallet
        /// </summary>
        /// <returns>Wallet - the created wallet info</returns>
        public ActionResult Create()
        {
            if (!ApiHelper.CheckApiKey(Request))
                return Content("Invalid API Key");

            return Json(WalletHelper.CreateWallet(), JsonRequestBehavior.AllowGet);
        }
    }
}