namespace Test.WCF.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;

    [TestClass]
    [TestData(Constant.Owner, Constant.Priority, Constant.Timeout, "")]
    public class CrossDomainTests
    {
        //[TestMethod]
        public void CrossDomainServerWebMediumTrust()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            string configurationFile = null;
            using (CommonAppDomain ad = CommonAppDomainFactory.CreateWebMediumTrust(configurationFile))
            {
                using (SampleServerCrossDomain server = ad.CreateInstance<SampleServerCrossDomain>())
                {
                    server.SelfHost();

                    SampleClient client = new SampleClient();
                    client.ServiceAddress = CommonMachine.LocalHost.SelfHostHttpBaseAddress().AbsoluteUri;
                    client.Default();
                }
            }
        }

        [TestMethod]
        public void CrossDomainClientWebMediumTrust()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (SampleServerCrossDomain server = new SampleServerCrossDomain())
            {
                server.SelfHost();

                string configurationFile = null;
                using (CommonAppDomain ad = CommonAppDomainFactory.CreateWebMediumTrust(configurationFile))
                {
                    SampleClient client = ad.CreateInstance<SampleClient>();
                    client.ServiceAddress = CommonMachine.LocalHost.SelfHostHttpBaseAddress().AbsoluteUri;
                    client.Default();
                }
            }
        }
    }
}
