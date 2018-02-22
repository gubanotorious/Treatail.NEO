using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public BigInteger GetCreateCost()
        {
            Contract contract = new Contract(_network, _privateKeyHex);
            return contract.GetAssetCreateCost();
        }

        public bool SetCreateCost(int cost)
        {
            Contract contract = new Contract(_network, _privateKeyHex);
            return contract.SetAssetCreateCost(cost);
        }

        public bool Create(string treatailAssetId, string ownerAddress, string assetDetails, bool chargeTTL)
        {
            Contract contract = new Contract(_network, _privateKeyHex);
            return contract.CreateAsset(treatailAssetId, ownerAddress, assetDetails, chargeTTL);
        }

        public string GetDetail(string treatailAssetId)
        {
            Contract contract = new Contract(_network, _privateKeyHex);
            return contract.GetAssetDetails(treatailAssetId);
        }

        public string GetOwner(string treatailAssetId)
        {
            Contract contract = new Contract(_network, _privateKeyHex);
            return contract.GetAssetOwner(treatailAssetId);
        }

        public bool Transfer(string treatailAssetId, string fromAddress, string toAddress)
        {
            Contract contract = new Contract(_network, _privateKeyHex);
            return contract.TransferAsset(treatailAssetId, fromAddress, toAddress);
        }
    }
}
