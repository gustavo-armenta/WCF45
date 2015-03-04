using System;
using System.ServiceModel;
using System.Transactions;
using Test.WCF.Common;

namespace Test.WCF.Msmq.Services
{
    [ServiceBehavior(ReleaseServiceInstanceOnTransactionComplete = false,
    TransactionIsolationLevel = IsolationLevel.Serializable,
    ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class PoisonMessageService : IHelloWorld
    {
        static int processMessageCount = 0;

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void HelloWorld(string message)
        {
            ++processMessageCount;
            CommonLog.WriteLine(processMessageCount.ToString());

            NetMsmqBinding serviceBinding = OperationContext.Current.Host.Description.Endpoints[0].Binding as NetMsmqBinding;
            if (processMessageCount == (serviceBinding.ReceiveRetryCount + 1) * (serviceBinding.MaxRetryCycles + 1))
            {
                ChannelFactory<IClientListener> factory = new ChannelFactory<IClientListener>(new BasicHttpBinding(), Common.ClientListenerAddress);
                IClientListener caller = factory.CreateChannel();
                caller.PassTest();
            }

            throw new ApplicationException("Throwing to make the message get retried.");
        }
    }
}