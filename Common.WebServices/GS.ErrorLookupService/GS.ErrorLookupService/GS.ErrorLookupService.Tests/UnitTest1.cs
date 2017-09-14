using System;
using System.Linq;
using GS.ErrorLookupService.ServiceModel.Models;
using GS.ErrorLookupService.ServiceInterface;
using GS.ErrorLookupService.ServiceModel;
using ServiceStack.Testing;
using ServiceStack;
using NUnit.Framework;

namespace GS.ErrorLookupService.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private readonly ServiceStackHost appHost;

        public UnitTests()
        {
            appHost = new BasicAppHost(typeof(MyServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
                }
            }
            .Init();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

        [Test]
        public void TestMethod1()
        {
            //IRestClient client = new JsonServiceClient();
            //var errorResults =
            //    client.Get(new ErrorLookup
            //    {
            //        CorrelationId = "235235235235235",
            //        EndDateTime = DateTime.Now.AddHours(1),
            //        StartDateTime = DateTime.Now
            //    }).Result.Output.Single();

            //Assert.That(errorResults.Message, !Is.EqualTo(""));

            var service = appHost.Container.Resolve<MyServices>();
            //var response = (QueryResult)service.Get(new ErrorLookup { CorrelationId = "235235235235235", EndDateTime = DateTime.Now.AddHours(1), StartDateTime = DateTime.Now }).;
            //Assert.That(response.Output.Single().Message.ToString(), Is.EqualTo(""));
        }
    }
}
