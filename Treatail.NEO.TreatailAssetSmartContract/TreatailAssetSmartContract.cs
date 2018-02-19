using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract;
using Neo.VM;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.Numerics;

namespace Treatail.NEO.TreatailAssetSmartContract
{
    public class TreatailAsset : SmartContract
    {
        private static string _assetOwnerPrefix = "O";

        private static string _assetDetailsPrefix = "D";

        private static readonly byte[] _treatail = "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y".ToScriptHash();

        public static object Main(string action, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Runtime.Log("Insufficient arguments provided.");
                return false;
            }

            string treatailId = (string)args[0];
            Runtime.Log(string.Concat("Action ",action));
            Runtime.Log(string.Concat("TreatailId ", treatailId));

            switch (action)
            {
                case "details":
                    return Details(treatailId);
                case "create":
                    return Create(treatailId, (byte[])args[1], (byte[])args[2]);
                case "transfer":
                    return Transfer(treatailId, (byte[])args[1], (byte[])args[2]);         
            }

            return false;
        }

        /// <summary>
        /// Used to retrieve details about a Treatail Asset 
        /// </summary>
        /// <param name="treatailId">string - Treatail Asset identifier</param>
        /// <returns></returns>
        public static byte[] Details(string treatailId)
        {
            return Storage.Get(Storage.CurrentContext, string.Concat("D",treatailId));
        }


        /// <summary>
        /// Used to create a Treatail asset and assign the owner
        /// </summary>
        /// <param name="treatailId">string - Treatail Asset identifier</param>
        /// <param name="address">byte[] - address of the Treatail Asset owner</param>
        /// <param name="assetDetails">byte[] - Treatail Asset details payload</param>
        /// <returns></returns>
        public static bool Create(string treatailId, byte[] address, byte[] assetDetails)
        {
            if (!Runtime.CheckWitness(_treatail))
            {
                Runtime.Log("You do not have permissions to create assets.");
                Runtime.Notify("You do not have permissions to create assets.");
                return false;
            }

            ////Verify the asset doesn't already exist
            byte[] treatailAsset = Details(treatailId);         
            if (treatailAsset != null && treatailAsset.Length > 0)
            {
                Runtime.Log("Asset already exists and cannot be created.  Use transfer instead.");
                return false;
            }

            //Set the asset details
            Runtime.Log("Asset not found, creating");
            Storage.Put(Storage.CurrentContext, string.Concat("D",treatailId), assetDetails);
            Runtime.Log("Asset created");

            //Set the asset owner
            Runtime.Log("Assigning...");
            Storage.Put(Storage.CurrentContext, string.Concat("O", treatailId), address);
            Runtime.Log("Asset owner assigned");

            return true;
        }

        /// <summary>
        /// Transfers ownership of a Treatail Asset between addresses 
        /// </summary>
        /// <param name="treatailId">string - Treatail Asset identifier</param>
        /// <param name="from">byte[] - address of the current owner of the Treatail Asset</param>
        /// <param name="to">byte[] - address of the new owner for the Treatail Asset</param>
        /// <returns>bool - success</returns>
        public static bool Transfer(string treatailId, byte[] from, byte[] to)
        {
            //Check the caller
            //if (!Runtime.CheckWitness(from)) 
            //{
            //    Runtime.Log("Cannot transfer, transaction not signed with sender account.");
            //    Runtime.Notify("Cannot transfer, transaction not signed with sender account.");         
            //    return false;
            //}

            //Check that they are the owner
            //byte[] owner = Storage.Get(Storage.CurrentContext, string.Concat("O", treatailId));
            //if(owner == null || owner.Length == 0 || owner != from)
            //{
            //    Runtime.Log("Cannot transfer, from account is not the owner of this asset.");
            //    Runtime.Notify("Cannot transfer, from account is not the owner of this asset.");
            //    return false;
            //}

            //Update the asset ownership record
            //Set the asset owner
            Runtime.Log("Assigning...");
            Storage.Put(Storage.CurrentContext, string.Concat("O", treatailId), to);
            Runtime.Log("Asset transferred");
            return true;
        }

        //[Appcall("17069BAAE1E5A0892E6C97AF0D7BAFD5E876C4E9E0B5319275")] //Contract address
        //public static extern int TransferTTL(byte[] address);
    }
}
