namespace Test.WCF.UnitTest
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;

    public class CrossDomainNetTcpPartialTrust : MarshalByRefObject
    {
        private ServiceHost host;

        public void SetupServer()
        {
            host = new ServiceHost(typeof(DuplexService), CommonLocalMachineUri.SelfHostNetTcpBaseAddress());
            host.AddServiceEndpoint(typeof(IDuplexService), NetTcpBindingHelper.SecurityModeNone(), string.Empty);
            host.Open();
        }

        public void ExecuteClient()
        {
            DuplexCallback callback = new DuplexCallback();
            callback.UploadAction = delegate(string value)
            {
                FullTrustAssert.AreEqual("Hello World!", value);
            };
            DuplexChannelFactory<IDuplexService> cf = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.SecurityModeNone(), CommonLocalMachineUri.SelfHostNetTcpBaseAddress().AbsoluteUri);
            IDuplexService client = cf.CreateChannel();
            client.Upload("Hello World!");
            callback.WaitForUpload();
        }
    }
}
