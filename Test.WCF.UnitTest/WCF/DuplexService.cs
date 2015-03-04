namespace Test.WCF.UnitTest.WCF
{
    using System.ServiceModel;
    using System.Threading;
    using Test.WCF.Common;

    public class DuplexService : IDuplexService
    {
        public void OneWayToServer(string value)
        {
            CommonLog.WriteLine("DuplexService.OneWayToServer");
            CommonLog.WriteLine("Thread.CurrentPrincipal.Identity.Name={0}", Thread.CurrentPrincipal.Identity.Name);
            OperationContext.Current.GetCallbackChannel<IDuplexCallback>().OneWayToClient(value);
            CommonLog.WriteLine("DuplexService.OneWayToServer end");
        }
    }
}
