using System.ServiceModel;
using System.Threading;
using System.Transactions;
using Test.WCF.Common;

namespace Test.WCF.Msmq.Services
{
    [ServiceBehavior(ReleaseServiceInstanceOnTransactionComplete = false,
        TransactionIsolationLevel = IsolationLevel.Serializable,
        ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class BatchingService : IHelloWorld
    {
        static int messagesReceived = 0;

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void HelloWorld(string message)
        {
            CommonLog.WriteLine(message);

            // When the expected number is reached, signal to the client.
            if (Interlocked.Increment(ref messagesReceived) == 1000)
            {
                ChannelFactory<IClientListener> factory = new ChannelFactory<IClientListener>(new BasicHttpBinding(), Common.ClientListenerAddress);
                IClientListener caller = factory.CreateChannel();
                caller.PassTest();
            }
        }
    }
}