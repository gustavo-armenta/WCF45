namespace Test.WCF.UnitTest
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;
    using Test.WCF.FullTrust;

    public class CrossDomainCode : MarshalByRefObject
    {
        public void Execute()
        {
            CommonLog.WriteLine("Hello");
            Assert.IsTrue(true);
        }
    }
}
