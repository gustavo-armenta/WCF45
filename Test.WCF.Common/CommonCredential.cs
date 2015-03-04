using System.ServiceModel;
namespace Test.WCF.Common
{
    public class CommonCredential
	{
        public class StandardUser
        {
            public static string Domain
            {
                get
                {
                    return null;
                }
            }

            public static string UserName
            {
                get
                {
                    return @"TestUser";
                }
            }

            public static string DomainAndUserName
            {
                get
                {
                    return @"TestUser";
                }
            }

            public static string Password
            {
                get
                {
                    string password = "coMmac!12";
                    return password;
                }
            }
        }
	}
}
