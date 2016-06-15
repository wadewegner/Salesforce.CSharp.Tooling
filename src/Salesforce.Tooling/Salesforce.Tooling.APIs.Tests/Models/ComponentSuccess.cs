namespace Salesforce.Tooling.APIs.Tests.Models
{
    public class ComponentSuccess
    {
        public bool changed { get; set; }
        public object columnNumber { get; set; }
        public string componentType { get; set; }
        public bool created { get; set; }
        public string createdDate { get; set; }
        public bool deleted { get; set; }
        public string fileName { get; set; }
        public bool forPackageManifestFile { get; set; }
        public string fullName { get; set; }
        public string id { get; set; }
        public bool knownPackagingProblem { get; set; }
        public object lineNumber { get; set; }
        public object problem { get; set; }
        public object problemType { get; set; }
        public bool requiresProductionTestRun { get; set; }
        public bool success { get; set; }
        public bool warning { get; set; }
    }
}