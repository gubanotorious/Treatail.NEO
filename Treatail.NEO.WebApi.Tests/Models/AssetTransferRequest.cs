using System;

namespace Treatail.NEO.WebApi.Tests.Models
{
    [Serializable]
    public class AssetTransferRequest
    {
        public string PrivateKeyHex { get; set; }
        public string TreatailAssetId { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
    }
}
