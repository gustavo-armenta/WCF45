namespace Test.WCF.Common
{
    using System;
    using System.IO;
    using System.Security.AccessControl;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Principal;
    using Test.WCF.FullTrust;

	public class CommonMachine
	{
        public static bool IsLessThanWin8()
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                return true;
            }

            if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor < 2)
            {
                return true;
            }

            return false;
        }

        public class LocalHost
        {
            public static string FullyQualifiedMachineName
            {
                get
                {
                    return FullTrustDns.GetFullyQualifiedMachineName();
                }
            }

            public static string TestPath
            {
                get
                {
                    string value = string.Format(@"{0}\school\WCF\WCF45x", FullTrustEnvironment.GetEnvironmentVariable("SystemDrive"));
                    return value;
                }
            }

            public static Uri SelfHostHttpBaseAddress()
            {
                string address = string.Format("http://{0}/SelfHost", FullTrustDns.GetFullyQualifiedMachineName());
                return new Uri(address);
            }

            public static Uri SelfHostHttpsBaseAddress()
            {
                string address = string.Format("https://{0}/SelfHost", FullTrustDns.GetFullyQualifiedMachineName());
                return new Uri(address);
            }

            public static Uri SelfHostNetTcpBaseAddress()
            {
                CommonCommandLine netsh = new CommonCommandLine();
                netsh.FileName = "netsh.exe";
                netsh.Arguments = "advfirewall firewall add rule name=\"Open Port 8081\" dir=in action=allow protocol=TCP localport=8081";
                netsh.Run();

                string address = string.Format("net.tcp://{0}:8081/SelfHost", FullTrustDns.GetFullyQualifiedMachineName());
                return new Uri(address);
            }

            public static void InstallTrustedRootCertificationAuthority()
            {
                //Command used to generate TrustedRootCertificationAuthority certificate
                //CommonCommandLine makecert = new CommonCommandLine();
                //makecert.FileName = CommonPath.MakeCertExe;
                //makecert.Arguments = "-pe -ss Root -sr LocalMachine -n CN=TestCertificateAuthority -sv TestCertificateAuthority.pvk -cy authority -r TestCertificateAuthority.cer";
                //makecert.Run();
                
                CommonCommandLine certutil = new CommonCommandLine();
                certutil.FileName = "certutil.exe";
                certutil.Arguments = "-addstore -f Root TestCertificateAuthority.cer";
                certutil.Run();
            }

            public static X509Certificate2 InstallPersonalCertificate()
            {
                X509Store store;
                X509Certificate2Collection certificates;
                X509Certificate2 certificate;

                store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                certificates = store.Certificates.Find(X509FindType.FindByIssuerName, "TestCertificateAuthority", false);
                
                if (certificates.Count > 0)
                {
                    certificate = certificates[0];
                    store.Close();

                    return certificate;
                }
                
                CommonCommandLine makecert = new CommonCommandLine();
                makecert.FileName = CommonPath.MakeCertExe;
                makecert.Arguments = string.Format(
                    //"-sk server -pe -ss My -sr LocalMachine -n CN={0} -ic TestCertificateAuthority.cer -iv TestCertificateAuthority.pvk -ir LocalMachine -sky exchange TestCertificate.cer",
                    "-pe -ss My -sr LocalMachine -n CN={0} -ic TestCertificateAuthority.cer -iv TestCertificateAuthority.pvk -ir LocalMachine -sky exchange TestCertificate.cer",
                    FullTrustDns.GetFullyQualifiedMachineName());
                makecert.Run();
                    
                store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                certificates = store.Certificates.Find(X509FindType.FindByIssuerName, "TestCertificateAuthority", false);
                certificate = certificates[0];

                using (RSACryptoServiceProvider csp = certificate.PrivateKey as RSACryptoServiceProvider)
                {
                    CspParameters cspParams = new CspParameters();
                    cspParams.Flags = CspProviderFlags.UseExistingKey | CspProviderFlags.UseMachineKeyStore;
                    cspParams.KeyContainerName = csp.CspKeyContainerInfo.KeyContainerName;
                    cspParams.CryptoKeySecurity = csp.CspKeyContainerInfo.CryptoKeySecurity;

                    SecurityIdentifier iisUsers = new SecurityIdentifier("S-1-5-32-568");
                    SecurityIdentifier localService = new SecurityIdentifier("S-1-5-19");
                    SecurityIdentifier networkService = new SecurityIdentifier("S-1-5-20");

                    cspParams.CryptoKeySecurity.SetAccessRule(new CryptoKeyAccessRule(iisUsers, CryptoKeyRights.FullControl, AccessControlType.Allow));
                    cspParams.CryptoKeySecurity.SetAccessRule(new CryptoKeyAccessRule(localService, CryptoKeyRights.FullControl, AccessControlType.Allow));
                    cspParams.CryptoKeySecurity.SetAccessRule(new CryptoKeyAccessRule(networkService, CryptoKeyRights.FullControl, AccessControlType.Allow));

                    using (RSACryptoServiceProvider csp2 = new RSACryptoServiceProvider(cspParams))
                    {
                    }
                }

                store.Close();
                
                return certificate;
            }

            public static X509Certificate2 InstallServiceCertificate()
            {
                X509Store store;
                X509Certificate2Collection certificates;
                X509Certificate2 certificate = null;

                store = new X509Store(StoreName.AddressBook, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                certificates = store.Certificates.Find(X509FindType.FindByIssuerName, "TestCertificateAuthority", true);

                if (certificates.Count > 0)
                {
                    certificate = certificates[0];
                    store.Close();

                    return certificate;
                }

                return certificate;
            }

            public static void SetupHttps()
            {
                X509Certificate2 certificate = InstallPersonalCertificate();

                CommonCommandLine netsh = new CommonCommandLine();
                netsh.FileName = "netsh.exe";
                netsh.Arguments = string.Format(
                    "http add sslcert ipport=0.0.0.0:443 certhash={0} appid={1}",
                    certificate.Thumbprint,
                    "{00112233-4455-6677-8899-AABBCCDDEEFF}");
                netsh.IgnoreExitCodes.Add(1);
                netsh.Run();
            }
        }

        public class Server
        {
            public static string FullyQualifiedMachineName
            {
                get
                {
                    string value = CommonTest.Settings.ServerMachineName;
                    string machine = FullTrustDns.GetFullyQualifiedMachineName(value);
                    return machine;
                }
            }

            public static string TestPath
            {
                get
                {
                    string value = string.Format(@"\\{0}\{1}\school\WCF\WCF45x", CommonMachine.Server.FullyQualifiedMachineName, FullTrustEnvironment.GetEnvironmentVariable("SystemDrive").Replace(':', '$'));
                    return value;
                }
            }

            public static Uri SelfHostHttpBaseAddress()
            {
                string address = string.Format("http://{0}/SelfHost", Server.FullyQualifiedMachineName);
                return new Uri(address);
            }

            public static Uri SelfHostHttpsBaseAddress()
            {
                string address = string.Format("https://{0}/SelfHost", Server.FullyQualifiedMachineName);
                return new Uri(address);
            }

            public static Uri SelfHostNetTcpBaseAddress()
            {
                string address = string.Format("net.tcp://{0}:8081/SelfHost", Server.FullyQualifiedMachineName);
                return new Uri(address);
            }
        }
	}
}
