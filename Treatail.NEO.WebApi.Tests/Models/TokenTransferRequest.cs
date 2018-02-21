using System;

namespace Treatail.NEO.WebApi.Tests.Models
{
    [Serializable]
    public class TokenTransferRequest
    {
        public string PrivateKeyHex { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public decimal Amount { get; set; }
    }
}
