using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Utility
{
    class ReportXmlHelper
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Utility.ReportXmlHelper");

        #region RDL Constants

        // RDL.xml tags
        public const string TagReportFile = "reportfile";
        public const string TagRDLName = "name";
        public const string TagRDLFile = "file";
        public const string TagRDLSproc = "sproc";
        public const string TagRDLDescription = "description";
        public const string TagRDLHidden = "hidden";

        #endregion

        #region static and public methods

        public static List<ReportInfo> LoadReportListFromXmlFile(string filename)
        {
            logX.loggerX.Info("Load Reports From XML File");

            List<ReportInfo> list = new List<ReportInfo>();
            try
            {
                FileInfo info = new FileInfo(filename);
                if (!info.Exists)
                {
                    throw new FileNotFoundException(filename);
                }

                string path = info.DirectoryName;
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNodeList xmlList = doc.GetElementsByTagName(TagReportFile);
                foreach (XmlNode node in xmlList)
                {
                    list.Add(XmlNodeToReportInfo(node, path));
                }
                list.Sort();
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error loading reports from xml file", ex);
                throw;
            }

            return list;
        }

        #endregion

        #region Xml Node/Element to Report Parameter

        private static ReportInfo XmlNodeToReportInfo(XmlNode node, string directory)
        {
            ReportInfo info = new ReportInfo();
            if (!node.HasChildNodes)
                return info;

            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.Name)
                {
                    case TagRDLName:
                        info.Name = child.InnerText;
                        break;
                    case TagRDLFile:
                        info.FileName = Path.Combine(directory, child.InnerText);
                        break;
                    case TagRDLSproc:
                        info.StoredProcedure = child.InnerText;
                        break;
                    case TagRDLDescription:
                        info.Description = child.InnerText;
                        break;
                    case TagRDLHidden:
                        info.Hidden = child.InnerText.Equals("true", StringComparison.CurrentCultureIgnoreCase);
                        break;
                }
            }

            return info;
        }

        #endregion

        public static Stream ReplaceDataSetReferences(string filename, Dictionary<string, string> dsrLookup)
        {
            FileInfo info = new FileInfo(filename);
            if (!info.Exists)
            {
                throw new FileNotFoundException(filename);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNamespaceManager xmlns = new XmlNamespaceManager(doc.NameTable);
            string nsReport = "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition";
            string xpath = "/r:Report/r:ReportParameters/r:ReportParameter/r:ValidValues/r:DataSetReference/../..";
            xmlns.AddNamespace("r", nsReport);

            XPathNavigator nav = doc.CreateNavigator();
            XPathNodeIterator iterator = nav.Select(xpath, xmlns);
            while (iterator.MoveNext())
            {
                XPathNavigator currentNode = iterator.Current;
                string name = currentNode.GetAttribute("Name", "");
                if (dsrLookup.ContainsKey(name) &&
                   currentNode.MoveToChild("ValidValues", nsReport) &&
                   currentNode.MoveToChild("DataSetReference", nsReport))
                {
                    currentNode.ReplaceSelf(dsrLookup[name]);
                }
            }
            MemoryStream retVal = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(retVal);
            doc.WriteTo(writer);
            writer.Flush();
            retVal.Seek(0, SeekOrigin.Begin);
            return retVal;
        }

        public static XmlDocument LoadRdlFile(string filename)
        {
            FileInfo info = new FileInfo(filename);
            if (!info.Exists)
            {
                throw new FileNotFoundException(filename);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            return doc;
        }
    }
}
