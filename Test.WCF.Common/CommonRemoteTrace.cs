namespace Test.WCF.Common
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    public interface ICommonRemoteTrace
    {
        void Start(string filenameWithoutExtension);
        void Stop();
    }

    public class CommonRemoteTrace : CommonRemoteTask, ICommonRemoteTrace
    {
        private Stream stream;
        private TextWriterTraceListener traceListener;

        public void Start(string filenameWithoutExtension)
        {
            stream = File.Create(string.Format("{0}.txt", filenameWithoutExtension));
            traceListener = new TextWriterTraceListener(stream);
            Trace.Listeners.Add(traceListener);

            CommonLog.WriteLine("Machine={0}", CommonMachine.LocalHost.FullyQualifiedMachineName);
            CommonLog.WriteLine("Thread.CurrentPrincipal.Identity.Name={0}", Thread.CurrentPrincipal.Identity.Name);
        }

        public void Stop()
        {
            traceListener.Flush();
            stream.Close();
            Trace.Listeners.Remove(traceListener);
        }
    }
}
