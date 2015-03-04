using System;
namespace Test.WCF.Common
{
	public class CommonLocalMachineUri
	{
        public static Uri SelfHostHttpBaseAddress()
        {
            return new Uri("http://localhost/SelfHost");
        }

        public static Uri SelfHostNetTcpBaseAddress()
        {
            return new Uri("net.tcp://localhost:40000/SelfHost");
        }
	}
}
