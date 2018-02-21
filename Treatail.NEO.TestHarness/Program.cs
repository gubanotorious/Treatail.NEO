using System;
using System.Threading.Tasks;
using Treatail.NEO.Core.Logic;

namespace Treatail.NEO.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = MainAsync(args);
            t.Wait();
        }

        static async Task MainAsync(string[] args)
        {
            Console.Write("Private Key Hex: ");
            string privateKeyHex = Console.ReadLine();
            NetworkType network = NetworkType.Testnet;
            string address = "AKwhdHvupN2dRrMRTpNAFYgFQZiLmftmz6";
            string servicesBaseUrl = "https://neoapi.treatail.com";
            string apiKey = "SECUREKEY";

            //Test the wallet
            Console.WriteLine("Testing wallet generation...");

            Tests.WalletTest walletTest = new Tests.WalletTest();
            bool success = walletTest.Create();
            Console.WriteLine("-Lux: " + (success ? "Success" : "Fail"));

            //Test the wallet REST endpoint
            success = false;
            WebApi.Tests.WalletTest apiWalletTest = new WebApi.Tests.WalletTest(servicesBaseUrl, apiKey);
            var wallet = await apiWalletTest.Create();
            if (wallet != null && !String.IsNullOrEmpty(wallet.Address) && !String.IsNullOrEmpty(wallet.PrivateKey))
                success = true;
            Console.WriteLine("-WebApi: " + (success ? "Success" : "Fail"));

            //Test the token
            Console.WriteLine("Verifying the token... via Lux");
            Treatail.NEO.Tests.TokenTest tokenTest = new Treatail.NEO.Tests.TokenTest(network, privateKeyHex);
            success = tokenTest.VerifyToken();
            Console.WriteLine(success ? "Success" : "Fail");
            Console.ReadLine();
        }
    }
}
