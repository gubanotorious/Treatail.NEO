using NeoLux;
using System;
using System.Numerics;
using Treatail.NEO.Core.Logic;

namespace Treatail.NEO.Core.Models
{
    /// <summary>
    /// Wrapper that manages all function calls for the Treatail Smart Contract
    /// </summary>
    public class Contract
    {
        private static string _contractScriptHash = "beea17e57b7e3fef4b36963502c8bf94d6f86ae3";
        private NeoRPC _api;
        private Wallet _contextWallet;
        private NEP5 _token;

        /// <summary>
        /// Facilitates the invoking of the Treatail Smart Contract
        /// </summary>
        /// <param name="networkType">Network to use</param>
        /// <param name="contextWallet">Wallet to use for signing context</param>
        public Contract(NetworkType networkType, string privateKeyHex)
        {
            _api = NetworkHelper.GetNeoRPCForType(networkType);
            _token = new NEP5(_api, _contractScriptHash);
            if (privateKeyHex != null)
                _contextWallet = WalletHelper.GetWallet(privateKeyHex);
        }

        /// <summary>
        /// Returns the balance of TTL for a specified address
        /// </summary>
        /// <param name="address">Address to get the balance of</param>
        /// <returns>Balance of tokens for the specified address</returns>
        public decimal GetTokenBalance()
        {
            return _token.BalanceOf(_contextWallet.Address);
        }

        /// <summary>
        /// Transfers TTL from the context wallet to a destination address
        /// </summary>
        /// <param name="toAddress">The destination address to send TTL to</param>
        /// <param name="amount">The amount to be sent</param>
        /// <returns>Success</returns>
        public bool TransferTokens(string fromAddress, string toAddress, decimal value)
        {
            var decs = _token.Decimals;
            while (decs > 0)
            {
                value *= 10;
                decs--;
            }

            BigInteger amount = new BigInteger((ulong)value);
            var sender_address_hash = fromAddress.GetScriptHashFromAddress();
            var to_address_hash = toAddress.GetScriptHashFromAddress();
            var response = _api.CallContract(_contextWallet.GetKeys(), _contractScriptHash, "transfer", new object[] { sender_address_hash, to_address_hash, amount });
            return response;
        }

        /// <summary>
        /// Sets the amount of TTL required to create a Treatail Asset
        /// </summary>
        /// <param name="cost">Number of TTL required</param>
        /// <returns>Success</returns>
        public bool SetAssetCreateCost(BigInteger cost)
        {
            return _api.CallContract(_contextWallet.GetKeys(), _contractScriptHash,  "setassetcreatecost", new object[] { cost });
        }

        /// <summary>
        /// Gets the required amount of TTL to create a Treatail Asset
        /// </summary>
        /// <returns>Cost of creating an asset</returns>
        public BigInteger GetAssetCreateCost()
        {
            var response = _api.TestInvokeScript(_contractScriptHash, "getassetcreatecost", new object[] { 0 });
            return (BigInteger)response.result;
        }

        /// <summary>
        /// Gets the owner address of a given Treatail Asset
        /// </summary>
        /// <param name="treatailId">Treatail Asset identifier</param>
        /// <returns>Address of the asset owner</returns>
        public string GetAssetOwner(string treatailId)
        {
            var response = _api.TestInvokeScript(_contractScriptHash, "getassetowner", new object[] {
                ConversionHelper.StringToBytes(treatailId) });

            return ((byte[])response.result).ToHexString();
        }

        /// <summary>
        /// Gets the details of a Treatail Asset
        /// </summary>
        /// <param name="treatailId">Treatail Asset identifier</param>
        /// <returns>Asset details</returns>
        public string GetAssetDetails(string treatailId)
        {
            var response = _api.TestInvokeScript(_contractScriptHash, "getassetdetails", new object[] {
                ConversionHelper.StringToBytes(treatailId) });

            return ConversionHelper.BytesToString((byte[])response.result);
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
                address.GetScriptHashFromAddress(),
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
                fromAddress.GetScriptHashFromAddress(),
                toAddress.GetScriptHashFromAddress()
            });
        }
    }
}