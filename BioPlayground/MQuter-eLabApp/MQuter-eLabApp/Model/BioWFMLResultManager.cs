using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using MQuter_eLabApp.ViewModel.BioWFMLResultModel;

namespace MQuter_eLabApp.Model
{
    public class BioWFMLResultManager
    {
        private ObservableCollection<string> propertiesName = new ObservableCollection<string>();


        public ResultCollections ReadBioWFML(string xmlContent)
        {
            bool hasRootEle = false;

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlContent)))
            {
                while (reader.Read())
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                if (reader.Name == "BioWFML") { hasRootEle = true; }
                                else if (!hasRootEle) { throw new XmlException("Root element not found / is not well formed."); }
                                else { return ReadResultElement(reader); }

                                break;
                            }
                    }
            }
            throw new Exception("No result set were decipered from the given XML string.");
        }

        private ResultCollections ReadResultElement(XmlReader reader)
        {
            ResultCollections _RCollection = new ResultCollections();
            ResultSet currSet = null;
            
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            switch (reader.Name)
                            {
                                case "Item":
                                    {
                                        currSet = new ResultSet();
                                        break;
                                    }
                                case "Property":
                                    {
                                        string propName = ReadAttrValue("name", reader);
                                        if ((propName != null) && (!propertiesName.Contains(propName)))
                                            propertiesName.Add(propName);

                                        reader.Read();

                                        if (reader.Value != null && reader.Value != "")
                                        {
                                            string propValue = reader.Value;

                                            //Temp solution; tricate the value if its too long
                                            if (propValue.Length > 60)
                                                propValue = propValue.Substring(0, 59);

                                            if (propValue != null)
                                            {
                                                currSet.dataValues.Add(propValue);
                                            }
                                        }
                                        break;
                                    }
                            }
                            break;
                        }

                    case XmlNodeType.EndElement:
                        {
                            switch (reader.Name)
                            {
                                case "Item":
                                    {
                                        _RCollection.resultCollection.Add(currSet);
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }

            foreach (ResultSet r in _RCollection.resultCollection)
            {
                foreach (string value in r.dataValues)
                    System.Diagnostics.Debug.WriteLine("value is! >" + value);
            }

            _RCollection.propertyNames = propertiesName;
            return _RCollection;
        }

        private string ReadAttrValue(string attrName, XmlReader reader)
        {
            reader.MoveToAttribute(attrName);

            return reader.GetAttribute(attrName) == null ? null : reader.Value;
        }

      

    }
}
