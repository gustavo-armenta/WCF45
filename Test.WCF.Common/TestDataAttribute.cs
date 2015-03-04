namespace Test.WCF.Common
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestDataAttribute : Attribute
    {
        public TestDataAttribute(string owner, int priority, int timeout, string description)
        {
            this.Owner = owner;
            this.Priority = priority;
            this.Timeout = timeout;
            this.Description = description;
        }

        public string Description { get; private set; }
        public string Owner { get; private set; }
        public int Priority { get; private set; }
        public int Timeout { get; private set; }
    }
}
