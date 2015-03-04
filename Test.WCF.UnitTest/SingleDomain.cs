namespace Test.WCF.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;

    [TestClass]
    public class SingleDomain
    {
        [TestMethod]
        public void SingleDomainSelfHost()
        {
            using (CommonETW etw = new CommonETW())
            {
                using (ServiceHost host = new ServiceHost(typeof(RequestReplyService), CommonLocalMachineUri.SelfHostHttpBaseAddress()))
                {
                    host.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpBindingHelper.Default(), string.Empty);
                    host.Open();

                    using (ChannelFactory<IRequestReplyService> cf = new ChannelFactory<IRequestReplyService>(NetHttpBindingHelper.Default(), CommonLocalMachineUri.SelfHostHttpBaseAddress().AbsoluteUri))
                    {
                        IRequestReplyService client = cf.CreateChannel();
                        string actual = client.Echo("Hello World!");
                        CommonLog.WriteLine("actual={0}", actual);

                        Assert.AreEqual<string>("Hello World!", actual);
                    }                    
                }

                etw.ExpectedEvents.AddRange(new List<int> {510, 3339});
            }
        }
    }
}
