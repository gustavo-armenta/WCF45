namespace Test.WCF.FullTrust
{
    using System;
    using System.Diagnostics;
    using System.Security;
    using System.Security.Permissions;

    [SecuritySafeCritical]
	public class FullTrustDebug : MarshalByRefObject
	{
        [PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
        public static TraceListenerCollection Listeners()
        {
            return Debug.Listeners;
        }

        [PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
        public void ClearListeners()
        {
            Debug.Listeners.Clear();
        }

        [PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
        public void AddListener(TraceListener listener)
        {
            Debug.Listeners.Add(listener);
        }

        [PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
        public static void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }
	}
}
