namespace Test.WCF.UnitTest
{
    using System;
    using System.Reflection;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;

    public class SvcUtilAsyncServiceClient : MarshalByRefObject
    {
        public void Execute(string path)
        {
            Assembly assembly = Assembly.LoadFile(path);

            Type clientType = assembly.GetType("AsyncServiceClient");

            var client = Activator.CreateInstance(clientType, "NetHttpBinding_IAsyncService");
            
            PropertyInfo propInfo = client.GetType().GetProperty("ChannelFactory");
            ChannelFactory channelFactory = (ChannelFactory)propInfo.GetValue(client, null);

            CommonLog.WriteLine("invoke client.EchoAsync()");
            Task<string> echoAsync = (Task<string>)client.GetType().GetInterface("IAsyncService").InvokeMember("EchoAsync", BindingFlags.InvokeMethod, null, client, new object[] { "EchoThisMessage" });
            CommonLog.WriteLine("waiting client.EchoAsync()");
            echoAsync.Wait();
            string result = echoAsync.Result;
            FullTrustAssert.AreEqual("EchoThisMessage", result);
        }
    }
}
