namespace Test.WCF.UnitTest.WCF
{
    using System;
    using System.Threading;
    using Test.WCF.Common;

    public class DuplexCallback : IDuplexCallback
    {
        public Action<string> CallbackAction;
        private ManualResetEvent CallbackEvent;

        public DuplexCallback()
        {
            this.CallbackAction = delegate(string value)
            {
            };
            this.CallbackEvent = new ManualResetEvent(false);
        }

        public void OneWayToClient(string value)
        {
            CommonLog.WriteLine("DuplexCallback.OneWayToClient({0})", value);
            this.CallbackAction(value);
            this.CallbackEvent.Set();
            CommonLog.WriteLine("DuplexCallback.OneWayToClient end");
        }

        public void WaitForCallback()
        {
            if (!this.CallbackEvent.WaitOne(TimeSpan.FromSeconds(10)))
            {
                throw new TimeoutException();
            }
            this.CallbackEvent.Reset();
        }
    }
}
