using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Net;

namespace KBOLib.Util
{
    public class XmlSTI
    {
        private string sport;

        #region [공개 속성들]
        public string Sport
        {
            get { return sport; }
            set { sport = value; }
        }
        #endregion
    }

    /// <summary>
    /// RssReader에 대한 요약 설명입니다.
    /// </summary>
    public class STIXmlReader
    {
        public XmlDocument Document;
        private XmlNode DocumentRoot;

        public STIXmlReader()
        {
            // 개체 생성
            Document = new XmlDocument();
        }

        #region [공개 속성들]
        public XmlSTI Sport
        {
            get { return Sport; }
            set { Sport = value; }
        }
        #endregion

        #region [공개 메서드들]
        public void Load(string filename)
        {
            LoadFromFile(filename);
            PopulateRssData();
        }

        public void LoadFromHttp(string Url)
        {
            LoadFromUrl(Url);
            PopulateRssData();
        }

        #endregion

        private XmlNode getNode(XmlNodeList list, string nodeName)
        {
            for (int i = 0; i <= list.Count - 1; i++)
            {
                if (list.Item(i).Name == nodeName)
                {
                    return list.Item(i);
                }
            }

            return null;
        }

        private void LoadFromFile(string filename)
        {
            Document.Load(filename);
        }

        private void LoadFromUrl(string Url)
        {
            HttpWebRequest request;
            string responseText = "";

            request = (HttpWebRequest)WebRequest.Create(Url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();

            StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding(65001));
            responseText = reader.ReadToEnd();

            response.Close();
            reader.Close();

            Document.LoadXml(responseText);
        }


        private void PopulateRssData()
        {
            XmlNode node;
            XmlNode itemNode;

            //헤더 초기화
            XmlSTI sti_xml = new XmlSTI();

            DocumentRoot = getNode(Document.ChildNodes, "Sports2i");
        }
    }
}