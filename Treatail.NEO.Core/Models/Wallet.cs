using Neo.Cryptography;
using NeoLux;
using System;
using System.Collections.Generic;
using Treatail.NEO.Core.Logic;

namespace Treatail.NEO.Core.Models
{
    public enum WalletBalanceType
    {
        NEO,
        NeoGas,
        TTL
    }

    [Serializable]
    public class Wallet
    {
        /// <summary>
        /// Primary address for this wallet for sending.  In Treatail, a user will have 1 wallet per account
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Private key for the wallet
        /// </summary>
        public string PrivateKey { get; set; }


        /// <summary>
        /// Private key for signing
        /// </summary>
        public KeyPair GetKeys()
        {
            if (PrivateKey == null)
                return null;

            return new KeyPair(PrivateKey.HexToBytes());
        }

        /// <summary>
        /// Gets the balance of the specified type from the specified network
        /// </summary>
        /// <param name="networkType">NetworkType - network to use</param>
        /// <param name="balanceType">WalletBalanceType - type of balance</param>
        /// <returns></returns>
        public decimal GetBalance(NetworkType networkType, WalletBalanceType type)
        {
            var balances = GetBalances(networkType);
            foreach (var balance in balances)
            {
                if (balance.Key == type.ToString())
                    return balance.Value;
            }
            return 0;
        }

        /// <summary>
        /// Gets the balances for the wallet from the specified network
        /// </summary>
        /// <param name="networkType">NetworkType - network to use</param>
        /// <returns>Dictionary - balances in the wallet</string></returns>
        private Dictionary<string, decimal> GetBalances(NetworkType networkType)
        {
            var network = NetworkHelper.GetNeoRPCForType(networkType);
            return network.GetBalancesOf(GetKeys(), true);
        }

    }
}
