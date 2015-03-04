using System.ServiceModel;
using System.ServiceModel.MsmqIntegration;

namespace Test.WCF.Msmq.Services
{
    public class MsmqIntegrationService : IMsmqIntegrationService
    {
        public void SubmitString(MsmqMessage<string> msg)
        {
            ChannelFactory<IClientListener> factory = new ChannelFactory<IClientListener>(new BasicHttpBinding(), Common.ClientListenerAddress);
            IClientListener caller = factory.CreateChannel();
            caller.PassTest();
        }
    }
}