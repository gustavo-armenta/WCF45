namespace Test.WCF.UnitTest.WCF
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public class NetHttpBindingHelper
    {
        public static NetHttpBinding Default()
        {
            NetHttpBinding binding = new NetHttpBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            return binding;
        }

        public static NetHttpBinding Streamed()
        {
            NetHttpBinding binding = new NetHttpBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            return binding;
        }

        public static NetHttpBinding MessageCertificate()
        {
            NetHttpBinding binding = new NetHttpBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.Security.Mode = BasicHttpSecurityMode.Message;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
            return binding;
        }

        public static NetHttpBinding MessageUserName()
        {
            NetHttpBinding binding = new NetHttpBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.Security.Mode = BasicHttpSecurityMode.Message;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            return binding;
        }

        public static NetHttpBinding StreamedMessageCertificate()
        {
            NetHttpBinding binding = new NetHttpBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = BasicHttpSecurityMode.Message;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
            return binding;
        }

        public static NetHttpBinding StreamedMessageUserName()
        {
            NetHttpBinding binding = new NetHttpBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = BasicHttpSecurityMode.Message;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            return binding;
        }
    }
}
