namespace Test.WCF.UnitTest
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;

    [TestClass]
    public class CrossProcess
    {
        //[TestMethod]
        public void CrossProcessSelfHost()
        {
            using (CommonCrossProcess crossProcess = new CommonCrossProcess(typeof(SampleServer)))
            {
                ISampleServer server = crossProcess.CreateChannel<ISampleServer>();
                crossProcess.DisposeMethod = server.SelfHost_Cleanup;
                server.SelfHost_Setup();

                SampleClient client = new SampleClient();
                client.SelfHost();
            }
        }

        [TestMethod]
        public void WebHostNetTcp()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/NetTcp";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\NetTcp");
            web.EnabledProtocols = "http,net.tcp";
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();

            SampleClient client = new SampleClient();
            client.NetTcp();
        }

        [TestMethod]
        public void WebHostNetTcpFactory()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/NetTcpFactory";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\NetTcpFactory");
            web.EnabledProtocols = "http,net.tcp";
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();

            SampleClient client = new SampleClient();
            client.NetTcpFactory();
        }

        [TestMethod]
        public void WebHostHttps()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/WebHostHttps";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\WebHostHttps");
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();

            SampleClient client = new SampleClient();
            client.Https();
        }

        [TestMethod]
        public void CrossProcessSvcutil()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/WebHost";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\WebHost");
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();

            CommonCommandLine svcutil = new CommonCommandLine();
            svcutil.FileName = CommonPath.SvcUtilExe;
            svcutil.Arguments = string.Format("{0} /out:{1} /config:{2}", "http://localhost/WebHost/Service1.svc?wsdl", "GeneratedCode.cs", "GeneratedCode.config");
            svcutil.WorkingDirectory = Path.GetFullPath(@"Test.WCF.UnitTest\svcutil\DuplexService");
            svcutil.Run();

            CommonCommandLine compiler = new CommonCommandLine();
            compiler.FileName = CommonPath.CscExe;
            compiler.Arguments = " /out:SvcUtilDuplexServiceClient.dll /target:library /debug+ /debug:full";
            compiler.Arguments += string.Format(" /reference:\"{0}\"", typeof(CommonLog).Assembly.Location);
            compiler.Arguments += string.Format(" /reference:\"{0}\"", typeof(FullTrustAssert).Assembly.Location);
            compiler.Arguments += " GeneratedCode.cs DuplexCallback.cs";
            compiler.WorkingDirectory = Path.GetFullPath(@"Test.WCF.UnitTest\svcutil\DuplexService");
            compiler.Run();

            using (CommonAppDomain ad = new CommonAppDomain())
            {
                ad.ConfigurationFile = Path.Combine(compiler.WorkingDirectory, "GeneratedCode.config");
                ad.CreateFullTrust();
                SvcUtilDuplexServiceClient remote = (SvcUtilDuplexServiceClient)ad.CreateInstance(typeof(SvcUtilDuplexServiceClient));
                remote.Execute(Path.Combine(compiler.WorkingDirectory, "SvcUtilDuplexServiceClient.dll"));
            }
        }
    }
}
