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
            var api = NetworkHelper.GetNeoRPCForType(NetworkType.Testnet);
            var token = api.GetToken("TTL");
            if (token == null)
            {
                Console.WriteLine("Token is not found");
                return false;
            }

            Console.WriteLine("Token verified");
            return true;
        }

        public bool Deploy()
        {
            Contract contract = new Contract(NetworkType.Testnet, _privateKeyHex);
            return contract.DeployTokens();
        }

        public decimal GetBalance(string address)
        {
            Contract contract = new Contract(NetworkType.Testnet, _privateKeyHex);
            return contract.GetTokensBalance(ConversionHelper.HexToBytes(address));
        }

        public bool Transfer(string fromAddress, string toAddress, decimal amount)
        {
            Contract contract = new Contract(NetworkType.Testnet, _privateKeyHex);
            return contract.TransferTokens(ConversionHelper.HexToBytes(fromAddress), ConversionHelper.HexToBytes(toAddress), amount);
        }
    }
}
