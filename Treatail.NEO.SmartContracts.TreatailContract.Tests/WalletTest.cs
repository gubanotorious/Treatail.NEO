using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treatail.NEO.Core.Logic;
using Treatail.NEO.Core.Models;

namespace Treatail.NEO.Tests
{
    public class WalletTest
    {
        public bool Create()
        {
            var wallet = WalletHelper.CreateWallet();

            if (wallet == null || String.IsNullOrEmpty(wallet.Address) || String.IsNullOrEmpty(wallet.PrivateKey))
                return false;

            return true;
        }

        public decimal GetNEOBalance(NetworkType network, string privateKeyHex)
        {
            var wallet = WalletHelper.GetWallet(privateKeyHex);
            return wallet.GetBalance(network, WalletBalanceType.NEO);
        }

        public decimal GetGASBalance(NetworkType network, string privateKeyHex)
        {
            var wallet = WalletHelper.GetWallet(privateKeyHex);
            return wallet.GetBalance(network, WalletBalanceType.GAS);
        }
    }
}
