using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo.Cryptography;

namespace Treatail.NEO.Service.Logic
{
    public static class ConversionHelper
    {
        public static string BytesToHex(byte[] bytes)
        {
            return bytes.ToHexString();
        }

        public static byte[] HexToBytes(string hexString)
        {
            return hexString.HexToBytes();
        }
    }
}