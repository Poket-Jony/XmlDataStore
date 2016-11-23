using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace XmlDataStore
{
    public class XmlDataStore
    {
        private string path;
        private string context;

        public XmlDataStore(string path, string context)
        {
            this.path = path;
            this.context = context;
        }

        public object GetObject(string property)
        {
            string value = GetString(property);
            return !string.IsNullOrEmpty(value) ? StringSerializer<object>.Deserialize(value) : null;
        }

        public bool GetBool(string property)
        {
            string value = GetString(property);
            return !string.IsNullOrEmpty(value) ? Convert.ToBoolean(GetString(property)) : false;
        }

        public double GetDouble(string property)
        {
            string value = GetString(property);
            return !string.IsNullOrEmpty(value) ? Convert.ToDouble(GetString(property)) : 0;
        }

        public int GetInt(string property)
        {
            string value = GetString(property);
            return !string.IsNullOrEmpty(value) ? Convert.ToInt32(GetString(property)) : 0;
        }

        public string GetString(string property)
        {
            XDocument xmlDoc;
            if (File.Exists(path))
                xmlDoc = XDocument.Load(path);
            else
                return null;
            if (xmlDoc.Element(context).Element(property) != null)
            {
                XElement element = xmlDoc.Element(context).Element(property);
                return element.Value;
            }
            return null;
        }

        public void SetObject(string property, object value)
        {
            SetString(property, StringSerializer<object>.Serialize(value));
        }

        public void SetBool(string property, bool value)
        {
            SetString(property, value.ToString().ToLower());
        }

        public void SetDouble(string property, double value)
        {
            SetString(property, value.ToString());
        }

        public void SetInt(string property, int value)
        {
            SetString(property, value.ToString());
        }

        public void SetString(string property, string value)
        {
            XDocument xmlDoc;
            if (File.Exists(path))
            {
                xmlDoc = XDocument.Load(path);
                if (xmlDoc.Element(context).Element(property) != null)
                {
                    XElement element = xmlDoc.Element(context).Element(property);
                    element.SetValue(value);
                }
                else
                    xmlDoc.Element(context).Add(new XElement(property, value));
            }
            else
                xmlDoc = new XDocument(new XElement(context, new XElement(property, value)));
            xmlDoc.Save(path);
        }
    }
}
