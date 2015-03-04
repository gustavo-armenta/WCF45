namespace Test.WCF.UnitTest
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;
    using Test.WCF.UnitTest;

    public class SvcUtilDuplexServiceClient : MarshalByRefObject
    {
        public void Execute(string path)
        {
            Assembly assembly = Assembly.LoadFile(path);

            Type callbackType = assembly.GetType("DuplexCallback");
            Type clientType = assembly.GetType("DuplexServiceClient");

            var callback = Activator.CreateInstance(callbackType);
            InstanceContext instanceContext = new InstanceContext(callback);
            var client = Activator.CreateInstance(clientType, instanceContext, "NetHttpBinding_IDuplexService");            
            
            PropertyInfo propInfo = client.GetType().GetProperty("ChannelFactory");
            ChannelFactory channelFactory = (ChannelFactory)propInfo.GetValue(client, null);

            Action<string> action = delegate(string value)
            {
                FullTrustAssert.AreEqual("EchoThisMessage", value);
            };

            CommonLog.WriteLine("invoke callback.CallbackAction");
            callback.GetType().InvokeMember("CallbackAction", BindingFlags.SetField, null, callback, new object[] { action });
            CommonLog.WriteLine("invoke client.OneWayToServer()");
            client.GetType().GetInterface("IDuplexService").InvokeMember("OneWayToServer", BindingFlags.InvokeMethod, null, client, new object[] { "EchoThisMessage" });
            CommonLog.WriteLine("invoke callback.WaitForCallback()");
            callback.GetType().InvokeMember("WaitForCallback", BindingFlags.InvokeMethod, null, callback, null);
        }
    }
}
