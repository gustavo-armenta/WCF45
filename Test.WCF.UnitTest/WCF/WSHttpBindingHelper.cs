namespace Test.WCF.UnitTest.WCF
{
    using System.ServiceModel;

    public class WSHttpBindingHelper
    {
        public static WSHttpBinding Default()
        {
            WSHttpBinding binding = new WSHttpBinding();
            return binding;
        }

        public static WSHttpBinding MessageCertificate()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
            return binding;
        }

        public static WSHttpBinding MessageIssuedToken()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.IssuedToken;
            return binding;
        }

        public static WSHttpBinding MessageNone()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            return binding;
        }

        public static WSHttpBinding MessageUserName()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            return binding;
        }

        public static WSHttpBinding MessageWindows()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            return binding;
        }

        public static WSHttpBinding TransportBasic()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            return binding;
        }

        public static WSHttpBinding TransportCertificate()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            return binding;
        }

        public static WSHttpBinding TransportDigest()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Digest;
            return binding;
        }

        public static WSHttpBinding TransportNone()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            return binding;
        }

        public static WSHttpBinding TransportNtlm()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            return binding;
        }

        public static WSHttpBinding TransportWindows()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            return binding;
        }        

        public static WSHttpBinding TransportWithMessageCredentialCertificate()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
            return binding;
        }

        public static WSHttpBinding TransportWithMessageCredentialUserName()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            return binding;
        }

        public static WSHttpBinding TransportWithMessageCredentialWindows()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            return binding;
        }        
    }
}
