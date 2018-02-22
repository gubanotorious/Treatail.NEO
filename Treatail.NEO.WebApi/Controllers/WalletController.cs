using System;
using System.Web.Mvc;
using Treatail.NEO.Core.Logic;
using Treatail.NEO.Core.Models;

namespace Treatail.NEO.WebApi.Controllers
{
    /// <summary>
    /// Handles any wallet related interaction
    /// </summary>
    public class WalletController : BaseController
    {
        public WalletController() : base()
        {
            
        }

        /// <summary>
        /// Creates the wallet and user account for the hosted user wallet
        /// </summary>
        /// <returns>The created wallet info</returns>
        public ActionResult Create()
        {
            return Json(WalletHelper.CreateWallet(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the neo balance.
        /// </summary>
        /// <param name="privateKeyHex">The private key hexadecimal.</param>
        /// <returns>The balance</returns>
        [HttpPost]
        public ActionResult GetNEOBalance(string privateKeyHex)
        {
            return Json(WalletHelper.GetWallet(privateKeyHex).GetBalance(CurrentNetwork, WalletBalanceType.NEO));
        }

        /// <summary>
        /// Gets the neo balance.
        /// </summary>
        /// <param name="privateKeyHex">The private key hexadecimal.</param>
        /// <returns>The balance</returns>
        [HttpPost]
        public ActionResult GetGASBalance(string privateKeyHex)
        {
            return Json(WalletHelper.GetWallet(privateKeyHex).GetBalance(CurrentNetwork, WalletBalanceType.GAS));
        }

        /// <summary>
        /// Gets the neo balance.
        /// </summary>
        /// <param name="privateKeyHex">The private key hexadecimal.</param>
        /// <returns>The balance</returns>
        [HttpPost]
        public ActionResult GetTTLBalance(string privateKeyHex)
        {
            var wallet = WalletHelper.GetWallet(privateKeyHex);
            Contract contract = new Contract(CurrentNetwork, privateKeyHex);
            return Json(contract.GetTokensBalance(wallet.Address));
        }
    }
}