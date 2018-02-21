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
        /// <summary>
        /// Get the balance of TTL in a specified address
        /// </summary>
        /// <param name="id">string - address to get the balance for</param>
        /// <returns>decimal - the requested balance</returns>
        public ActionResult GetBalance(string id)
        {
            if (!ApiHelper.CheckApiKey(Request))
                return Content("Invalid API Key");

            Contract contract = new Contract(CurrentNetwork, null);
            return Json(contract.GetTokensBalance(ConversionHelper.HexToBytes(id)),JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Transfers TTL from one address to another
        /// </summary>
        /// <param name="privateKeyHex">string - the private key used for the signer of the transaction</param>
        /// <param name="fromAddress">string - address to send the TTL from</param>
        /// <param name="toAddress">string - address to send the TTL to</param>
        /// <param name="amount">decimal - the amount to send</param>
        /// <returns>bool - success</returns>
        [HttpPost]
        public ActionResult Transfer(string privateKeyHex, string fromAddress, string toAddress, decimal amount)
        {
            //Check the API key since we're writing and there's a cost associated with it
            if (!ApiHelper.CheckApiKey(Request))
                return Content("Invalid API Key");

            Contract contract = new Contract(CurrentNetwork, privateKeyHex);
            return Json(contract.TransferTokens(ConversionHelper.HexToBytes(fromAddress), ConversionHelper.HexToBytes(toAddress), amount));
        }
    }
}