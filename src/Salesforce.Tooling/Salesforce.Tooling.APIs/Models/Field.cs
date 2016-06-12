using System.Collections.Generic;

namespace Salesforce.Tooling.APIs.Models
{
    public class Field
    {
        public bool aggregatable { get; set; }
        public bool autoNumber { get; set; }
        public int byteLength { get; set; }
        public bool calculated { get; set; }
        public object calculatedFormula { get; set; }
        public bool cascadeDelete { get; set; }
        public bool caseSensitive { get; set; }
        public object controllerName { get; set; }
        public bool createable { get; set; }
        public bool custom { get; set; }
        public object defaultValue { get; set; }
        public object defaultValueFormula { get; set; }
        public bool defaultedOnCreate { get; set; }
        public bool dependentPicklist { get; set; }
        public bool deprecatedAndHidden { get; set; }
        public int digits { get; set; }
        public bool displayLocationInDecimal { get; set; }
        public bool encrypted { get; set; }
        public bool externalId { get; set; }
        public object extraTypeInfo { get; set; }
        public bool filterable { get; set; }
        public object filteredLookupInfo { get; set; }
        public bool groupable { get; set; }
        public bool highScaleNumber { get; set; }
        public bool htmlFormatted { get; set; }
        public bool idLookup { get; set; }
        public object inlineHelpText { get; set; }
        public string label { get; set; }
        public int length { get; set; }
        public object mask { get; set; }
        public object maskType { get; set; }
        public string name { get; set; }
        public bool nameField { get; set; }
        public bool namePointing { get; set; }
        public bool nillable { get; set; }
        public bool permissionable { get; set; }
        public List<object> picklistValues { get; set; }
        public int precision { get; set; }
        public bool queryByDistance { get; set; }
        public object referenceTargetField { get; set; }
        public List<object> referenceTo { get; set; }
        public string relationshipName { get; set; }
        public object relationshipOrder { get; set; }
        public bool restrictedDelete { get; set; }
        public bool restrictedPicklist { get; set; }
        public int scale { get; set; }
        public string soapType { get; set; }
        public bool sortable { get; set; }
        public string type { get; set; }
        public bool unique { get; set; }
        public bool updateable { get; set; }
        public bool writeRequiresMasterRead { get; set; }
    }
}