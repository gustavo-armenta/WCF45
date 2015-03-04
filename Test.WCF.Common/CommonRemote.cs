namespace Test.WCF.Common
{
    using System;
    using System.Diagnostics;
    using MS.Test.Fiji;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Security.AccessControl;

    internal class CommonRemoteSingleton : IDisposable
    {
        private Controller controller;
        private MachineClient machineClient;
        private ProcessClient processClient;
        public AppDomainClient appDomainClient;
        public TaskAgentClient taskAgentTraces;

        public string SingletonKey { get; set; }
        public string WorkingDirectory { get; set; }

        internal CommonRemoteSingleton()
        {
        }

        public void CrossMachine()
        {
            CommonLog.WriteLine("CommonRemoteSingleton.CrossMachine begin");

            controller = new Controller();

            MachineInfo machineInfo = new MachineInfo();
            machineInfo.FriendlyName = "ServerBox";
            machineInfo.HostName = CommonMachine.Server.FullyQualifiedMachineName;

            machineClient = controller.AddMachine(machineInfo);

            ProcessInfo processInfo = new ProcessInfo();
            processInfo.FriendlyName = this.SingletonKey;
            processInfo.Credentials.RequestedExecutionLevel = RequestedExecutionLevel.RequiresAdministrator;
            processInfo.HostSettings.WindowStyle = ProcessWindowStyle.Normal;
            processInfo.HostSettings.WorkingDirectory = this.WorkingDirectory;

            processClient = machineClient.AddProcess(processInfo);

            AppDomainInfo appDomainInfo = new AppDomainInfo();
            appDomainInfo.Setup.ConfigurationFile = "CommonCrossMachine.config";
            appDomainInfo.Setup.ApplicationBase = this.WorkingDirectory;

            appDomainClient = processClient.AddAppDomain(appDomainInfo);

            TaskAgentInfo taskAgentInfo = new TaskAgentInfo()
            {
                TypeName = typeof(CommonRemoteTrace).AssemblyQualifiedName
            };

            taskAgentTraces = appDomainClient.AddTaskAgent(taskAgentInfo);
            taskAgentTraces.BuildOut();

            CommonLog.WriteLine("CommonRemoteSingleton.CrossMachine end");
        }

        public void CrossProcess()
        {
            CommonLog.WriteLine("CommonRemoteSingleton.CrossProcess begin");

            controller = new Controller();

            ProcessInfo processInfo = new ProcessInfo();
            processInfo.FriendlyName = this.SingletonKey;
            processInfo.Credentials.RequestedExecutionLevel = RequestedExecutionLevel.RequiresAdministrator;
            processInfo.HostSettings.WindowStyle = ProcessWindowStyle.Normal;

            processClient = controller.AddProcess(processInfo);

            AppDomainInfo appDomainInfo = new AppDomainInfo();
            appDomainInfo.Setup.ConfigurationFile = "CommonCrossProcess.config";

            appDomainClient = processClient.AddAppDomain(appDomainInfo);

            TaskAgentInfo taskAgentInfo = new TaskAgentInfo()
            {
                TypeName = typeof(CommonRemoteTrace).AssemblyQualifiedName
            };

            taskAgentTraces = appDomainClient.AddTaskAgent(taskAgentInfo);
            taskAgentTraces.BuildOut();

            CommonLog.WriteLine("CommonRemoteSingleton.CrossProcess end");
        }

        public void CrossProcessStandardUser()
        {
            CommonLog.WriteLine("CommonRemoteSingleton.CrossProcessStandardUser begin");

            CommonCommandLine cmd = new CommonCommandLine();
            cmd.FileName = "net.exe";
            cmd.Arguments = @"user TestUser coMmac!12 /add";
            cmd.IgnoreExitCodes.Add(2);
            cmd.Run();

            //Give access permissions to BUILTIN\Users
            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
            SecurityIdentifier securityIdentifier = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
            NTAccount ntAccount = (NTAccount)securityIdentifier.Translate(typeof(NTAccount));
            DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();
            directorySecurity.AddAccessRule(new FileSystemAccessRule(ntAccount, FileSystemRights.FullControl, AccessControlType.Allow));
            directorySecurity.AddAccessRule(new FileSystemAccessRule(ntAccount, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
            directorySecurity.AddAccessRule(new FileSystemAccessRule(ntAccount, FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
            directoryInfo.SetAccessControl(directorySecurity);

            controller = new Controller();

            ProcessInfo processInfo = new ProcessInfo();
            processInfo.FriendlyName = this.SingletonKey;
            processInfo.Credentials.Domain = CommonCredential.StandardUser.Domain;
            processInfo.Credentials.UserName = CommonCredential.StandardUser.UserName;
            processInfo.Credentials.Password = CommonCredential.StandardUser.Password;
            processInfo.HostSettings.WindowStyle = ProcessWindowStyle.Normal;

            processClient = controller.AddProcess(processInfo);

            AppDomainInfo appDomainInfo = new AppDomainInfo();
            appDomainInfo.Setup.ConfigurationFile = "CommonCrossProcessStandardUser.config";

            appDomainClient = processClient.AddAppDomain(appDomainInfo);

            TaskAgentInfo taskAgentInfo = new TaskAgentInfo()
            {
                TypeName = typeof(CommonRemoteTrace).AssemblyQualifiedName
            };

            taskAgentTraces = appDomainClient.AddTaskAgent(taskAgentInfo);
            taskAgentTraces.BuildOut();

            CommonLog.WriteLine("CommonRemoteSingleton.CrossProcessStandardUser end");
        }

        ~CommonRemoteSingleton()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            CommonLog.WriteLine("CommonRemoteSingleton.Dispose begin");

            try
            {
                Action action = delegate
                {
                    controller.Teardown();
                };
                IAsyncResult iar = action.BeginInvoke(null, null);
                if (!iar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5)))
                {
                    CommonLog.WriteLine("controller.Teardown() did not complete");
                }
            }
            catch (Exception exception)
            {
                CommonLog.WriteException(exception);
            }

            CommonLog.WriteLine("CommonRemoteSingleton.Dispose end");
        }
    }

	public class CommonRemote : IDisposable
	{        
        public Action DisposeMethod { get; set; }
        public string LogFilePath { get; private set; }

        private Type instanceType;
        private TestContext testContext;
        private CommonRemoteSingleton singleton;
        private ICommonRemoteTrace traces;        
        private TaskAgentClient taskAgentClient;

        internal CommonRemote(Type type, TestContext testContext, CommonRemoteSingleton singleton, string logFilePath)
        {
            this.instanceType = type;
            this.testContext = testContext;
            this.singleton = singleton;
            this.LogFilePath = logFilePath;
            this.DisposeMethod = this.DefaultDisposeMethod;
            
            traces = singleton.taskAgentTraces.CreateTaskChannel<ICommonRemoteTrace>();
            traces.Start(this.LogFilePath);
        }

        public TContract CreateChannel<TContract>()
        {
            CommonLog.WriteLine("CommonRemote.CreateChannel begin");            

            TaskAgentInfo taskAgentInfo = new TaskAgentInfo()
            {
                TypeName = this.instanceType.AssemblyQualifiedName
            };

            taskAgentClient = singleton.appDomainClient.AddTaskAgent(taskAgentInfo);
            taskAgentClient.BuildOut();

            TContract instance = taskAgentClient.CreateTaskChannel<TContract>();

            CommonLog.WriteLine("CommonRemote.CreateChannel end");
            return instance;
        }

        public void Dispose()
        {
            CommonLog.WriteLine("CommonRemote.Dispose begin");

            try
            {
                CommonLog.WriteLine("CommonRemote.DisposeMethod begin");
                this.DisposeMethod();
                CommonLog.WriteLine("CommonRemote.DisposeMethod end");
            }
            catch (Exception exception)
            {
                CommonLog.WriteException(exception);
            }

            try
            {                
                taskAgentClient.Teardown();
            }
            catch (Exception exception)
            {
                CommonLog.WriteException(exception);
            }

            try
            {
                traces.Stop();
                this.testContext.AddResultFile(string.Format("{0}.txt", this.LogFilePath));
            }
            catch (Exception exception)
            {
                CommonLog.WriteException(exception);
            }

            CommonLog.WriteLine("CommonRemote.Dispose end");
        }

        private void DefaultDisposeMethod()
        {
            CommonLog.WriteLine("CommonRemote.DefaultDisposeMethod");
        }
    }
}
