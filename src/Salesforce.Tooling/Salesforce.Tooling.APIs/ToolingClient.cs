using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Salesforce.Common;
using Salesforce.Common.Models;
using Salesforce.Common.Serializer;
using Salesforce.Tooling.APIs.Models;

namespace Salesforce.Tooling.APIs
{
    public class ToolingClient
    {
        public Uri ToolingApiUri;
        public string InstanceName;
        public string ApiVersion;

        private HttpClient _httpClient;

        public ToolingClient(AuthenticationClient authClient, string apiVersion = "v36.0", HttpClient httpClient = null)
        {
            var uri = new Uri(authClient.InstanceUrl);
            var host = uri.Host;
            var sections = host.Split(new char[] { '.' });
            
            InstanceName = sections[0];
            ApiVersion = apiVersion;

            var toolingApiUrl = string.Format("https://{0}.salesforce.com/services/data/{1}/tooling/", InstanceName, ApiVersion);
            ToolingApiUri = new Uri(toolingApiUrl);

            if (httpClient == null)
            {
                _httpClient = new HttpClient();
            }
            else
            {
                _httpClient = httpClient;
            }

            _httpClient.DefaultRequestHeaders.UserAgent.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authClient.AccessToken);

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        public async Task<SObjectsModel> SObjects<T>()
        {
            var uri = new Uri(ToolingApiUri, "sobjects");
            var result = await HttpGetAsync<SObjectsModel>(uri);

            return result;
        }

        public async Task<SObjectModel> SObject(string sObject)
        {
            var uri = new Uri(ToolingApiUri, string.Format("sobjects/{0}", sObject));
            var result = await HttpGetAsync<SObjectModel>(uri);

            return result;
        }

        public async Task<SObjectDescribeModel> SObjectDescribe(string sObject)
        {
            var uri = new Uri(ToolingApiUri, string.Format("sobjects/{0}/describe", sObject));
            var result = await HttpGetAsync<SObjectDescribeModel>(uri);

            return result;
        }

        public async Task<SuccessResponse> CreateRecord(string sObjectName, object record)
        {
            var uri = new Uri(ToolingApiUri, string.Format("sobjects/{0}", sObjectName));
            var result = await HttpPostAsync<SuccessResponse>(uri, record);

            return result;
        }

        private async Task<T> HttpGetAsync<T>(Uri uri)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = HttpMethod.Get
            };

            var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var response = await responseMessage.Content.ReadAsDecompressedStringAsync().ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                var jToken = JToken.Parse(response);
                if (jToken.Type == JTokenType.Array)
                {
                    var jArray = JArray.Parse(response);

                    var r = JsonConvert.DeserializeObject<T>(jArray.ToString());
                    return r;
                }
                else
                {
                    var jObject = JObject.Parse(response);

                    var r = JsonConvert.DeserializeObject<T>(jObject.ToString());
                    return r;
                }
            }

            if (responseMessage.Content.Headers.ContentType != null && responseMessage.Content.Headers.ContentType.ToString().Contains("application/json"))
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponses>(response);
                throw new ForceException(errorResponse[0].ErrorCode, errorResponse[0].Message);
            }

            throw new ForceException(Error.NonJsonErrorResponse, response);
        }

        public async Task<T> HttpPostAsync<T>(Uri uri, object inputObject)
        {
            var json = JsonConvert.SerializeObject(inputObject,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DateFormatString = "s"
                });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync(uri, content).ConfigureAwait(false);
            var response = await responseMessage.Content.ReadAsDecompressedStringAsync().ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                var r = JsonConvert.DeserializeObject<T>(response);
                return r;
            }

            if (responseMessage.Content.Headers.ContentType != null && responseMessage.Content.Headers.ContentType.ToString().Contains("application/json"))
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponses>(response);
                throw new ForceException(errorResponse[0].ErrorCode, errorResponse[0].Message);
            }
            
            throw new ForceException(Error.NonJsonErrorResponse, response);
        }
    }
}
