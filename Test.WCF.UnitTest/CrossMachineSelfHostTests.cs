namespace Test.WCF.UnitTest
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;

    [TestClass]
    [TestData(Constant.Owner, Constant.Priority, Constant.Timeout, "")]
    [CrossMachine]
    public class CrossMachineSelfHostTests : CommonTest
    {
        //[TestMethod]
        public void CrossMachineSelfHostMessageCertificate()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(SelfHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.Cleanup;
                server.MessageCertificate();

                SampleClient client = new SampleClient();
                client.ServiceAddress = CommonMachine.Server.SelfHostHttpBaseAddress().AbsoluteUri;
                client.ServiceAddressNetTcpBinding = CommonMachine.Server.SelfHostNetTcpBaseAddress().AbsoluteUri;
                client.MessageCertificate();
            }
        }

        //[TestMethod]
        public void CrossMachineSelfHostMessageUserName()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(SelfHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.Cleanup;
                server.MessageUserName();

                SampleClient client = new SampleClient();
                client.ServiceAddress = CommonMachine.Server.SelfHostHttpBaseAddress().AbsoluteUri;
                client.ServiceAddressNetTcpBinding = CommonMachine.Server.SelfHostNetTcpBaseAddress().AbsoluteUri;
                client.MessageUserName();
            }
        }

        //[TestMethod]
        public void CrossMachineSelfHostMessageWindows()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(SelfHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.Cleanup;
                server.MessageWindows();

                SampleClient client = new SampleClient();
                client.ServiceAddress = CommonMachine.Server.SelfHostHttpBaseAddress().AbsoluteUri;
                client.ServiceAddressNetTcpBinding = CommonMachine.Server.SelfHostNetTcpBaseAddress().AbsoluteUri;
                client.MessageWindows();
            }
        }

        [TestMethod]
        public void CrossMachineSelfHostTransportBasic()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(SelfHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.Cleanup;
                server.TransportBasic();

                SampleClient client = new SampleClient();
                client.ServiceAddress = CommonMachine.Server.SelfHostHttpsBaseAddress().AbsoluteUri;
                client.ServiceAddressNetTcpBinding = CommonMachine.Server.SelfHostNetTcpBaseAddress().AbsoluteUri;
                client.TransportBasic();
            }
        }

        [TestMethod]
        public void CrossMachineSelfHostTransportCertificate()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(SelfHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.Cleanup;
                server.TransportCertificate();

                SampleClient client = new SampleClient();
                client.ServiceAddress = CommonMachine.Server.SelfHostHttpsBaseAddress().AbsoluteUri;
                client.ServiceAddressNetTcpBinding = CommonMachine.Server.SelfHostNetTcpBaseAddress().AbsoluteUri;
                client.TransportCertificate();
            }
        }

        [TestMethod]
        public void CrossMachineSelfHostTransportDigest()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(SelfHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.Cleanup;
                server.TransportDigest();

                SampleClient client = new SampleClient();
                client.ServiceAddress = CommonMachine.Server.SelfHostHttpsBaseAddress().AbsoluteUri;
                client.ServiceAddressNetTcpBinding = CommonMachine.Server.SelfHostNetTcpBaseAddress().AbsoluteUri;
                client.TransportDigest();
            }
        }

        [TestMethod]
        public void CrossMachineSelfHostTransportNtlm()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(SelfHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.Cleanup;
                server.TransportNtlm();

                SampleClient client = new SampleClient();
                client.ServiceAddress = CommonMachine.Server.SelfHostHttpsBaseAddress().AbsoluteUri;
                client.ServiceAddressNetTcpBinding = CommonMachine.Server.SelfHostNetTcpBaseAddress().AbsoluteUri;
                client.TransportNtlm();
            }
        }

        [TestMethod]
        public void CrossMachineSelfHostTransportWindows()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(SelfHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.Cleanup;
                server.TransportWindows();

                SampleClient client = new SampleClient();
                client.ServiceAddress = CommonMachine.Server.SelfHostHttpsBaseAddress().AbsoluteUri;
                client.ServiceAddressNetTcpBinding = CommonMachine.Server.SelfHostNetTcpBaseAddress().AbsoluteUri;
                client.TransportWindowsSelfHost();
            }
        }

        //[TestMethod]
        public void CrossMachineSelfHostTransportWithMessageCredentialCertificate()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(SelfHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.Cleanup;
                server.TransportWithMessageCredentialCertificate();

                SampleClient client = new SampleClient();
                client.ServiceAddress = CommonMachine.Server.SelfHostHttpsBaseAddress().AbsoluteUri;
                client.ServiceAddressNetTcpBinding = CommonMachine.Server.SelfHostNetTcpBaseAddress().AbsoluteUri;
                client.TransportWithMessageCredentialCertificate();
            }
        }

        //[TestMethod]
        public void CrossMachineSelfHostTransportWithMessageCredentialUserName()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(SelfHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.Cleanup;
                server.TransportWithMessageCredentialUserName();

                SampleClient client = new SampleClient();
                client.ServiceAddress = CommonMachine.Server.SelfHostHttpsBaseAddress().AbsoluteUri;
                client.ServiceAddressNetTcpBinding = CommonMachine.Server.SelfHostNetTcpBaseAddress().AbsoluteUri;
                client.TransportWithMessageCredentialUserName();
            }
        }

        //[TestMethod]
        public void CrossMachineSelfHostTransportWithMessageCredentialWindows()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(SelfHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.Cleanup;
                server.TransportWithMessageCredentialWindows();

                SampleClient client = new SampleClient();
                client.ServiceAddress = CommonMachine.Server.SelfHostHttpsBaseAddress().AbsoluteUri;
                client.ServiceAddressNetTcpBinding = CommonMachine.Server.SelfHostNetTcpBaseAddress().AbsoluteUri;
                client.TransportWithMessageCredentialWindowsSelfHost();
            }
        }
    }
}
