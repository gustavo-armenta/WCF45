namespace Test.WCF.UnitTest
{
    using System.IO;
    using Test.WCF.Common;

    public class Constant
    {
        public static string TestDirectory
        {
            get
            {
                string path = Path.GetFullPath("Test.WCF.UnitTest");
                return path;
            }
        }

        public const string ShortMessage = "Hello World!";

        public const string Owner = "gustavoa";
        public const int Priority = 1;
        public const int Timeout = 60*1000;
    }
}
