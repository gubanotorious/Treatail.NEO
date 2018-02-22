using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treatail.NEO.Core.Logic;
using Treatail.NEO.Core.Models;

namespace Treatail.NEO.Tests
{
    public class TokenTest
    {
        private NetworkType _network = NetworkType.Testnet;
        private string _privateKeyHex = String.Empty;

        public TokenTest(NetworkType network, string privateKeyHex)
        {
            _network = network;
            _privateKeyHex = privateKeyHex;
        }

        public decimal GetBalance()
        {
            Contract contract = new Contract(NetworkType.Testnet, _privateKeyHex);
            return contract.GetTokenBalance();
        }

        public bool Transfer(string fromAddress, string toAddress, decimal amount)
        {
            Contract contract = new Contract(NetworkType.Testnet, _privateKeyHex);
            return contract.TransferTokens(fromAddress, toAddress, amount);
        }
    }
}
