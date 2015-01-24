using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Parity.Base.App
{
    public class XmlRewriter
    {

        public System.Xml.XmlDocument Document { get; private set; }

        public XmlRewriter()
        {
            this.Document = new System.Xml.XmlDocument();
            this.Document.LoadXml("<Document></Document>");
        }

        protected string EscapeTag(string Input)
        {
            Input = String.Join("", Input.Trim().Split('(', ')')).Replace(' ', '_');
            return char.IsDigit(Input[0]) ? "_" + Input : Input;
        }

        protected void NodeProcessing(string DocumentContent, System.Xml.XmlNode CurrentNode, int TagDeepness = 0)
        {
            int NodeCount = 0;
            if (!Regex.IsMatch(DocumentContent, @"\[(?<NodeName>[a-zA-Z0-9-_\s]+)\](?<NodeContent>.+)\[/\1\]", RegexOptions.Singleline | RegexOptions.Compiled))
            {
                MatchCollection SubNodeCollection = Regex.Matches(DocumentContent, @"<!--\s+?<(?<FirstTag>[a-zA-Z0-9-_\s]+)>(?<NodeContent>.*?)\s+?</(?<LastTag>[a-zA-Z0-9-_\s]+)>\s+?//-->", RegexOptions.Singleline | RegexOptions.Compiled);
                foreach (Match iMatch in SubNodeCollection)
                {
                    string TempDocument = "<" + iMatch.Groups["FirstTag"].Value + ">" + iMatch.Groups["NodeContent"].Value + "</" + iMatch.Groups["LastTag"].Value + ">";
                    var MainNode = this.Document.CreateElement("Entry");
                    CurrentNode.AppendChild(MainNode);
                    NodeProcessing(TempDocument, MainNode);
                }
                NodeCount += SubNodeCollection.Count;
            }

            if (NodeCount == 0)
            {
                MatchCollection MainNodeCollection = Regex.Matches(DocumentContent, @"[\[|<](?<NodeName>[a-zA-Z0-9-_\s]+)[\]|>](?<NodeContent>.*?)[\[|<]/\1[\]|>]", RegexOptions.Singleline | RegexOptions.Compiled);
                foreach (Match iMatch in MainNodeCollection)
                {
                    var MainNode = this.Document.CreateElement(this.EscapeTag(iMatch.Groups["NodeName"].Value));
                    CurrentNode.AppendChild(MainNode);
                    NodeProcessing(iMatch.Groups["NodeContent"].Value, MainNode, TagDeepness + 1);
                }
                NodeCount = MainNodeCollection.Count;
                if (NodeCount == 0)
                {
                    MatchCollection ValueCollection = Regex.Matches(DocumentContent, @"\s*(?<Key>[a-zA-Z0-9_\(\) ]+)\s*=\s*(?<Value>[a-zA-Z0-9\.\,_-]+)", RegexOptions.Singleline | RegexOptions.Compiled);
                    foreach (Match iMatch in ValueCollection)
                    {
                        string Key = iMatch.Groups["Key"].Value;
                        string Value = iMatch.Groups["Value"].Value;

                        var SubentryNode = this.Document.CreateElement(this.EscapeTag(Key));
                        SubentryNode.InnerText = Value;
                        CurrentNode.AppendChild(SubentryNode);

                    }
                }
            }

            //CurrentNode.OwnerDocument.AppendChild(CurrentNode);
        }

        public void Process(string Content)
        {
            uint DocumentChecksum = CRC32.CRC32String(Content);
            var ChecksumElement = this.Document.CreateElement("CHECKSUM");
            ChecksumElement.InnerText = DocumentChecksum.ToString("X2");
            this.Document["Document"].AppendChild(ChecksumElement);

            string Document = Regex.Match(Content, @"<\!--(.+)//-->", System.Text.RegularExpressions.RegexOptions.Singleline | RegexOptions.Compiled).Groups[1].Value;
            NodeProcessing(Document, this.Document["Document"]);
        }

    }
}
