namespace Test.WCF.FullTrust
{
    using System;
    using System.Security;
    using System.Security.Permissions;

    [SecuritySafeCritical]
	public class FullTrustEnvironment
	{
        [PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
        public static string GetEnvironmentVariable(string value)
        {
            return Environment.GetEnvironmentVariable(value);
        }        
	}
}
