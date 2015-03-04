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
    public class CrossProcessWebHostTests : CommonTest
    {
        [TestMethod]
        public void CrossProcessWebHostDefault()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            WebHostServer server = new WebHostServer();
            server.Default();

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("http://{0}/Default/", CommonMachine.LocalHost.FullyQualifiedMachineName);
            client.Default();
        }

        [TestMethod]
        public void CrossProcessWebHostServiceHostFactory()
        {
            WebHostServer server = new WebHostServer();
            server.ServiceHostFactory();

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("net.tcp://{0}/ServiceHostFactory/Service1.svc", CommonMachine.LocalHost.FullyQualifiedMachineName);
            client.ServiceHostFactory();
        }

        [TestMethod]
        public void CrossProcessWebHostClientRunningAsStandardUser()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            WebHostServer server = new WebHostServer();
            server.Default();

            using (CommonRemote crossProcess = CommonRemoteFactory.CreateCrossProcessStandardUser(typeof(SampleCrossProcessClient), this))
            {
                ISampleCrossProcessClient client = crossProcess.CreateChannel<ISampleCrossProcessClient>();
                client.ServiceAddress = string.Format("http://{0}/Default/", CommonMachine.LocalHost.FullyQualifiedMachineName);
                client.Default();
            }
        }

        [TestMethod]
        public void CrossProcessSvcutil()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            WebHostServer server = new WebHostServer();
            server.Default();

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("http://{0}/Default/", CommonMachine.LocalHost.FullyQualifiedMachineName);
            client.SvcUtilAsyncService();
            client.SvcUtilDuplexService();
            client.SvcUtilRequestReplyService();
            client.SvcUtilStreamService();
        }
    }
}
