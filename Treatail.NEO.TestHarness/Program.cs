using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treatail.NEO.Core.Logic;

namespace Treatail.NEO.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkType network = NetworkType.Testnet;
            string address = "AKwhdHvupN2dRrMRTpNAFYgFQZiLmftmz6";

            Console.Write("Private Key Hex: ");
            string privateKeyHex = Console.ReadLine();

            //Test the wallet
            Console.WriteLine("Testing wallet generation...");
            Treatail.NEO.Tests.WalletTest walletTest = new Treatail.NEO.Tests.WalletTest();
            bool success = walletTest.Create();
            Console.WriteLine(success ? "Success" : "Fail");

            //Test the token
            Console.WriteLine("Verifying the token...");
            Treatail.NEO.Tests.TokenTest tokenTest = new Treatail.NEO.Tests.TokenTest(network, privateKeyHex);
            success = tokenTest.VerifyToken();
            Console.WriteLine(success ? "Success" : "Fail");
            Console.ReadLine();
        }
    }
}
