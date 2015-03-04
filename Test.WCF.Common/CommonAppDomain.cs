namespace Test.WCF.Common
{
    using System;
    using System.Diagnostics;
    using System.Security;
    using System.Security.Policy;
    using Test.WCF.FullTrust;

	public class CommonAppDomain : IDisposable
	{
        private AppDomain appDomain;

        internal CommonAppDomain(PermissionSet permissionSet, string name, string configurationFile)
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup()
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                TargetFrameworkName = ".NETFramework,Version=v4.5"
            };
            if (!string.IsNullOrEmpty(configurationFile))
            {
                appDomainSetup.ConfigurationFile = configurationFile;
            }

            StrongName[] fullTrustAssemblies = new StrongName[]
            {
                typeof(FullTrustAssert).Assembly.Evidence.GetHostEvidence<StrongName>(),
            };

            this.appDomain = AppDomain.CreateDomain(name, null, appDomainSetup, permissionSet, fullTrustAssemblies);

            FullTrustDebug fullTrustDebug = (FullTrustDebug)this.CreateInstance<FullTrustDebug>();
            fullTrustDebug.ClearListeners();
            foreach (TraceListener listener in Debug.Listeners)
            {
                fullTrustDebug.AddListener(listener);
            }
        }

        public T CreateInstance<T>()
        {
            Type type = typeof(T);
            return (T)this.appDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public void Dispose()
        {
            if (this.appDomain != null)
            {
                AppDomain.Unload(this.appDomain);
            }
        }
    }
}
