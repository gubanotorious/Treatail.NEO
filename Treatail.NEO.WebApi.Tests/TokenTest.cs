﻿using Newtonsoft.Json;
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

        public async Task<decimal> GetBalance(string address)
        {
            var url = string.Format("{0}/GetBalance/{1}", _tokenServiceBaseUrl, address);
            var result = await ServicesHelper.CallService(ServiceAction.GET, _apiKey, url, JsonConvert.SerializeObject(address));
            return JsonConvert.DeserializeObject<decimal>(result);
        }

        public async Task<bool> Transfer(string fromAddress, string toAddress, decimal amount)
        {
            var transferRequest = new TokenTransferRequest
            {
                PrivateKeyHex = _privateKeyHex,
                FromAddress = fromAddress,
                ToAddress = toAddress,
                Amount = amount
            };

            var url = string.Format("{0}/Transfer", _tokenServiceBaseUrl);
            var result = await ServicesHelper.CallService(ServiceAction.POST, _apiKey, url, JsonConvert.SerializeObject(transferRequest));
            return JsonConvert.DeserializeObject<bool>(result);
        }
    }
}
