namespace Test.WCF.UnitTest
{
    using System.IO;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;
    using Test.WCF.UnitTest;
    using Test.WCF.UnitTest.WCF;

    public interface ISampleCrossProcessClient
    {
        string ServiceAddress { get; set; }
        void Default();
    }

    public class SampleCrossProcessClient : CommonRemoteTask, ISampleCrossProcessClient
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

        private string serviceAddress;

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

        private void CallbackAction(string value)
        {
            FullTrustAssert.AreEqual(Constant.ShortMessage, value);
        }
    }
}
