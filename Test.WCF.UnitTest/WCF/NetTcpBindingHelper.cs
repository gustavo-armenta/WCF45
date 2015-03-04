namespace Test.WCF.UnitTest.WCF
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public class NetTcpBindingHelper
    {
        public static NetTcpBinding Default()
        {
            NetTcpBinding binding = new NetTcpBinding();
            return binding;
        }

        public static NetTcpBinding SecurityModeNone()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            return binding;
        }

        public static NetTcpBinding Streamed()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            return binding;
        }

        public static NetTcpBinding MessageCertificate()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            return binding;
        }

        public static NetTcpBinding MessageWindows()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            return binding;
        }

        public static NetTcpBinding TransportCertificate()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            return binding;
        }

        public static NetTcpBinding TransportWindows()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            return binding;
        }

        public static NetTcpBinding TransportWithMessageCredentialCertificate()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
            return binding;
        }

        public static NetTcpBinding TransportWithMessageCredentialUserName()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            return binding;
        }

        public static NetTcpBinding TransportWithMessageCredentialWindows()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            return binding;
        }

        public static NetTcpBinding StreamedMessageCertificate()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            return binding;
        }

        public static NetTcpBinding StreamedMessageWindows()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            return binding;
        }

        public static NetTcpBinding StreamedTransportCertificate()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            return binding;
        }

        public static NetTcpBinding StreamedTransportWindows()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            return binding;
        }

        public static NetTcpBinding StreamedTransportWithMessageCredentialCertificate()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
            return binding;
        }

        public static NetTcpBinding StreamedTransportWithMessageCredentialUserName()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            return binding;
        }

        public static NetTcpBinding StreamedTransportWithMessageCredentialWindows()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            return binding;
        }
    }
}
