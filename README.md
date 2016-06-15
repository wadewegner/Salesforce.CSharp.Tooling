# Salesforce.CSharp.Tooling

A simple C# SDK for interacting with the Salesforce Tooling API using the REST interface.

## Usage

Below you'll find some sample codes for using this library. You can also [review the functional tests](https://github.com/wadewegner/Salesforce.CSharp.Tooling/blob/master/src/Salesforce.Tooling/Salesforce.Tooling.APIs.Tests/Tests.cs) for cues on usage.

### Login

This uses the DeveloperForce.Force library for authentication.

```csharp
var auth = new AuthenticationClient();
var auth.UsernamePasswordAsync(ConsumerKey, ConsumerSecret, Username, Password, TokenRequestEndpointUrl).Wait();
```

### Get all sObjects

```csharp
var toolingClient = new ToolingClient(auth);
var sObjectsResults = await toolingClient.SObjects<SObjectsModel>();
```

### Get a specific sObject (e.g. ApexClass)

```csharp
var traceFlagResult = await toolingClient.SObject("ApexClass");
```

### Describe a specific sObject

```csharp
var apexClassDescribeResult = await toolingClient.SObjectDescribe("ApexClass");
```

### Query a specific sObject (e.g. ApexClass)

```csharp
const string query = "SELECT Id, NamespacePrefix, Name, ApiVersion, Status, IsValid, BodyCrc, Body, LengthWithoutComments, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, SymbolTable, Metadata, FullName FROM ApexClass";

var result = await toolingClient.Query<dynamic>(query);
```

### Create an ApexClass

```csharp
var ticks = DateTime.Now.Ticks;
var apexClassName = "ac" + ticks;

var apexClass = new Models.ApexClass
{
  Body = string.Format("public class {0} {{\n\n}}", apexClassName),
  Name = "n" + ticks
};

var createApexClassResult = await toolingClient.CreateRecord("ApexClass", apexClass);
```

### Create an ApexClass using MetadataContainer

```csharp
var ticks = DateTime.Now.Ticks;
var apexClassName = "ac" + ticks;
var metadataContainerName = "mc" + ticks;

var metadataContainer = new MetadataContainer
{
    Name = metadataContainerName
};

var createMetadataContainerResult = await toolingClient.CreateRecord("MetadataContainer", metadataContainer);

var apexClassMember = new ApexClassMember
{
  MetadataContainerId = createMetadataContainerResult.Id,
  FullName = "fn" + ticks,
  Body = string.Format("public class {0} {{\n\n}}", apexClassName),
  Metadata = new ApexClassMemberMetadata {apiVersion = "36.0", status = "Active"}
};

var createApexClassMemberResult = await toolingClient.CreateRecord("ApexClassMember", apexClassMember);

var containerAsyncRequest = new ContainerAsyncRequest
{
    MetadataContainerId = createMetadataContainerResult.Id,
    IsCheckOnly = true
};

var containerAsyncRequestResult = await toolingClient.CreateRecord("ContainerAsyncRequest", containerAsyncRequest);
```
