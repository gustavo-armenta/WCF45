namespace Test.WCF.Common
{
    using System;
    using System.ServiceModel;

    public static class CommonServiceHost
	{
        public static void Cleanup(ServiceHost host)
        {
            if (host != null)
            {
                try
                {
                    host.Close();
                }
                catch (Exception exception)
                {
                    CommonLog.WriteExceptionMessage(exception, "Calling host.Abort() because host.Close() failed");
                    host.Abort();
                }
            }
        }
	}
}
