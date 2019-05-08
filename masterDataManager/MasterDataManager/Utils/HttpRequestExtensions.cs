using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MasterDataManager.Utils
{
    public static class HttpRequestExtensions
    {
        public static async Task<T> BinanceSignedRequest<T>(
            this HttpClient client,
            string baseUrl,
            HttpMethod httpMethod,
            Dictionary<string, string> requestParameters, 
            string apiKey,
            string apiSecret)
        {
            requestParameters = requestParameters ?? new Dictionary<string, string>();

            if(!requestParameters.ContainsKey("timestamp"))
            {
                var timestamp = GetTimestamp();
                requestParameters.Add("timestamp", timestamp);
            }
            var urlToSign = QueryHelpers.AddQueryString(String.Empty, requestParameters);
            var signature = HashHMAC(apiSecret, urlToSign.Substring(1)); //Without '?' on the beginning

            var url = baseUrl + urlToSign + "&signature=" + signature;

            var requestMessage = new HttpRequestMessage(httpMethod, url);
            requestMessage.Headers.Add("X-MBX-APIKEY", apiKey);

            var rawResponse = await client.SendAsync(requestMessage);

            rawResponse.EnsureSuccessStatusCode();

            var responseContent = await rawResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }


        public static async Task<T> PoloniexSignedRequest<T>(
            this HttpClient client,
            string baseUrl,
            HttpMethod httpMethod,
            Dictionary<string, string> requestParameters,
            string apiKey,
            string apiSecret)
        {
            requestParameters = requestParameters ?? new Dictionary<string, string>();

            if (!requestParameters.ContainsKey("nonce"))
            {
                var timestamp = GetTimestamp();
                requestParameters.Add("nonce", timestamp);
            }
            var urlToSign = QueryHelpers.AddQueryString(String.Empty, requestParameters);
            var signature = HashSha512(apiSecret, urlToSign.Substring(1));

            var requestMessage = new HttpRequestMessage(httpMethod, baseUrl) { Content = new FormUrlEncodedContent(requestParameters)};
            requestMessage.Headers.Add("Key", apiKey);
            requestMessage.Headers.Add("Sign", signature);
            requestMessage.Headers.Add(HttpRequestHeader.ContentType.ToString(), "application/x-www-form-urlencoded");
            requestMessage.Headers.Add(HttpRequestHeader.Accept.ToString(), "*/*");

            var rawResponse = await client.SendAsync(requestMessage);

            rawResponse.EnsureSuccessStatusCode();

            var responseContent = await rawResponse.Content.ReadAsStringAsync();

            var resultObj =  JObject.Parse(responseContent).ToObject(typeof(T));
            return (T)resultObj;
        }


        private static string HashSha512(string key, string message)
        {
            using (HMACSHA512 hmac = new HMACSHA512(key.ToByteArray()))
            {
                var rawHash = hmac.ComputeHash(message.ToByteArray());
                return BitConverter.ToString(rawHash).Replace("-", "").ToLower();
            }

        }

        private static string HashHMAC(string key, string message)
        {
            var hash = new HMACSHA256(key.ToByteArray());
            var rawHash = hash.ComputeHash(message.ToByteArray());
            return BitConverter.ToString(rawHash).Replace("-", "").ToLower();
        }

        public static byte[] ToByteArray(this string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }

        private static long ToUnixTimestamp(DateTime time)
        {
            return (long)(time - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        private static string GetTimestamp()
        {
            return ToUnixTimestamp(DateTime.UtcNow).ToString();
        }
    }
}
