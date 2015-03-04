namespace Test.WCF.NetFx50.Common
{
    using System;
    using System.ServiceModel;

    public static class CommonDuplexChannel
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
	}
}
