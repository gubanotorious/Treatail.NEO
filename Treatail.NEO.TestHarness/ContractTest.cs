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
        public static void Run(string privateKeyHex, string address, string address2)
        {
            NetworkType network = NetworkType.Testnet;
            bool success = false;
            decimal balance = 0;
            string testAssetId = "TESTASSET";

            Tests.WalletTest walletTest = new Tests.WalletTest();
            Tests.TokenTest tokenTest = new Tests.TokenTest(network, privateKeyHex);
            Tests.AssetTest assetTest = new Tests.AssetTest(network, privateKeyHex);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("TREATAIL SMART CONTRACT TEST");
            Console.WriteLine("----------------------------");
            Console.WriteLine("Private Key:" + privateKeyHex);
            Console.WriteLine("Address:" + address);
            Console.WriteLine("Address2:" + address2);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Wallet Functions Testing");
            Console.WriteLine("------------------------");
            

            //Test the wallet generation
            Console.WriteLine("Wallet generation...");
            success = walletTest.Create();
            Console.WriteLine((success ? "Success" : "Fail"));

            Console.WriteLine();

            //Get NEO balance
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

            Console.WriteLine();

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

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Token Functions Testing");
            Console.WriteLine("------------------------");
            

            //Test the token exists
            Console.WriteLine("Verifying the token...");
            try
            {
                success = tokenTest.VerifyToken();
                Console.WriteLine("Verification Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Verification Fail " + ex.Message);
            }

            Console.WriteLine();

            //Check the wallet balance
            Console.WriteLine("Getting balance...");
            try
            {
                balance = tokenTest.GetBalance(address);
                Console.WriteLine("Balance " + balance);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Balance fail " + ex.Message);
                Console.ReadLine();
            }

            Console.WriteLine();

            //Check transfer
            Console.WriteLine("Transferring...");
            try
            {
                success = tokenTest.Transfer(address, address2, 1);
                Console.WriteLine(success ? "Success" : "Couldn't transfer tokens.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transfer fail " + ex.Message);
                Console.ReadLine();
            }


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Asset Functions Testing");
            Console.WriteLine("------------------------");

            Console.WriteLine("Getting asset create cost");
            //Check the asset create cost
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

            Console.WriteLine("Setting asset create cost");
            //Check the asset create cost
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

            Console.WriteLine("Creating...");
            //Check create
            try
            {
                success = assetTest.Create(testAssetId, address, "1234567890", false);
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
                var owner = assetTest.GetDetail(testAssetId);
                Console.WriteLine(!String.IsNullOrEmpty(owner) ? "Success" : "Couldn't get asset owner.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Owner fail " + ex.Message);
                Console.ReadLine();
            }

            Console.WriteLine();
            Console.WriteLine("Transferring...");
            //Check transfer
            try
            {
                success = assetTest.Transfer(testAssetId, address, address2);
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
