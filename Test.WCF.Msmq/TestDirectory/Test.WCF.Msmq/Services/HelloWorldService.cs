using System.ServiceModel;
using Test.WCF.Common;

namespace Test.WCF.Msmq.Services
{
    class HelloWorldService : IHelloWorld
    {
        public void HelloWorld(string message)
        {
            ChannelFactory<IClientListener> factory = new ChannelFactory<IClientListener>(new BasicHttpBinding(), Common.ClientListenerAddress);
            IClientListener caller = factory.CreateChannel();
            caller.PassTest();
        }
    }
}