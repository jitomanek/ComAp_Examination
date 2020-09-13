using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ExaminationLib.Helpers
{
    public abstract class MessageBase<T>
    {
        [JsonIgnore]
        [XmlIgnore]
        public bool Success { get; set; }
        [JsonProperty(PropertyName = "InputLine")]
        [XmlAttribute(AttributeName = "InputLine")]
        public T Content { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public Exception Exception { get; set; }
    }
}
