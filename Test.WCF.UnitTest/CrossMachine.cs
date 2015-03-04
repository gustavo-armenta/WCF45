namespace Test.WCF.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Test.WCF.Common;

    [TestClass]
    public class CrossMachine
    {
        //[TestMethod]
        public void CrossMachineSelfHost()
        {
            using (CommonCrossMachine crossMachine = new CommonCrossMachine(typeof(SampleServer)))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.SelfHost_Cleanup;
                server.SelfHost_Setup();

                SampleClient client = new SampleClient();
                client.SelfHost();
            }
        }

        //[TestMethod]
        public void CrossMachineWebHost()
        {
            using (CommonCrossMachine crossMachine = new CommonCrossMachine(typeof(SampleServer)))
            {
                ISampleServer server = crossMachine.CreateChannel<ISampleServer>();
                crossMachine.DisposeMethod = server.WebHost_Cleanup;
                server.WebHost_Setup();

                SampleClient client = new SampleClient();
                client.NetTcp();
            }
        }
    }
}
