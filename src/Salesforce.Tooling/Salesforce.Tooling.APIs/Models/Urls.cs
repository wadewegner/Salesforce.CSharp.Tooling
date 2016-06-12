using Newtonsoft.Json;

namespace Salesforce.Tooling.APIs.Models
{
    public class Urls
    {
        [JsonProperty(PropertyName = "rowTemplate")]
        public string RowTemplate { get; set; }

        [JsonProperty(PropertyName = "defaultValues")]
        public string DefaultValues { get; set; }

        [JsonProperty(PropertyName = "describe")]
        public string Describe { get; set; }

        [JsonProperty(PropertyName = "sobject")]
        public string SObject { get; set; }

        [JsonProperty(PropertyName = "compactLayouts")]
        public string CompactLayouts { get; set; }

        [JsonProperty(PropertyName = "layouts")]
        public string Layouts { get; set; }

        [JsonProperty(PropertyName = "namedLayouts")]
        public string NamedLayouts { get; set; }

        [JsonProperty(PropertyName = "passwordUtilities")]
        public string PasswordUtilities { get; set; }

        [JsonProperty(PropertyName = "quickActions")]
        public string QuickActions { get; set; }
    }
}