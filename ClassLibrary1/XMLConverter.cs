using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NeonMika.XML
{
    public static class XMLConverter
    {
        public static string ConvertXMLPairToXMLString(XMLPair xmlPair)
        {
            string xml = "<" + xmlPair.Key + ">";

            if (xmlPair.Value is XMLPair[])
                foreach (XMLPair pair in (XMLPair[])xmlPair.Value)
                    xml += ConvertXMLPairToXMLString((XMLPair)pair.Value);
            else if (xmlPair.Value is XMLPair)
                xml += ConvertXMLPairToXMLString((XMLPair)xmlPair.Value);
            else if (xmlPair.Value is Hashtable)
                xml += ConvertHashtableToXMLString((Hashtable)xmlPair.Value);
            else
                xml += xmlPair.Value.ToString();

            xml += "</" + xmlPair.Key + ">";

            return xml;

        }

        public static string ConvertHashtableToXMLString(Hashtable hashtable)
        {
            string xml = "";

            foreach (var actualKey in hashtable.Keys)
            {
                if (hashtable[actualKey] is XMLPair)
                    xml += ConvertXMLPairToXMLString((XMLPair)hashtable[actualKey]);
                else if (hashtable[actualKey] is XMLPair[])
                {
                    xml += "<" + actualKey + ">";
                    foreach (XMLPair pair in (XMLPair[])hashtable[actualKey])
                        xml += ConvertXMLPairToXMLString(pair);
                    xml += "</" + actualKey + ">";
                }
                else if (hashtable[actualKey] is Hashtable)
                    xml += "<" + actualKey + ">" + ConvertHashtableToXMLString((Hashtable)hashtable[actualKey]) + "</" + actualKey + ">";
                else
                    xml += "<" + actualKey + ">" + hashtable[actualKey].ToString() + "</" + actualKey + ">";
            }

            return xml;
        }
    }
}
