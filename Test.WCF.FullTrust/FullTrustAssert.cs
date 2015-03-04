namespace Test.WCF.FullTrust
{
    using System.Security;
    using System.Security.Permissions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [SecuritySafeCritical]
	public class FullTrustAssert
	{
        [PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
        public static void IsTrue(bool condition)
        {
            Assert.IsTrue(condition);
        }

        [PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
        public static void IsNotNull(object value)
        {
            Assert.IsNotNull(value);
        }

        [PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
        public static void AreEqual(object expected, object actual)
        {
            Assert.AreEqual(expected, actual);
        }
	}
}
