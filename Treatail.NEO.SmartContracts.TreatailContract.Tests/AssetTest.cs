using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treatail.NEO.Core.Logic;
using Treatail.NEO.Core.Models;

namespace Treatail.NEO.Tests
{
    public class AssetTest
    {
        private NetworkType _network = NetworkType.Testnet;
        private string _privateKeyHex = String.Empty;

        public AssetTest(NetworkType network, string privateKeyHex)
        {
            _network = network;
            _privateKeyHex = privateKeyHex;
        }

        public bool Create(string treatailAssetId, string ownerAddress, string assetDetails, bool chargeTTL)
        {
            Contract contract = new Contract(_network, _privateKeyHex);
            return contract.CreateAsset(treatailAssetId, ownerAddress, assetDetails, chargeTTL);
        }

        public string GetDetail(string treatailAssetId)
        {
            Contract contract = new Contract(_network, _privateKeyHex);
            byte[] details = contract.GetAssetDetails(treatailAssetId);
            return ConversionHelper.BytesToHex(details);
        }

        public string GetOwner(string treatailAssetId)
        {
            Contract contract = new Contract(_network, _privateKeyHex);
            byte[] details = contract.GetAssetOwner(treatailAssetId);
            return ConversionHelper.BytesToHex(details);
        }

        public bool Transfer(string treatailAssetId, string fromAddress, string toAddress)
        {
            Contract contract = new Contract(_network, _privateKeyHex);
            return contract.TransferAsset(treatailAssetId, fromAddress, toAddress);
        }
    }
}
