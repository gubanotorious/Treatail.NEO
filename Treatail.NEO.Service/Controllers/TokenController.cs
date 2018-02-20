using Treatail.NEO.Service.Logic;
using Treatail.NEO.Service.Models;

namespace Treatail.NEO.Service.Controllers
{
    /// <summary>
    /// Manages all activity for Treatail Token
    /// </summary>
    public class TokenController : BaseController
    {
        /// <summary>
        /// Get the balance of TTL in a specified address
        /// </summary>
        /// <param name="address">string - address to get the balance for</param>
        /// <returns>decimal - the requested balance</returns>
        public decimal GetBalance(string address)
        {
            Contract contract = new Contract(CurrentNetwork, null);
            return contract.GetTokensBalance(ConversionHelper.HexToBytes(address));
        }

        /// <summary>
        /// Transfers TTL from one address to another
        /// </summary>
        /// <param name="privateKeyHex">string - the private key used for the signer of the transaction</param>
        /// <param name="fromAddress">string - address to send the TTL from</param>
        /// <param name="toAddress">string - address to send the TTL to</param>
        /// <param name="amount">decimal - the amount to send</param>
        /// <returns>bool - success</returns>
        public bool Transfer(string privateKeyHex, string fromAddress, string toAddress, decimal amount)
        {
            Contract contract = new Contract(CurrentNetwork, privateKeyHex);
            return contract.TransferTokens(ConversionHelper.HexToBytes(fromAddress), ConversionHelper.HexToBytes(toAddress), amount);
        }
    }
}