namespace Test.WCF.Common
{
    using System;
    using MS.Test.Fiji;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.IO;

    public class CommonRemoteFactory
	{
        private static Dictionary<string, CommonRemoteSingleton> singletons;

        static CommonRemoteFactory()
        {
            singletons = new Dictionary<string, CommonRemoteSingleton>();
        }

        public static CommonRemote CreateCrossProcess(Type type, CommonTest commonTest)
        {
            string singletonKey = "CrossProcess";

            if (!singletons.ContainsKey(singletonKey))
            {
                CommonRemoteSingleton item = new CommonRemoteSingleton();
                item.SingletonKey = singletonKey;
                item.WorkingDirectory = Environment.CurrentDirectory;
                item.CrossProcess();
                singletons.Add(singletonKey, item);
            }

            CommonRemoteSingleton singleton = singletons[singletonKey];

            string logFilePath = string.Format(@"{0}", singletonKey);
            CommonRemote remote = new CommonRemote(type, commonTest.TestContext, singleton, logFilePath);

            return remote;
        }

        public static CommonRemote CreateCrossProcessStandardUser(Type type, CommonTest commonTest)
        {
            string singletonKey = "CrossProcessStandardUser";

            if (!singletons.ContainsKey(singletonKey))
            {
                CommonRemoteSingleton item = new CommonRemoteSingleton();
                item.SingletonKey = singletonKey;
                item.WorkingDirectory = Environment.CurrentDirectory;
                item.CrossProcessStandardUser();
                singletons.Add(singletonKey, item);
            }

            CommonRemoteSingleton singleton = singletons[singletonKey];

            string logFilePath = string.Format(@"{0}", singletonKey);
            CommonRemote remote = new CommonRemote(type, commonTest.TestContext, singleton, logFilePath);

            return remote;
        }

        public static CommonRemote CreateCrossMachine(Type type, CommonTest commonTest)
        {
            string singletonKey = "CrossMachine";
            
            if (!singletons.ContainsKey(singletonKey))
            {
                CommonRemoteSingleton item = new CommonRemoteSingleton();
                item.SingletonKey = singletonKey;
                item.WorkingDirectory = CommonMachine.LocalHost.TestPath;
                item.CrossMachine();
                singletons.Add(singletonKey, item);
            }

            CommonRemoteSingleton singleton = singletons[singletonKey];

            string logFilePath = Path.Combine(CommonMachine.Server.TestPath, singletonKey);
            CommonRemote remote = new CommonRemote(type, commonTest.TestContext, singleton, logFilePath);            
            
            return remote;
        }
	}
}
