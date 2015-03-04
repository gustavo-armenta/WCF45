namespace Test.WCF.UnitTest.WCF
{
    using Test.WCF.Common;

    public class RequestReplyService : IRequestReplyService
    {
        public string Echo(string value)
        {
            CommonLog.WriteLine("RequestReplyService.Echo({0})", value);
            return value;
        }
    }
}
