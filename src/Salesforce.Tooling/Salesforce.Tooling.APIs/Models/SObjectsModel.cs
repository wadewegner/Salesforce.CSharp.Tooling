using System.Collections.Generic;
using Newtonsoft.Json;

namespace Salesforce.Tooling.APIs.Models
{
    public class SObjectsModel
    {
        [JsonProperty(PropertyName = "encoding")]
        public string Encoding { get; set; }

        [JsonProperty(PropertyName = "maxBatchSize")]
        public int MaxBatchSize { get; set; }

        [JsonProperty(PropertyName = "sobjects")]
        public List<SObject> SObjects { get; set; }
    }
}
