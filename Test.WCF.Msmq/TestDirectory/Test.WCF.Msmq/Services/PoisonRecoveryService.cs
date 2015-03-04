using System.ServiceModel;

namespace Test.WCF.Msmq.Services
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class PoisonRecoveryService : IHelloWorld
    {
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void HelloWorld(string message)
        {
            ChannelFactory<IClientListener> factory = new ChannelFactory<IClientListener>(new BasicHttpBinding(), Common.ClientListenerAddress);
            IClientListener caller = factory.CreateChannel();
            caller.PassTest();
        }
    }
}