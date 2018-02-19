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
        private static readonly byte[] _treatail = "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y".ToScriptHash();

        public static object Main(string action, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Runtime.Notify("Insufficient arguments provided.");
                return false;
            }

            switch (action)
            {
                case "details":
                    return AssetDetails((byte[])args[0]);
                case "create":
                    return CreateAsset((byte[])args[0], (byte[])args[1], (byte[])args[2]);
                case "transfer":
                    return TransferAsset((byte[])args[0], (byte[])args[1], (byte[])args[2]);         
            }

            return false;
        }

        /// <summary>
        /// Used to retrieve details about a Treatail Asset 
        /// </summary>
        /// <param name="treatailId">string - Treatail Asset identifier</param>
        /// <returns></returns>
        public static byte[] AssetDetails(byte[] treatailId)
        {
            return Storage.Get(Storage.CurrentContext, string.Concat("D",treatailId.AsString()));
        }


        /// <summary>
        /// Used to create a Treatail asset and assign the owner
        /// </summary>
        /// <param name="treatailId">string - Treatail Asset identifier</param>
        /// <param name="address">byte[] - address of the Treatail Asset owner</param>
        /// <param name="assetDetails">byte[] - Treatail Asset details payload</param>
        /// <returns></returns>
        public static bool CreateAsset(byte[] treatailId, byte[] address, byte[] assetDetails)
        {
            //Remove for debugging
            //if (!Runtime.CheckWitness(_treatail))
            //{
            //    Runtime.Notify("You do not have permissions to create assets.");
            //    return false;
            //}

            ////Verify the asset doesn't already exist
            byte[] treatailAsset = AssetDetails(treatailId);         
            if (treatailAsset != null && treatailAsset.Length > 0)
            {
                Runtime.Notify("Asset already exists", treatailId);
                return false;
            }

            //Create the asset
            Storage.Put(Storage.CurrentContext, string.Concat("D",treatailId.AsString()), assetDetails);
            Runtime.Notify("Asset created", treatailId);

            //Set the asset owner
            Storage.Put(Storage.CurrentContext, string.Concat("O", treatailId.AsString()), address);
            Runtime.Notify("Asset transferred",treatailId,address);

            return true;
        }

        /// <summary>
        /// Transfers ownership of a Treatail Asset between addresses 
        /// </summary>
        /// <param name="treatailId">string - Treatail Asset identifier</param>
        /// <param name="from">byte[] - address of the current owner of the Treatail Asset</param>
        /// <param name="to">byte[] - address of the new owner for the Treatail Asset</param>
        /// <returns>bool - success</returns>
        public static bool TransferAsset(byte[] treatailId, byte[] from, byte[] to)
        {
            //Check the caller
            //if (!Runtime.CheckWitness(from)) 
            //{
            //    Runtime.Log("Cannot transfer, transaction not signed with sender account.");
            //    Runtime.Notify("Cannot transfer, transaction not signed with sender account.");         
            //    return false;
            //}

            //Check that they are the owner
            byte[] owner = Storage.Get(Storage.CurrentContext, string.Concat("O", treatailId.AsString()));
            if (owner == null || owner.Length == 0 || owner != from)
            {
                Runtime.Notify("Cannot transfer, from account is not the owner of this asset.");
                return false;
            }

            //Update the asset ownership record
            Storage.Put(Storage.CurrentContext, string.Concat("O", treatailId.AsString()), to);
            Runtime.Notify("Asset transferred", treatailId, to);
            return true;
        }
    }
}
