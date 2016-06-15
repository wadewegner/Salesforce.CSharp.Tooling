using System.Collections.Generic;

namespace Salesforce.Tooling.APIs.Tests.Models
{
    public class QueryResult<T>
    {
        public int size { get; set; }
        public int totalSize { get; set; }
        public bool done { get; set; }
        public object queryLocator { get; set; }
        public string entityTypeName { get; set; }
        public List<T> records { get; set; }
    }
}