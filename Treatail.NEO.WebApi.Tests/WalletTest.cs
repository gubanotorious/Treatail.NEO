using Newtonsoft.Json;
using System.Threading.Tasks;
using Treatail.NEO.Core.Models;
using Treatail.NEO.WebApi.Tests.Logic;

namespace Treatail.NEO.WebApi.Tests
{
    public class WalletTest
    {
        private string _walletServiceBaseUrl;
        private string _apiKey;

        public WalletTest(string serviceBaseUrl, string apiKey)
        {
            _walletServiceBaseUrl = string.Concat(serviceBaseUrl,"/Wallet");
            _apiKey = apiKey;
        }

        public bool Create()
        {
            var url = string.Format("{0}/Create", _walletServiceBaseUrl);
            var result = ServicesHelper.CallService(ServiceAction.GET, _apiKey, url, null);
            var wallet = JsonConvert.DeserializeObject<Wallet>(result);
            if (wallet == null || string.IsNullOrEmpty(wallet.Address) || string.IsNullOrEmpty(wallet.PrivateKey))
                return false;
 
            return true;
        }

        public decimal GetNEOBalance(string privateKeyHex)
        {
            var url = string.Format("{0}/GetNEOBalance", _walletServiceBaseUrl);
            var result = ServicesHelper.CallService(ServiceAction.POST, _apiKey, url, JsonConvert.SerializeObject(new { privateKeyHex=privateKeyHex }));
            return JsonConvert.DeserializeObject<decimal>(result);
        }

        public decimal GetGASBalance(string privateKeyHex)
        {
            var url = string.Format("{0}/GetGASBalance", _walletServiceBaseUrl);
            var result = ServicesHelper.CallService(ServiceAction.POST, _apiKey, url, JsonConvert.SerializeObject(new { privateKeyHex=privateKeyHex }));
            return JsonConvert.DeserializeObject<decimal>(result);
        }

        public decimal GetTTLBalance(string privateKeyHex)
        {
            var url = string.Format("{0}/GetTTLBalance", _walletServiceBaseUrl);
            var result = ServicesHelper.CallService(ServiceAction.POST, _apiKey, url, JsonConvert.SerializeObject(new { privateKeyHex=privateKeyHex }));
            return JsonConvert.DeserializeObject<decimal>(result);
        }
    }
}
