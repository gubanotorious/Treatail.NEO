using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace Treatail.NEO.TreatailTokenSmartContract
{
    public class TreatailToken : SmartContract
    {
        //Name of the token
        private static string _name = "TTL";

        //Symbol for the token
        private static string _symbol = "TTL";

        //Decimal precision for the token
        private static byte _decimals = 8;

        //Storage key used to store the total supply
        private const string _totalSupplyStorageKey = "total_supply";

        //1 bil, account for 8 decimal places
        private const ulong _maxSupply = 100000000000000000; 

        //Owner address
        private static readonly byte[] _owner = "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y".ToScriptHash();

        public static object Main(string action, params object[] args)
        {
            Runtime.Notify(action);

            switch (action)
            {
                case "decimals":
                    return Decimals();
                case "name":
                    return Name();
                case "deploy":
                    return Deploy();
                case "balanceOf":
                    return BalanceOf((byte[])args[0]);
                case "totalSupply":
                    return TotalSupply();
                case "transfer":
                    return Transfer((byte[])args[0], (byte[])args[1], (BigInteger)args[2]);
            }

            return false;
        }

        /// <summary>
        /// Deploys the tokens to the admin account
        /// </summary>
        /// <param name="originator">byte[] - originator of the request</param>
        /// <param name="supply">BigInteger</param>
        /// <returns></returns>
        public static bool Deploy()
        {
            if (!Runtime.CheckWitness(_owner))
            {
                Runtime.Log("No permissions to deploy tokens.");
                return false;
            }
                
            var totalSupply = TotalSupply();
            if (totalSupply > 0)
            {
                Runtime.Notify(totalSupply);
                Runtime.Log("Token(s) already deployed.");
                return false;
            }

            Runtime.Notify("Deploying", _maxSupply);

            //Deploy the full supply of tokens to the admin account
            Storage.Put(Storage.CurrentContext, _totalSupplyStorageKey, _maxSupply);
            Storage.Put(Storage.CurrentContext, _owner, _maxSupply);

            //Let's check the owner address to verify the balance
            var balance = BalanceOf(_owner);
            Transferred(null, _owner, balance);

            return true;
        }


        #region NEP5 Required Methods

        /// <summary>
        /// Returns the decimals of precision used by the token
        /// </summary>
        /// <returns>byte - decimals used</returns>
        public static byte Decimals() => _decimals;
        
        /// <summary>
        /// Returns the name of the token.  This is the name of the asset in NEO-GUI
        /// </summary>
        /// <returns>string - name of the token</returns>
        public static string Name() => _name;

        /// <summary>
        /// Returns the abbreviated symbol / ticker of the token.
        /// </summary>
        /// <returns>string - symbol of the token</returns>
        public static string Symbol() => _symbol;

        /// <summary>
        /// Returns the total token supply deployed in the system.
        /// </summary>
        /// <returns>BigInteger - token supply</returns>
        public static BigInteger TotalSupply()
        {
            var totalSupplyValue = Storage.Get(Storage.CurrentContext, _totalSupplyStorageKey);
            return totalSupplyValue.AsBigInteger();
        }

        /// <summary>
        /// Returns the token balance of the account
        /// </summary>
        /// <param name="account">byte[] - account to get balance for</param>
        /// <returns></returns>
        public static BigInteger BalanceOf(byte[] address)
        {
            var balanceValue = Storage.Get(Storage.CurrentContext, address);
            return balanceValue.AsBigInteger();
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
            //Do we need this for testnet?
            if (!Runtime.CheckWitness(from))
            {
                Runtime.Notify("Cannot send, transaction not signed as the sender address.");
                return false;
            }
                
            //Let's get the balance of the from account and verify we can do this
            byte[] fromValue = Storage.Get(Storage.CurrentContext, from);
            BigInteger fromAccountBalance = 0;
            if (fromValue != null)
                fromAccountBalance = fromValue.AsBigInteger();

            //Get the balance of the receiving address
            byte[] toValue = Storage.Get(Storage.CurrentContext, to);
            BigInteger toAccountBalance = 0;
            if(toValue == null)
                toAccountBalance = toValue.AsBigInteger();

            if(fromAccountBalance < amount)
            {
                Runtime.Notify("Cannot send, insufficient balance in sender address.");
                return false;
            }

            //Write the new account balances to storage
            Storage.Put(Storage.CurrentContext, from, fromAccountBalance - amount);
            Storage.Put(Storage.CurrentContext, to, toAccountBalance + amount);

            //Notify the runtime of the transfer
            Transferred(from, to, amount);

            return true;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Notifies the runtime of transfers
        /// </summary>
        /// <param name="from">byte[] - address that tokens were sent from</param>
        /// <param name="to">byte[] - address that tokens were sent to</param>
        /// <param name="value">BigInteger - number of tokens sent</param>
        private static void Transferred(byte[] from, byte[] to, BigInteger amount)
        {
            Runtime.Notify("TTL Transferred", from, to, amount);
        }

        #endregion
    }
}
