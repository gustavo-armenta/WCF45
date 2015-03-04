namespace Test.WCF.UnitTest
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;
    using System.Threading.Tasks;

    public class SvcUtilStreamServiceClient : MarshalByRefObject
    {
        public void Execute(string path)
        {
            Assembly assembly = Assembly.LoadFile(path);

            Type clientType = assembly.GetType("StreamServiceClient");

            var client = Activator.CreateInstance(clientType, "NetHttpBinding_IStreamService");
            
            PropertyInfo propInfo = client.GetType().GetProperty("ChannelFactory");
            ChannelFactory channelFactory = (ChannelFactory)propInfo.GetValue(client, null);

            CommonLog.WriteLine("invoke client.Echo()");
            MemoryStream stream = new MemoryStream(new byte[60 * 1024]);
            byte[] buffer = new byte[1024];
            int readLength = 0;
            long totalLength = 0;
            Stream response = (Stream)client.GetType().GetInterface("IStreamService").InvokeMember("Echo", BindingFlags.InvokeMethod, null, client, new object[] { stream });           
            CommonLog.WriteLine("client is reading response");
            do
            {
                readLength = response.Read(buffer, 0, buffer.Length);
                totalLength += readLength;
            } while (readLength > 0);
            CommonLog.WriteLine("client finished reading response");
            CommonChannel.Cleanup((ICommunicationObject)client);
            FullTrustAssert.AreEqual(stream.Length, totalLength);
        }
    }
}
