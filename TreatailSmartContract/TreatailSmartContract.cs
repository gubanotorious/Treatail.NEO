using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace TreatailSmartContract
{
    public class TreatailSmartContract : SmartContract
    {
        #region General Parameters

        //Storage key to store the treatail account
        private const string _treatailAccountStorageKey = "treatail_account";

        #endregion


        #region Treatail Token (TTL) Parameters

        //Name of the token
        private static string _tokenName = "TTL";

        //Symbol for the token
        private static string _tokenSymbol = "TTL";

        //Decimal precision for the token
        private static byte _tokenDecimals = 8;

        //Storage key used to store the total supply
        private const string _tokenTotalSupplyStorageKey = "ttl_total_supply";

        //1 bil, account for 8 decimal places
        private const ulong _tokenMaxSupply = 100000000000000000;

        #endregion

        #region Treatail Asset (TTA) Parameters

        private static string _createAssetCostStorageKey = "tta_create_cost";

        private const ulong _createAssetCost = 1;

        #endregion

        public static object Main(string action, params object[] args)
        {
            //TTL Actions
            if (action == "name")
            {
                return Name();
            }
            if (action == "symbol")
            {
                return Symbol();
            }
            else if (action == "decimals")
            {
                return Decimals();
            }
            else if (action == "deploy")
            {
                return DeployTokens((byte[])args[0]);
            }
            else if (action == "balanceOf")
            {
                return BalanceOf((byte[])args[0]);
            }
            else if (action == "totalSupply")
            {
                return TotalSupply();
            }
            else if (action == "transfer")
            {
                return Transfer((byte[])args[0], (byte[])args[1], (BigInteger)args[2]);
            }

            //TTA Actions
            if (action == "setassetcreatecost")
            {
                return SetAssetCreateCost((BigInteger)args[0]);
            }
            else if (action == "getactioncreatecost")
            {
                return GetAssetCreateCost();
            }
            else if (action == "getassetdetails")
            {
                return GetAssetDetails((byte[])args[0]);
            }
            else if (action == "createasset")
            {
                return CreateAsset((byte[])args[0], (byte[])args[1], (byte[])args[2]);
            }
            else if (action == "transferasset")
            {
                return TransferAsset((byte[])args[0], (byte[])args[1], (byte[])args[2]);
            }

            Runtime.Notify("Missing or unknown action.");
            return false;
        }

        #region Treatail Token (TTL) Methods

        #region NEP5 Required Methods

        /// <summary>
        /// Returns the decimals of precision used by the token
        /// </summary>
        /// <returns>byte - decimals used</returns>
        public static byte Decimals() => _tokenDecimals;

        /// <summary>
        /// Returns the name of the token.  This is the name of the asset in NEO-GUI
        /// </summary>
        /// <returns>string - name of the token</returns>
        public static string Name() => _tokenName;

        /// <summary>
        /// Returns the abbreviated symbol / ticker of the token.
        /// </summary>
        /// <returns>string - symbol of the token</returns>
        public static string Symbol() => _tokenSymbol;

        /// <summary>
        /// Returns the total token supply deployed in the system.
        /// </summary>
        /// <returns>BigInteger - token supply</returns>
        public static BigInteger TotalSupply()
        {
            var supplyValue = Storage.Get(Storage.CurrentContext, _tokenTotalSupplyStorageKey);

            BigInteger supply = 0;
            if (supplyValue != null)
                supply = supplyValue.AsBigInteger();

            NotifyTTLMessage(false, "Total Supply", new object[] { supply });

            return supply;
        }

        /// <summary>
        /// Returns the token balance of the account
        /// </summary>
        /// <param name="account">byte[] - account to get balance for</param>
        /// <returns></returns>
        public static BigInteger BalanceOf(byte[] address)
        {
            var balanceValue = Storage.Get(Storage.CurrentContext, address);

            if (balanceValue == null)
                return 0;

            var balance = balanceValue.AsBigInteger();
            Runtime.Notify("TTL", "Balance", address, balance);

            return balance;
        }

        /// <summary>
        /// Transfer the specified amount of tokens from one account to another account
        /// </summary>
        /// <param name="from">byte[] - sending address</param>
        /// <param name="to">byte[] - receiving address</param>
        /// <param name="amount">BigInteger - number of tokens to transfer</param>
        /// <returns></returns>
        public static bool Transfer(byte[] from, byte[] to, BigInteger amount)
        {
            //Comment out for debug
            //if (!Runtime.CheckWitness(from))
            //{
            //    Runtime.Notify("Cannot send, transaction not signed as the sender address.");
            //    return false;
            //}

            //Let's get the balance of the from account and verify we can do this
            BigInteger fromAccountBalance = BalanceOf(from);

            //Get the balance of the receiving address
            BigInteger toAccountBalance = BalanceOf(to);

            if (fromAccountBalance < amount)
            {
                NotifyTTLMessage(true, "Cannot transfer.  Insufficient balance in sending account.", new object[] { from, fromAccountBalance });
                return false;
            }

            //Write the new account balances to storage
            Storage.Put(Storage.CurrentContext, from, fromAccountBalance - amount);
            Storage.Put(Storage.CurrentContext, to, toAccountBalance + amount);

            //Notify the runtime of the transfer
            NotifyTTLMessage(false, "Transferred", new object[] { from, to, amount });

            return true;
        }

        #endregion

        /// <summary>
        /// Deploys the tokens to the admin account
        /// </summary>
        /// <param name="originator">byte[] - originator of the request</param>
        /// <param name="supply">BigInteger</param>
        /// <returns></returns>
        public static bool DeployTokens(byte[] address)
        {
            var totalSupply = TotalSupply();
            if (totalSupply > 0)
            {
                NotifyTTLMessage(true, "Tokens already deployed.", null);
                return false;
            }

            //Deploy the full supply of tokens to the first calling account
            Storage.Put(Storage.CurrentContext, _treatailAccountStorageKey, address);
            Storage.Put(Storage.CurrentContext, _tokenTotalSupplyStorageKey, _tokenMaxSupply);
            Storage.Put(Storage.CurrentContext, address, _tokenMaxSupply);

            //Let's check the owner address to verify the balance
            var balance = BalanceOf(address);
            NotifyTTLMessage(false, "Transferred", new object[] { address, balance });

            return true;
        }

        #endregion

        #region Treatail Asset (TTA) Methods

        /// <summary>
        /// Gets the number of TTL required to create a Treatail Asset
        /// </summary>
        /// <returns>BigInteger - cost in TTL</returns>
        private static BigInteger GetAssetCreateCost()
        {
            var costValue = Storage.Get(Storage.CurrentContext, _createAssetCostStorageKey);

            if (costValue == null)
                return _createAssetCost;

            return costValue.AsBigInteger();
        }

        /// <summary>
        /// Sets the cost in TTL required to create a Treatail Asset
        /// </summary>
        /// <param name="cost">BigInteger - number of TTL required to create a Treatail Asset</param>
        /// <returns>bool - success</returns>
        private static bool SetAssetCreateCost(BigInteger cost)
        {
            Storage.Put(Storage.CurrentContext, _createAssetCostStorageKey, cost);

            NotifyTTAMessage(false, "Asset create cost updated", new object[] { cost });
            return true;
        }

        /// <summary>
        /// Used to retrieve details about a Treatail Asset 
        /// </summary>
        /// <param name="treatailId">string - Treatail Asset identifier</param>
        /// <returns></returns>
        public static byte[] GetAssetDetails(byte[] treatailId)
        {
            return Storage.Get(Storage.CurrentContext, string.Concat("D", treatailId.AsString()));
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
            ////Verify the asset doesn't already exist
            byte[] treatailAsset = GetAssetDetails(treatailId);
            if (treatailAsset != null && treatailAsset.Length > 0)
            {
                NotifyTTAMessage(true, "Asset already exists", new object[] { treatailId });
                return false;
            }

            //Charge the TTL for the transaction
            if (!ChargeAssetCreateCost(address))
                return false;

            //Create the asset
            Storage.Put(Storage.CurrentContext, string.Concat("D", treatailId.AsString()), assetDetails);
            NotifyTTAMessage(false, "Asset created", new object[] { treatailId });

            //Set the asset owner
            Storage.Put(Storage.CurrentContext, string.Concat("O", treatailId.AsString()), address);
            NotifyTTAMessage(false, "Asset owner updated", new object[] { treatailId, address });

            return true;
        }

        /// <summary>
        /// Charges the account for the TTL required to create an asset
        /// </summary>
        /// <param name="address">byte[] - address to charge</param>
        /// <returns>bool - success</returns>
        private static bool ChargeAssetCreateCost(byte[] address)
        {
            BigInteger assetCreateCost = GetAssetCreateCost();
            BigInteger balance = BalanceOf(address);
            if (balance < assetCreateCost)
            {
                NotifyTTAMessage(true, "Address does not have sufficient TTL to create asset", new object[] { address });
                return false;
            }

            byte[] treatailAddress = GetTreatailAddress();
            return Transfer(address, treatailAddress, assetCreateCost);
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
            //    NotifyTTAMessage(true, "Transaction not signed with sender account", new object[] { from });
            //    return false;
            //}

            //Check that they are the owner
            byte[] owner = Storage.Get(Storage.CurrentContext, string.Concat("O", treatailId.AsString()));
            if (owner == null || owner.Length == 0 || owner != from)
            {
                NotifyTTAMessage(true, "Sender account is not the owner of this asset", new object[] { from });
                return false;
            }

            //Update the asset ownership record
            Storage.Put(Storage.CurrentContext, string.Concat("O", treatailId.AsString()), to);
            NotifyTTAMessage(false, "Transferred", new object[] { treatailId });
            return true;
        }

        #endregion


        #region Helpers

        private static byte[] GetTreatailAddress()
        {
            return Storage.Get(Storage.CurrentContext, _treatailAccountStorageKey);
        }

        private static void NotifyTTLMessage(bool error, string message, object[] detail)
        {
            NotifyMessage("TTL", error, message, detail);
        }

        private static void NotifyTTAMessage(bool error, string message, object[] detail)
        {
            NotifyMessage("TTA", error, message, detail);
        }

        private static void NotifyMessage(string entityType, bool error, string message, object[] detail)
        {
            string status = "Error";
            if (!error)
                status = "Info";

            Runtime.Notify(entityType, status, message, detail);
        }

        #endregion
    }
}
