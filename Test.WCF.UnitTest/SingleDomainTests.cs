namespace Test.WCF.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;
    using Test.WCF.UnitTest;
    using Test.WCF.UnitTest.WCF;

    [TestClass]
    [TestData(Constant.Owner, Constant.Priority, Constant.Timeout, "")]
    public class SingleDomainTests : CommonTest
    {
        [TestMethod]
        public void SingleDomainSelfHost()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (ServiceHost host = new ServiceHost(typeof(RequestReplyService), CommonMachine.LocalHost.SelfHostHttpBaseAddress()))
            {
                host.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpBindingHelper.Default(), string.Empty);
                host.Open();

                using (ChannelFactory<IRequestReplyService> cf = new ChannelFactory<IRequestReplyService>(NetHttpBindingHelper.Default(), CommonMachine.LocalHost.SelfHostHttpBaseAddress().AbsoluteUri))
                {
                    IRequestReplyService client = cf.CreateChannel();
                    string actual = client.Echo(Constant.ShortMessage);
                    CommonLog.WriteLine("actual={0}", actual);

                    Assert.AreEqual<string>(Constant.ShortMessage, actual);
                }

                CommonServiceHost.Cleanup(host);
            }
        }
    }
}
