using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treatail.NEO.TestHarness
{
    public static class WebApiTest
    {
        public static void Run(string serviceBaseUrl, string apiKey, string privateKeyHex, string fromAddress, string toAddress)
        {
            WebApi.Tests.WalletTest walletTest = new WebApi.Tests.WalletTest(serviceBaseUrl, apiKey);
            WebApi.Tests.TokenTest tokenTest = new WebApi.Tests.TokenTest(serviceBaseUrl, apiKey, privateKeyHex);
            WebApi.Tests.AssetTest assetTest = new WebApi.Tests.AssetTest(serviceBaseUrl, apiKey, privateKeyHex);

            bool success = false;
            decimal balance = 0;
            string testAssetId = "TESTASSET12345";

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("TREATAIL WEBAPI TEST");
            Console.WriteLine("----------------------------");
            Console.WriteLine("Base Api Url: " + serviceBaseUrl);
            Console.WriteLine("Private Key:" + privateKeyHex);
            Console.WriteLine("From Address:" + fromAddress);
            Console.WriteLine("To Address:" + toAddress);

            Console.WriteLine();
            //Test the wallet generation
            Console.WriteLine("Wallet generation...");
            Console.WriteLine((walletTest.Create() ? "Success" : "Fail"));

            Console.WriteLine();
            Console.WriteLine("Check the wallet balances for the private key...");
            //Get NEO balance
            try
            {
                balance = walletTest.GetNEOBalance(privateKeyHex);
                Console.WriteLine("NEO Balance: " + balance);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not get NeoGas Balance: " + ex);
            }
            //Get Gas balance
            try
            {
                balance = walletTest.GetGASBalance(privateKeyHex);
                Console.WriteLine("GAS Balance: " + balance);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not get GAS Balance: " + ex);
            }
            try
            {
                balance = walletTest.GetTTLBalance(privateKeyHex);
                Console.WriteLine("TTLBalance " + balance);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Balance fail " + ex.Message);
                Console.ReadLine();
            }

            //Check transfer
            Console.WriteLine();
            Console.WriteLine("Transferring tokens...");
            try
            {
                success = tokenTest.Transfer(fromAddress, toAddress, 10);
                Console.WriteLine(success ? "Success" : "Couldn't transfer tokens.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transfer fail " + ex.Message);
                Console.ReadLine();
            }

            //Check create
            Console.WriteLine();
            Console.WriteLine("Creating asset..."); 
            try
            {
                success = assetTest.Create(testAssetId, fromAddress, "1234567890", false);
                Console.WriteLine(success ? "Success" : "Couldn't create asset.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create fail " + ex.Message);
                Console.ReadLine();
            }

            //Console.WriteLine();
            //Console.WriteLine("Getting details...");
            ////Check details
            //try
            //{
            //    var details = assetTest.GetDetail(testAssetId);
            //    Console.WriteLine(!String.IsNullOrEmpty(details) ? "Success" : "Couldn't get asset details.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Details fail " + ex.Message);
            //    Console.ReadLine();
            //}

            //Console.WriteLine();
            //Console.WriteLine("Getting owner...");
            ////Check owner
            //try
            //{
            //    var owner = assetTest.GetDetail(testAssetId);
            //    Console.WriteLine(!String.IsNullOrEmpty(owner) ? "Success" : "Couldn't get asset owner.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Owner fail " + ex.Message);
            //    Console.ReadLine();
            //}

            //Console.WriteLine();
            //Console.WriteLine("Transferring asset...");
            ////Check transfer
            //try
            //{
            //    success = assetTest.Transfer(testAssetId, fromAddress, toAddress);
            //    Console.WriteLine(success ? "Success" : "Couldn't transfer asset.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Transfer fail " + ex.Message);
            //    Console.ReadLine();
            //}

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("TREATAIL WEBAPI TEST COMPLETE");
        }
    }
}
