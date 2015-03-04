namespace Test.WCF.UnitTest.WCF
{
    using System;
    using System.Threading.Tasks;
    using Test.WCF.Common;

    public class AsyncService : IAsyncService
    {
        public async Task<string> EchoAsync(string value)
        {
            CommonLog.WriteLine("AsyncService.EchoAsync({0})", value);
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            CommonLog.WriteLine("AsyncService.EchoAsync end");
            return value;
        }
    }
}
