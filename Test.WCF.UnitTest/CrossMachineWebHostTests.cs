namespace Test.WCF.UnitTest
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;

    [TestClass]
    [TestData(Constant.Owner, Constant.Priority, Constant.Timeout, "")]
    [CrossMachine]
    public class CrossMachineWebHostTests : CommonTest
    {
        private static bool runSetupOnce;

        [TestInitialize]
        public void Setup()
        {
            CommonLog.WriteLine("CrossMachineWebHostTests.Setup()");
            if (runSetupOnce)
            {
                return;
            }

            runSetupOnce = true;

            using (CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(WebHostServer), this))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                server.CustomSiteAppPool();
                server.Default();
                server.MediumPartialTrust();
                server.MessageCertificate();
                server.MessageUserName();
                server.MessageWindows();
                server.ServiceHostFactory();
                server.TransportBasic();
                server.TransportCertificate();
                server.TransportDigest();
                server.TransportNtlm();
                server.TransportNtlmExtendedProtection();
                server.TransportWindows();
                server.TransportWindowsExtendedProtection();
                server.TransportWithMessageCredentialCertificate();
                server.TransportWithMessageCredentialUserName();
                server.TransportWithMessageCredentialWindows();
            }
        }

        //[TestMethod]
        public void CrossMachineWebHostMessageCertificate()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("http://{0}/MessageCertificate/", CommonMachine.Server.FullyQualifiedMachineName);
            client.ServiceAddressNetTcpBinding = string.Format("net.tcp://{0}/MessageCertificate/", CommonMachine.Server.FullyQualifiedMachineName);
            client.MessageCertificate();
        }

        //[TestMethod]
        public void CrossMachineWebHostMessageUserName()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("http://{0}/MessageUserName/", CommonMachine.Server.FullyQualifiedMachineName);
            client.ServiceAddressNetTcpBinding = string.Format("net.tcp://{0}/MessageUserName/", CommonMachine.Server.FullyQualifiedMachineName);
            client.MessageUserName();
        }

        //[TestMethod]
        public void CrossMachineWebHostMessageWindows()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("http://{0}/MessageWindows/", CommonMachine.Server.FullyQualifiedMachineName);
            client.ServiceAddressNetTcpBinding = string.Format("net.tcp://{0}/MessageWindows/", CommonMachine.Server.FullyQualifiedMachineName);
            client.MessageWindows();
        }

        [TestMethod]
        public void CrossMachineWebHostTransportBasic()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("https://{0}/TransportBasic/", CommonMachine.Server.FullyQualifiedMachineName);
            client.TransportBasic();
        }

        [TestMethod]
        public void CrossMachineWebHostTransportCertificate()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("https://{0}/TransportCertificate/", CommonMachine.Server.FullyQualifiedMachineName);
            client.ServiceAddressNetTcpBinding = string.Format("net.tcp://{0}/TransportCertificate/", CommonMachine.Server.FullyQualifiedMachineName);
            client.TransportCertificate();
        }

        [TestMethod]
        public void CrossMachineWebHostTransportDigest()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("https://{0}/TransportDigest/", CommonMachine.Server.FullyQualifiedMachineName);
            client.TransportDigest();
        }

        [TestMethod]
        public void CrossMachineWebHostTransportNtlm()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("https://{0}/TransportNtlm/", CommonMachine.Server.FullyQualifiedMachineName);
            client.TransportNtlm();
        }

        [TestMethod]
        public void CrossMachineWebHostTransportNtlmExtendedProtection()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("https://{0}/TransportNtlmExtendedProtection/", CommonMachine.Server.FullyQualifiedMachineName);
            client.TransportNtlm();
        }

        [TestMethod]
        public void CrossMachineWebHostTransportWindows()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("https://{0}/TransportWindows/", CommonMachine.Server.FullyQualifiedMachineName);
            client.ServiceAddressNetTcpBinding = string.Format("net.tcp://{0}/TransportWindows/", CommonMachine.Server.FullyQualifiedMachineName);
            client.TransportWindowsWebHost();
        }

        [TestMethod]
        public void CrossMachineWebHostTransportWindowsExtendedProtection()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("https://{0}/TransportWindowsExtendedProtection/", CommonMachine.Server.FullyQualifiedMachineName);
            client.ServiceAddressNetTcpBinding = string.Format("net.tcp://{0}/TransportWindowsExtendedProtection/", CommonMachine.Server.FullyQualifiedMachineName);
            client.TransportWindowsWebHost();
        }

        //[TestMethod]
        public void CrossMachineWebHostTransportWithMessageCredentialCertificate()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("https://{0}/TransportWithMessageCredentialCertificate/", CommonMachine.Server.FullyQualifiedMachineName);
            client.ServiceAddressNetTcpBinding = string.Format("net.tcp://{0}/TransportWithMessageCredentialCertificate/", CommonMachine.Server.FullyQualifiedMachineName);
            client.TransportWithMessageCredentialCertificate();
        }

        //[TestMethod]
        public void CrossMachineWebHostTransportWithMessageCredentialUserName()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("https://{0}/TransportWithMessageCredentialUserName/", CommonMachine.Server.FullyQualifiedMachineName);
            client.ServiceAddressNetTcpBinding = string.Format("net.tcp://{0}/TransportWithMessageCredentialUserName/", CommonMachine.Server.FullyQualifiedMachineName);
            client.TransportWithMessageCredentialUserName();
        }

        [TestMethod]
        public void CrossMachineWebHostTransportWithMessageCredentialWindows()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("https://{0}/TransportWithMessageCredentialWindows/", CommonMachine.Server.FullyQualifiedMachineName);
            client.ServiceAddressNetTcpBinding = string.Format("net.tcp://{0}/TransportWithMessageCredentialWindows/", CommonMachine.Server.FullyQualifiedMachineName);
            client.TransportWithMessageCredentialWindowsWebHost();
        }

        [TestMethod]
        public void CrossMachineWebHostCustomSite()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            SampleClient client = new SampleClient();
            client.ServiceAddress = string.Format("http://{0}:8080/CustomSite/", CommonMachine.Server.FullyQualifiedMachineName);
            client.CustomSite();
        }

        [TestMethod]
        public void CrossMachineWebHostMediumPartialTrustEtw()
        {
            if (CommonMachine.IsLessThanWin8())
            {
                return;
            }

            string remotePath;
            string localPath;

            using (CommonEtw etwRemote = CommonEtwFactory.CreateCrossMachine(this))
            {
                remotePath = etwRemote.CommonRemoteEtw.Path;

                using (CommonEtw etwLocal = CommonEtwFactory.CreateLocalMachine(this))
                {
                    localPath = etwLocal.CommonRemoteEtw.Path;

                    SampleClient client = new SampleClient();
                    client.ServiceAddress = string.Format("http://{0}/MediumPartialTrust/", CommonMachine.Server.FullyQualifiedMachineName);
                    client.MediumPartialTrust();
                }
            }

            CommonEtwValidator validator = new CommonEtwValidator();
            validator.Path = localPath;
            validator.ExpectedEvents.Add(3416); //connection request sent
            validator.ExpectedEvents.Add(3422); //TransportSend
            validator.ExpectedEvents.Add(3425); //TransportReceive
            validator.ExpectedEvents.Add(3427); //sending close message
            validator.ExpectedEvents.Add(3429); //close message received            
            validator.ExpectedEvents.Add(3428); //connection closed
            validator.ValidateEvents();

            File.Copy(remotePath, "RemoteMachine.etl");
            validator = new CommonEtwValidator();
            validator.Path = "RemoteMachine.etl";
            validator.ExpectedEvents.Add(3418); //Connect
            validator.ExpectedEvents.Add(3425); //TransportReceive
            validator.ExpectedEvents.Add(3422); //TransportSend
            validator.ExpectedEvents.Add(3429); //close message received
            validator.ExpectedEvents.Add(3426); //creating close message
            validator.ExpectedEvents.Add(3427); //sending close message
            validator.ExpectedEvents.Add(3428); //connection closed
            validator.ValidateEvents();
        }
    }
}
