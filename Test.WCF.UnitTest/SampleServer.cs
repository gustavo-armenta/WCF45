namespace Test.WCF.UnitTest
{
    using Microsoft.Web.Administration;
    using System;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.ServiceModel.Security;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;
    using Test.WCF.UnitTest;
    using Test.WCF.UnitTest.WCF;

    public interface ISampleServer
    {
        void Cleanup();        
        void CustomSiteAppPool();
        void Default();
        void MediumPartialTrust();
        void MessageCertificate();
        void MessageUserName();
        void MessageWindows();
        void ServiceHostFactory();
        void TransportBasic();
        void TransportCertificate();
        void TransportDigest();
        void TransportNtlm();
        void TransportNtlmExtendedProtection();
        void TransportWindows();        
        void TransportWindowsExtendedProtection();
        void TransportWithMessageCredentialCertificate();
        void TransportWithMessageCredentialUserName();
        void TransportWithMessageCredentialWindows();        
    }

    public class SelfHostServer : CommonRemoteTask, ISampleServer
    {
        private ServiceHost asyncServer;
        private ServiceHost duplexServer;
        private ServiceHost requestReplyServer;
        private ServiceHost streamServer;

        public const string AsyncService = "AsyncService.svc";
        public const string DuplexService = "DuplexService.svc";
        public const string RequestReplyService = "RequestReplyService.svc";
        public const string StreamService = "StreamService.svc";

        public const string NetTcpBindingPathSuffix = "/netTcpBinding";
        public const string WSHttpBindingPathSuffix = "/wsHttpBinding";

        public void Cleanup()
        {
            CommonLog.WriteLine("SelfHostServer.Cleanup");
            CommonServiceHost.Cleanup(asyncServer);
            CommonServiceHost.Cleanup(duplexServer);
            CommonServiceHost.Cleanup(requestReplyServer);
            CommonServiceHost.Cleanup(streamServer);
        }

        public void CustomSiteAppPool()
        {
        }

        public void Default()
        {
            CommonLog.WriteLine("SelfHostServer.Default");

            asyncServer = new ServiceHost(typeof(AsyncService), new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetHttpBindingHelper.Default(), string.Empty);
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService), new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetHttpBindingHelper.Default(), string.Empty);
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService), new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpBindingHelper.Default(), string.Empty);
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService), new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetHttpBindingHelper.Streamed(), string.Empty);
            streamServer.Open();
        }

        public void MediumPartialTrust()
        {
        }

        public void MessageCertificate()
        {
            CommonLog.WriteLine("SelfHostServer.MessageCertificate");
            X509Certificate2 clientCertificate = CommonMachine.LocalHost.InstallServiceCertificate();
            X509Certificate2 serverCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();            

            asyncServer = new ServiceHost(typeof(AsyncService), 
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetHttpBindingHelper.MessageCertificate(), string.Empty);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetTcpBindingHelper.MessageCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), WSHttpBindingHelper.MessageCertificate(), SelfHostServer.WSHttpBindingPathSuffix);
            asyncServer.Credentials.ClientCertificate.Certificate = clientCertificate;
            asyncServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService), 
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetHttpBindingHelper.MessageCertificate(), string.Empty);
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetTcpBindingHelper.MessageCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            duplexServer.Credentials.ClientCertificate.Certificate = clientCertificate;
            duplexServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService), 
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpBindingHelper.MessageCertificate(), string.Empty);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetTcpBindingHelper.MessageCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), WSHttpBindingHelper.MessageCertificate(), SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyServer.Credentials.ClientCertificate.Certificate = clientCertificate;
            requestReplyServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService), 
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetHttpBindingHelper.StreamedMessageCertificate(), string.Empty);
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetTcpBindingHelper.StreamedMessageCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            streamServer.Credentials.ClientCertificate.Certificate = clientCertificate;
            streamServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            streamServer.Open();
        }

        public void MessageUserName()
        {
            CommonLog.WriteLine("SelfHostServer.MessageUserName");

            asyncServer = new ServiceHost(typeof(AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetHttpBindingHelper.MessageUserName(), string.Empty);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), WSHttpBindingHelper.MessageUserName(), SelfHostServer.WSHttpBindingPathSuffix);
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetHttpBindingHelper.MessageUserName(), string.Empty);
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpBindingHelper.MessageUserName(), string.Empty);
           requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), WSHttpBindingHelper.MessageUserName(), SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetHttpBindingHelper.StreamedMessageUserName(), string.Empty);
            streamServer.Open();
        }

        public void MessageWindows()
        {
            CommonLog.WriteLine("SelfHostServer.MessageWindows");

            asyncServer = new ServiceHost(typeof(AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetTcpBindingHelper.MessageWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), WSHttpBindingHelper.MessageWindows(), SelfHostServer.WSHttpBindingPathSuffix);
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetTcpBindingHelper.MessageWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetTcpBindingHelper.MessageWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), WSHttpBindingHelper.MessageWindows(), SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpBaseAddress() + SelfHostServer.StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetTcpBindingHelper.StreamedMessageWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            streamServer.Open();
        }

        public void ServiceHostFactory()
        {
        }

        public void TransportBasic()
        {
            CommonLog.WriteLine("SelfHostServer.TransportBasic");
            CommonMachine.LocalHost.SetupHttps();

            asyncServer = new ServiceHost(typeof(AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetHttpsBindingHelper.TransportBasic(), string.Empty);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), WSHttpBindingHelper.TransportBasic(), SelfHostServer.WSHttpBindingPathSuffix);
            asyncServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Basic;
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetHttpsBindingHelper.TransportBasic(), string.Empty);
            duplexServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Basic;
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpsBindingHelper.TransportBasic(), string.Empty);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), WSHttpBindingHelper.TransportBasic(), SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Basic;
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetHttpsBindingHelper.StreamedTransportBasic(), string.Empty);
            streamServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Basic;
            streamServer.Open();
        }

        public void TransportCertificate()
        {
            CommonLog.WriteLine("SelfHostServer.TransportCertificate");
            X509Certificate2 serverCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();
            CommonMachine.LocalHost.SetupHttps();

            asyncServer = new ServiceHost(typeof(AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetHttpsBindingHelper.TransportCertificate(), string.Empty);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetTcpBindingHelper.TransportCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), WSHttpBindingHelper.TransportCertificate(), SelfHostServer.WSHttpBindingPathSuffix);
            asyncServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            asyncServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            asyncServer.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetHttpsBindingHelper.TransportCertificate(), string.Empty);
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetTcpBindingHelper.TransportCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            duplexServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            duplexServer.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            duplexServer.Credentials.ServiceCertificate.Certificate = serverCertificate;            
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpsBindingHelper.TransportCertificate(), string.Empty);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetTcpBindingHelper.TransportCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), WSHttpBindingHelper.TransportCertificate(), SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            requestReplyServer.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            requestReplyServer.Credentials.ServiceCertificate.Certificate = serverCertificate;            
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetHttpsBindingHelper.StreamedTransportCertificate(), string.Empty);
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetTcpBindingHelper.StreamedTransportCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            streamServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            streamServer.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            streamServer.Credentials.ServiceCertificate.Certificate = serverCertificate;            
            streamServer.Open();
        }

        public void TransportDigest()
        {
            CommonLog.WriteLine("SelfHostServer.TransportDigest");
            CommonMachine.LocalHost.SetupHttps();

            asyncServer = new ServiceHost(typeof(AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetHttpsBindingHelper.TransportDigest(), string.Empty);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), WSHttpBindingHelper.TransportDigest(), SelfHostServer.WSHttpBindingPathSuffix);
            asyncServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Digest;
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetHttpsBindingHelper.TransportDigest(), string.Empty);
            duplexServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Digest;
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpsBindingHelper.TransportDigest(), string.Empty);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), WSHttpBindingHelper.TransportDigest(), SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Digest;
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetHttpsBindingHelper.StreamedTransportDigest(), string.Empty);
            streamServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Digest;
            streamServer.Open();
        }

        public void TransportNtlm()
        {
            CommonLog.WriteLine("SelfHostServer.TransportNtlm");
            CommonMachine.LocalHost.SetupHttps();

            asyncServer = new ServiceHost(typeof(AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetHttpsBindingHelper.TransportNtlm(), string.Empty);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), WSHttpBindingHelper.TransportNtlm(), SelfHostServer.WSHttpBindingPathSuffix);
            asyncServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Ntlm;
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetHttpsBindingHelper.TransportNtlm(), string.Empty);
            duplexServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Ntlm;
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpsBindingHelper.TransportNtlm(), string.Empty);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), WSHttpBindingHelper.TransportNtlm(), SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Ntlm;
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetHttpsBindingHelper.StreamedTransportNtlm(), string.Empty);
            streamServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Ntlm;
            streamServer.Open();
        }

        public void TransportNtlmExtendedProtection()
        {
        }

        public void TransportWindows()
        {
            CommonLog.WriteLine("SelfHostServer.TransportWindows");
            CommonMachine.LocalHost.SetupHttps();

            asyncServer = new ServiceHost(typeof(AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetHttpsBindingHelper.TransportWindows(), string.Empty);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetTcpBindingHelper.TransportWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), WSHttpBindingHelper.TransportWindows(), SelfHostServer.WSHttpBindingPathSuffix);
            asyncServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetHttpsBindingHelper.TransportWindows(), string.Empty);
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetTcpBindingHelper.TransportWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            duplexServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpsBindingHelper.TransportWindows(), string.Empty);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetTcpBindingHelper.TransportWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), WSHttpBindingHelper.TransportWindows(), SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetHttpsBindingHelper.StreamedTransportWindows(), string.Empty);
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetTcpBindingHelper.StreamedTransportWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            streamServer.Authentication.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
            streamServer.Open();
        }

        public void TransportWindowsExtendedProtection()
        {
        }

        public void TransportWithMessageCredentialCertificate()
        {
            CommonLog.WriteLine("SelfHostServer.TransportWithMessageCredentialCertificate");
            X509Certificate2 serverCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();
            CommonMachine.LocalHost.SetupHttps();

            asyncServer = new ServiceHost(typeof(AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetHttpsBindingHelper.TransportWithMessageCredentialCertificate(), string.Empty);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetTcpBindingHelper.TransportWithMessageCredentialCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), WSHttpBindingHelper.TransportWithMessageCredentialCertificate(), SelfHostServer.WSHttpBindingPathSuffix);
            asyncServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetHttpsBindingHelper.TransportWithMessageCredentialCertificate(), string.Empty);
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetTcpBindingHelper.TransportWithMessageCredentialCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            duplexServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpsBindingHelper.TransportWithMessageCredentialCertificate(), string.Empty);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetTcpBindingHelper.TransportWithMessageCredentialCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), WSHttpBindingHelper.TransportWithMessageCredentialCertificate(), SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetHttpsBindingHelper.StreamedTransportWithMessageCredentialCertificate(), string.Empty);
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetTcpBindingHelper.TransportWithMessageCredentialCertificate(), SelfHostServer.NetTcpBindingPathSuffix);
            streamServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            streamServer.Open();
        }

        public void TransportWithMessageCredentialUserName()
        {
            CommonLog.WriteLine("SelfHostServer.TransportWithMessageCredentialUserName");
            X509Certificate2 serverCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();
            CommonMachine.LocalHost.SetupHttps();

            asyncServer = new ServiceHost(typeof(AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetHttpsBindingHelper.TransportWithMessageCredentialUserName(), string.Empty);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetTcpBindingHelper.TransportWithMessageCredentialUserName(), SelfHostServer.NetTcpBindingPathSuffix);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), WSHttpBindingHelper.TransportWithMessageCredentialUserName(), SelfHostServer.WSHttpBindingPathSuffix);
            asyncServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetHttpsBindingHelper.TransportWithMessageCredentialUserName(), string.Empty);
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetTcpBindingHelper.TransportWithMessageCredentialUserName(), SelfHostServer.NetTcpBindingPathSuffix);
            duplexServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetHttpsBindingHelper.TransportWithMessageCredentialUserName(), string.Empty);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetTcpBindingHelper.TransportWithMessageCredentialUserName(), SelfHostServer.NetTcpBindingPathSuffix);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), WSHttpBindingHelper.TransportWithMessageCredentialUserName(), SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetTcpBindingHelper.StreamedTransportWithMessageCredentialUserName(), SelfHostServer.NetTcpBindingPathSuffix);
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetHttpsBindingHelper.StreamedTransportWithMessageCredentialUserName(), string.Empty);
            streamServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            streamServer.Open();
        }

        public void TransportWithMessageCredentialWindows()
        {
            CommonLog.WriteLine("SelfHostServer.TransportWithMessageCredentialWindows");
            X509Certificate2 serverCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();
            CommonMachine.LocalHost.SetupHttps();

            asyncServer = new ServiceHost(typeof(AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.AsyncService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.AsyncService));
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), NetTcpBindingHelper.TransportWithMessageCredentialWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            asyncServer.AddServiceEndpoint(typeof(IAsyncService), WSHttpBindingHelper.TransportWithMessageCredentialWindows(), SelfHostServer.WSHttpBindingPathSuffix);
            asyncServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            asyncServer.Open();

            duplexServer = new ServiceHost(typeof(DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.DuplexService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.DuplexService));
            duplexServer.AddServiceEndpoint(typeof(IDuplexService), NetTcpBindingHelper.TransportWithMessageCredentialWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            duplexServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            duplexServer.Open();

            requestReplyServer = new ServiceHost(typeof(RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.RequestReplyService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.RequestReplyService));
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), NetTcpBindingHelper.TransportWithMessageCredentialWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            requestReplyServer.AddServiceEndpoint(typeof(IRequestReplyService), WSHttpBindingHelper.TransportWithMessageCredentialWindows(), SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            requestReplyServer.Open();

            streamServer = new ServiceHost(typeof(StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostHttpsBaseAddress() + SelfHostServer.StreamService),
                new Uri(CommonMachine.LocalHost.SelfHostNetTcpBaseAddress() + SelfHostServer.StreamService));
            streamServer.AddServiceEndpoint(typeof(IStreamService), NetTcpBindingHelper.StreamedTransportWithMessageCredentialWindows(), SelfHostServer.NetTcpBindingPathSuffix);
            streamServer.Credentials.ServiceCertificate.Certificate = serverCertificate;
            streamServer.Open();
        }
    }

    public class WebHostServer : CommonRemoteTask, ISampleServer
    {
        public void Cleanup()
        {
        }

        public void CustomSiteAppPool()
        {
            string customSite = "CustomSite";
            string customSitePhysicalPath = string.Format(@"{0}\inetpub\test\CustomSite", FullTrustEnvironment.GetEnvironmentVariable("SystemDrive"));
            string customAppPool = "CustomAppPool";

            if (!Directory.Exists(customSitePhysicalPath))
            {
                Directory.CreateDirectory(customSitePhysicalPath);
            }

            ServerManager serverManager = new ServerManager();
            Site site = serverManager.Sites[customSite];
            if (site != null)
            {
                serverManager.Sites.Remove(site);
            }

            ApplicationPool applicationPool = serverManager.ApplicationPools[customAppPool];
            if (applicationPool != null)
            {
                serverManager.ApplicationPools.Remove(applicationPool);
            }

            site = serverManager.Sites.Add(customSite, customSitePhysicalPath, 8080);
            applicationPool = serverManager.ApplicationPools.Add(customAppPool);

            serverManager.CommitChanges();

            CommonWebApplication web = new CommonWebApplication();
            web.SiteName = customSite;
            web.ApplicationPoolName = customAppPool;
            web.VirtualDirectoryPath = "/CustomSite";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\CustomSite");
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();

            CommonCommandLine netsh = new CommonCommandLine();
            netsh.FileName = "netsh.exe";
            netsh.Arguments = "advfirewall firewall add rule name=\"Open Port 8080\" dir=in action=allow protocol=TCP localport=8080";
            netsh.Run();
        }

        public void Default()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/Default";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\Default");
            web.EnabledProtocols = "http,net.tcp";
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void MediumPartialTrust()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/MediumPartialTrust";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\MediumPartialTrust");
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();

            CommonCommandLine gacutil = new CommonCommandLine();
            gacutil.FileName = CommonPath.GacUtilExe;
            gacutil.Arguments = string.Format(@"/if {0}\bin\Test.WCF.FullTrust.dll", web.VirtualDirectoryPhysicalPath);
            gacutil.Run();
        }

        public void MessageCertificate()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/MessageCertificate";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\MessageCertificate");
            web.EnabledProtocols = "http,net.tcp";
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void MessageUserName()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/MessageUserName";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\MessageUserName");
            web.EnabledProtocols = "http,net.tcp";
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void MessageWindows()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/MessageWindows";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\MessageWindows");
            web.EnabledProtocols = "http,net.tcp";
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void ServiceHostFactory()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/ServiceHostFactory";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\ServiceHostFactory");
            web.EnabledProtocols = "http,net.tcp";
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void TransportBasic()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/TransportBasic";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\TransportBasic");
            web.EnabledAuthentications = CommonAuthentication.Basic;
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void TransportCertificate()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/TransportCertificate";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\TransportCertificate");
            web.EnabledProtocols = "http,net.tcp";
            web.EnabledAuthentications = CommonAuthentication.Anonymous | CommonAuthentication.ClientCertificate;
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void TransportDigest()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/TransportDigest";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\TransportDigest");
            web.EnabledAuthentications = CommonAuthentication.Digest;
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void TransportNtlm()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/TransportNtlm";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\TransportNtlm");
            web.EnabledAuthentications = CommonAuthentication.Windows;
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void TransportNtlmExtendedProtection()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/TransportNtlmExtendedProtection";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\TransportNtlmExtendedProtection");
            web.EnabledAuthentications = CommonAuthentication.Windows;
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();

            using (ServerManager serverManager = new ServerManager())
            {
                string locationPath = web.SiteName + web.VirtualDirectoryPath;
                Configuration config = serverManager.GetApplicationHostConfiguration();
                ConfigurationSection windowsAuthentication = config.GetSection("system.webServer/security/authentication/windowsAuthentication", locationPath);
                ConfigurationElement extendedProtection = windowsAuthentication.GetChildElement("extendedProtection");
                extendedProtection["tokenChecking"] = @"Require";
                extendedProtection["flags"] = @"None";
                serverManager.CommitChanges();
            }
        }

        public void TransportWindows()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/TransportWindows";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\TransportWindows");
            web.EnabledProtocols = "http,net.tcp";
            web.EnabledAuthentications = CommonAuthentication.Windows;
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void TransportWindowsExtendedProtection()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/TransportWindowsExtendedProtection";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\TransportWindowsExtendedProtection");
            web.EnabledProtocols = "http,net.tcp";
            web.EnabledAuthentications = CommonAuthentication.Windows;
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();

            using (ServerManager serverManager = new ServerManager())
            {
                string locationPath = web.SiteName + web.VirtualDirectoryPath;
                Configuration config = serverManager.GetApplicationHostConfiguration();
                ConfigurationSection windowsAuthentication = config.GetSection("system.webServer/security/authentication/windowsAuthentication", locationPath);
                ConfigurationElement extendedProtection = windowsAuthentication.GetChildElement("extendedProtection");
                extendedProtection["tokenChecking"] = @"Require";
                extendedProtection["flags"] = @"None";
                serverManager.CommitChanges();
            }
        }        

        public void TransportWithMessageCredentialCertificate()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/TransportWithMessageCredentialCertificate";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\TransportWithMessageCredentialCertificate");
            web.EnabledProtocols = "http,net.tcp";
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void TransportWithMessageCredentialUserName()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/TransportWithMessageCredentialUserName";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\TransportWithMessageCredentialUserName");
            web.EnabledProtocols = "http,net.tcp";
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }

        public void TransportWithMessageCredentialWindows()
        {
            CommonWebApplication web = new CommonWebApplication();
            web.VirtualDirectoryPath = "/TransportWithMessageCredentialWindows";
            web.VirtualDirectoryPhysicalPath = Path.Combine(Constant.TestDirectory, @"wwwroot\TransportWithMessageCredentialWindows");
            web.EnabledProtocols = "http,net.tcp";
            web.BinFiles.Add(this.GetType().Assembly.Location);
            web.Deploy();
        }        
    }
}
