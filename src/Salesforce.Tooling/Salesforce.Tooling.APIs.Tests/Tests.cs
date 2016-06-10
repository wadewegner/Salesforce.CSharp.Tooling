using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Salesforce.Common;
using Salesforce.Common.Models;

namespace Salesforce.Tooling.APIs.Tests
{
    [TestFixture]
    public class Tests
    {
        private static readonly string TokenRequestEndpointUrl = ConfigurationManager.AppSettings["TokenRequestEndpointUrl"];
        private static string _consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
        private static string _consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
        private static string _username = ConfigurationManager.AppSettings["Username"];
        private static string _password = ConfigurationManager.AppSettings["Password"];

        private AuthenticationClient _auth;

        [TestFixtureSetUp]
        public void Init()
        {
            // Use TLS 1.2 (instead of defaulting to 1.0)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _auth = new AuthenticationClient();
            _auth.UsernamePasswordAsync(_consumerKey, _consumerSecret, _username, _password, TokenRequestEndpointUrl).Wait();
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
        }
    }
}
