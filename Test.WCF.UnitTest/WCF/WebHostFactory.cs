namespace Test.WCF.UnitTest.WCF
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using Test.WCF.Common;

    public class WebHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            CommonLog.WriteLine("WebHostFactory.CreateServiceHost");
            ServiceHost serviceHost = new ServiceHost(serviceType, baseAddresses);
            return serviceHost;
        }
    }
}
