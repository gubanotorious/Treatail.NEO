using System;
using System.Web.Mvc;
using Treatail.NEO.Core.Logic;
using Treatail.NEO.Core.Models;
using Treatail.NEO.WebApi.Logic;

namespace Treatail.NEO.WebApi.Controllers
{
    /// <summary>
    /// Manages all activity for Treatail Token
    /// </summary>
    public class TokenController : BaseController
    {
        public TokenController() : base()
        {

        }

        /// <summary>
        /// Get the balance of TTL in a specified address
        /// </summary>
        /// <param name="id">Address to get the balance for</param>
        /// <returns>The requested balance</returns>
        public ActionResult GetBalance(string id)
        {
            Contract contract = new Contract(CurrentNetwork, null);
            return Json(contract.GetTokensBalance(id),JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Transfers TTL from one address to another
        /// </summary>
        /// <param name="privateKeyHex">The private key used for the signer of the transaction</param>
        /// <param name="fromAddress">Address to send the TTL from</param>
        /// <param name="toAddress">Address to send the TTL to</param>
        /// <param name="amount">The amount to send</param>
        /// <returns>Success</returns>
        [HttpPost]
        public ActionResult Transfer(string privateKeyHex, string fromAddress, string toAddress, int amount)
        {
            Contract contract = new Contract(CurrentNetwork, privateKeyHex);
            return Json(contract.TransferTokens(fromAddress, toAddress, amount));
        }
    }
}