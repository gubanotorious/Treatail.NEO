using System;
using NeoLux;
using Treatail.NEO.Service.Models;

namespace Treatail.NEO.Service.Logic
{
    public static class WalletHelper
    {
        /// <summary>
        /// Generates a new address and returns the Address and Private Key that were generated
        /// </summary>
        /// <returns>Wallet - the generated wallet object</returns>
        public static Wallet CreateWallet()
        {
            var rnd = new Random();
            var bytes = new byte[32];
            rnd.NextBytes(bytes);
            var key = new KeyPair(bytes);

            //I need to load the wallet somehow?
            return new Wallet
            {
                Address = key.address,
                PrivateKey = key.PrivateKey.ToHexString()
            };
        }

        /// <summary>
        /// Gets a wallet from the provided private key
        /// </summary>
        /// <param name="privateKeyHex">string - hex string of the private key</param>
        /// <returns>Wallet - the requested wallet object</returns>
        public static Models.Wallet GetWallet(string privateKeyHex)
        {
            var privateKey = ConversionHelper.HexToBytes(privateKeyHex);
            var key = new KeyPair(privateKey);

            return new Wallet
            {
                Address = key.address,
                PrivateKey = key.PrivateKey.ToHexString()
            };
        }
    }
}