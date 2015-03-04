namespace Test.WCF.UnitTest.WCF
{
    using System.ServiceModel;

    [ServiceContract(CallbackContract = typeof(IDuplexCallback))]
    public interface IDuplexService
    {
        [OperationContract(IsOneWay=true)]
        void OneWayToServer(string value);
    }
}
