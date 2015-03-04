using System.IO;
using System.ServiceModel;
using Test.WCF.Common;

namespace Test.WCF.Msmq
{
    class ServiceDeployer
    {
        // The assumption here is that "name" is the same for both the virtual directory and physical directory.
        internal void GenericDeployWebhostedService(string name)
        {
            CommonWebApplication webApp = new CommonWebApplication();
            webApp.VirtualDirectoryPath = "/" + name;
            webApp.VirtualDirectoryPhysicalPath = Path.Combine(Path.GetFullPath("Test.WCF.Msmq"), @"wwwroot\" + name);
            webApp.EnabledProtocols = "http,net.msmq";
            webApp.BinFiles.Add(this.GetType().Assembly.Location);
            webApp.Deploy();
        }
    }
}