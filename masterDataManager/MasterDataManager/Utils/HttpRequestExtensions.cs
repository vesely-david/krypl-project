using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MasterDataManager.Utils
{
    public static class HttpRequestExtensions
    {
        public static int Pokus(this HttpClient client)
        {
            return 1;
        }

        public static async Task<T> BinanceSignedRequest<T>(
            this HttpClient client,
            string baseUrl,
            HttpMethod httpMethod,
            Dictionary<string, string> requestParameters, 
            string apiKey,
            string apiSecret)
        {
            requestParameters = requestParameters ?? new Dictionary<string, string>();

            var timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            requestParameters.Add("timestamp", timestamp.ToString());
            var urlToSign = QueryHelpers.AddQueryString(String.Empty, requestParameters);
            var signature = HashHMAC(apiSecret, urlToSign);

            var url = baseUrl + urlToSign + "&signature=" + signature;

            var requestMessage = new HttpRequestMessage(httpMethod, url);
            requestMessage.Headers.Add("X-MBX-APIKEY", apiKey);
            var rawResponse = await client.SendAsync(requestMessage);

            rawResponse.EnsureSuccessStatusCode();

            var responseContent = await rawResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        private static byte[] HashHMAC(string key, string message)
        {
            var hash = new HMACSHA256(key.ToByteArray());
            return hash.ComputeHash(message.ToByteArray());
        }

        public static byte[] ToByteArray(this string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }
    }
}
