﻿using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace TreatailSmartContract
{
    public class TreatailSmartContract : SmartContract
    {
        private static readonly byte[] _treatailAddress = "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y".ToScriptHash();

        #region Treatail Token (TTL) Specific Parameters
        //Name of the token
        private const string _tokenName = "TretailToken";
        //Symbol for the token
        private const string _tokenSymbol = "TTL";
        //Decimal precision for the token
        private const byte _tokenDecimals = 8;
        //Storage key used to store the total supply
        private const string _tokenTotalSupplyStorageKey = "ttl_total_supply";
        //1 bil, account for 8 decimal places
        private const ulong _tokenMaxSupply = 100000000000000000;
        #endregion

        #region Treatail Asset (TTA) Specific Parameters
        //Storage key for asset create cost
        private const string _createAssetCostStorageKey = "tta_create_cost";
        //Default asset create cost
        private const ulong _createAssetCost = 1;
        #endregion

        public static object Main(string action, params object[] args)
        {
            //TTL Actions
            if (action == "name") //NEP5 required
                return Name();
            if (action == "symbol") //NEP5 required
                return Symbol();
            else if (action == "decimals") //NEP5 required
                return Decimals();
            else if (action == "balanceOf") //NEP5 required
                return BalanceOf((byte[])args[0]);
            else if (action == "totalSupply") //NEP5 required
                return TotalSupply();
            else if (action == "transfer") //NEP5 required
                return Transfer((byte[])args[0], (byte[])args[1], (BigInteger)args[2]);
            else if (action == "deploy")
                return DeployTokens();

            //TTA Actions
            if (action == "setassetcreatecost")
                return SetAssetCreateCost((BigInteger)args[0]);
            else if (action == "getactioncreatecost")
                return GetAssetCreateCost();
            else if (action == "getassetdetails")
                return GetAssetDetails((byte[])args[0]);
            else if (action == "createasset")
                return CreateAsset((byte[])args[0], (byte[])args[1], (byte[])args[2]);
            else if (action == "transferasset")
                return TransferAsset((byte[])args[0], (byte[])args[1], (byte[])args[2]);

            Runtime.Notify("Missing or invalid action");
            return false;
        }

        #region Treatail Token (TTL) Methods
        /// <summary>
        /// (NEP5 Required) Returns the decimals of precision used by the token
        /// </summary>
        /// <returns>byte - decimals used</returns>
        public static byte Decimals()
        {
            return _tokenDecimals;
        }

        /// <summary>
        /// (NEP5 Required) Returns the name of the token.  This is the name of the asset in NEO-GUI
        /// </summary>
        /// <returns>string - name of the token</returns>
        public static string Name()
        {
            return _tokenName;
        }

        /// <summary>
        /// (NEP5 Required) Returns the abbreviated symbol / ticker of the token.
        /// </summary>
        /// <returns>string - symbol of the token</returns>
        public static string Symbol()
        {
            return _tokenSymbol;
        }

        /// <summary>
        /// (NEP5 Required) Returns the total token supply deployed in the system.
        /// </summary>
        /// <returns>BigInteger - token supply</returns>
        public static BigInteger TotalSupply()
        {
            BigInteger supply = 0;
            var supplyValue = Storage.Get(Storage.CurrentContext, _tokenTotalSupplyStorageKey);

            if (supplyValue != null && supplyValue.Length > 0)
                supply = supplyValue.AsBigInteger();

            return supply;
        }

        /// <summary>
        /// (NEP5 Required) Returns the token balance of the account
        /// </summary>
        /// <param name="account">byte[] - account to get balance for</param>
        /// <returns></returns>
        public static BigInteger BalanceOf(byte[] address)
        {
            BigInteger balance = 0;
            var balanceValue = Storage.Get(Storage.CurrentContext, address);

            if (balanceValue != null && balanceValue.Length > 0)
                balance = balanceValue.AsBigInteger();

            return balance;
        }

        /// <summary>
        /// (NEP5 Required) Transfer the specified amount of tokens from one account to another account
        /// </summary>
        /// <param name="from">byte[] - sending address</param>
        /// <param name="to">byte[] - receiving address</param>
        /// <param name="amount">BigInteger - number of tokens to transfer</param>
        /// <returns></returns>
        public static bool Transfer(byte[] from, byte[] to, BigInteger amount)
        {
            //Comment out for debug
            if (!Runtime.CheckWitness(from))
            {
                Runtime.Notify("Cannot send, transaction not signed as the sender address.");
                return false;
            }

            //Let's get the balance of the from account and verify we can do this
            BigInteger fromAccountBalance = BalanceOf(from);

            //Get the balance of the receiving address
            BigInteger toAccountBalance = BalanceOf(to);

            if (fromAccountBalance < amount)
            {
                Runtime.Notify("Cannot transfer.  Insufficient balance in sending account.", from, fromAccountBalance);
                return false;
            }

            //Write the new account balances to storage
            Storage.Put(Storage.CurrentContext, from, fromAccountBalance - amount);
            Storage.Put(Storage.CurrentContext, to, toAccountBalance + amount);

            //Notify the runtime of the transfer
            Runtime.Notify("Deployed", to, from, amount);

            return true;
        }

        /// <summary>
        /// Deploys the tokens to the Treatail admin account
        /// </summary>
        /// <param name="originator">byte[] - originator of the request</param>
        /// <param name="supply">BigInteger</param>
        /// <returns></returns>
        public static bool DeployTokens()
        {
            //If we have the total supply in storage, we've deployed the tokens already
            var totalSupply = TotalSupply();
            if (totalSupply > 0)
            {
                Runtime.Notify("Tokens already deployed.");
                return false;
            }

            //Store the max token supply after we deployed it, this will prevent re-deploys
            Storage.Put(Storage.CurrentContext, _tokenTotalSupplyStorageKey, _tokenMaxSupply);
            //Write the storage record for the "deploy" of the tokens to the Treatail admin account
            Storage.Put(Storage.CurrentContext, _treatailAddress, _tokenMaxSupply);
            //Let's check the owner address and output the actual balance to ensure the storage set / get worked.
            Runtime.Notify("Deployed", _treatailAddress, BalanceOf(_treatailAddress));

            return true;
        }
        #endregion

        #region Treatail Asset (TTA) Methods
        /// <summary>
        /// Gets the number of TTL required to create a Treatail Asset
        /// </summary>
        /// <returns>BigInteger - cost in TTL</returns>
        public static BigInteger GetAssetCreateCost()
        {
            byte[] costValue = Storage.Get(Storage.CurrentContext, _createAssetCostStorageKey);

            //If we don't have any pushed updates in storage, use the default
            if (costValue == null)
                return _createAssetCost;

            return costValue.AsBigInteger();
        }

        /// <summary>
        /// Sets the cost in TTL required to create a Treatail Asset
        /// </summary>
        /// <param name="cost">BigInteger - number of TTL required to create a Treatail Asset</param>
        /// <returns>bool - success</returns>
        public static bool SetAssetCreateCost(BigInteger cost)
        {
            Storage.Put(Storage.CurrentContext, _createAssetCostStorageKey, cost);
            Runtime.Notify("Asset create cost updated", cost);
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
                Runtime.Notify("Asset already exists", treatailId);
                return false;
            }

            //Charge the TTL for the transaction
            if (!ChargeAssetCreateCost(address))
                return false;

            //Create the asset
            Storage.Put(Storage.CurrentContext, string.Concat("D", treatailId.AsString()), assetDetails);
            Runtime.Notify("Asset created", treatailId);

            //Set the asset owner
            Storage.Put(Storage.CurrentContext, string.Concat("O", treatailId.AsString()), address);
            Runtime.Notify("Asset owner updated", treatailId, address);

            return true;
        }

        /// <summary>
        /// Charges the account for the TTL required to create an asset
        /// </summary>
        /// <param name="address">byte[] - address to charge</param>
        /// <returns>bool - success</returns>
        public static bool ChargeAssetCreateCost(byte[] address)
        {
            BigInteger assetCreateCost = GetAssetCreateCost();
            BigInteger balance = BalanceOf(address);
            if (balance < assetCreateCost)
            {
                Runtime.Notify("Address does not have sufficient TTL to create asset", address, balance);
                return false;
            }

            return Transfer(address, _treatailAddress, assetCreateCost);
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
            if (!Runtime.CheckWitness(from))
            {
                Runtime.Notify("Transaction not signed with sender account", from);
                return false;
            }

            //Check that they are the owner
            byte[] owner = Storage.Get(Storage.CurrentContext, string.Concat("O", treatailId.AsString()));
            if (owner == null || owner.Length == 0 || owner != from)
            {
                Runtime.Notify("Sender account is not the owner of this asset", treatailId, from);
                return false;
            }

            //Update the asset ownership record
            Storage.Put(Storage.CurrentContext, string.Concat("O", treatailId.AsString()), to);
            Runtime.Notify("Transferred", treatailId, from, to);
            return true;
        }
        #endregion

    }
}