using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails
{
	public class Processor
	{

        public static System.Xml.XmlDocument ItemTranslation(System.Xml.XmlDocument itemsDocument, bool withCostumes = true)
        {
            var iDoc = new System.Xml.XmlDocument();
            iDoc.LoadXml("<ItemTranslation></ItemTranslation>");

            var scanFunction = new Action<System.Xml.XmlNode>((System.Xml.XmlNode aNode) =>
            {
                int iItemIndex = 0;
                foreach (System.Xml.XmlNode kNode in aNode)
                {
                    if (kNode.NodeType == System.Xml.XmlNodeType.Element && kNode.Name.ToLower() == "entry")
                    {
                        string itemCode = kNode["BASIC_INFO"]["CODE"].InnerText;
                        string itemTranslation = kNode["BASIC_INFO"]["ENGLISH"].InnerText;
                        if (System.Text.RegularExpressions.Regex.IsMatch(itemTranslation, "^[a-zA-Z0-9]{2}_"))
                            itemTranslation = itemTranslation.Substring(3);
                        else if (System.Text.RegularExpressions.Regex.IsMatch(itemTranslation, "^[a-zA-Z0-9]{1}_"))
                            itemTranslation = itemTranslation.Substring(2);
                        var iNode = iDoc.CreateElement("Entry");

                        var iAttribute = iDoc.CreateAttribute("Code");
                        iAttribute.Value = itemCode;
                        iNode.Attributes.Append(iAttribute);

                        iAttribute = iDoc.CreateAttribute("Translation");
                        iAttribute.Value = itemTranslation;
                        iNode.Attributes.Append(iAttribute);

                        iAttribute = iDoc.CreateAttribute("ID");
                        iAttribute.Value = iItemIndex.ToString();
                        iNode.Attributes.Append(iAttribute);

                        iDoc["ItemTranslation"].AppendChild(iNode);
                        iItemIndex++;
                    }
                }
            });

            if(withCostumes)
                scanFunction(itemsDocument["Document"]["ITEM_DATA"]["CHARACTER"]);

            scanFunction(itemsDocument["Document"]["ITEM_DATA"]["WEAPON"]);
            
            return iDoc;
        }

        //System.Reflection.Emit.EnumBuilder iBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new System.Reflection.AssemblyName() { Name = "EnumAsm" }, System.Reflection.Emit.AssemblyBuilderAccess.Run).DefineDynamicModule("EnumModule").DefineEnum("Derp", System.Reflection.TypeAttributes.Public, typeof(System.Int32));
        public static Map.DetailContainer MapDetails(System.Xml.XmlDocument SourceDocument)
        {
            var iList = new List<Map.Details>();
            foreach (System.Xml.XmlNode iNode in SourceDocument["MapDetails"])
            {
                if (iNode.Name == "Map")
                {
                    // basic details
                    int iID = int.Parse(iNode.Attributes["ID"].Value);
                    string iName = iNode.Attributes["Name"].Value;
                    Map.ChannelRestriction iChannelRestriction = new Map.ChannelRestriction(
                        iNode["Head"]["Channel"].Attributes["CQC"].Value.ToLower() == "true",
                        iNode["Head"]["Channel"].Attributes["BG"].Value.ToLower() == "true",
                        iNode["Head"]["Channel"].Attributes["AI"].Value.ToLower() == "true");
                    Base.Enum.Premium iPremium = Base.Enum.Premium.None;
                    Base.Types.ParseEnum<Base.Enum.Premium>(iNode["Head"]["Premium"].Attributes["Type"].Value, out iPremium);
                    bool iIsNew = (iNode["Head"]["Special"].Attributes["New"].Value.ToLower() == "true");
                    bool iIsEvent = (iNode["Head"]["Special"].Attributes["Event"].Value.ToLower() == "true");
                    double iExperience = double.Parse(iNode["Head"]["Special"].Attributes["Experience"].Value, System.Globalization.CultureInfo.InvariantCulture);

                    // allowed game modes
                    var iAllowed = new List<Base.Enum.Game.Mode>();
                    foreach (System.Xml.XmlNode aNode in iNode["Head"]["GameMode"])
                    {
                        if (aNode.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            if (aNode.Name == "Mode")
                            {
                                Base.Enum.Game.Mode iRoomMode = default(Base.Enum.Game.Mode);
                                Base.Types.ParseEnum<Base.Enum.Game.Mode>(aNode.Attributes["Name"].Value, out iRoomMode);
                                if (aNode.Attributes["Allowed"].Value.ToLower() == "true")
                                    iAllowed.Add(iRoomMode);
                            }
                        }
                    }
                    Map.GameModeRestriction iGameModeRestriction = new Map.GameModeRestriction(iAllowed.ToArray());

                    // map objects
                    var iObjects = new List<Map.Object.Base>();
                    foreach (System.Xml.XmlNode bNode in iNode["Object"])
                    {
                        if (bNode.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            int X = int.Parse(bNode.Attributes["X"].Value),
                                Y = int.Parse(bNode.Attributes["Y"].Value),
                                Z = int.Parse(bNode.Attributes["Z"].Value);
                            Map.Object.Base aObject = null;
                            if (bNode.Name.ToLower() == "flag")
                            {
                                Parity.Base.Enum.Team aTeam = Base.Enum.Team.None;
                                Base.Types.ParseEnum<Parity.Base.Enum.Team>(bNode.Attributes["Team"].Value, out aTeam);
                                aObject = new Map.Object.Flag(X, Y, Z, aTeam);
                            }
                            else if (bNode.Name.ToLower() == "entity")
                            {
                                string aCode = bNode.Attributes["Code"].Value;
                                string aTarget = bNode.Attributes["Target"].Value;
                                int aSpawnInterval = int.Parse(bNode.Attributes["SpawnInterval"].Value);
                                aObject = new Map.Object.Entity(X, Y, Z, aCode, aTarget, aSpawnInterval);
                            }
                            if (aObject != null)
                                iObjects.Add(aObject);
                        }
                    }
                    iList.Add(new Map.Details(iID, iName, iChannelRestriction, iGameModeRestriction, iPremium, iExperience, iIsNew, iIsEvent, iObjects.ToArray()));
                }
            }
            return new Map.DetailContainer(iList.ToArray());
        }

	}
}
