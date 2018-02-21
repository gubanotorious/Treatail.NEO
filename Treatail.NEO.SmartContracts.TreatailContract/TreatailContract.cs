using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace Treatail.NEO.SmartContracts.TreatailContract
{
    /// <summary>
    /// Name: Treatail NEO Smart Contract
    /// Version: 1
    /// Author: Alex Guba
    /// ScriptHash: 0xe15a3b08b56fbcae28391bb1547d303febccda55
    /// Script: 60c56b6c766b00527ac46c766b51527ac4616c766b00c3046e616d65876c766b52527ac46c766b52c3641100616520036c766b53527ac462f5026c766b00c30673796d626f6c876c766b54527ac46c766b54c364110061651a036c766b53527ac462cb026c766b00c308646563696d616c73876c766b55527ac46c766b55c36411006165b2026c766b53527ac4629f026c766b00c30962616c616e63654f66876c766b56527ac46c766b56c36418006c766b51c300c361656c036c766b53527ac4626b026c766b00c30b746f74616c537570706c79876c766b57527ac46c766b57c36411006165a6026c766b53527ac4623c026c766b00c3087472616e73666572876c766b58527ac46c766b58c36428006c766b51c300c36c766b51c351c36c766b51c352c3615272658c036c766b53527ac462f9016c766b00c3066465706c6f79876c766b59527ac46c766b59c36411006165c1056c766b53527ac462cf016c766b00c3127365746173736574637265617465636f7374876c766b5a527ac46c766b5ac36418006c766b51c300c3616503086c766b53527ac46292016c766b00c3126765746173736574637265617465636f7374876c766b5b527ac46c766b5bc364110061654d076c766b53527ac4625c016c766b00c30f676574617373657464657461696c73876c766b5c527ac46c766b5cc36418006c766b51c300c36165d8086c766b53527ac46222016c766b00c30d67657461737365746f776e6572876c766b5d527ac46c766b5dc36418006c766b51c300c36165f2086c766b53527ac462ea006c766b00c30b6372656174656173736574876c766b5e527ac46c766b5ec36441006c766b51c300c36c766b51c351c36c766b51c352c36c766b51c353c361537951795572755172755279527954727552727565e5086c766b53527ac4628b006c766b00c30d7472616e736665726173736574876c766b5f527ac46c766b5fc36428006c766b51c300c36c766b51c351c36c766b51c352c361527265e40b6c766b53527ac462430051c57600194d697373696e67206f7220696e76616c696420616374696f6ec46168124e656f2e52756e74696d652e4e6f7469667961006c766b53527ac46203006c766b53c3616c756651c56b61586c766b00527ac46203006c766b00c3616c756651c56b610c5472657461696c546f6b656e6c766b00527ac46203006c766b00c3616c756651c56b610354544c6c766b00527ac46203006c766b00c3616c756654c56b61006c766b00527ac46168164e656f2e53746f726167652e476574436f6e746578741074746c5f746f74616c5f737570706c79617c680f4e656f2e53746f726167652e4765746c766b51527ac46c766b51c3640e006c766b51c3c000a0620400006c766b52527ac46c766b52c3640f006c766b51c36c766b00527ac46c766b00c36c766b53527ac46203006c766b53c3616c756655c56b6c766b00527ac461006c766b51527ac46168164e656f2e53746f726167652e476574436f6e746578746c766b00c3617c680f4e656f2e53746f726167652e4765746c766b52527ac46c766b52c3640e006c766b52c3c000a0620400006c766b53527ac46c766b53c3640f006c766b52c36c766b51527ac46c766b51c36c766b54527ac46203006c766b54c3616c756658c56b6c766b00527ac46c766b51527ac46c766b52527ac4616c766b00c36168184e656f2e52756e74696d652e436865636b5769746e65737363390061142dbc4c3e09574c8b647428e84e2e4c87504946c76168184e656f2e52756e74696d652e436865636b5769746e657373009c620400006c766b55527ac46c766b55c36465006151c576003a43616e6e6f742073656e642c207472616e73616374696f6e206e6f74207369676e6564206173207468652073656e64657220616464726573732ec46168124e656f2e52756e74696d652e4e6f7469667961006c766b56527ac46275016c766b00c3616584fe6c766b53527ac46c766b51c3616574fe6c766b54527ac46c766b53c36c766b52c39f6c766b57527ac46c766b57c36475006153c576003a43616e6e6f74207472616e736665722e2020496e73756666696369656e742062616c616e636520696e2073656e64696e67206163636f756e742ec476516c766b00c3c476526c766b53c3c46168124e656f2e52756e74696d652e4e6f7469667961006c766b56527ac462c9006168164e656f2e53746f726167652e476574436f6e746578746c766b00c36c766b53c36c766b52c394615272680f4e656f2e53746f726167652e507574616168164e656f2e53746f726167652e476574436f6e746578746c766b51c36c766b54c36c766b52c393615272680f4e656f2e53746f726167652e5075746154c576000b5472616e73666572726564c476516c766b51c3c476526c766b00c3c476536c766b52c3c46168124e656f2e52756e74696d652e4e6f7469667961516c766b56527ac46203006c766b56c3616c756654c56b6161142dbc4c3e09574c8b647428e84e2e4c87504946c76168184e656f2e52756e74696d652e436865636b5769746e657373009c6c766b51527ac46c766b51c36458006151c576002d596f7520646f206e6f74206861766520706572736d697373696f6e7320746f206465706c6f7920746f6b656e73c46168124e656f2e52756e74696d652e4e6f7469667961006c766b52527ac4625e016165dcfb6c766b00527ac46c766b00c300a06c766b53527ac46c766b53c36443006151c5760018546f6b656e7320616c7265616479206465706c6f7965642ec46168124e656f2e52756e74696d652e4e6f7469667961006c766b52527ac462fd006168164e656f2e53746f726167652e476574436f6e746578741074746c5f746f74616c5f737570706c790800008a5d78456301615272680f4e656f2e53746f726167652e507574616168164e656f2e53746f726167652e476574436f6e7465787461142dbc4c3e09574c8b647428e84e2e4c87504946c70800008a5d78456301615272680f4e656f2e53746f726167652e5075746153c57600084465706c6f796564c4765161142dbc4c3e09574c8b647428e84e2e4c87504946c7c4765261142dbc4c3e09574c8b647428e84e2e4c87504946c761653efbc46168124e656f2e52756e74696d652e4e6f7469667961516c766b52527ac46203006c766b52c3616c756653c56b616168164e656f2e53746f726167652e476574436f6e746578740f7474615f6372656174655f636f7374617c680f4e656f2e53746f726167652e4765746c766b00527ac46c766b00c3009c6c766b51527ac46c766b51c3640e00516c766b52527ac46212006c766b00c36c766b52527ac46203006c766b52c3616c756653c56b6c766b00527ac46161142dbc4c3e09574c8b647428e84e2e4c87504946c76168184e656f2e52756e74696d652e436865636b5769746e657373009c6c766b51527ac46c766b51c36467006151c576003c596f7520646f206e6f74206861766520706572736d697373696f6e7320746f206368616e6765207468652061737365742063726561746520636f7374c46168124e656f2e52756e74696d652e4e6f7469667961006c766b52527ac4628e006168164e656f2e53746f726167652e476574436f6e746578740f7474615f6372656174655f636f73746c766b00c3615272680f4e656f2e53746f726167652e5075746152c576001941737365742063726561746520636f73742075706461746564c476516c766b00c3c46168124e656f2e52756e74696d652e4e6f7469667961516c766b52527ac46203006c766b52c3616c756652c56b6c766b00527ac4616168164e656f2e53746f726167652e476574436f6e7465787401446c766b00c37e617c680f4e656f2e53746f726167652e4765746c766b51527ac46203006c766b51c3616c756652c56b6c766b00527ac4616168164e656f2e53746f726167652e476574436f6e74657874014f6c766b00c37e617c680f4e656f2e53746f726167652e4765746c766b51527ac46203006c766b51c3616c75665ac56b6c766b00527ac46c766b51527ac46c766b52527ac46c766b53527ac46161142dbc4c3e09574c8b647428e84e2e4c87504946c76168184e656f2e52756e74696d652e436865636b5769746e657373009c6c766b55527ac46c766b55c36457006151c576002c596f7520646f206e6f742068617665207065726d697373696f6e7320746f2063726561746520617373657473c46168124e656f2e52756e74696d652e4e6f7469667961006c766b56527ac462a6016c766b00c36165a0fe6c766b54527ac46c766b54c3640e006c766b54c3c000a0620400006c766b57527ac46c766b57c36447006152c5760014417373657420616c726561647920657869737473c476516c766b00c3c46168124e656f2e52756e74696d652e4e6f7469667961006c766b56527ac4622f016c766b53c36c766b58527ac46c766b58c3642a00616c766b51c361651a01009c6c766b59527ac46c766b59c3640e00006c766b56527ac462f500616168164e656f2e53746f726167652e476574436f6e7465787401446c766b00c37e6c766b52c3615272680f4e656f2e53746f726167652e5075746152c576000d41737365742063726561746564c476516c766b00c3c46168124e656f2e52756e74696d652e4e6f74696679616168164e656f2e53746f726167652e476574436f6e74657874014f6c766b00c37e6c766b51c3615272680f4e656f2e53746f726167652e5075746153c57600134173736574206f776e65722075706461746564c476516c766b00c3c476526c766b51c3c46168124e656f2e52756e74696d652e4e6f7469667961516c766b56527ac46203006c766b56c3616c756655c56b6c766b00527ac461616529fb6c766b51527ac46c766b00c3616529f66c766b52527ac46c766b52c36c766b51c39f6c766b53527ac46c766b53c3646f006153c57600344164647265737320646f6573206e6f7420686176652073756666696369656e742054544c20746f20637265617465206173736574c476516c766b00c3c476526c766b52c3c46168124e656f2e52756e74696d652e4e6f7469667961006c766b54527ac46233006c766b00c361142dbc4c3e09574c8b647428e84e2e4c87504946c76c766b51c36152726508f66c766b54527ac46203006c766b54c3616c756657c56b6c766b00527ac46c766b51527ac46c766b52527ac46161142dbc4c3e09574c8b647428e84e2e4c87504946c76168184e656f2e52756e74696d652e436865636b5769746e657373009c6c766b54527ac46c766b54c36457006152c5760024496e73756666696369656e74207065726d697373696f6e7320746f207472616e73666572c476516c766b51c3c46168124e656f2e52756e74696d652e4e6f7469667961006c766b55527ac4625a016168164e656f2e53746f726167652e476574436f6e74657874014f6c766b00c37e617c680f4e656f2e53746f726167652e4765746c766b53527ac46c766b53c3641c006c766b53c3c06413006c766b53c36c766b51c39c009c620400516c766b56527ac46c766b56c36469006153c576002e53656e64696e67206163636f756e74206973206e6f7420746865206f776e6572206f662074686973206173736574c476516c766b00c3c476526c766b51c3c46168124e656f2e52756e74696d652e4e6f7469667961006c766b55527ac46288006168164e656f2e53746f726167652e476574436f6e74657874014f6c766b00c37e6c766b52c3615272680f4e656f2e53746f726167652e5075746154c576000b5472616e73666572726564c476516c766b00c3c476526c766b51c3c476536c766b52c3c46168124e656f2e52756e74696d652e4e6f7469667961516c766b55527ac46203006c766b55c3616c7566
    /// </summary>
    public class TreatailContract : SmartContract
    {
        /// <summary>
        /// Does this affect the scripthash?
        /// </summary>
        private static readonly byte[] _treatailAddress = "AKwhdHvupN2dRrMRTpNAFYgFQZiLmftmz6".ToScriptHash();

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
        //Default asset create cost
        private const ulong _createAssetCost = 1;
        //Storage key for asset create cost
        private const string _assetCostStorageKey = "tta_create_cost";
        //Prefix used for asset details storage
        private const string _assetStorageDetailPrefix = "D";
        //Prefix used for asset owner storage
        private const string _assetStorageOwnerPrefix = "O";
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
            else if (action == "getassetcreatecost")
                return GetAssetCreateCost();
            else if (action == "getassetdetails")
                return GetAssetDetails((byte[])args[0]);
            else if (action == "getassetowner")
                return GetAssetOwner((byte[])args[0]);
            else if (action == "createasset")
                return CreateAsset((byte[])args[0], (byte[])args[1], (byte[])args[2], (bool)args[3]);
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
            //From account can transfer, and treatail can always transfer on behalf of the user
            if (!Runtime.CheckWitness(from) && !Runtime.CheckWitness(_treatailAddress))
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
            Runtime.Notify("Transferred", to, from, amount);

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
            //Only Treatail should be calling to deploy tokens
            if (!Runtime.CheckWitness(_treatailAddress))
            {
                Runtime.Notify("You do not have persmissions to deploy tokens");
                return false;
            }

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
            byte[] costValue = Storage.Get(Storage.CurrentContext, _assetCostStorageKey);

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
            if (!Runtime.CheckWitness(_treatailAddress))
            {
                Runtime.Notify("You do not have persmissions to change the asset create cost");
                return false;
            }

            Storage.Put(Storage.CurrentContext, _assetCostStorageKey, cost);
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
            return Storage.Get(Storage.CurrentContext, string.Concat(_assetStorageDetailPrefix, treatailId.AsString()));
        }

        public static byte[] GetAssetOwner(byte[] treatailId)
        {
            return Storage.Get(Storage.CurrentContext, string.Concat(_assetStorageOwnerPrefix, treatailId.AsString()));
        }

        /// <summary>
        /// Used to create a Treatail asset and assign the owner
        /// </summary>
        /// <param name="treatailId">string - Treatail Asset identifier</param>
        /// <param name="address">byte[] - address of the Treatail Asset owner</param>
        /// <param name="assetDetails">byte[] - Treatail Asset details payload</param>
        /// <returns></returns>
        public static bool CreateAsset(byte[] treatailId, byte[] address, byte[] assetDetails, bool chargeForCreate)
        {
            //Treatail needs to create the assets
            if (!Runtime.CheckWitness(_treatailAddress))
            {
                Runtime.Notify("You do not have permissions to create assets");
                return false;
            }

            ////Verify the asset doesn't already exist
            byte[] treatailAsset = GetAssetDetails(treatailId);
            if (treatailAsset != null && treatailAsset.Length > 0)
            {
                Runtime.Notify("Asset already exists", treatailId);
                return false;
            }

            if (chargeForCreate)
            {
                //Charge the account for the TTL for the transaction
                if (!ChargeAssetCreateCost(address))
                    return false;
            }

            //Create the asset
            Storage.Put(Storage.CurrentContext, string.Concat(_assetStorageDetailPrefix, treatailId.AsString()), assetDetails);
            Runtime.Notify("Asset created", treatailId);

            //Set the asset owner
            Storage.Put(Storage.CurrentContext, string.Concat(_assetStorageOwnerPrefix, treatailId.AsString()), address);
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
            //Check the caller, treatail can always transfer on the users behalf
            if (!Runtime.CheckWitness(_treatailAddress))
            {
                Runtime.Notify("Insufficient permissions to transfer", from);
                return false;
            }

            //Check that they are the owner
            byte[] owner = Storage.Get(Storage.CurrentContext, string.Concat(_assetStorageOwnerPrefix, treatailId.AsString()));
            if (owner == null || owner.Length == 0 || owner != from)
            {
                Runtime.Notify("Sending account is not the owner of this asset", treatailId, from);
                return false;
            }

            //Update the asset ownership record
            Storage.Put(Storage.CurrentContext, string.Concat(_assetStorageOwnerPrefix, treatailId.AsString()), to);
            Runtime.Notify("Transferred", treatailId, from, to);
            return true;
        }
        #endregion

    }
}
