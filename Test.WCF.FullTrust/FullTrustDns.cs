namespace Test.WCF.FullTrust
{
    using System;
    using System.Net;
    using System.Security;
    using System.Security.Permissions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [SecuritySafeCritical]
	public class FullTrustDns
	{
        [PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
        public static string GetFullyQualifiedMachineName()
        {
            return Dns.GetHostEntry(string.Empty).HostName;
        }

        [PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
        public static string GetFullyQualifiedMachineName(string hostNameOrAddress)
        {
            return Dns.GetHostEntry(hostNameOrAddress).HostName;
        }
	}
}
