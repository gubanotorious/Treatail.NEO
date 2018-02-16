using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace Treatail.NEO.TreatailAssetSmartContract
{
    public class TreatailAsset : SmartContract
    {
        //Name of the token
        private static string _name = "TTL";

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

            switch (action)
            {
                case "details":
                    return Details((string)args[0]);
                case "create":
                    if (args.Length < 2)
                    {
                        Runtime.Notify("Insufficient parameters provided to create asset.");
                        return false;
                    }
                    return Create((string)args[0], (byte[])args[1], (byte[])args[2]);
                case "transfer":
                    if (args.Length < 3)
                    {
                        Runtime.Notify("Insufficient parameters provided to transfer asset.");
                        return false;
                    }
                    return Transfer((string)args[0],(byte[])args[1],(byte[])args[2]);         
            }

            return false;
        }

        /// <summary>
        /// Used to retrieve a Treatail Asset
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static byte[] Details(string treatailId)
        {
            return GetAssetDetails(treatailId);
        }

        /// <summary>
        /// This will create a treatail asset.  Owner information is already seralized into the payload
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool Create(string treatailId, byte[] address, byte[] assetDetails)
        {
            if (!Runtime.CheckWitness(_treatail))
            {
                Runtime.Log("You do not have permissions to create assets.");
                return false;
            }
            else if(address == null || address.Length == 0)
            {
                Runtime.Notify("Invalid address provided for create");
                return false;
            }
            else if (assetDetails == null || assetDetails.Length == 0)
            {
                Runtime.Notify("No asset details provided");
                return false;
            }
           
            //Verify the asset doesn't already exist
            byte[] treatailAsset = Details(treatailId);
            if (treatailAsset.Length > 0)
            {
                Runtime.Notify("Asset already exists and cannot be created.  Use transfer instead.");
                return false;
            }

            //Set the asset owner
            SetAssetOwner(treatailId, address);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="treatailId">string - treatail asset identifier</param>
        /// <param name="from">byte[] - address of the current owner of the treatail asset</param>
        /// <param name="to">byte[] - address of the new owner for the asset</param>
        /// <returns>bool - success</returns>
        public static bool Transfer(string treatailId, byte[] from, byte[] to)
        {
            if(from == null || from.Length == 0)
            {
                Runtime.Notify("No sender address specified for transfer.");
                return false;
            }
            else if (from == null || from.Length == 0)
            {
                Runtime.Notify("No receiving address specified for transfer.");
                return false;
            }

            //Go get the owner info
            byte[] assetOwner = GetAssetOwner(treatailId);                 
            if(assetOwner == null || assetOwner.Length == 0)
            {
                Runtime.Notify("Cannot transfer, asset identifier is invalid.");
                return false;
            }
            else if (!Runtime.CheckWitness(from)) 
            {
                Runtime.Notify("Cannot transfer, transaction not signed with sender account.");
                return false;
            }

            //Update the asset ownership record
            SetAssetOwner(treatailId, to);

            return true;
        }

        /// <summary>
        /// Retrieves the owner information for the specified asset
        /// </summary>
        /// <param name="treatailId">string - treatail asset identifier.</param>
        /// <returns>byte[] - address of the current asset owner</returns>
        private static byte[] GetAssetOwner(string treatailId)
        {
            var key = _assetOwnerPrefix + treatailId;
            return Storage.Get(Storage.CurrentContext, key);
        }

        /// <summary>
        /// Sets the owner for the asset
        /// </summary>
        /// <param name="treatailId">string - treatail identifier for the asset</param>
        /// <param name="owner">byte[] - address to assign the asset ownership to</param>
        /// <returns>bool - success</returns>
        public static bool SetAssetOwner(string treatailId, byte[] owner)
        {
            var key = _assetOwnerPrefix + treatailId;
            Storage.Put(Storage.CurrentContext, key, owner);
            return true;
        }

        /// <summary>
        /// Retrieves the asset details
        /// </summary>
        /// <param name="treatailId">string - treatail asset identifier</param>
        /// <returns>byte[] - retrieves the details for the specified asset</returns>
        private static byte[] GetAssetDetails(string treatailId)
        {
            var key = _assetDetailsPrefix + treatailId;
            return Storage.Get(Storage.CurrentContext, key);
        }

        /// <summary>
        /// Sets the asset details in NEO Storage
        /// </summary>
        /// <param name="treatailId">string - treatail asset identifier</param>
        /// <param name="assetDetails">byte[] - the details payload to be written for the asset</param>
        /// <returns></returns>
        private static bool SetAssetDetails(string treatailId, byte[] assetDetails)
        {
            var key = _assetDetailsPrefix + treatailId;
            Storage.Put(Storage.CurrentContext, key, assetDetails);
            return true;
        }

        /// <summary>
        /// Notify the runtime that an asset was transferred
        /// </summary>
        /// <param name="from">byte[] - address that the asset was sent from</param>
        /// <param name="to">byte[] - address that the asset was sent to</param>
        /// <param name="treatailId">string - treatail asset identifier</param>
        private static void Transferred(byte[] from, byte[] to, string treatailId)
        {
            Runtime.Notify("Asset Transferred", from, to, treatailId);
        }
    }
}
