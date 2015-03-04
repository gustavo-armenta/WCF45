namespace Test.WCF.Common
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;
    using System.Xml.Serialization;

	public class CommonTest
	{
        public static CommonSettings Settings { get; set; }

        static CommonTest()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CommonSettings));
            using (FileStream stream = File.Open("CommonSettings.xml", FileMode.Open))
            {
                CommonTest.Settings = (CommonSettings)serializer.Deserialize(stream);
            }
        }

        public TestContext TestContext
        {
            get;
            set;
        }
	}
}
