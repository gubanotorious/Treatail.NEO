using Treatail.NEO.Service.Logic;
using Treatail.NEO.Service.Models;

namespace Treatail.NEO.Service.Controllers
{
    /// <summary>
    /// Handles the management of Treatail Assets
    /// </summary>
    public class AssetController : BaseController
    {
        /// <summary>
        /// Create a Treatail Asset
        /// </summary>
        /// <param name="privateKeyHex">string - the private key of the signing wallet for the tranasction</param>
        /// <param name="treatailAssetId">string - the Treatail Asset identifier to be created</param>
        /// <param name="ownerAddress">string - the address to assign ownership to</param>
        /// <param name="assetDetails">string - the payload containing the details of the asset</param>
        /// <returns></returns>
        public bool Create(string privateKeyHex, string treatailAssetId, string ownerAddress, string assetDetails, bool chargeTTL)
        {
            Contract contract = new Contract(CurrentNetwork, privateKeyHex);
            return contract.CreateAsset(ConversionHelper.HexToBytes(treatailAssetId), ConversionHelper.HexToBytes(ownerAddress), ConversionHelper.HexToBytes(assetDetails), chargeTTL);
        }

        /// <summary>
        /// Returns the details payload about the requested Treatail Asset
        /// </summary>
        /// <param name="treatailAssetId">string - Treatail Asset identifier</param>
        /// <returns>string - payload containing the details about the asset</returns>
        public string GetDetail(string treatailAssetId)
        {
            Contract contract = new Contract(CurrentNetwork, null);
            byte[] details = contract.GetAssetDetails(ConversionHelper.HexToBytes(treatailAssetId));
            return ConversionHelper.BytesToHex(details);
        }

        /// <summary>
        /// Returns the address of the owner of the specified asset
        /// </summary>
        /// <param name="treatailAssetId">string - Treatail Asset identifier</param>
        /// <returns>string - payload containing the details about the asset</returns>
        public string GetOwner(string treatailAssetId)
        {
            Contract contract = new Contract(CurrentNetwork, null);
            byte[] details = contract.GetAssetOwner(ConversionHelper.HexToBytes(treatailAssetId));
            return ConversionHelper.BytesToHex(details);
        }

        /// <summary>
        /// Transfers a Treatail Asset from one address to another
        /// </summary>
        /// <param name="privateKeyHex">string - the private key signing the transaction</param>
        /// <param name="treatailAssetId">string - the Treatail Asset identifier</param>
        /// <param name="fromAddress">string - the address to send the asset from</param>
        /// <param name="toAddress">string - the address to send the asset to</param>
        /// <returns></returns>
        public bool TransferAsset(string privateKeyHex, string treatailAssetId, string fromAddress, string toAddress)
        {
            Contract contract = new Contract(CurrentNetwork, privateKeyHex);
            return contract.TransferAsset(ConversionHelper.HexToBytes(treatailAssetId), ConversionHelper.HexToBytes(fromAddress), ConversionHelper.HexToBytes(toAddress));
        }
    }
}