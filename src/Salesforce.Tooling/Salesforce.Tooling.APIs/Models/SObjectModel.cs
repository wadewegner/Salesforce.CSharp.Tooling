using System.Collections.Generic;
using Newtonsoft.Json;

namespace Salesforce.Tooling.APIs.Models
{
    public class SObjectModel
    {
        [JsonProperty(PropertyName = "objectDescribe")]
        public SObject ObjectDescribe { get; set; }

        [JsonProperty(PropertyName = "recentItems")]
        public List<object> RecentItems { get; set; }
    }
}
