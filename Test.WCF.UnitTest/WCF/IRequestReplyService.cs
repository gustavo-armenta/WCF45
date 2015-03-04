namespace Test.WCF.UnitTest.WCF
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IRequestReplyService
    {
        [OperationContract]
        string Echo(string value);
    }
}
