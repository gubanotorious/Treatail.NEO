using Neo.Cryptography;

namespace Treatail.NEO.Core.Logic
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