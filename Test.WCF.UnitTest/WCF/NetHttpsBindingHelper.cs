namespace Test.WCF.UnitTest.WCF
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public class NetHttpsBindingHelper
    {
        public static NetHttpsBinding Default()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            return binding;
        }

        public static NetHttpsBinding Streamed()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            return binding;
        }

        public static NetHttpsBinding TransportBasic()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            return binding;
        }

        public static NetHttpsBinding TransportCertificate()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            return binding;
        }

        public static NetHttpsBinding TransportDigest()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Digest;
            return binding;
        }

        public static NetHttpsBinding TransportNone()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            return binding;
        }

        public static NetHttpsBinding TransportNtlm()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;            
            return binding;
        }

        public static NetHttpsBinding TransportWindows()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;            
            return binding;
        }        

        public static NetHttpsBinding TransportWithMessageCredentialCertificate()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.Security.Mode = BasicHttpsSecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
            return binding;
        }

        public static NetHttpsBinding TransportWithMessageCredentialUserName()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.Security.Mode = BasicHttpsSecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            return binding;
        }

        public static NetHttpsBinding StreamedTransportBasic()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            return binding;
        }

        public static NetHttpsBinding StreamedTransportCertificate()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            return binding;
        }

        public static NetHttpsBinding StreamedTransportDigest()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Digest;
            return binding;
        }

        public static NetHttpsBinding StreamedTransportNone()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            return binding;
        }

        public static NetHttpsBinding StreamedTransportNtlm()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            return binding;
        }

        public static NetHttpsBinding StreamedTransportWindows()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            return binding;
        }

        public static NetHttpsBinding StreamedTransportWithMessageCredentialCertificate()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = BasicHttpsSecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
            return binding;
        }

        public static NetHttpsBinding StreamedTransportWithMessageCredentialUserName()
        {
            NetHttpsBinding binding = new NetHttpsBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.Security.Mode = BasicHttpsSecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            return binding;
        }
    }
}
