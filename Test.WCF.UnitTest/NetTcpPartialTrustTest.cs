namespace Test.WCF.UnitTest
{
    using System;
    using System.IO;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;

    [TestClass]
    public class NetTcpPartialTrustTest
    {
        //[TestMethod]
        public void SelfHost()
        {
            using (CommonAppDomain ad = new CommonAppDomain())
            {
                //ad.CreateFullTrust();
                //ad.CreateWebMediumTrust();
                //ad.CreateAppFabricContainerTrust();
                //ad.CreateLightWeightWebRoleTrust();
                ad.CreateAppFabricHighTrust();
                
                CrossDomainNetTcpPartialTrust remote = (CrossDomainNetTcpPartialTrust)ad.CreateInstance(typeof(CrossDomainNetTcpPartialTrust));

                remote.SetupServer();

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

        //[TestMethod]
        public void Client()
        {
            using (ServiceHost host = new ServiceHost(typeof(DuplexService), CommonLocalMachineUri.SelfHostNetTcpBaseAddress()))
            {
                host.AddServiceEndpoint(typeof(IDuplexService), NetTcpBindingHelper.Default(), string.Empty);
                host.Open();

                using (CommonAppDomain ad = new CommonAppDomain())
                {
                    //ad.CreateFullTrust();
                    //ad.CreateWebMediumTrust();
                    //ad.CreateAppFabricContainerTrust();
                    //ad.CreateLightWeightWebRoleTrust();
                    ad.CreateAppFabricHighTrust();

                    CrossDomainNetTcpPartialTrust remote = (CrossDomainNetTcpPartialTrust)ad.CreateInstance(typeof(CrossDomainNetTcpPartialTrust));

                    remote.ExecuteClient();
                }
            }
            
        }
    }
}
