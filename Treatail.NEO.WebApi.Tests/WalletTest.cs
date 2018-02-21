using Newtonsoft.Json;
using System.Threading.Tasks;
using Treatail.NEO.Core.Models;
using Treatail.NEO.RestApi.Tests.Logic;

namespace Treatail.NEO.RestApi.Tests
{
    public class WalletTest
    {
        private string _walletServiceBaseUrl;
        private string _apiKey;

        public WalletTest(string walletServiceBaseUrl, string apiKey)
        {
            _walletServiceBaseUrl = walletServiceBaseUrl;
            _apiKey = apiKey;
        }

        public async Task<Wallet> Create()
        {
            var url = string.Format("{0}/Create", _walletServiceBaseUrl);
            var result = await ServicesHelper.CallService(ServiceAction.GET, _apiKey, url, null);
            return JsonConvert.DeserializeObject<Wallet>(result);
        }
    }
}
