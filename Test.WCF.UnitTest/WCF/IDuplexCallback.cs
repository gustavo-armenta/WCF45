namespace Test.WCF.UnitTest
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IDuplexCallback
    {
        [OperationContract(IsOneWay=true)]
        void OneWayToClient(string value);
    }
}
