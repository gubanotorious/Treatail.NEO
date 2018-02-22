using NeoLux;
using System;
using Treatail.NEO.Core.Logic;

namespace Treatail.NEO.Core.Models
{
    /// <summary>
    /// Wrapper that manages all function calls for the Treatail Smart Contract
    /// </summary>
    public class Contract
    {
        private static string _contractScriptHash = "e15a3b08b56fbcae28391bb1547d303febccda55";
        private static string _tokenSymbol = "TTL";
        private NeoRPC _api;
        private NEP5 _token;
        private Wallet _contextWallet;

        /// <summary>
        /// Facilitates the invoking of the Treatail Smart Contract
        /// </summary>
        /// <param name="networkType">Network to use</param>
        /// <param name="contextWallet">Wallet to use for signing context</param>
        public Contract(NetworkType networkType, string privateKeyHex)
        {
            _api = NetworkHelper.GetNeoRPCForType(networkType);
            _token = _api.GetToken("TTL");
            if(privateKeyHex != null)
                _contextWallet = WalletHelper.GetWallet(privateKeyHex);
        }

        /// <summary>
        /// Deploys the intial supply of TTL to the Treatail Address
        /// </summary>
        /// <returns>Success</returns>
        public bool DeployTokens()
        {
            //Must be invoked with the Treatail Wallet
            if (_contextWallet == null)
                return false;

            return _api.CallContract(_contextWallet.GetKeys(), _contractScriptHash, "deploy", new object[] { 0 });
        }

        /// <summary>
        /// Returns the balance of TTL for a specified address
        /// </summary>
        /// <param name="address">Address to get the balance of</param>
        /// <returns>Balance of tokens for the specified address</returns>
        public decimal GetTokensBalance(string address)
        {
            return _token.BalanceOf(ConversionHelper.StringToBytes(address));
        }

        /// <summary>
        /// Transfers TTL from a wallet to a destination address
        /// </summary>
        /// <param name="from">The address TTL should be sent from</param>
        /// <param name="toAddress">The destination address to send TTL to</param>
        /// <param name="amount">The amount to be sent</param>
        /// <returns></returns>
        public bool TransferTokens(string fromAddress, string toAddress, int amount)
        {
            return _api.CallContract(_contextWallet.GetKeys(), _contractScriptHash, "transfer", new object[] {
                    ConversionHelper.StringToBytes(fromAddress),
                    ConversionHelper.StringToBytes(toAddress),
                    BitConverter.GetBytes(amount)
             });
        }

        /// <summary>
        /// Sets the amount of TTL required to create a Treatail Asset
        /// </summary>
        /// <param name="cost">Number of TTL required</param>
        /// <returns>Success</returns>
        public bool SetAssetCreateCost(int cost)
        {
            return _api.CallContract(_contextWallet.GetKeys(), _contractScriptHash,  "setassetcreatecost", new object[] { BitConverter.GetBytes(cost) });
        }

        /// <summary>
        /// Gets the required amount of TTL to create a Treatail Asset
        /// </summary>
        /// <returns>Cost of creating an asset</returns>
        public int GetAssetCreateCost()
        {
            var response = _api.TestInvokeScript(_contractScriptHash, "getassetcreatecost", new object[] { 0 });
            return (int)response.result;
        }

        /// <summary>
        /// Gets the owner address of a given Treatail Asset
        /// </summary>
        /// <param name="treatailId">Treatail Asset identifier</param>
        /// <returns>Address of the asset owner</returns>
        public byte[] GetAssetOwner(string treatailId)
        {
            var response = _api.TestInvokeScript(_contractScriptHash, "getassetowner", new object[] { ConversionHelper.StringToBytes(treatailId) });
            return (byte[])response.result;
        }

        /// <summary>
        /// Gets the details of a Treatail Asset
        /// </summary>
        /// <param name="treatailId">Treatail Asset identifier</param>
        /// <returns>Asset details</returns>
        public byte[] GetAssetDetails(string treatailId)
        {
            var response = _api.TestInvokeScript(_contractScriptHash, "getassetdetails", new object[] { ConversionHelper.StringToBytes(treatailId) });
            return (byte[])response.result;
        }

        /// <summary>
        /// Creates a treatail asset with the specified details and assigns the target address as the owner
        /// </summary>
        /// <param name="treatailId">Treatail Asset identifier</param>
        /// <param name="address">Address to assign ownership to</param>
        /// <param name="assetDetails">Details of the asset</param>
        /// <param name="chargeTTL">Whether or not to charge the account TTL for the create</param>
        /// <returns>Success</returns>
        public bool CreateAsset(string treatailId, string address, string assetDetails, bool chargeTTL)
        {
            return _api.CallContract(_contextWallet.GetKeys(), _contractScriptHash, "createasset", new object[] {
                ConversionHelper.StringToBytes(treatailId),
                ConversionHelper.StringToBytes(address),
                ConversionHelper.StringToBytes(assetDetails),
                BitConverter.GetBytes(chargeTTL)
            });
        }

        /// <summary>
        /// Transfers a Treatail Asset
        /// </summary>
        /// <param name="treatailId">Treatail Asset identifier</param>
        /// <param name="fromAddress">Address to transfer the asset from</param>
        /// <param name="toAddress">Address to transfer the asset to</param>
        /// <returns>Success</returns>
        public bool TransferAsset(string treatailId, string fromAddress, string toAddress)
        {
            return _api.CallContract(_contextWallet.GetKeys(), _contractScriptHash, "transferasset", new object[] {
                ConversionHelper.StringToBytes(treatailId),
                ConversionHelper.StringToBytes(fromAddress),
                ConversionHelper.StringToBytes(toAddress)
            });
        }
       
    }
}