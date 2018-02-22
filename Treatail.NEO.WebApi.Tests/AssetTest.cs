using Newtonsoft.Json;
using System.Threading.Tasks;
using Treatail.NEO.WebApi.Tests.Logic;
using Treatail.NEO.WebApi.Tests.Models;

namespace Treatail.NEO.WebApi.Tests
{
    public class AssetTest
    {
        private string _assetServiceBaseUrl;
        private string _apiKey;
        private string _privateKeyHex;

        public AssetTest(string serviceBaseUrl, string apiKey, string privateKeyHex)
        {
            _assetServiceBaseUrl = string.Concat(serviceBaseUrl,"/Asset");
            _apiKey = apiKey;
            _privateKeyHex = privateKeyHex;
        }

        public bool Create(string treatailAssetId, string ownerAddress, string assetDetails, bool chargeTTL)
        {
            var createRequest = new AssetCreateRequest
            {
                PrivateKeyHex = _privateKeyHex,
                TreatailAssetId = treatailAssetId,
                OwnerAddress = ownerAddress,
                AssetDetails = assetDetails,
                ChargeTTL = chargeTTL
            };

            var url = string.Format("{0}/Create", _assetServiceBaseUrl);
            var result = ServicesHelper.CallService(ServiceAction.POST, _apiKey, url, JsonConvert.SerializeObject(createRequest));
            return JsonConvert.DeserializeObject<bool>(result);
        }

        public string GetDetail(string treatailAssetId)
        {
            var url = string.Format("{0}/GetDetail/{1}", _assetServiceBaseUrl, treatailAssetId);
            var result = ServicesHelper.CallService(ServiceAction.GET, _apiKey, url, null);
            return JsonConvert.DeserializeObject<string>(result);
        }

        public string GetOwner(string treatailAssetId)
        {
            var url = string.Format("{0}/GetOwner/{1}", _assetServiceBaseUrl, treatailAssetId);
            var result = ServicesHelper.CallService(ServiceAction.GET, _apiKey, url, null);
            return JsonConvert.DeserializeObject<string>(result);
        }

        public bool Transfer(string treatailAssetId, string fromAddress, string toAddress)
        {
            var transferRequest = new AssetTransferRequest
            {
                PrivateKeyHex = _privateKeyHex,
                TreatailAssetId = treatailAssetId,
                FromAddress = fromAddress,
                ToAddress = toAddress
            };

            var url = string.Format("{0}/Transfer", _assetServiceBaseUrl);
            var result = ServicesHelper.CallService(ServiceAction.POST, _apiKey, url, JsonConvert.SerializeObject(transferRequest));
            return JsonConvert.DeserializeObject<bool>(result);
        }
    }
}
