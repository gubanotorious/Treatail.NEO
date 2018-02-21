using System;

namespace Treatail.NEO.WebApi.Tests.Models
{
    [Serializable]
    public class AssetCreateRequest
    {
        public string PrivateKeyHex { get; set; }
        public string TreatailAssetId { get; set; }
        public string OwnerAddress { get; set; }
        public string AssetDetails { get; set; }
        public bool ChargeTTL { get; set; }
    }
}
