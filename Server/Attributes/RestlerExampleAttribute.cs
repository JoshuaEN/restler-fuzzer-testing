using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class RestlerExampleAttribute : Attribute
    {
        private readonly string data;
        private readonly string title;

        public RestlerExampleAttribute(string title, string exampleGeneratorName)
        {
            this.title = title;
            data = exampleGeneratorName;
        }

        public string Title
        {
            get => title;
        }

        public string ExampleGeneratorName
        {
            get => data;
        }
    }
}
