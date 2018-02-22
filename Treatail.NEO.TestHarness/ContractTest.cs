using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treatail.NEO.Core.Logic;

namespace Treatail.NEO.TestHarness
{
    public static class ContractTest
    {
        public static void Run(string privateKeyHex, string fromAddress, string toAddress)
        {
            NetworkType network = NetworkType.Testnet;
            bool success = false;
            decimal balance = 0;
            string testAssetId = "TestAssetId123";

            Tests.WalletTest walletTest = new Tests.WalletTest();
            Tests.TokenTest tokenTest = new Tests.TokenTest(network, privateKeyHex);
            Tests.AssetTest assetTest = new Tests.AssetTest(network, privateKeyHex);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("TREATAIL SMART CONTRACT TEST");
            Console.WriteLine("----------------------------");
            Console.WriteLine("Private Key:" + privateKeyHex);
            Console.WriteLine("From Address:" + fromAddress);
            Console.WriteLine("To Address:" + toAddress);


            //Test the wallet generation
            Console.WriteLine();
            Console.Write("Testing wallet generation:");
            Console.WriteLine((walletTest.Create() ? "Success" : "Fail"));

            Console.WriteLine();
            Console.WriteLine("Check the wallet balances for the private key...");
            //Get NEO balance
            try
            {
                balance = walletTest.GetNEOBalance(network, privateKeyHex);
                Console.WriteLine("NEO Balance: " + balance);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not get NeoGas Balance: " + ex);
            }
            //Get Gas balance
            try
            {
                balance = walletTest.GetGASBalance(network, privateKeyHex);
                Console.WriteLine("GAS Balance: " + balance);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not get GAS Balance: " + ex);
            }
            //Check the wallet balance
            try
            {
                balance = tokenTest.GetBalance();
                Console.WriteLine("TTL Balance " + balance);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Balance fail " + ex.Message);
                Console.ReadLine();
            }

            ////Check transfer
            Console.WriteLine();
            Console.WriteLine("Transferring TTL...");
            try
            {
                success = tokenTest.Transfer(fromAddress, toAddress, 50);
                Console.WriteLine(success ? "Success" : "Couldn't transfer tokens.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transfer fail " + ex.Message);
                Console.ReadLine();
            }

            //Check the asset create cost
            Console.WriteLine();
            Console.WriteLine("Getting asset create cost");
            try
            {
                var cost = assetTest.GetCreateCost();
                Console.WriteLine("Asset Create Cost: " + cost);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not get create cost " + ex.Message);
                Console.ReadLine();
            }

            //Set the asset create cost
            Console.WriteLine("Setting asset create cost");
            try
            {
                assetTest.SetCreateCost(1);
                Console.WriteLine("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not set create cost " + ex.Message);
                Console.ReadLine();
            }

            Console.WriteLine();
            Console.WriteLine("Creating asset...");
            //Check create
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

            Console.WriteLine();
            Console.WriteLine("Getting details...");
            //Check details
            try
            {
                var details = assetTest.GetDetail(testAssetId);
                Console.WriteLine("Details: " + details);
                Console.WriteLine(!String.IsNullOrEmpty(details) ? "Success" : "Couldn't get asset details.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Details fail " + ex.Message);
                Console.ReadLine();
            }

            Console.WriteLine();
            Console.WriteLine("Getting owner...");
            //Check owner
            try
            {
                var owner = assetTest.GetOwner(testAssetId);
                Console.WriteLine("Owner: " + owner);
                Console.WriteLine(!String.IsNullOrEmpty(owner) ? "Success" : "Couldn't get asset owner.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Owner fail " + ex.Message);
                Console.ReadLine();
            }

            Console.WriteLine();
            Console.WriteLine("Transferring asset...");
            //Check transfer
            try
            {
                success = assetTest.Transfer(testAssetId, fromAddress, toAddress);
                Console.WriteLine(success ? "Success" : "Couldn't transfer asset.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transfer fail " + ex.Message);
                Console.ReadLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("TREATAIL SMART CONTRACT TEST COMPLETE");
        }
    }
}
