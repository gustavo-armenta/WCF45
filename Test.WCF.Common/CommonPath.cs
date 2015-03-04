namespace Test.WCF.Common
{
    using System;
    using System.IO;
    using Test.WCF.FullTrust;

	public class CommonPath
	{
        private static string ProgramFiles
        {
            get
            {
                return FullTrustEnvironment.GetEnvironmentVariable("ProgramFiles");
            }
        }

        private static string ProgramFilesX86
        {
            get
            {
                string value = FullTrustEnvironment.GetEnvironmentVariable("ProgramFiles(x86)");
                if (String.IsNullOrEmpty(value))
                {
                    value = FullTrustEnvironment.GetEnvironmentVariable("ProgramFiles");
                }
                return value;
            }
        }

        private static string SystemRoot
        {
            get
            {
                return FullTrustEnvironment.GetEnvironmentVariable("SystemRoot");
            }
        }

        public static string Framework
        {
            get
            {
                string path = Path.Combine(CommonPath.SystemRoot, @"Microsoft.NET\Framework64\v4.0.30319");
                if (!Directory.Exists(path))
                {
                    path = Path.Combine(CommonPath.SystemRoot, @"Microsoft.NET\Framework\v4.0.30319");
                }
                return path;
            }
        }

        public static string Sdk
        {
            get
            {
                // Search for SDK directory from newest to oldest.            
                string path = Path.Combine(CommonPath.ProgramFilesX86, @"Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\x64");
                
                if(!Directory.Exists(path))
                {
                    path = Path.Combine(CommonPath.ProgramFilesX86, @"Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools");
                }
                if (!Directory.Exists(path))
                {
                    path = Path.Combine(CommonPath.ProgramFilesX86, @"Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\x64");
                }
                if (!Directory.Exists(path))
                {
                    path = Path.Combine(CommonPath.ProgramFilesX86, @"Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools");
                }
                    

                return path;
            }
        }

        public static string WebMediumTrustConfigFile
        {
            get
            {
                string path = Path.Combine(CommonPath.Framework, @"Config\web_mediumtrust.config");
                return path;
            }
        }

        public static string WebHighTrustConfigFile
        {
            get
            {
                string path = Path.Combine(CommonPath.Framework, @"Config\web_hightrust.config");
                return path;
            }
        }

        public static string GacUtilExe
        {
            get
            {
                string path = Path.Combine(CommonPath.Sdk, "GacUtil.exe");
                return path;
            }
        }

        public static string SvcUtilExe
        {
            get
            {
                string path = Path.Combine(CommonPath.Sdk, "SvcUtil.exe");
                return path;
            }
        }
        
        public static string CscExe
        {
            get
            {
                string path = Path.Combine(CommonPath.Framework, "csc.exe");
                return path;
            }
        }

        public static string MakeCertExe
        {
            get
            {
                string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Windows Kits");
                string[] makeCertPath = Directory.GetFiles(basePath, "makecert.exe", SearchOption.AllDirectories);
                return makeCertPath[0];
            }
        }
	}
}
