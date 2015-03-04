namespace Test.WCF.Common
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

	public class CommonEtwValidator
	{
        public string Path { get; set; }
        public List<int> ExpectedEvents { get; private set; }
        private Dictionary<int, object> eventsNotFound;

        public CommonEtwValidator()
        {
            this.ExpectedEvents = new List<int>();
        }

        public void ValidateEvents()
        {
            CommonCommandLine tracerpt = new CommonCommandLine();
            tracerpt.FileName = "tracerpt.exe";
            tracerpt.Arguments = string.Format("\"{0}\" -o \"{0}.xml\" -of XML", this.Path);
            tracerpt.Run();

            if (this.ExpectedEvents.Count == 0)
            {
                return;
            }

            this.eventsNotFound = new Dictionary<int, object>();
            foreach (int eventId in this.ExpectedEvents)
            {
                this.eventsNotFound.Add(eventId, null);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(string.Format("{0}.xml", this.Path));
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ev", "http://schemas.microsoft.com/win/2004/08/events/event");

            XmlNodeList eventIds = doc.DocumentElement.SelectNodes("//ev:Event/ev:System/ev:EventID", nsmgr);
            foreach (XmlNode xmlNode in eventIds)
            {
                int value = int.Parse(xmlNode.InnerText);
                eventsNotFound.Remove(value);
                if(eventsNotFound.Count == 0)
                {
                    break;
                }
            }

            if (eventsNotFound.Count > 0)
            {
                StringBuilder keysNotFound = new StringBuilder();
                foreach (int keyNotFound in eventsNotFound.Keys)
                {
                    keysNotFound.Append(keyNotFound + ",");
                }

                throw new Exception(string.Format("ETW events not found: {0}", keysNotFound));
            }

            CommonLog.WriteLine("ETW events validated successfully");
        }
    }
}
