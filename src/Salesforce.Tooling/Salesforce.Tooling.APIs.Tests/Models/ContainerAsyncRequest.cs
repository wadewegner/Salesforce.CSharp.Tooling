namespace Salesforce.Tooling.APIs.Tests.Models
{
    public class ContainerAsyncRequest
    {
        public Attributes attributes { get; set; }
        public DeployDetails DeployDetails { get; set; }
        public string Id { get; set; }
        public object ErrorMsg { get; set; }
        public bool IsCheckOnly { get; set; }
        public bool IsRunTests { get; set; }
        public string MetadataContainerId { get; set; }
        public object MetadataContainerMemberId { get; set; }
        public string State { get; set; }
    }
}