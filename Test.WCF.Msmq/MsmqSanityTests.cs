using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Messaging;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.MsmqIntegration;
using System.Threading;
using System.Transactions;
using Test.WCF.Msmq.Services;
using Test.WCF.Common;

namespace Test.WCF.Msmq
{
    [TestClass]
    [TestData("jamesosb", 1, 600, "WCF MSMQ tests.")]
    public class MsmqSanityTests
    {
        static ManualResetEvent signalFromServer;
        static bool resultFromService;
        ServiceHost clientListener;

        [TestInitialize]
        public void InitializeTest()
        {
            resultFromService = false;
            signalFromServer = new ManualResetEvent(false);
            DeployClientListener();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.clientListener.Close();
        }

        [TestMethod]
        public void MsmqHelloWorld()
        {
            Common.CreateQueue(@".\Private$\helloworldservice");

            NetMsmqBinding binding = new NetMsmqBinding(NetMsmqSecurityMode.None);
            binding.ExactlyOnce = false;

            string address = "net.msmq://localhost/private/helloworldservice";

            ChannelFactory<IHelloWorld> factory = new ChannelFactory<IHelloWorld>(binding, address);
            IHelloWorld client = factory.CreateChannel();

            client.HelloWorld("Hello Service!");

            ServiceDeployer deployer = new ServiceDeployer();
            deployer.GenericDeployWebhostedService("HelloWorldService");

            // Wait for response from service.
            if (!signalFromServer.WaitOne(TimeSpan.FromSeconds(60)))
            {
                Common.ResetIIS();
            }

            if (!signalFromServer.WaitOne(TimeSpan.FromSeconds(60)))
            {
                CommonLog.WriteLine("No response from service.");
            }

            if (!resultFromService)
            {
                throw new ApplicationException("Test failed. See log for details.");
            }
        }

        [TestMethod]
        public void MsmqIntegrationJournalMessages()
        {
            // Clear the system journal.
            MessageQueue systemJournal = new MessageQueue(".\\Journal$");
            systemJournal.Purge();

            Common.CreateQueue(@".\Private$\msmqintegrationservice", false, false);

            MsmqIntegrationBinding binding = new MsmqIntegrationBinding(MsmqIntegrationSecurityMode.None);
            binding.ExactlyOnce = false;
            binding.UseSourceJournal = true;
            
            string address = @"msmq.formatname:DIRECT=OS:.\private$\msmqintegrationservice";
            
            ChannelFactory<IMsmqIntegrationService> factory = new ChannelFactory<IMsmqIntegrationService>(binding, address);
            IMsmqIntegrationService client = factory.CreateChannel();

            client.SubmitString(new MsmqMessage<string>("Hello Service!"));

            ServiceDeployer deployer = new ServiceDeployer();
            deployer.GenericDeployWebhostedService("MsmqIntegrationService");

            // Wait for response from service.
            if (!signalFromServer.WaitOne(TimeSpan.FromSeconds(60)))
            {
                Common.ResetIIS();
            }

            if (!signalFromServer.WaitOne(TimeSpan.FromSeconds(60)))
            {
                CommonLog.WriteLine("No response from service.");
            }

            // Check the System Journal Queue for the message - Receive will throw after timeout if no message is found.
            systemJournal.Receive(TimeSpan.FromSeconds(30));

            if (!resultFromService)
            {
                throw new ApplicationException("Test failed. See log for details.");
            }
        }

        // Note - this test tends to throw during debugging, more details here:
        // http://social.msdn.microsoft.com/Forums/pl/wcf/thread/ed5371c9-b25c-4632-bdc1-8ef855089ce6
        [TestMethod]
        public void ReadFromDeadLetterQueueSelfHost()
        {
            MessageQueue dlq = new MessageQueue(@".\DEADLETTER$");
            dlq.Purge();

            string messageQueuePath = @".\Private$\helloworldservice";
            Common.CreateQueue(messageQueuePath);

            NetMsmqBinding binding = new NetMsmqBinding(NetMsmqSecurityMode.None);
            binding.ExactlyOnce = false;
            
            string address = "net.msmq://localhost/private/helloworldservice";
            
            ChannelFactory<IHelloWorld> factory = new ChannelFactory<IHelloWorld>(binding, address);
            IHelloWorld client = factory.CreateChannel();

            client.HelloWorld("Hello Service!");

            // This will cause the message sent above to go to the system DLQ.
            MessageQueue.Delete(messageQueuePath);

            string deadLetterQueueAddress = "net.msmq://localhost/SYSTEM$;DEADLETTER";
            binding.DeadLetterQueue = DeadLetterQueue.System;

            using (ServiceHost host = new ServiceHost(typeof(HelloWorldService)))
            {
                host.AddServiceEndpoint(typeof(IHelloWorld), binding, deadLetterQueueAddress);

                ServiceBehaviorAttribute behavior = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behavior.AddressFilterMode = AddressFilterMode.Any;

                host.Open();
                if (!signalFromServer.WaitOne(TimeSpan.FromSeconds(60)))
                {
                    CommonLog.WriteLine("No response from service.");
                }
            }

            if (!resultFromService)
            {
                throw new ApplicationException("Test failed. See log for details.");
            }
        }

        [TestMethod]
        public void BatchingSelfhost()
        {
            Common.CreateQueue(@".\Private$\batchingservice", true);

            string address = "net.msmq://localhost/private/batchingservice";
            NetMsmqBinding binding = new NetMsmqBinding(NetMsmqSecurityMode.None);
            binding.ExactlyOnce = true;

            ChannelFactory<IHelloWorld> factory = new ChannelFactory<IHelloWorld>(binding, address);
            IHelloWorld client = factory.CreateChannel();

            for (int i = 0; i < 100; ++i)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    for (int j = 0; j < 10; ++j)
                    {
                        client.HelloWorld(string.Format("Order Number {0}.{1}", i, j));
                    }

                    scope.Complete();
                }
            }

            using (ServiceHost host = new ServiceHost(typeof(BatchingService)))
            {
                host.AddServiceEndpoint(typeof(IHelloWorld), binding, address);
                TransactedBatchingBehavior batchingBehavior = new TransactedBatchingBehavior(100);
                host.Description.Endpoints.Find(typeof(IHelloWorld)).EndpointBehaviors.Add(batchingBehavior);
                host.Open();

                if (!signalFromServer.WaitOne(TimeSpan.FromSeconds(60)))
                {
                    CommonLog.WriteLine("No response from service.");
                }
            }

            if (!resultFromService)
            {
                throw new ApplicationException("Test failed. See log for details.");
            }
        }

        [TestMethod]
        public void PoisonMessageHandlingSelfhost()
        {
            string messageQueuePath = @".\Private$\helloworldservice";
            Common.CreateQueue(messageQueuePath, true);

            NetMsmqBinding clientBinding = new NetMsmqBinding(NetMsmqSecurityMode.None);
            clientBinding.ExactlyOnce = true;
            
            string address = "net.msmq://localhost/private/helloworldservice";
            
            ChannelFactory<IHelloWorld> factory = new ChannelFactory<IHelloWorld>(clientBinding, address);
            IHelloWorld client = factory.CreateChannel();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                client.HelloWorld("Hello Service!");
                scope.Complete();
            }

            NetMsmqBinding poisonBinding = new NetMsmqBinding(NetMsmqSecurityMode.None);
            poisonBinding.ExactlyOnce = true;
            poisonBinding.ReceiveRetryCount = 3;
            poisonBinding.MaxRetryCycles = 5;
            poisonBinding.RetryCycleDelay = TimeSpan.FromSeconds(1);
            poisonBinding.ReceiveErrorHandling = ReceiveErrorHandling.Move;

            using (ServiceHost host = new ServiceHost(typeof(PoisonMessageService)))
            {
                host.AddServiceEndpoint(typeof(IHelloWorld), poisonBinding, address);
                host.Open();

                signalFromServer.WaitOne(TimeSpan.FromMinutes(1));

                // Check for the poison message to appear in the poison subqueue
                MessageQueue poisonQueue = new MessageQueue(messageQueuePath + ";poison");
                if (poisonQueue.Peek(TimeSpan.FromMinutes(1)) == null)
                {
                    throw new ApplicationException("Poison message never appeared in the queue.");
                }

                signalFromServer.Reset();
                resultFromService = false;
            }

            // Need to change the poison message handling, since we are now going to read from the poison subqueue
            NetMsmqBinding poisonHandlingBinding = new NetMsmqBinding(NetMsmqSecurityMode.None);
            poisonHandlingBinding.ExactlyOnce = true;
            poisonHandlingBinding.ReceiveErrorHandling = ReceiveErrorHandling.Reject;

            using (ServiceHost poisonRecoveryHost = new ServiceHost(typeof(PoisonRecoveryService)))
            {
                poisonRecoveryHost.AddServiceEndpoint(typeof(IHelloWorld), poisonHandlingBinding, address + ";poison");
                poisonRecoveryHost.Open();

                // Read from the poison subqueue
                signalFromServer.WaitOne(TimeSpan.FromMinutes(1));
            }

            if (!resultFromService)
            {
                throw new ApplicationException("Test failed. See log for details.");
            }
        }

        // Public queues have the inherit problem that after creating one, it takes a very long time 
        // for it to populate in AD.  So, all scenarios that use a public queue will use the same queue.
        [TestMethod]
        public void MsmqHelloWorldPublicQueue()
        {
            string messageQueuePath = @".\helloworldservicepublic";
            string journalQueuePath = messageQueuePath + "\\Journal$";

            MessageQueue queue = null;
            MessageQueue journalQueue = new MessageQueue(journalQueuePath);

            queue = Common.ClearOrCreateQueue(messageQueuePath);
            queue.SetPermissions("EVERYONE", MessageQueueAccessRights.FullControl);
            queue.UseJournalQueue = true;
            queue.EncryptionRequired = EncryptionRequired.None;

            NetMsmqBinding binding = new NetMsmqBinding(NetMsmqSecurityMode.None);
            binding.ExactlyOnce = false;

            string address = "net.msmq://localhost/helloworldservicepublic";

            ChannelFactory<IHelloWorld> factory = new ChannelFactory<IHelloWorld>(binding, address);
            IHelloWorld client = factory.CreateChannel();

            client.HelloWorld("Hello Service!");

            ServiceDeployer deployer = new ServiceDeployer();
            deployer.GenericDeployWebhostedService("HelloWorldServicePublic");

            // Wait for response from service.
            if (!signalFromServer.WaitOne(TimeSpan.FromSeconds(60)))
            {
                Common.ResetIIS();
                HttpClient pingClient = new HttpClient();
                HttpResponseMessage pingResponse = pingClient.GetAsync(address.Replace("net.msmq", "http") + "/HelloWorldServicePublic.svc").Result;
                if (pingResponse.StatusCode != HttpStatusCode.OK)
                {
                    CommonLog.WriteLine("Status from service was not OK. Http content is:");
                    CommonLog.WriteLine(pingResponse.Content.ReadAsStringAsync().Result);
                }
            }

            if (!signalFromServer.WaitOne(TimeSpan.FromSeconds(60)))
            {
                CommonLog.WriteLine("No response from service.");
            }

            // Verify Journaling
            journalQueue.Receive(TimeSpan.FromSeconds(5));

            if (!resultFromService)
            {
                throw new ApplicationException("Test failed. See log for details.");
            }
        }

        // Public queues have the inherit problem that after creating one, it takes a very long time 
        // for it to populate in AD.  So, all scenarios that use a public queue will use the same queue.
        [TestMethod]
        public void MsmqEncryptAndSign()
        {
            string messageQueuePath = @".\helloworldservicepublic";

            MessageQueue queue = null;

            queue = Common.ClearOrCreateQueue(messageQueuePath);

            queue.EncryptionRequired = EncryptionRequired.Body;
            queue.UseJournalQueue = true;
            queue.SetPermissions("EVERYONE", MessageQueueAccessRights.FullControl);

            NetMsmqBinding binding = new NetMsmqBinding(NetMsmqSecurityMode.Transport);
            binding.ExactlyOnce = false;
            binding.UseActiveDirectory = true;
            binding.Security.Transport.MsmqProtectionLevel = ProtectionLevel.EncryptAndSign;
            binding.Security.Transport.MsmqEncryptionAlgorithm = MsmqEncryptionAlgorithm.Aes;

            string address = "net.msmq://localhost/helloworldservicepublic";
            ChannelFactory<IHelloWorld> factory = new ChannelFactory<IHelloWorld>(binding, address);
            IHelloWorld client = factory.CreateChannel();

            client.HelloWorld("Hello Service!");

            ServiceDeployer deployer = new ServiceDeployer();
            deployer.GenericDeployWebhostedService("EncryptAndSign");

            // Wait for response from service.
            if (!signalFromServer.WaitOne(TimeSpan.FromSeconds(60)))
            {
                CommonLog.WriteLine("No response from service.");
            }

            if (!resultFromService)
            {
                throw new ApplicationException("Test failed. See log for details.");
            }
        }

        void DeployClientListener()
        {
            this.clientListener = new ServiceHost(typeof(ClientListener));
            this.clientListener.AddServiceEndpoint(typeof(IClientListener), new BasicHttpBinding(), Common.ClientListenerAddress);
            this.clientListener.Open();
        }

        class ClientListener : IClientListener
        {
            public void PassTest()
            {
                resultFromService = true;
                signalFromServer.Set();
            }

            public void FailTest(string failureReason)
            {
                resultFromService = false;
                CommonLog.WriteLine(failureReason);
                signalFromServer.Set();
            }
        }
    }
}