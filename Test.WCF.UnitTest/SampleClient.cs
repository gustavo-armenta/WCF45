namespace Test.WCF.UnitTest
{
    using System;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Principal;
    using System.ServiceModel;
    using System.ServiceModel.Security;
    using System.Threading.Tasks;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;
    using Test.WCF.UnitTest;
    using Test.WCF.UnitTest.WCF;

    public class SampleClient : MarshalByRefObject
    {
        public string ServiceAddress
        {
            get
            {
                return this.serviceAddress;
            }
            set
            {
                this.serviceAddress = value;
                CommonLog.WriteLine("SampleClient.ServiceAddress={0}", this.serviceAddress);
            }
        }

        public string ServiceAddressNetTcpBinding
        {
            get
            {
                return this.serviceAddressNetTcpBinding;
            }
            set
            {
                this.serviceAddressNetTcpBinding = value;
                CommonLog.WriteLine("SampleClient.ServiceAddressNetTcpBinding={0}", this.serviceAddressNetTcpBinding);
            }
        }

        private string serviceAddress;
        private string serviceAddressNetTcpBinding;

        private async Task TestAsync(IAsyncService client)
        {
            string response = await client.EchoAsync(Constant.ShortMessage);
            CommonLog.WriteLine("client received async response: {0}", response);
            CommonChannel.Cleanup((IClientChannel)client);
            FullTrustAssert.AreEqual(Constant.ShortMessage, response);            
        }

        private void TestRequestReply(IRequestReplyService client)
        {
            string response = client.Echo(Constant.ShortMessage);
            CommonLog.WriteLine("client received response: {0}", response);
            CommonChannel.Cleanup((IClientChannel)client);
            FullTrustAssert.AreEqual(Constant.ShortMessage, response);
        }

        private void TestDuplex(IDuplexService client, DuplexCallback callback)
        {
            client.OneWayToServer(Constant.ShortMessage);
            callback.WaitForCallback();
            CommonChannel.Cleanup((IDuplexContextChannel)client);
        }

        private void TestStream(IStreamService client)
        {
            //Bug DevDiv.603976
            return;

            //MemoryStream stream = new MemoryStream(new byte[1024 * 1024]);
            //byte[] buffer = new byte[1024];
            //int readLength = 0;
            //long totalLength = 0;
            //Stream response = client.Echo(stream);
            //CommonLog.WriteLine("client is reading stream response");
            //do
            //{
            //    readLength = response.Read(buffer, 0, buffer.Length);
            //    totalLength += readLength;
            //} while (readLength > 0);
            //CommonLog.WriteLine("client finished reading stream response");
            //CommonChannel.Cleanup((IClientChannel)client);
            //FullTrustAssert.AreEqual(stream.Length, totalLength);
        }

        public void CustomSite()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.AsyncService);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.DuplexService);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpBindingHelper.Streamed(), this.ServiceAddress + SelfHostServer.StreamService);
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        public void Default()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.AsyncService);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.DuplexService);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpBindingHelper.Streamed(), this.ServiceAddress + SelfHostServer.StreamService);
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void DefaultNetTcpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetTcpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.NetTcpBindingPathSuffix);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetTcpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.NetTcpBindingPathSuffix);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.DuplexService + SelfHostServer.NetTcpBindingPathSuffix);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetTcpBindingHelper.Streamed(), this.ServiceAddress + SelfHostServer.StreamService);
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void DefaultWSHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, WSHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.DuplexService + SelfHostServer.WSHttpBindingPathSuffix);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);
        }

        public void MediumPartialTrust()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.AsyncService);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpBindingHelper.Default(), this.ServiceAddress + SelfHostServer.DuplexService);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpBindingHelper.Streamed(), this.ServiceAddress + SelfHostServer.StreamService);
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        public void MessageCertificate()
        {
            this.MessageCertificateNetHttpBinding();
            this.MessageCertificateNetTcpBinding();
            this.MessageCertificateWSHttpBinding();
        }

        private void MessageCertificateNetHttpBinding()
        {
            X509Certificate2 clientCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();
            X509Certificate2 serverCertificate = CommonMachine.LocalHost.InstallServiceCertificate();

            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpBindingHelper.MessageCertificate(), this.ServiceAddress + SelfHostServer.AsyncService);
            asyncChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            asyncChannelFactory.Credentials.ServiceCertificate.DefaultCertificate = serverCertificate;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpBindingHelper.MessageCertificate(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            requestReplyChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            requestReplyChannelFactory.Credentials.ServiceCertificate.DefaultCertificate = serverCertificate;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpBindingHelper.MessageCertificate(), this.ServiceAddress + SelfHostServer.DuplexService);
            duplexChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            duplexChannelFactory.Credentials.ServiceCertificate.DefaultCertificate = serverCertificate;
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpBindingHelper.StreamedMessageCertificate(), this.ServiceAddress + SelfHostServer.StreamService);
            streamChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            streamChannelFactory.Credentials.ServiceCertificate.DefaultCertificate = serverCertificate;
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void MessageCertificateNetTcpBinding()
        {
            X509Certificate2 clientCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();
            X509Certificate2 serverCertificate = CommonMachine.LocalHost.InstallServiceCertificate();

            EndpointAddress asyncEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.AsyncService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateSpnIdentity(string.Format("host/{0}", CommonMachine.Server.FullyQualifiedMachineName)));
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetTcpBindingHelper.MessageCertificate(), asyncEndpointAddress);
            asyncChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            asyncChannelFactory.Credentials.ServiceCertificate.DefaultCertificate = serverCertificate;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            EndpointAddress requestReplyEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.RequestReplyService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateSpnIdentity(string.Format("host/{0}", CommonMachine.Server.FullyQualifiedMachineName)));
            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetTcpBindingHelper.MessageCertificate(), requestReplyEndpointAddress);
            requestReplyChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            requestReplyChannelFactory.Credentials.ServiceCertificate.DefaultCertificate = serverCertificate;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            EndpointAddress duplexEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.DuplexService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateSpnIdentity(string.Format("host/{0}", CommonMachine.Server.FullyQualifiedMachineName)));
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.MessageCertificate(), duplexEndpointAddress);
            duplexChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            duplexChannelFactory.Credentials.ServiceCertificate.DefaultCertificate = serverCertificate;
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            EndpointAddress streamedEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.StreamService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateSpnIdentity(string.Format("host/{0}", CommonMachine.Server.FullyQualifiedMachineName)));
            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetTcpBindingHelper.StreamedMessageCertificate(), streamedEndpointAddress);
            streamChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            streamChannelFactory.Credentials.ServiceCertificate.DefaultCertificate = serverCertificate;
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void MessageCertificateWSHttpBinding()
        {
            X509Certificate2 clientCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();
            X509Certificate2 serverCertificate = CommonMachine.LocalHost.InstallServiceCertificate();

            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.MessageCertificate(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            asyncChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            asyncChannelFactory.Credentials.ServiceCertificate.DefaultCertificate = serverCertificate;
            asyncChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.MessageCertificate(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            requestReplyChannelFactory.Credentials.ServiceCertificate.DefaultCertificate = serverCertificate;
            requestReplyChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void MessageUserName()
        {
            this.MessageUserNameNetHttpBinding();
            this.MessageUserNameWSHttpBinding();
        }

        private void MessageUserNameNetHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpBindingHelper.MessageUserName(), this.ServiceAddress + SelfHostServer.AsyncService);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpBindingHelper.MessageUserName(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpBindingHelper.MessageUserName(), this.ServiceAddress + SelfHostServer.DuplexService);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpBindingHelper.StreamedMessageUserName(), this.ServiceAddress + SelfHostServer.StreamService);
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void MessageUserNameWSHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.MessageUserName(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            asyncChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            asyncChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.MessageUserName(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            requestReplyChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void MessageWindows()
        {
            this.MessageWindowsNetTcpBinding();
            this.MessageWindowsWSHttpBinding();
        }

        private void MessageWindowsNetTcpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetTcpBindingHelper.MessageWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.AsyncService + SelfHostServer.NetTcpBindingPathSuffix);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetTcpBindingHelper.MessageWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.RequestReplyService + SelfHostServer.NetTcpBindingPathSuffix);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.MessageWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.DuplexService + SelfHostServer.NetTcpBindingPathSuffix);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetTcpBindingHelper.StreamedMessageWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.StreamService + SelfHostServer.NetTcpBindingPathSuffix);
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void MessageWindowsWSHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.MessageWindows(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.MessageWindows(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void ServiceHostFactory()
        {
            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.Default(), this.ServiceAddress);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);
        }

        public void TransportBasic()
        {
            this.TransportBasicNetHttpsBinding();
            this.TransportBasicWSHttpBinding();
        }

        private void TransportBasicNetHttpsBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpsBindingHelper.TransportBasic(), this.ServiceAddress + SelfHostServer.AsyncService);
            asyncChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            asyncChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpsBindingHelper.TransportBasic(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            requestReplyChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            requestReplyChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpsBindingHelper.TransportBasic(), this.ServiceAddress + SelfHostServer.DuplexService);
            duplexChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            duplexChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            //ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpsBindingHelper.StreamedTransportBasic(), this.ServiceAddress + SelfHostServer.StreamService);
            //streamChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            //streamChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            //IStreamService streamClient = streamChannelFactory.CreateChannel();
            //this.TestStream(streamClient);
        }

        private void TransportBasicWSHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.TransportBasic(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            asyncChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            asyncChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.TransportBasic(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            requestReplyChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void TransportCertificate()
        {
            this.TransportCertificateNetHttpsBinding();
            this.TransportCertificateNetTcpBinding();
            this.TransportCertificateWSHttpBinding();
        }

        private void TransportCertificateNetHttpsBinding()
        {
            X509Certificate2 clientCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();

            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpsBindingHelper.TransportCertificate(), this.ServiceAddress + SelfHostServer.AsyncService);
            asyncChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpsBindingHelper.TransportCertificate(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            requestReplyChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpsBindingHelper.TransportCertificate(), this.ServiceAddress + SelfHostServer.DuplexService);
            duplexChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            //ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpsBindingHelper.StreamAuthenticationCertificate(), this.ServiceAddress + SelfHostServer.StreamService);
            //streamChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            //IStreamService streamClient = streamChannelFactory.CreateChannel();
            //this.TestStream(streamClient);
        }

        private void TransportCertificateNetTcpBinding()
        {
            X509Certificate2 clientCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();

            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetTcpBindingHelper.TransportCertificate(), this.ServiceAddressNetTcpBinding + SelfHostServer.AsyncService + SelfHostServer.NetTcpBindingPathSuffix);
            asyncChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            asyncChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetTcpBindingHelper.TransportCertificate(), this.ServiceAddressNetTcpBinding + SelfHostServer.RequestReplyService + SelfHostServer.NetTcpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            requestReplyChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.TransportCertificate(), this.ServiceAddressNetTcpBinding + SelfHostServer.DuplexService + SelfHostServer.NetTcpBindingPathSuffix);
            duplexChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            duplexChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetTcpBindingHelper.StreamedTransportCertificate(), this.ServiceAddressNetTcpBinding + SelfHostServer.StreamService + SelfHostServer.NetTcpBindingPathSuffix);
            streamChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            streamChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void TransportCertificateWSHttpBinding()
        {
            X509Certificate2 clientCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();

            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.TransportCertificate(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            asyncChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            asyncChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.TransportCertificate(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            requestReplyChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void TransportDigest()
        {
            this.TransportDigestNetHttpsBinding();
            this.TransportDigestWSHttpBinding();
        }

        private void TransportDigestNetHttpsBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpsBindingHelper.TransportDigest(), this.ServiceAddress + SelfHostServer.AsyncService);
            asyncChannelFactory.Credentials.HttpDigest.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpsBindingHelper.TransportDigest(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            requestReplyChannelFactory.Credentials.HttpDigest.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpsBindingHelper.TransportDigest(), this.ServiceAddress + SelfHostServer.DuplexService);
            duplexChannelFactory.Credentials.HttpDigest.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            //ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpBindingHelper.StreamAuthenticationDigest(), this.ServiceAddress + SelfHostServer.StreamService);
            //streamChannelFactory.Credentials.HttpDigest.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            //IStreamService streamClient = streamChannelFactory.CreateChannel();
            //this.TestStream(streamClient);
        }

        private void TransportDigestWSHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.TransportDigest(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            asyncChannelFactory.Credentials.HttpDigest.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;            
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.TransportDigest(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.HttpDigest.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void TransportMessageUserName()
        {
            this.TransportMessageUserNameNetHttpBinding();
            this.TransportMessageUserNameWSHttpBinding();
        }

        private void TransportMessageUserNameNetHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpBindingHelper.MessageUserName(), this.ServiceAddress + SelfHostServer.AsyncService);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpBindingHelper.MessageUserName(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpBindingHelper.MessageUserName(), this.ServiceAddress + SelfHostServer.DuplexService);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpBindingHelper.StreamedMessageUserName(), this.ServiceAddress + SelfHostServer.StreamService);
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void TransportMessageUserNameWSHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.MessageUserName(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            asyncChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            asyncChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.MessageUserName(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            requestReplyChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void TransportNtlm()
        {
            this.TransportNtlmNetHttpsBinding();
            this.TransportNtlmWSHttpBinding();
        }

        private void TransportNtlmNetHttpsBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpsBindingHelper.TransportNtlm(), this.ServiceAddress + SelfHostServer.AsyncService);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpsBindingHelper.TransportNtlm(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpsBindingHelper.TransportNtlm(), this.ServiceAddress + SelfHostServer.DuplexService);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            //ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpsBindingHelper.StreamedTransportNtlm(), this.ServiceAddress + SelfHostServer.StreamService);
            //IStreamService streamClient = streamChannelFactory.CreateChannel();
            //this.TestStream(streamClient);
        }

        private void TransportNtlmWSHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.TransportNtlm(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.TransportNtlm(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void TransportWindowsSelfHost()
        {
            this.TransportWindowsSelfHostNetHttpsBinding();
            this.TransportWindowsSelfHostNetTcpBinding();
            this.TransportWindowsWSHttpBinding();
        }

        private void TransportWindowsSelfHostNetHttpsBinding()
        {
            EndpointAddress asyncEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddress + SelfHostServer.AsyncService), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpsBindingHelper.TransportWindows(), asyncEndpointAddress);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            EndpointAddress requestReplyEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddress + SelfHostServer.RequestReplyService), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpsBindingHelper.TransportWindows(), requestReplyEndpointAddress);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            EndpointAddress duplexEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddress + SelfHostServer.DuplexService), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpsBindingHelper.TransportWindows(), duplexEndpointAddress);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            //EndpointAddress streamEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddress + SelfHostServer.StreamService), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
            //ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpsBindingHelper.StreamedTransportWindows(), streamEndpointAddress);
            //IStreamService streamClient = streamChannelFactory.CreateChannel();
            //this.TestStream(streamClient);
        }

        private void TransportWindowsSelfHostNetTcpBinding()
        {
            EndpointAddress asyncEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.AsyncService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetTcpBindingHelper.TransportWindows(), asyncEndpointAddress);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            EndpointAddress requestReplyEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.RequestReplyService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetTcpBindingHelper.TransportWindows(), requestReplyEndpointAddress);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            EndpointAddress duplexEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.DuplexService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.TransportWindows(), duplexEndpointAddress);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            EndpointAddress streamEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.StreamService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetTcpBindingHelper.StreamedTransportWindows(), streamEndpointAddress);
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        public void TransportWindowsWebHost()
        {
            this.TransportWindowsWebHostNetHttpsBinding();
            this.TransportWindowsNetTcpBinding();
            this.TransportWindowsWSHttpBinding();
        }

        private void TransportWindowsWebHostNetHttpsBinding()
        {
            EndpointAddress asyncEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddress + SelfHostServer.AsyncService), EndpointIdentity.CreateSpnIdentity(string.Format("host/{0}", CommonMachine.Server.FullyQualifiedMachineName)));
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpsBindingHelper.TransportWindows(), asyncEndpointAddress);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            EndpointAddress requestReplyEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddress + SelfHostServer.RequestReplyService), EndpointIdentity.CreateSpnIdentity(string.Format("host/{0}", CommonMachine.Server.FullyQualifiedMachineName)));
            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpsBindingHelper.TransportWindows(), requestReplyEndpointAddress);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            EndpointAddress duplexEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddress + SelfHostServer.DuplexService), EndpointIdentity.CreateSpnIdentity(string.Format("host/{0}", CommonMachine.Server.FullyQualifiedMachineName)));
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpsBindingHelper.TransportWindows(), duplexEndpointAddress);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            //EndpointAddress streamEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddress + SampleServer.StreamService), EndpointIdentity.CreateSpnIdentity(string.Format("host/{0}", CommonMachine.Server.FullyQualifiedMachineName)));
            //ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpsBindingHelper.StreamedTransportWindows(), streamEndpointAddress);
            //IStreamService streamClient = streamChannelFactory.CreateChannel();
            //this.TestStream(streamClient);
        }

        private void TransportWindowsNetTcpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetTcpBindingHelper.TransportWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.AsyncService + SelfHostServer.NetTcpBindingPathSuffix);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetTcpBindingHelper.TransportWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.RequestReplyService + SelfHostServer.NetTcpBindingPathSuffix);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.TransportWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.DuplexService + SelfHostServer.NetTcpBindingPathSuffix);
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetTcpBindingHelper.StreamedTransportWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.StreamService + SelfHostServer.NetTcpBindingPathSuffix);
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void TransportWindowsWSHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.TransportWindows(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.TransportWindows(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void TransportWithMessageCredentialCertificate()
        {
            this.TransportWithMessageCredentialCertificateNetHttpsBinding();
            this.TransportWithMessageCredentialCertificateNetTcpBinding();
            this.TransportWithMessageCredentialCertificateWSHttpBinding();
        }

        private void TransportWithMessageCredentialCertificateNetHttpsBinding()
        {
            X509Certificate2 clientCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();

            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpsBindingHelper.TransportWithMessageCredentialCertificate(), this.ServiceAddress + SelfHostServer.AsyncService);
            asyncChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;            
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpsBindingHelper.TransportWithMessageCredentialCertificate(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            requestReplyChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;            
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpsBindingHelper.TransportWithMessageCredentialCertificate(), this.ServiceAddress + SelfHostServer.DuplexService);
            duplexChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;            
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpsBindingHelper.StreamedTransportWithMessageCredentialCertificate(), this.ServiceAddress + SelfHostServer.StreamService);
            streamChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;            
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void TransportWithMessageCredentialCertificateNetTcpBinding()
        {
            X509Certificate2 clientCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();

            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetTcpBindingHelper.TransportWithMessageCredentialCertificate(), this.ServiceAddressNetTcpBinding + SelfHostServer.AsyncService + SelfHostServer.NetTcpBindingPathSuffix);
            asyncChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            asyncChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetTcpBindingHelper.TransportWithMessageCredentialCertificate(), this.ServiceAddressNetTcpBinding + SelfHostServer.RequestReplyService + SelfHostServer.NetTcpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            requestReplyChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.TransportWithMessageCredentialCertificate(), this.ServiceAddressNetTcpBinding + SelfHostServer.DuplexService + SelfHostServer.NetTcpBindingPathSuffix);
            duplexChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            duplexChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetTcpBindingHelper.StreamedTransportWithMessageCredentialCertificate(), this.ServiceAddressNetTcpBinding + SelfHostServer.StreamService + SelfHostServer.NetTcpBindingPathSuffix);
            streamChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            streamChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void TransportWithMessageCredentialCertificateWSHttpBinding()
        {
            X509Certificate2 clientCertificate = CommonMachine.LocalHost.InstallPersonalCertificate();

            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.TransportWithMessageCredentialCertificate(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            asyncChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.TransportWithMessageCredentialCertificate(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void TransportWithMessageCredentialUserName()
        {
            this.TransportWithMessageCredentialUserNameNetHttpsBinding();
            this.TransportWithMessageCredentialUserNameNetTcpBinding();
            this.TransportWithMessageCredentialUserNameWSHttpBinding();
        }

        private void TransportWithMessageCredentialUserNameNetHttpsBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetHttpsBindingHelper.TransportWithMessageCredentialUserName(), this.ServiceAddress + SelfHostServer.AsyncService);
            asyncChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            asyncChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetHttpsBindingHelper.TransportWithMessageCredentialUserName(), this.ServiceAddress + SelfHostServer.RequestReplyService);
            requestReplyChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            requestReplyChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetHttpsBindingHelper.TransportWithMessageCredentialUserName(), this.ServiceAddress + SelfHostServer.DuplexService);
            duplexChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            duplexChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetHttpsBindingHelper.StreamedTransportWithMessageCredentialUserName(), this.ServiceAddress + SelfHostServer.StreamService);
            streamChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            streamChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void TransportWithMessageCredentialUserNameNetTcpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetTcpBindingHelper.TransportWithMessageCredentialUserName(), this.ServiceAddressNetTcpBinding + SelfHostServer.AsyncService + SelfHostServer.NetTcpBindingPathSuffix);
            asyncChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            asyncChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            asyncChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetTcpBindingHelper.TransportWithMessageCredentialUserName(), this.ServiceAddressNetTcpBinding + SelfHostServer.RequestReplyService + SelfHostServer.NetTcpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            requestReplyChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            requestReplyChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.TransportWithMessageCredentialUserName(), this.ServiceAddressNetTcpBinding + SelfHostServer.DuplexService + SelfHostServer.NetTcpBindingPathSuffix);
            duplexChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            duplexChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            duplexChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetTcpBindingHelper.StreamedTransportWithMessageCredentialUserName(), this.ServiceAddressNetTcpBinding + SelfHostServer.StreamService + SelfHostServer.NetTcpBindingPathSuffix);
            streamChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            streamChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            streamChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void TransportWithMessageCredentialUserNameWSHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.TransportWithMessageCredentialUserName(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            asyncChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            asyncChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.TransportWithMessageCredentialUserName(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.UserName.UserName = CommonCredential.StandardUser.DomainAndUserName;
            requestReplyChannelFactory.Credentials.UserName.Password = CommonCredential.StandardUser.Password;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void TransportWithMessageCredentialWindowsWebHost()
        {
            this.TransportWithMessageCredentialWindowsNetTcpBinding();
            this.TransportWithMessageCredentialWindowsWSHttpBinding();
        }

        private void TransportWithMessageCredentialWindowsNetTcpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetTcpBindingHelper.TransportWithMessageCredentialWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.AsyncService + SelfHostServer.NetTcpBindingPathSuffix);
            asyncChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetTcpBindingHelper.TransportWithMessageCredentialWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.RequestReplyService + SelfHostServer.NetTcpBindingPathSuffix);
            requestReplyChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.TransportWithMessageCredentialWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.DuplexService + SelfHostServer.NetTcpBindingPathSuffix);
            duplexChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetTcpBindingHelper.StreamedTransportWithMessageCredentialWindows(), this.ServiceAddressNetTcpBinding + SelfHostServer.StreamService + SelfHostServer.NetTcpBindingPathSuffix);
            streamChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        private void TransportWithMessageCredentialWindowsWSHttpBinding()
        {
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(WSHttpBindingHelper.TransportWithMessageCredentialWindows(), this.ServiceAddress + SelfHostServer.AsyncService + SelfHostServer.WSHttpBindingPathSuffix);
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(WSHttpBindingHelper.TransportWithMessageCredentialWindows(), this.ServiceAddress + SelfHostServer.RequestReplyService + SelfHostServer.WSHttpBindingPathSuffix);
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);
        }

        public void TransportWithMessageCredentialWindowsSelfHost()
        {
            this.TransportWithMessageCredentialWindowsSelfHostNetTcpBinding();
            this.TransportWithMessageCredentialWindowsWSHttpBinding();
        }

        private void TransportWithMessageCredentialWindowsSelfHostNetTcpBinding()
        {
            EndpointAddress asyncEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.AsyncService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateSpnIdentity(string.Format("host/{0}", CommonMachine.Server.FullyQualifiedMachineName)));
            ChannelFactory<IAsyncService> asyncChannelFactory = new ChannelFactory<IAsyncService>(NetTcpBindingHelper.TransportWithMessageCredentialWindows(), asyncEndpointAddress);
            asyncChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IAsyncService asyncClient = asyncChannelFactory.CreateChannel();
            this.TestAsync(asyncClient).Wait();

            EndpointAddress requestReplyEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.RequestReplyService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
            ChannelFactory<IRequestReplyService> requestReplyChannelFactory = new ChannelFactory<IRequestReplyService>(NetTcpBindingHelper.TransportWithMessageCredentialWindows(), requestReplyEndpointAddress);
            requestReplyChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IRequestReplyService requestReplyClient = requestReplyChannelFactory.CreateChannel();
            this.TestRequestReply(requestReplyClient);

            DuplexCallback callback = new DuplexCallback();
            callback.CallbackAction = this.CallbackAction;
            EndpointAddress duplexEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.DuplexService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
            DuplexChannelFactory<IDuplexService> duplexChannelFactory = new DuplexChannelFactory<IDuplexService>(callback, NetTcpBindingHelper.TransportWithMessageCredentialWindows(), duplexEndpointAddress);
            duplexChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IDuplexService duplexClient = duplexChannelFactory.CreateChannel();
            this.TestDuplex(duplexClient, callback);

            EndpointAddress streamEndpointAddress = new EndpointAddress(new Uri(this.ServiceAddressNetTcpBinding + SelfHostServer.StreamService + SelfHostServer.NetTcpBindingPathSuffix), EndpointIdentity.CreateUpnIdentity(WindowsIdentity.GetCurrent().Name));
            ChannelFactory<IStreamService> streamChannelFactory = new ChannelFactory<IStreamService>(NetTcpBindingHelper.StreamedTransportWithMessageCredentialWindows(), streamEndpointAddress);
            streamChannelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            IStreamService streamClient = streamChannelFactory.CreateChannel();
            this.TestStream(streamClient);
        }

        public void SvcUtilAsyncService()
        {
            CommonCommandLine svcutil = new CommonCommandLine();
            svcutil.FileName = CommonPath.SvcUtilExe;
            svcutil.Arguments = string.Format("{0}?wsdl /out:{1} /config:{2}", serviceAddress + SelfHostServer.AsyncService, "GeneratedCode.cs", "GeneratedCode.config");
            svcutil.WorkingDirectory = Path.Combine(Constant.TestDirectory, @"svcutil\AsyncService");
            svcutil.Run();

            CommonCommandLine compiler = new CommonCommandLine();
            compiler.FileName = CommonPath.CscExe;
            compiler.Arguments = " /out:SvcUtilAsyncServiceClient.dll /target:library /debug+ /debug:full";
            compiler.Arguments += string.Format(" /reference:\"{0}\"", typeof(CommonLog).Assembly.Location);
            compiler.Arguments += string.Format(" /reference:\"{0}\"", typeof(FullTrustAssert).Assembly.Location);
            compiler.Arguments += " GeneratedCode.cs";
            compiler.WorkingDirectory = Path.Combine(Constant.TestDirectory, @"svcutil\AsyncService");
            compiler.Run();

            string configurationFile = Path.Combine(compiler.WorkingDirectory, "GeneratedCode.config");
            using (CommonAppDomain ad = CommonAppDomainFactory.CreateFullTrust(configurationFile))
            {
                SvcUtilAsyncServiceClient remote = ad.CreateInstance<SvcUtilAsyncServiceClient>();
                remote.Execute(Path.Combine(compiler.WorkingDirectory, "SvcUtilAsyncServiceClient.dll"));
            }
        }

        public void SvcUtilDuplexService()
        {
            CommonCommandLine svcutil = new CommonCommandLine();
            svcutil.FileName = CommonPath.SvcUtilExe;
            svcutil.Arguments = string.Format("{0}?wsdl /out:{1} /config:{2}", serviceAddress + SelfHostServer.DuplexService, "GeneratedCode.cs", "GeneratedCode.config");
            svcutil.WorkingDirectory = Path.Combine(Constant.TestDirectory, @"svcutil\DuplexService");
            svcutil.Run();

            CommonCommandLine compiler = new CommonCommandLine();
            compiler.FileName = CommonPath.CscExe;
            compiler.Arguments = " /out:SvcUtilDuplexServiceClient.dll /target:library /debug+ /debug:full";
            compiler.Arguments += string.Format(" /reference:\"{0}\"", typeof(CommonLog).Assembly.Location);
            compiler.Arguments += string.Format(" /reference:\"{0}\"", typeof(FullTrustAssert).Assembly.Location);
            compiler.Arguments += " GeneratedCode.cs DuplexCallback.cs";
            compiler.WorkingDirectory = Path.Combine(Constant.TestDirectory, @"svcutil\DuplexService");
            compiler.Run();

            string configurationFile = Path.Combine(compiler.WorkingDirectory, "GeneratedCode.config");
            using (CommonAppDomain ad = CommonAppDomainFactory.CreateFullTrust(configurationFile))
            {
                SvcUtilDuplexServiceClient remote = ad.CreateInstance<SvcUtilDuplexServiceClient>();
                remote.Execute(Path.Combine(compiler.WorkingDirectory, "SvcUtilDuplexServiceClient.dll"));
            }
        }

        public void SvcUtilRequestReplyService()
        {
            CommonCommandLine svcutil = new CommonCommandLine();
            svcutil.FileName = CommonPath.SvcUtilExe;
            svcutil.Arguments = string.Format("{0}?wsdl /out:{1} /config:{2}", serviceAddress + SelfHostServer.RequestReplyService, "GeneratedCode.cs", "GeneratedCode.config");
            svcutil.WorkingDirectory = Path.Combine(Constant.TestDirectory, @"svcutil\RequestReplyService");
            svcutil.Run();

            CommonCommandLine compiler = new CommonCommandLine();
            compiler.FileName = CommonPath.CscExe;
            compiler.Arguments = " /out:SvcUtilRequestReplyServiceClient.dll /target:library /debug+ /debug:full";
            compiler.Arguments += string.Format(" /reference:\"{0}\"", typeof(CommonLog).Assembly.Location);
            compiler.Arguments += string.Format(" /reference:\"{0}\"", typeof(FullTrustAssert).Assembly.Location);
            compiler.Arguments += " GeneratedCode.cs";
            compiler.WorkingDirectory = Path.Combine(Constant.TestDirectory, @"svcutil\RequestReplyService");
            compiler.Run();

            string configurationFile = Path.Combine(compiler.WorkingDirectory, "GeneratedCode.config");
            using (CommonAppDomain ad = CommonAppDomainFactory.CreateFullTrust(configurationFile))
            {
                SvcUtilRequestReplyServiceClient remote = ad.CreateInstance<SvcUtilRequestReplyServiceClient>();
                remote.Execute(Path.Combine(compiler.WorkingDirectory, "SvcUtilRequestReplyServiceClient.dll"));
            }
        }

        public void SvcUtilStreamService()
        {
            CommonCommandLine svcutil = new CommonCommandLine();
            svcutil.FileName = CommonPath.SvcUtilExe;
            svcutil.Arguments = string.Format("{0}?wsdl /out:{1} /config:{2}", serviceAddress + SelfHostServer.StreamService, "GeneratedCode.cs", "GeneratedCode.config");
            svcutil.WorkingDirectory = Path.Combine(Constant.TestDirectory, @"svcutil\StreamService");
            svcutil.Run();

            CommonCommandLine compiler = new CommonCommandLine();
            compiler.FileName = CommonPath.CscExe;
            compiler.Arguments = " /out:SvcUtilStreamServiceClient.dll /target:library /debug+ /debug:full";
            compiler.Arguments += string.Format(" /reference:\"{0}\"", typeof(CommonLog).Assembly.Location);
            compiler.Arguments += string.Format(" /reference:\"{0}\"", typeof(FullTrustAssert).Assembly.Location);
            compiler.Arguments += " GeneratedCode.cs";
            compiler.WorkingDirectory = Path.Combine(Constant.TestDirectory, @"svcutil\StreamService");
            compiler.Run();

            string configurationFile = Path.Combine(compiler.WorkingDirectory, "GeneratedCode.config");
            using (CommonAppDomain ad = CommonAppDomainFactory.CreateFullTrust(configurationFile))
            {
                SvcUtilStreamServiceClient remote = ad.CreateInstance<SvcUtilStreamServiceClient>();
                remote.Execute(Path.Combine(compiler.WorkingDirectory, "SvcUtilStreamServiceClient.dll"));
            }
        }

        private void CallbackAction(string value)
        {
            FullTrustAssert.AreEqual(Constant.ShortMessage, value);
        }
    }
}
