using System;
using System.Diagnostics;
using System.IO;
using System.Messaging;
using System.Threading;
using Test.WCF.Common;

namespace Test.WCF.Msmq
{
    public class Common
    {
        public static string ClientListenerAddress
        {
            get
            {
                return "http://" + CommonMachine.LocalHost.FullyQualifiedMachineName + "/ClientListener";
            }
        }

        public static void ResetIIS()
        {
            CommonCommandLine cmdLine = new CommonCommandLine();
            cmdLine.FileName = Path.Combine(Environment.SystemDirectory, "iisreset.exe");
            cmdLine.Run();
        }

        public static MessageQueue ClearOrCreateQueue(string messageQueuePath)
        {
            MessageQueue queue = new MessageQueue(messageQueuePath);

            if (MessageQueue.Exists(messageQueuePath))
            {
                queue.Purge();
                MessageQueue journalQueue = new MessageQueue(messageQueuePath + "\\Journal$");
                journalQueue.Purge();
            }
            else
            {
                queue = MessageQueue.Create(messageQueuePath, false);
            }

            // Need to wait for the queue to populate AD entries
            while (!MessageQueue.Exists(messageQueuePath))
            {
                Thread.CurrentThread.Join(TimeSpan.FromSeconds(5));
            }

            return queue;
        }

        public static void CreateQueue(string messageQueuePath, bool transactional = false, bool useJournal = true)
        {
            if (MessageQueue.Exists(messageQueuePath))
            {
                MessageQueue.Delete(messageQueuePath);
            }

            MessageQueue queue = MessageQueue.Create(messageQueuePath, transactional);
            queue.SetPermissions("EVERYONE", MessageQueueAccessRights.FullControl);
            queue.UseJournalQueue = useJournal;
        }
    }
}