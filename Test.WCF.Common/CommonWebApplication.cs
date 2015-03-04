namespace Test.WCF.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Security.AccessControl;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Principal;
    using System.Threading;
    using Microsoft.Web.Administration;
    using Test.WCF.FullTrust;

    public enum CommonAuthentication
    {
        Anonymous = 1,
        Basic = 2,
        ClientCertificate = 4,
        Digest = 8,
        Windows = 16,
    }

	public class CommonWebApplication
	{
        public string SiteName { get; set; }
        public string ApplicationPoolName { get; set; }
        public string VirtualDirectoryPath { get; set; }
        public string VirtualDirectoryPhysicalPath { get; set; }
        public string EnabledProtocols { get; set; }
        public CommonAuthentication EnabledAuthentications { get; set; }
        public List<string> BinFiles { get; set; }

        public CommonWebApplication()
        {
            this.SiteName = "Default Web Site";
            this.ApplicationPoolName = "DefaultAppPool";
            this.EnabledProtocols = "http";
            this.EnabledAuthentications = CommonAuthentication.Anonymous;

            this.BinFiles = new List<string>();
            this.BinFiles.Add(typeof(CommonWebApplication).Assembly.Location);
            this.BinFiles.Add(typeof(FullTrustAssert).Assembly.Location);
        }

        public void Deploy()
        {
            FullTrustAssert.IsNotNull(this.VirtualDirectoryPhysicalPath);
            FullTrustAssert.IsTrue(Directory.Exists(this.VirtualDirectoryPhysicalPath));            

            ServerManager serverManager = new ServerManager();
            Site site = serverManager.Sites[this.SiteName];
            string physicalPath = site.Applications["/"].VirtualDirectories["/"].PhysicalPath;
            physicalPath = physicalPath.Replace("%SystemDrive%", FullTrustEnvironment.GetEnvironmentVariable("SystemDrive"));

            physicalPath = Path.Combine(physicalPath, Path.GetFileName(this.VirtualDirectoryPhysicalPath));

            CommonCommandLine cmd = new CommonCommandLine();
            cmd.FileName = "robocopy.exe";
            cmd.Arguments = string.Format(@"/E /NJH /NP ""{0}"" ""{1}""", this.VirtualDirectoryPhysicalPath, physicalPath);
            cmd.IgnoreExitCodes.AddRange(new List<int> {1, 2, 3});
            cmd.Run();

            this.VirtualDirectoryPhysicalPath = physicalPath;

            CommonLog.WriteLine(@"CommonWebApplication.Deploy
    Site={0}
    ApplicationPool={1}
    Path={2}
    PhysicalPath={3}
    Authentication={4}", 
                this.SiteName, this.ApplicationPoolName, this.VirtualDirectoryPath, this.VirtualDirectoryPhysicalPath, this.EnabledAuthentications);

            //Give access permissions to IIS_IUSRS
            DirectoryInfo directoryInfo = new DirectoryInfo(this.VirtualDirectoryPhysicalPath);
            SecurityIdentifier securityIdentifier = new SecurityIdentifier("S-1-5-32-568");
            NTAccount ntAccount = (NTAccount)securityIdentifier.Translate(typeof(NTAccount));
            DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();
            directorySecurity.AddAccessRule(new FileSystemAccessRule(ntAccount, FileSystemRights.FullControl, AccessControlType.Allow));
            directorySecurity.AddAccessRule(new FileSystemAccessRule(ntAccount, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
            directorySecurity.AddAccessRule(new FileSystemAccessRule(ntAccount, FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
            directoryInfo.SetAccessControl(directorySecurity);

            //Give access permissions to BUILTIN\Users
            directoryInfo = new DirectoryInfo(this.VirtualDirectoryPhysicalPath);
            securityIdentifier = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
            ntAccount = (NTAccount)securityIdentifier.Translate(typeof(NTAccount));
            directorySecurity = directoryInfo.GetAccessControl();
            directorySecurity.AddAccessRule(new FileSystemAccessRule(ntAccount, FileSystemRights.FullControl, AccessControlType.Allow));
            directorySecurity.AddAccessRule(new FileSystemAccessRule(ntAccount, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
            directorySecurity.AddAccessRule(new FileSystemAccessRule(ntAccount, FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
            directoryInfo.SetAccessControl(directorySecurity);

            this.CopyBinFiles();

            Application application = site.Applications[this.VirtualDirectoryPath];
            if (application != null)
            {
                site.Applications.Remove(application);
            }
            application = site.Applications.Add(this.VirtualDirectoryPath, this.VirtualDirectoryPhysicalPath);
            application.ApplicationPoolName = this.ApplicationPoolName;
            application.EnabledProtocols = this.EnabledProtocols;

            Configuration config = serverManager.GetApplicationHostConfiguration();
            if (site.Name == "Default Web Site")
            {
                this.ConfigureHttps(site);
            }            
            this.ConfigureAuthentication(config);

            serverManager.CommitChanges();

            this.Ping();
        }

        private void CopyBinFiles()
        {
            FullTrustAssert.IsNotNull(this.VirtualDirectoryPhysicalPath);

            string binDirectoryPath = Path.Combine(this.VirtualDirectoryPhysicalPath, "bin");
            if(!Directory.Exists(binDirectoryPath))
            {
                Directory.CreateDirectory(binDirectoryPath);
            }

            foreach (string file in this.BinFiles)
            {
                string destFileName = Path.Combine(binDirectoryPath, Path.GetFileName(file));
                File.Copy(file, destFileName, overwrite: true);
            }
        }

        private void Ping()
        {
            string prefix = "http";
            if (this.EnabledProtocols.Contains("https"))
            {
                prefix = "https";
            }

            string[] svcFiles = Directory.GetFiles(this.VirtualDirectoryPhysicalPath, "*.svc", SearchOption.TopDirectoryOnly);
            if (svcFiles.Length == 0)
            {
                return;
            }

            Uri requestUri = new Uri(string.Format("{0}://localhost{1}/{2}", prefix, this.VirtualDirectoryPath, Path.GetFileName(svcFiles[0])));
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now.AddSeconds(4);
            CommonLog.WriteLine("Pinging {0}", requestUri);
            bool success = false;

            while (DateTime.Now < endTime)
            {
                try
                {
                    HttpWebRequest request = HttpWebRequest.CreateHttp(requestUri);
                    if (this.EnabledAuthentications.HasFlag(CommonAuthentication.Basic))
                    {
                        CredentialCache credentialCache = new CredentialCache();
                        credentialCache.Add(
                            requestUri,
                            "Basic",
                            new NetworkCredential(CommonCredential.StandardUser.UserName, CommonCredential.StandardUser.Password, CommonCredential.StandardUser.Domain));
                        request.Credentials = credentialCache;
                    }
                    else if (this.EnabledAuthentications.HasFlag(CommonAuthentication.Digest))
                    {
                        request.Credentials = CredentialCache.DefaultCredentials;
                    }
                    else if (this.EnabledAuthentications.HasFlag(CommonAuthentication.Windows))
                    {
                        request.Credentials = CredentialCache.DefaultCredentials;
                    }
                    request.GetResponse();
                    success = true;
                    break;
                }
                catch (WebException exception)
                {
                    HttpWebResponse response = (HttpWebResponse)exception.Response;
                    CommonLog.WriteExceptionMessage(exception, "Ping failed, retry in 200 milliseconds");
                    Thread.Sleep(200);
                }
            }

            CommonLog.WriteLine("Ping sucess={0}", success);
        }

        private void ConfigureHttps(Site site)
        {
            string bindingInformation = "*:443:";

            foreach (Binding binding in site.Bindings)
            {
                if (binding.BindingInformation == bindingInformation)
                {
                    return;
                }
            }

            CommonMachine.LocalHost.InstallTrustedRootCertificationAuthority();
            X509Certificate2 certificate = CommonMachine.LocalHost.InstallPersonalCertificate();            

            site.Bindings.Add(bindingInformation, certificate.GetCertHash(), "My");
        }

        private void ConfigureAuthentication(Configuration config)
        {
            string locationPath = this.SiteName + this.VirtualDirectoryPath;

            ConfigurationSection directoryBrowse = config.GetSection("system.webServer/directoryBrowse", locationPath);
            ConfigurationSection anonymousAuthentication = config.GetSection("system.webServer/security/authentication/anonymousAuthentication", locationPath);
            ConfigurationSection basicAuthentication = config.GetSection("system.webServer/security/authentication/basicAuthentication", locationPath);
            ConfigurationSection clientCertificateMappingAuthentication = config.GetSection("system.webServer/security/authentication/clientCertificateMappingAuthentication", locationPath);
            ConfigurationSection digestAuthentication = config.GetSection("system.webServer/security/authentication/digestAuthentication", locationPath);
            ConfigurationSection iisClientCertificateMappingAuthentication = config.GetSection("system.webServer/security/authentication/iisClientCertificateMappingAuthentication", locationPath);
            ConfigurationSection windowsAuthentication = config.GetSection("system.webServer/security/authentication/windowsAuthentication", locationPath);
            ConfigurationSection access = config.GetSection("system.webServer/security/access", locationPath);

            directoryBrowse["enabled"] = true;
            anonymousAuthentication["enabled"] = this.EnabledAuthentications.HasFlag(CommonAuthentication.Anonymous);
            basicAuthentication["enabled"] = this.EnabledAuthentications.HasFlag(CommonAuthentication.Basic);
            clientCertificateMappingAuthentication["enabled"] = this.EnabledAuthentications.HasFlag(CommonAuthentication.ClientCertificate);
            digestAuthentication["enabled"] = this.EnabledAuthentications.HasFlag(CommonAuthentication.Digest);
            windowsAuthentication["enabled"] = this.EnabledAuthentications.HasFlag(CommonAuthentication.Windows);

            if (this.EnabledAuthentications.HasFlag(CommonAuthentication.ClientCertificate))
            {
                access["sslFlags"] = "Ssl, SslNegotiateCert, SslRequireCert";
            }
        }
	}
}
