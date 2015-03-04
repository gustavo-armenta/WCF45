namespace Test.WCF.Common
{
    using System;
    using System.ServiceModel;

    public static class CommonChannel
	{
        public static void Cleanup(IDuplexContextChannel channel)
        {
            try
            {
                channel.CloseOutputSession(TimeSpan.FromSeconds(1));
                channel.Close();
            }
            catch (Exception exception)
            {
                CommonLog.WriteExceptionMessage(exception, "Calling channel.Abort() because channel.Close() failed");
                channel.Abort();
            }
        }

        public static void Cleanup(IClientChannel channel)
        {
            try
            {
                channel.Close();
            }
            catch (Exception exception)
            {
                CommonLog.WriteExceptionMessage(exception, "Calling channel.Abort() because channel.Close() failed");
                channel.Abort();
            }
        }

        public static void Cleanup(ICommunicationObject channel)
        {
            try
            {
                channel.Close();
            }
            catch (Exception exception)
            {
                CommonLog.WriteExceptionMessage(exception, "Calling channel.Abort() because channel.Close() failed");
                channel.Abort();
            }
        }
	}
}
