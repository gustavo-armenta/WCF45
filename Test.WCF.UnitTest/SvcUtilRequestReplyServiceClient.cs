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

    public class SvcUtilRequestReplyServiceClient : MarshalByRefObject
    {
        public void Execute(string path)
        {
            Assembly assembly = Assembly.LoadFile(path);

            Type clientType = assembly.GetType("RequestReplyServiceClient");

            var client = Activator.CreateInstance(clientType, "NetHttpBinding_IRequestReplyService");
            
            PropertyInfo propInfo = client.GetType().GetProperty("ChannelFactory");
            ChannelFactory channelFactory = (ChannelFactory)propInfo.GetValue(client, null);

            CommonLog.WriteLine("invoke client.Echo()");
            string result = (string)client.GetType().GetInterface("IRequestReplyService").InvokeMember("Echo", BindingFlags.InvokeMethod, null, client, new object[] { "EchoThisMessage" });
            FullTrustAssert.AreEqual("EchoThisMessage", result);
        }
    }
}
