namespace Test.WCF.UnitTest.WCF
{
    using System.IO;
    using Test.WCF.Common;

    public class StreamService : IStreamService
    {
        public Stream Echo(Stream value)
        {
            CommonLog.WriteLine("StreamService.Echo");
            return value;
        }
    }
}
