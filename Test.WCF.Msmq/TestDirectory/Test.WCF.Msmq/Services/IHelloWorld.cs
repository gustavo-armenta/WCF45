using System.ServiceModel;

namespace Test.WCF.Msmq.Services
{
    [ServiceContract]
    public interface IHelloWorld
    {
        [OperationContract(IsOneWay = true)]
        void HelloWorld(string message);
    }
}