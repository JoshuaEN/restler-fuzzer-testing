using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ResterAnnotationAttribute : Attribute
    {
        private readonly Annotation data;

        public ResterAnnotationAttribute(string producerEndpoint = "", string producerMethod = "", string producerResourceName = "", string consumerParam = "")
        {
            data = new Annotation(producerEndpoint, producerMethod, producerResourceName, consumerParam);
        }

        public ResterAnnotationAttribute(Annotation annotation)
        {
            data = annotation;
        }

        public Annotation Data
        {
            get => data;
        }

        public struct Annotation
        {
            public readonly string ProducerEndpoint;
            public readonly string ProducerMethod;
            public readonly string ProducerResourceName;
            public readonly string ConsumerParam;

            public Annotation(string producerEndpoint = "", string producerMethod = "", string producerResourceName = "", string consumerParam = "")
            {
                if (producerEndpoint == null)
                {
                    throw new ArgumentNullException(nameof(producerEndpoint));
                }
                if (producerMethod == null)
                {
                    throw new ArgumentNullException(nameof(producerMethod));
                }
                if (producerResourceName == null)
                {
                    throw new ArgumentNullException(nameof(producerResourceName));
                }
                if (consumerParam == null)
                {
                    throw new ArgumentNullException(nameof(consumerParam));
                }

                ProducerEndpoint = producerEndpoint;
                ProducerMethod = producerMethod;
                ProducerResourceName = producerResourceName;
                ConsumerParam = consumerParam;
            }
        }
    }
}
