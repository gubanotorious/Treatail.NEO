using System;
using System.Threading.Tasks;
using Treatail.NEO.Core.Logic;

namespace Treatail.NEO.TestHarness
{
    class Program
    {
        private static string _privateKeyHex;
        private static string _address = "AKwhdHvupN2dRrMRTpNAFYgFQZiLmftmz6";
        private static string _address2 = "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y";
        private static string _serviceBaseUrl = "https://neoapi.treatail.com";
        private static string _apiKey = "SECUREKEY";

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            //Get the private key
            Console.Write("Private Key Hex: ");
            _privateKeyHex = Console.ReadLine();

            //Run the contract tests via Lux
            ContractTest.Run(_privateKeyHex, _address, _address2);

            //Run the tests via the WebApi
            //WebApiTest.Run(_serviceBaseUrl, _apiKey, _privateKeyHex, _address, _address2);

            Console.WriteLine("Press any key");
            Console.Read();
        }
    }
}
