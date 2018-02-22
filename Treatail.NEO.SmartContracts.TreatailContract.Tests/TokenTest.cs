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

        public bool VerifyToken()
        {
            //var api = NetworkHelper.GetNeoRPCForType(NetworkType.Testnet);
            //var token = api.GetToken("TTL");
            //if (token == null)
            //{
            //    Console.WriteLine("Token is not found");
            //    return false;
            //}

            //Console.WriteLine("Token found");
            //return true;

            //This won't get added to neo-lux until deployed on the mainnet.
            return false;
        }

        public decimal GetBalance(string address)
        {
            Contract contract = new Contract(NetworkType.Testnet, _privateKeyHex);
            return contract.GetTokensBalance(address);
        }

        public bool Transfer(string fromAddress, string toAddress, int amount)
        {
            Contract contract = new Contract(NetworkType.Testnet, _privateKeyHex);
            return contract.TransferTokens(fromAddress, toAddress, amount);
        }
    }
}
