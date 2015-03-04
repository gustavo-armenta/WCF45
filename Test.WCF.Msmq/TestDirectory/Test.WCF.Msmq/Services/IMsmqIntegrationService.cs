using System.ServiceModel;
using System.ServiceModel.MsmqIntegration;

namespace Test.WCF.Msmq.Services
{
    [ServiceContract]
    [ServiceKnownType(typeof(string))]
    public interface IMsmqIntegrationService
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        void SubmitString(MsmqMessage<string> msg);
    }
}