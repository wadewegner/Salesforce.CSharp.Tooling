using System.Collections.Generic;

namespace Salesforce.Tooling.APIs.Tests.Models
{
    public class DeployDetails
    {
        public List<AllComponentMessage> allComponentMessages { get; set; }
        public List<object> componentFailures { get; set; }
        public List<ComponentSuccess> componentSuccesses { get; set; }
        public object runTestResult { get; set; }
    }
}