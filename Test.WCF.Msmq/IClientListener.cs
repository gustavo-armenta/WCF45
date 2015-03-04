using System.ServiceModel;

namespace Test.WCF.Msmq
{
    // Use an implementation of IClientListener as a ServiceHost in test methods.  The services can call back
    // into this implementation to control ManualResetEvents in the test code.
    [ServiceContract]
    public interface IClientListener
    {
        [OperationContract]
        void PassTest();

        [OperationContract]
        void FailTest(string failureReason);
    }
}