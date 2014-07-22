using System;
using System.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Text;

namespace DesignerTool.Common.Utils
{
    public class XML
    {
        /// <summary>
        /// Reads the XML object according to the relate string path provided and casts it to object of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"><see cref="class"/> to which the XML file should be cast to.</typeparam>
        /// <param name="filepath">relate string path for the XML file</param>
        /// <returns>instance of the <see cref="class"/> <see cref="T"/></returns>
        public static T DeserializeFile<T>(string filepath) where T : class
        {
            // Reading the XML document requires a FileStream.
            using (Stream reader = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                // Call the Deserialize method to restore the object's state.
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Reads the XML object according to the relate string path provided and casts it to object of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"><see cref="class"/> to which the XML file should be cast to.</typeparam>
        /// <param name="xml">xml content to use for serialization</param>
        /// <returns>instance of the <see cref="class"/> <see cref="T"/></returns>
        public static T Deserialize<T>(string xml) where T : class
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                using (XmlTextReader xmlReader = new XmlTextReader(stringReader))
                {
                    // Call the Deserialize method to restore the object's state.
                    return (T)new XmlSerializer(typeof(T)).Deserialize(xmlReader);
                }
            }
        }

        /// <summary>
        /// Serializes the object passed into XML
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <returns>xml string value</returns>
        public static string Serialize(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Encoding = System.Text.Encoding.ASCII;

                using (XmlWriter tw = XmlWriter.Create(ms, settings))
                {
                    new XmlSerializer(obj.GetType()).Serialize(tw, obj, ns);
                    return new UTF8Encoding().GetString(ms.ToArray());
                }
            }
        }
    }
}
