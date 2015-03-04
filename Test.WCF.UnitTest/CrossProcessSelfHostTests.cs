namespace Test.WCF.UnitTest
{
    using System.IO;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;
    using Test.WCF.UnitTest;

    [TestClass]
    [TestData(Constant.Owner, Constant.Priority, Constant.Timeout, "")]
    public class CrossProcessSelfHostTests : CommonTest
    {
        [TestMethod]
        public void CrossProcessSelfHostClientRunningAsStandardUser()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SelfHostServer server = new SelfHostServer();
            server.Default();

            using (CommonRemote crossProcess = CommonRemoteFactory.CreateCrossProcessStandardUser(typeof(SampleCrossProcessClient), this))
            {
                ISampleCrossProcessClient client = crossProcess.CreateChannel<ISampleCrossProcessClient>();
                client.ServiceAddress = CommonMachine.LocalHost.SelfHostHttpBaseAddress().AbsoluteUri;
                client.Default();
            }
        }
    }
}
