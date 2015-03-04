namespace Test.WCF.UnitTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;

    [TestClass]
    public class CrossDomain
    {
        [TestMethod]
        public void CrossDomainWebMediumTrust()
        {
            using (CommonAppDomain ad = new CommonAppDomain())
            {
                ad.CreateWebMediumTrust();
                CrossDomainCode remote = (CrossDomainCode)ad.CreateInstance(typeof(CrossDomainCode));

                try
                {
                    remote.Execute();
                }
                catch (Exception e)
                {
                    CommonLog.WriteLine(e.ToString());
                }
            }
        }
    }
}
