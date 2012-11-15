using System;
using System.Collections;

namespace NeonMika.XML
{
    public class XMLPair
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public Hashtable Attributes { get; set; }

        public XMLPair(string Key, object Value)
        {
            this.Key = Key;
            this.Value = Value;
        }

        public XMLPair(string Key)
        {
            this.Key = Key;
        }

        public override string ToString()
        {
            return XMLConverter.ConvertXMLPairToXMLString(this);
        }        
    }
}
