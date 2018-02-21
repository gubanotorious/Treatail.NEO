using NeoLux;
using System;
using Treatail.NEO.Core.Logic;

namespace Treatail.NEO.Core.Models
{
    public class Contract
    {
        private const string _treatailContractScriptHash = "0xacdce8e8cbc5e539d749c23340f9801d43bb3a92";
        private const string _treatailTokenSymbol = "TTL";
        private NeoRPC _api;
        private NEP5 _token;
        private Wallet _contextWallet;

        /// <summary>
        /// Facilitates the invoking of the Treatail Smart Contract
        /// </summary>
        /// <param name="networkType">NetworkType - network to use</param>
        /// <param name="contextWallet">Wallet - the wallet to use for signing context</param>
        public Contract(NetworkType networkType, string privateKeyHex)
        {
            _api = NetworkHelper.GetNeoRPCForType(networkType);
            _contextWallet = WalletHelper.GetWallet(privateKeyHex);
            _token = _api.GetToken("TTL");
        }

        /// <summary>
        /// Deploys the intial supply of TTL to the Treatail Address
        /// </summary>
        /// <returns>bool - success</returns>
        public bool DeployTokens()
        {
            //Must be invoked with the Treatail Wallet
            if (_contextWallet == null)
                return false;

            return _api.CallContract(_contextWallet.GetKeys(), _treatailContractScriptHash, "deploy", null);
        }

        /// <summary>
        /// Returns the balance of TTL for a specified address
        /// </summary>
        /// <param name="address">string - NEO Address (ie: "AVQ6jAQ3Prd32BXU5r2Vb3QL1gYzTpFhaf")</param>
        /// <returns>decimal - balance of tokens for the specified address</returns>
        public decimal GetTokensBalance(byte[] address)
        {
            return _token.BalanceOf(address);
        }

        /// <summary>
        /// Transfers TTL from a wallet to a destination address
        /// </summary>
        /// <param name="from">byte[] - The wallet TTL should be sent from</param>
        /// <param name="toAddress">byte[] - the destination address to send TTL to</param>
        /// <param name="amount">decimal - the amount to be sent</param>
        /// <returns></returns>
        public bool TransferTokens(byte[] fromAddress, byte[] toAddress, decimal amount)
        {
            return _api.CallContract(_contextWallet.GetKeys(), _treatailContractScriptHash, "transfer", new object[] { fromAddress, toAddress, amount });
        }

        /// <summary>
        /// Sets the amount of TTL required to create a Treatail Asset
        /// </summary>
        /// <param name="cost">decimal - number of TTL required</param>
        /// <returns>bool - success</returns>
        public bool SetAssetCreateCost(decimal cost)
        {
            return _api.CallContract(_contextWallet.GetKeys(), _treatailContractScriptHash,  "setassetcreatecost", new object[] { cost });
        }

        /// <summary>
        /// Gets the required amount of TTL to create a Treatail Asset
        /// </summary>
        /// <returns>int - cost of creating an asset</returns>
        public int GetAssetCreateCost()
        {
            var response = _api.TestInvokeScript(_treatailContractScriptHash, "getassetcreatecost", null);
            return (int)response.result;
        }

        /// <summary>
        /// Gets the owner address of a given Treatail Asset
        /// </summary>
        /// <param name="treatailId">byte[] - Treatail Asset identifier</param>
        /// <returns>byte[] - address of the asset owner</returns>
        public byte[] GetAssetOwner(byte[] treatailId)
        {
            var response = _api.TestInvokeScript(_treatailContractScriptHash, "getassetowner", new object[] { treatailId });
            return (byte[])response.result;
        }

        /// <summary>
        /// Gets the details of a Treatail Asset
        /// </summary>
        /// <param name="treatailId">byte[] - Treatail Asset identifier</param>
        /// <returns>byte[] - payload containing the asset details</returns>
        public byte[] GetAssetDetails(byte[] treatailId)
        {
            var response = _api.TestInvokeScript(_treatailContractScriptHash, "getassetdetails", new object[] { treatailId });
            return (byte[])response.result;
        }

        /// <summary>
        /// Creates a treatail asset with the specified details and assigns the target address as the owner
        /// </summary>
        /// <param name="treatailId">byte[] - treatailId</param>
        /// <param name="address">byte[]</param>
        /// <param name="assetDetails"></param>
        /// <returns></returns>
        public bool CreateAsset(byte[] treatailId, byte[] address, byte[] assetDetails, bool chargeTTL)
        {
            return _api.CallContract(_contextWallet.GetKeys(), _treatailContractScriptHash, "createasset", new object[] { treatailId, address, assetDetails, chargeTTL });
        }

        /// <summary>
        /// Transfers a Treatail Asset
        /// </summary>
        /// <param name="treatailId">byte[] - Treatail Asset identifier</param>
        /// <param name="fromAddress">byte[] - address to transfer the asset from</param>
        /// <param name="toAddress">byte[] - address to transfer the asset to</param>
        /// <returns>bool - success</returns>
        public bool TransferAsset(byte[] treatailId, byte[] fromAddress, byte[] toAddress)
        {
            return _api.CallContract(_contextWallet.GetKeys(), _treatailContractScriptHash, "transferasset", new object[] { treatailId, fromAddress, toAddress });
        }
       
    }
}