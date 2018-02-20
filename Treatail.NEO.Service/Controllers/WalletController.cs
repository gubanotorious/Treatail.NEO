using System.Web.Mvc;
using Treatail.NEO.Service.Logic;

namespace Treatail.NEO.Service.Controllers
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
            return Json(WalletHelper.CreateWallet(), JsonRequestBehavior.AllowGet);
        }
    }
}