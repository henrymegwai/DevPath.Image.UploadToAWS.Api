using BlinkCash.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BlinkCash.Infrastructure.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage> Get(string url, IDictionary<string, string> headers, string token)
        {
            var client = _clientFactory.CreateClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            foreach (var tm in headers)
            {
                client.DefaultRequestHeaders.Add(tm.Key, tm.Value);
            }
            return await client.GetAsync(url);
        }
        public async Task<HttpResponseMessage> Post(string url, HttpContent content, IDictionary<string, string> headers, string token)
        {
            var client = _clientFactory.CreateClient("http");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            foreach (var tm in headers)
            {
                client.DefaultRequestHeaders.Add(tm.Key, tm.Value);
            }
            return await client.PostAsync(url, content);
        }
        public async Task<HttpResponseMessage> Put(string url, HttpContent content, IDictionary<string, string> headers, string token)
        {
            var client = _clientFactory.CreateClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            foreach (var tm in headers)
            {
                client.DefaultRequestHeaders.Add(tm.Key, tm.Value);
            }
            return await client.PutAsync(url, content);
        }


        public async Task<HttpResponseMessage> PostTryHeader(string url, HttpContent content, IDictionary<string, string> headers, string token)
        {
            ;
            using (var client = _clientFactory.CreateClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);


                return await client.PostAsync(url, content);
            }
        }
    }
}
