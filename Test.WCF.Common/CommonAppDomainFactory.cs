namespace Test.WCF.Common
{
    using System.Security;
    using System.Security.Permissions;
    using System.Xml;

    public class CommonAppDomainFactory
	{
        public static CommonAppDomain CreateFullTrust(string configurationFile)
        {
            XmlDocument xmlDocument = CommonPartialTrust.PrepareSecurityPolicyFile(CommonPath.WebMediumTrustConfigFile);
            XmlElement permissionSetElement = xmlDocument.SelectSingleNode("//NamedPermissionSets/PermissionSet[@Name='FullTrust']") as XmlElement;
            SecurityElement securityElement = SecurityElement.FromString(permissionSetElement.OuterXml);

            PermissionSet permissionSet = new PermissionSet(PermissionState.None);
            permissionSet.FromXml(securityElement);

            CommonAppDomain appDomain = new CommonAppDomain(permissionSet, "FullTrust", configurationFile);

            return appDomain;
        }

        public static CommonAppDomain CreateWebMediumTrust(string configurationFile)
        {
            XmlDocument xmlDocument = CommonPartialTrust.PrepareSecurityPolicyFile(CommonPath.WebMediumTrustConfigFile);
            XmlElement permissionSetElement = xmlDocument.SelectSingleNode("//NamedPermissionSets/PermissionSet[@Name='ASP.Net']") as XmlElement;
            SecurityElement securityElement = SecurityElement.FromString(permissionSetElement.OuterXml);

            PermissionSet permissionSet = new PermissionSet(PermissionState.None);
            permissionSet.FromXml(securityElement);

            CommonAppDomain appDomain = new CommonAppDomain(permissionSet, "WebMediumTrust", configurationFile);

            return appDomain;
        }

        public static CommonAppDomain CreateWebHighTrust(string configurationFile)
        {
            XmlDocument xmlDocument = CommonPartialTrust.PrepareSecurityPolicyFile(CommonPath.WebHighTrustConfigFile);
            XmlElement permissionSetElement = xmlDocument.SelectSingleNode("//NamedPermissionSets/PermissionSet[@Name='ASP.Net']") as XmlElement;
            SecurityElement securityElement = SecurityElement.FromString(permissionSetElement.OuterXml);

            PermissionSet permissionSet = new PermissionSet(PermissionState.None);
            permissionSet.FromXml(securityElement);

            CommonAppDomain appDomain = new CommonAppDomain(permissionSet, "WebHighTrust", configurationFile);

            return appDomain;
        }
	}
}
