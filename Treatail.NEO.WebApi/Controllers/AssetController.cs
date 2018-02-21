using Treatail.NEO.Core.Logic;
using Treatail.NEO.Core.Models;
using System.Web.Mvc;
using Treatail.NEO.WebApi.Logic;

namespace Treatail.NEO.WebApi.Controllers
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
        [HttpPost]
        public ActionResult Create(string privateKeyHex, string treatailAssetId, string ownerAddress, string assetDetails, bool chargeTTL)
        {
            //Check the API key since we're writing and there's a cost associated with it
            if (!ApiHelper.CheckApiKey(Request))
                return Content("Invalid API Key");

            Contract contract = new Contract(CurrentNetwork, privateKeyHex);
            return Json(contract.CreateAsset(ConversionHelper.HexToBytes(treatailAssetId), ConversionHelper.HexToBytes(ownerAddress), ConversionHelper.HexToBytes(assetDetails), chargeTTL));
        }

        /// <summary>
        /// Returns the details payload about the requested Treatail Asset
        /// </summary>
        /// <param name="id">string - Treatail Asset identifier</param>
        /// <returns>string - payload containing the details about the asset</returns>
        public ActionResult GetDetail(string id)
        {
            if (!ApiHelper.CheckApiKey(Request))
                return Content("Invalid API Key");

            Contract contract = new Contract(CurrentNetwork, null);
            byte[] details = contract.GetAssetDetails(ConversionHelper.HexToBytes(id));
            return Json(ConversionHelper.BytesToHex(details), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns the address of the owner of the specified asset
        /// </summary>
        /// <param name="id">string - Treatail Asset identifier</param>
        /// <returns>string - payload containing the details about the asset</returns>
        public ActionResult GetOwner(string id)
        {
            if (!ApiHelper.CheckApiKey(Request))
                return Content("Invalid API Key");

            Contract contract = new Contract(CurrentNetwork, null);
            byte[] details = contract.GetAssetOwner(ConversionHelper.HexToBytes(id));
            return Json(ConversionHelper.BytesToHex(details), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Transfers a Treatail Asset from one address to another
        /// </summary>
        /// <param name="privateKeyHex">string - the private key signing the transaction</param>
        /// <param name="treatailAssetId">string - the Treatail Asset identifier</param>
        /// <param name="fromAddress">string - the address to send the asset from</param>
        /// <param name="toAddress">string - the address to send the asset to</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Transfer(string privateKeyHex, string treatailAssetId, string fromAddress, string toAddress)
        {
            //Check the API key since we're writing and there's a cost associated with it
            if (!ApiHelper.CheckApiKey(Request))
                return Content("Invalid API Key");

            Contract contract = new Contract(CurrentNetwork, privateKeyHex);
            return Json(contract.TransferAsset(ConversionHelper.HexToBytes(treatailAssetId), ConversionHelper.HexToBytes(fromAddress), ConversionHelper.HexToBytes(toAddress)));
        }
    }
}