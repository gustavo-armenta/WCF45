namespace Test.WCF.UnitTest
{
    using System;
    using System.ServiceModel;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;
    using Test.WCF.UnitTest;
    using Test.WCF.UnitTest.WCF;

    public class SampleServerCrossDomain : MarshalByRefObject, IDisposable
    {
        private ServiceHost asyncServer;
        private ServiceHost duplexServer;
        private ServiceHost requestReplyServer;
        private ServiceHost streamServer;

        public void SelfHost()
        {
            asyncServer = new ServiceHost(typeof(AsyncService), new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress().AbsoluteUri + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetHttpBindingHelper.Default(), string.Empty);
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService), new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress().AbsoluteUri + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetHttpBindingHelper.Default(), string.Empty);
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService), new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpBindingHelper.Default(), string.Empty);
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService), new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetHttpBindingHelper.Streamed(), string.Empty);
            streamServer.Open();
        }

        public void Dispose()
        {
            CommonLog.WriteLine("SampleServerCrossDomain.Dispose");
            CommonServiceHost.Cleanup(asyncServer);
            CommonServiceHost.Cleanup(duplexServer);
            CommonServiceHost.Cleanup(requestReplyServer);
        }
    }
}
