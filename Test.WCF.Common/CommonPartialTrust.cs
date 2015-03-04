namespace Test.WCF.Common
{
    using System;
    using System.IO;
    using System.Xml;

	public class CommonPartialTrust
	{
        public static XmlDocument PrepareSecurityPolicyFile(string fileName)
        {
            FileInfo permissionSetFileInfo = new FileInfo(fileName);

            XmlDocument permissionSetXmlDocument = new XmlDocument();
            using (StreamReader sr = permissionSetFileInfo.OpenText())
            {
                string permissionsXml = sr.ReadToEnd();
                permissionsXml = permissionsXml.Replace("$AppDir$", AppDomain.CurrentDomain.BaseDirectory);
                permissionSetXmlDocument.LoadXml(permissionsXml);
            }

            XmlNodeList securityClasses = permissionSetXmlDocument.SelectNodes("/configuration/mscorlib/security/policy/PolicyLevel/SecurityClasses/SecurityClass");
            if (securityClasses == null || securityClasses.Count == 0)
            {
                throw new ArgumentNullException("securityClasses");
            }

            foreach (XmlElement securityClassElement in securityClasses)
            {
                if (securityClassElement.Attributes["Name"] != null &&
                    securityClassElement.Attributes["Description"] != null)
                {
                    string name = securityClassElement.Attributes["Name"].Value;
                    string typeName = securityClassElement.Attributes["Description"].Value;

                    XmlNodeList permissionNodes = permissionSetXmlDocument.SelectNodes(string.Format("//IPermission[@class='{0}']", name));

                    if (permissionNodes != null && permissionNodes.Count > 0)
                    {
                        foreach (XmlElement permissionElement in permissionNodes)
                        {
                            permissionElement.Attributes["class"].Value = typeName;
                        }
                    }
                }
            }

            return permissionSetXmlDocument;
        }
    }
}
