namespace Test.WCF.UnitTest.WCF
{
    using System.ServiceModel;
    using System.Threading.Tasks;

    [ServiceContract]
    public interface IAsyncService
    {
        [OperationContract]
        Task<string> EchoAsync(string value);
    }
}
