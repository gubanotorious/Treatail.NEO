using Newtonsoft.Json;
using System.Threading.Tasks;
using Treatail.NEO.WebApi.Tests.Logic;
using Treatail.NEO.WebApi.Tests.Models;

namespace Treatail.NEO.WebApi.Tests
{
    public class TokenTest
    {
        private string _tokenServiceBaseUrl;
        private string _apiKey;
        private string _privateKeyHex;

        public TokenTest(string serviceBaseUrl, string apiKey, string privateKeyHex)
        {
            _tokenServiceBaseUrl = string.Concat(serviceBaseUrl,"/Token");
            _apiKey = apiKey;
            _privateKeyHex = privateKeyHex;
        }



        public bool Transfer(string fromAddress, string toAddress, int amount)
        {
            var transferRequest = new TokenTransferRequest
            {
                PrivateKeyHex = _privateKeyHex,
                FromAddress = fromAddress,
                ToAddress = toAddress,
                Amount = amount
            };

            var url = string.Format("{0}/Transfer", _tokenServiceBaseUrl);
            var result = ServicesHelper.CallService(ServiceAction.POST, _apiKey, url, JsonConvert.SerializeObject(transferRequest));
            return JsonConvert.DeserializeObject<bool>(result);
        }
    }
}
