using System;
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
            {
                XMLPair[] arr = (XMLPair[])xmlPair.Value;
                foreach (XMLPair pair in arr)
                    xml += ConvertXMLPairToXMLString(pair);
            }
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
                xml += "<" + actualKey + ">";

                if (hashtable[actualKey] is XMLPair)
                    xml += ConvertXMLPairToXMLString((XMLPair)hashtable[actualKey]);
                else if (hashtable[actualKey] is XMLPair[])
                {
                    XMLPair[] arr = (XMLPair[])hashtable[actualKey];
                    foreach (XMLPair pair in arr)
                        xml += ConvertXMLPairToXMLString(pair);
                }
                else if (hashtable[actualKey] is Hashtable)
                    xml += ConvertHashtableToXMLString((Hashtable)hashtable[actualKey]);
                else
                    xml += hashtable[actualKey].ToString();

                xml += "</" + actualKey + ">";
            }

            return xml;
        }
    }
}
