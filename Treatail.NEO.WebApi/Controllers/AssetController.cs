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
        public AssetController() : base()
        {

        }

        /// <summary>
        /// Create a Treatail Asset
        /// </summary>
        /// <param name="privateKeyHex">The private key of the signing wallet for the tranasction</param>
        /// <param name="treatailAssetId">Treatail Asset identifier to be created</param>
        /// <param name="ownerAddress">Address to assign ownership to</param>
        /// <param name="assetDetails">Details of the asset</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(string privateKeyHex, string treatailAssetId, string ownerAddress, string assetDetails, bool chargeTTL)
        {
            Contract contract = new Contract(CurrentNetwork, privateKeyHex);
            return Json(contract.CreateAsset(treatailAssetId, ownerAddress, assetDetails, chargeTTL));
        }

        /// <summary>
        /// Returns the details payload about the requested Treatail Asset
        /// </summary>
        /// <param name="id">Treatail Asset identifier</param>
        /// <returns>Details of the asset</returns>
        public ActionResult GetDetail(string id)
        {
            Contract contract = new Contract(CurrentNetwork, null);
            return Json(contract.GetAssetDetails(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns the address of the owner of the specified asset
        /// </summary>
        /// <param name="id">Treatail Asset identifier</param>
        /// <returns>Details of the asset</returns>
        public ActionResult GetOwner(string id)
        {
            Contract contract = new Contract(CurrentNetwork, null);
            return Json(contract.GetAssetOwner(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Transfers a Treatail Asset from one address to another
        /// </summary>
        /// <param name="privateKeyHex">Private key signing the transaction</param>
        /// <param name="treatailAssetId">Treatail Asset identifier</param>
        /// <param name="fromAddress">Address to send the asset from</param>
        /// <param name="toAddress">Address to send the asset to</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Transfer(string privateKeyHex, string treatailAssetId, string fromAddress, string toAddress)
        {
            Contract contract = new Contract(CurrentNetwork, privateKeyHex);
            return Json(contract.TransferAsset(treatailAssetId, fromAddress, toAddress));
        }
    }
}