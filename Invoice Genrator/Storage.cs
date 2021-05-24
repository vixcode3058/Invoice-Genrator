using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoice_Genrator
{
    class Storage
    {
        public static void WriteXML<T>(T CompanyProfile, string v)
        {
            FileStream stream;

            XmlSerializer xmlSer = new XmlSerializer(typeof(T));
            stream = new FileStream(v, FileMode.Create);
            xmlSer.Serialize(stream, CompanyProfile);
            stream.Close();
        }


        public static T ReadXml<T>(string file)
        {
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    return (T)xmlSerializer.Deserialize(sr);

                }
            }
            catch (Exception)
            {

                return default(T);
            }

        }

        internal static void WriteXML<T>(ObservableCollection<T> invoiceList, string v)
        {

            FileStream stream;

            XmlSerializer xmlSer = new XmlSerializer(typeof(T));
            stream = new FileStream(v, FileMode.Create);
            xmlSer.Serialize(stream, invoiceList);
            stream.Close();
        }
    }
}
