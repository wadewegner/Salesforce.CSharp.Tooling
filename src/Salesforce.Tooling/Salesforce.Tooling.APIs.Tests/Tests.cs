﻿using System;
using System.Configuration;
using System.Dynamic;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using NUnit.Framework;
using Salesforce.Common;
using Salesforce.Tooling.APIs.Models;
using Salesforce.Tooling.APIs.Tests.Models;

namespace Salesforce.Tooling.APIs.Tests
{
    [TestFixture]
    public class Tests
    {
        private static readonly string TokenRequestEndpointUrl = ConfigurationManager.AppSettings["TokenRequestEndpointUrl"];
        private static readonly string ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
        private static readonly string ConsumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
        private static readonly string Username = ConfigurationManager.AppSettings["Username"];
        private static readonly string Password = ConfigurationManager.AppSettings["Password"];

        private AuthenticationClient _auth;
        private ToolingClient _toolingClient;

        [TestFixtureSetUp]
        public void Init()
        {
            // Use TLS 1.2 (instead of defaulting to 1.0)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _auth = new AuthenticationClient();
            _auth.UsernamePasswordAsync(ConsumerKey, ConsumerSecret, Username, Password, TokenRequestEndpointUrl).Wait();

            _toolingClient = new ToolingClient(_auth);
        }

        [Test]
        public void AuthenticationClient()
        {
            Assert.IsNotNull(_auth);
            Assert.IsNotNull(_auth.AccessToken);
            Assert.IsNotNull(_auth.ApiVersion);
            Assert.IsNotNull(_auth.Id);
            Assert.IsNotNull(_auth.InstanceUrl);
            Assert.IsNull(_auth.RefreshToken);

            var uri = new Uri(_auth.InstanceUrl);

            Assert.IsNotNull(uri);
        }

        [Test]
        public void GetProperInstanceAndUri()
        {
            Assert.IsNotNull(_toolingClient);
            Assert.IsNotNull(_toolingClient.InstanceName);
            Assert.AreEqual("na30", _toolingClient.InstanceName);
            Assert.IsNotNull(_toolingClient.ToolingApiUri);
        }

        [Test]
        public async Task SObjects()
        {
            var sObjectsResults = await _toolingClient.SObjects<SObjectsModel>();

            Assert.IsNotNull(sObjectsResults);
            Assert.AreEqual("UTF-8", sObjectsResults.Encoding);
            Assert.Greater(sObjectsResults.MaxBatchSize, 1);
            Assert.Greater(sObjectsResults.SObjects.Count, 1);
        }

        [Test]
        public async Task TraceFlag()
        {
            var traceFlagResult = await _toolingClient.SObject("TraceFlag");
            Assert.IsNotNull(traceFlagResult);

            var traceFlagDescribeResult = await _toolingClient.SObjectDescribe("TraceFlag");
            Assert.IsNotNull(traceFlagDescribeResult);
        }

        [Test]
        public async Task AllSObjects()
        {
            var sObjectsResults = await _toolingClient.SObjects<SObjectsModel>();
            Assert.IsNotNull(sObjectsResults);

            foreach (var sObject in sObjectsResults.SObjects)
            {
                var sObjectModel = await _toolingClient.SObject(sObject.Name);
                Assert.IsNotNull(sObjectModel);
            }
        }

        [Test]
        public async Task CreateSObject()
        {
            var name = "TestContainer" + DateTime.Now.Ticks;

            dynamic testContainer = new ExpandoObject();
            testContainer.Name = name;

            var createObjectResult = await _toolingClient.CreateRecord("MetadataContainer", testContainer);
            Assert.IsNotNull(createObjectResult);
            Assert.IsNotNull(createObjectResult.Id);
            Assert.IsNotNull(createObjectResult.Success);
            Assert.AreEqual(createObjectResult.Success, true);
            Assert.IsNotNull(createObjectResult.Errors);
        }

        [Test]
        public async Task DescribeApexClass()
        {
            var apexClassDescribeResult = await _toolingClient.SObjectDescribe("ApexClass");
            Assert.IsNotNull(apexClassDescribeResult);
        }

        [Test]
        public async Task QueryApexClass()
        {
            const string query = "SELECT Id, NamespacePrefix, Name, ApiVersion, Status, IsValid, BodyCrc, Body, LengthWithoutComments, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, SystemModstamp, SymbolTable FROM ApexClass";
            var result = await _toolingClient.Query<dynamic>(query);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task QueryAuraDefinition()
        {
            const string query = "SELECT AuraDefinitionBundleId, DefType, Format, Source FROM AuraDefinition";
            var result = await _toolingClient.Query<dynamic>(query);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task QueryAuraDefinitionBundle()
        {
            const string query = "SELECT ApiVersion, Description, DeveloperName, Language, MasterLabel, NamespacePrefix FROM AuraDefinitionBundle";
            var result = await _toolingClient.Query<dynamic>(query);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task CreateApexClass()
        {
            var ticks = DateTime.Now.Ticks;
            var apexClassName = "ac" + ticks;

            var apexClass = new Models.ApexClass
            {
                Body = string.Format("public class {0} {{\n\n}}", apexClassName),
                Name = "n" + ticks
            };

            var createApexClassResult = await _toolingClient.CreateRecord("ApexClass", apexClass);
            Assert.IsNotNull(createApexClassResult);

            var apexClassResult = await _toolingClient.SObject("ApexClass", createApexClassResult.Id);
            Assert.IsNotNull(apexClassResult);
        }

        [Test]
        public async Task CreateApexClassWithMetadataContainer()
        {
            var ticks = DateTime.Now.Ticks;
            var apexClassName = "ac" + ticks;
            var metadataContainerName = "mc" + ticks;

            var metadataContainer = new MetadataContainer
            {
                Name = metadataContainerName
            };

            var createMetadataContainerResult = await _toolingClient.CreateRecord("MetadataContainer", metadataContainer);
            Assert.IsNotNull(createMetadataContainerResult);

            var apexClassMember = new ApexClassMember
            {
                MetadataContainerId = createMetadataContainerResult.Id,
                FullName = "fn" + ticks,
                Body = string.Format("public class {0} {{\n\n}}", apexClassName),
                Metadata = new ApexClassMemberMetadata {apiVersion = "36.0", status = "Active"}
            };

            var createApexClassMemberResult = await _toolingClient.CreateRecord("ApexClassMember", apexClassMember);

            Assert.IsNotNull(createApexClassMemberResult);
            Assert.IsNotNull(createApexClassMemberResult.Id);

            var containerAsyncRequest = new ContainerAsyncRequest
            {
                MetadataContainerId = createMetadataContainerResult.Id,
                IsCheckOnly = true
            };

            var containerAsyncRequestResult = await _toolingClient.CreateRecord("ContainerAsyncRequest", containerAsyncRequest);
            Assert.IsNotNull(containerAsyncRequestResult);

            var state = "Queued";
            QueryResult<ContainerAsyncRequest> result = null;

            while (state == "Queued")
            {
                var query = string.Format("SELECT Id, DeployDetails, ErrorMsg, IsCheckOnly, IsRunTests, MetadataContainerId, MetadataContainerMemberId, State FROM ContainerAsyncRequest WHERE Id = '{0}'", containerAsyncRequestResult.Id);
                result = await _toolingClient.Query<QueryResult<ContainerAsyncRequest>>(query);

                state = result.records[0].State;
                Assert.IsNotNull(result);
            }

            Assert.AreEqual(null, result.records[0].ErrorMsg);
            Assert.AreEqual(true, result.records[0].DeployDetails.allComponentMessages[0].created);
        }
    }
}
