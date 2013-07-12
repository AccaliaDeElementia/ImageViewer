using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ImageViewer
{
    public class Preferences : IXmlSerializable
    {
        private Dictionary<string, string> _mHistory;

        private Dictionary<string, string> History
        {
            get { return _mHistory ?? (_mHistory = new Dictionary<string, string>()); }
        }

        private string _currentImage;

        public string CurrentImage
        {
            get { return _currentImage; }
            set
            {
                if (File.Exists(value))
                {
                    var dir = Path.GetDirectoryName(value);
                    if (dir != null)
                    {
                        History[dir] = value;
                    }
                }
                _currentImage = value;
            }
        }

        public string GetDefaultImage(string directory = null)
        {
            if (directory == null) return CurrentImage;
            if (string.IsNullOrWhiteSpace((directory))) return null;
            if (!History.ContainsKey(directory)) return null;
            return History[directory];
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.LocalName == "History")
                {
                    var dir = reader["Directory"];
                    if (string.IsNullOrWhiteSpace(dir))
                    {
                        throw new FormatException();
                    }
                    History[dir] = reader["Path"];
                }
                else if (reader.LocalName == "CurrentImage")
                {
                    CurrentImage = reader["Value"];
                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (var item in History)
            {
                writer.WriteStartElement("History");
                writer.WriteAttributeString("Directory", item.Key);
                writer.WriteAttributeString("Path", item.Value);
                writer.WriteEndElement();
            }
            writer.WriteStartElement("CurrentImage");
            writer.WriteAttributeString("Value", CurrentImage);
            writer.WriteEndElement();
        }
    }
}
