namespace Test.WCF.Common
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    public interface ICommonRemoteEtw
    {
        string FilenameWithoutExtension { get; set; }
        string Path { get; set; }
        void Start();
        void Stop();
    }

    public class CommonRemoteEtw : CommonRemoteTask, ICommonRemoteEtw
	{
        public string FilenameWithoutExtension { get; set; }
        public string Path { get; set; }

        public void Start()
        {
            CommonCommandLine logman = new CommonCommandLine();
            logman.FileName = "logman.exe";
            logman.Arguments = string.Format("create trace {0} -ow -o {0}.etl -p {{c651f5f6-1c0d-492e-8ae1-b4efd7c9d503}} -ets", this.FilenameWithoutExtension);
            logman.Run();
        }

        public void Stop()
        {
            CommonCommandLine logman = new CommonCommandLine();
            logman.FileName = "logman.exe";
            logman.Arguments = string.Format("stop -ets {0}", this.FilenameWithoutExtension);
            logman.Run();
        }        
    }

    public class CommonEtw : IDisposable
    {
        public CommonTest CommonTest { get; set; }
        public CommonRemote CommonRemote { get; set; }
        public ICommonRemoteEtw CommonRemoteEtw { get; set; }

        public void Dispose()
        {
            CommonRemoteEtw.Stop();
            string path = this.CommonRemoteEtw.Path;
            if (this.CommonRemote != null)
            {
                this.CommonRemote.Dispose();
            }
            CommonTest.TestContext.AddResultFile(path);
        }
    }

    public class CommonEtwFactory
    {
        public static CommonEtw CreateCrossMachine(CommonTest test)
        {
            CommonRemote crossMachine = CommonRemoteFactory.CreateCrossMachine(typeof(CommonRemoteEtw), test);
            ICommonRemoteEtw remoteEtw = crossMachine.CreateChannel<ICommonRemoteEtw>();
            crossMachine.DisposeMethod = remoteEtw.Stop;
            remoteEtw.FilenameWithoutExtension = "RemoteMachine";
            remoteEtw.Path = Path.Combine(CommonMachine.Server.TestPath, "RemoteMachine.etl");
            remoteEtw.Start();

            CommonEtw etw = new CommonEtw();
            etw.CommonTest = test;
            etw.CommonRemote = crossMachine;
            etw.CommonRemoteEtw = remoteEtw;

            return etw;            
        }

        public static CommonEtw CreateLocalMachine(CommonTest test)
        {
            CommonRemoteEtw localEtw = new CommonRemoteEtw();
            localEtw.FilenameWithoutExtension = "LocalMachine";
            localEtw.Path = "LocalMachine.etl";
            localEtw.Start();

            CommonEtw etw = new CommonEtw();
            etw.CommonTest = test;
            etw.CommonRemoteEtw = localEtw;
            
            return etw;
        }
    }
}
