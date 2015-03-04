namespace Test.WCF.Common
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    public interface ICommonTraceWriter
    {
        void Listen(string filename);
        void Terminate();
    }

    public class CommonTraceWriter : CommonRemoteTask, ICommonTraceWriter
    {
        private Stream stream;
        private TextWriterTraceListener traceListener;

        public void Listen(string filename)
        {
            stream = File.Create(filename);
            traceListener = new TextWriterTraceListener(stream);
            Trace.Listeners.Add(traceListener);

            CommonLog.WriteLine("Machine={0}", CommonMachine.LocalHost.FullyQualifiedMachineName);
        }

        public void Terminate()
        {
            traceListener.Flush();
            stream.Close();
            Trace.Listeners.Remove(traceListener);
        }
    }
}
