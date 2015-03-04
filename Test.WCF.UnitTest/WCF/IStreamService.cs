namespace Test.WCF.UnitTest.WCF
{
    using System.IO;
    using System.ServiceModel;

    [ServiceContract]
    public interface IStreamService
    {
        [OperationContract]
        Stream Echo(Stream value);
    }
}
