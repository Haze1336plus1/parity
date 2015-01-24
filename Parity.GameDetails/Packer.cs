using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails
{
    public class Packer
    {

        protected static Dictionary<Base.Enum.Game.Mode, string> GameModeTranslator { get; private set; }
        protected static Dictionary<string, Base.Enum.Premium> PremiumTranslator { get; private set; }

        public System.Xml.XmlDocument Document { get; private set; }
        protected Dictionary<int, string> MapList { get; private set; }

        static Packer()
        {
            Packer.GameModeTranslator = new Dictionary<Base.Enum.Game.Mode, string>();
            Packer.GameModeTranslator.Add(Base.Enum.Game.Mode.Explosive, "Explosive");
            Packer.GameModeTranslator.Add(Base.Enum.Game.Mode.FreeForAll, "FFA");
            Packer.GameModeTranslator.Add(Base.Enum.Game.Mode.FourVsFour, "DeathMatch"); // requires CQC
            Packer.GameModeTranslator.Add(Base.Enum.Game.Mode.Deathmatch, "DeathMatch|DeathMatchLarge");
            Packer.GameModeTranslator.Add(Base.Enum.Game.Mode.Conquest, "Conquest");
            Packer.GameModeTranslator.Add(Base.Enum.Game.Mode.ExplosiveBG, "LargeMission");
            Packer.GameModeTranslator.Add(Base.Enum.Game.Mode.HeroMode, "SmallHero");
            Packer.GameModeTranslator.Add(Base.Enum.Game.Mode.TotalWar, "TotalWar");
            Packer.GameModeTranslator.Add(Base.Enum.Game.Mode.Survive, "Survival");
            Packer.GameModeTranslator.Add(Base.Enum.Game.Mode.Defence, "Defence");
            Packer.GameModeTranslator.Add(Base.Enum.Game.Mode.Escape, "Infection");
            
            Packer.PremiumTranslator = new Dictionary<string, Base.Enum.Premium>();
            Packer.PremiumTranslator.Add("Free", Base.Enum.Premium.None);
            Packer.PremiumTranslator.Add("Bronze", Base.Enum.Premium.Bronce);
            Packer.PremiumTranslator.Add("Silver", Base.Enum.Premium.Silver);
            Packer.PremiumTranslator.Add("Gold", Base.Enum.Premium.Gold);
            Packer.PremiumTranslator.Add("Platinum", Base.Enum.Premium.Platin); //not sure
        }

        public Packer()
        {
            this.Document = new System.Xml.XmlDocument();
            this.Document.LoadXml("<MapDetails></MapDetails>");

            this.MapList = new Dictionary<int, string>();
        }

        /// <summary>
        /// Use this to automatically skip disabled maps
        /// </summary>
        /// <param name="MapFolder">Folder containg MapList.xml and Map Folders</param>
        public bool ProcessMapsFolder(string MapFolder)
        {
            string MapListFile = System.IO.Path.Combine(MapFolder, "MapList.xml");
            if (!System.IO.File.Exists(MapListFile))
                return false;
            System.Xml.XmlDocument MapListDocument = new System.Xml.XmlDocument();
            MapListDocument.Load(MapListFile);
            foreach (string iFolder in System.IO.Directory.GetDirectories(MapFolder))
            {
                int mapReturnCode = this.DoMap(iFolder);
                if (mapReturnCode == 1 || mapReturnCode == -1)
                    continue;
                else if (mapReturnCode == -2)
                    return false;
            }
            return true;
        }

        public int DoMap(string MapFolder)
        {
            string MapInfoFile = System.IO.Path.Combine(MapFolder, "MapInfo.xml");
            string ControlPointTemplateFile = System.IO.Path.Combine(MapFolder, "ControlPointTemplate.dat");
            string StaticObjectFile = System.IO.Path.Combine(MapFolder, "StaticObject.dat");

            string ObjectSpawnTemplateFile = System.IO.Path.Combine(MapFolder, "ObjectSpawnTemplate.dat");

            if (!System.IO.File.Exists(MapInfoFile) ||
                !System.IO.File.Exists(ControlPointTemplateFile) ||
                !System.IO.File.Exists(StaticObjectFile))
                return -1;

            System.Xml.XmlDocument iMapInfo = new System.Xml.XmlDocument();
            iMapInfo.Load(MapInfoFile);

            System.Xml.XmlNode nMap = this.Document.CreateElement("Map");

            // Head
            {

                System.Xml.XmlAttribute aMapID = this.Document.CreateAttribute("ID");
                aMapID.Value = iMapInfo["MapInfo"]["Identity"].Attributes["ID"].Value;
                nMap.Attributes.Append(aMapID);

                System.Xml.XmlAttribute aMapName = this.Document.CreateAttribute("Name");
                aMapName.Value = iMapInfo["MapInfo"]["Display"].Attributes["DisplayName"].Value;
                nMap.Attributes.Append(aMapName);

                int iMapID = int.Parse(aMapID.Value);
                if (this.MapList.ContainsKey(iMapID))
                {
                    Console.WriteLine("Map exists [" + iMapID.ToString() + ";" + aMapName.Value + "]: " + this.MapList[iMapID]);
                    return -2;
                }
                else
                    this.MapList.Add(int.Parse(aMapID.Value), aMapName.Value);

                System.Xml.XmlNode nMapHead = this.Document.CreateElement("Head");

                System.Xml.XmlNode nMapHeadChannel = this.Document.CreateElement("Channel");

                System.Xml.XmlAttribute aMapHeadChannelCQC = this.Document.CreateAttribute("CQC");
                bool allowsCQC = (iMapInfo["MapInfo"]["Channel"].Attributes["Mission"].Value.ToLower()[0] == 't' ? true : false);
                aMapHeadChannelCQC.Value = allowsCQC.ToString().ToLower();
                nMapHeadChannel.Attributes.Append(aMapHeadChannelCQC);

                System.Xml.XmlAttribute aMapHeadChannelBG = this.Document.CreateAttribute("BG");
                aMapHeadChannelBG.Value = (iMapInfo["MapInfo"]["Channel"].Attributes["Large"].Value.ToLower()[0] == 't' ? "true" : "false");
                nMapHeadChannel.Attributes.Append(aMapHeadChannelBG);

                // v -> complicated shit.
                System.Xml.XmlAttribute aMapHeadChannelAI = this.Document.CreateAttribute("AI");
                aMapHeadChannelAI.Value = (iMapInfo["MapInfo"]["Channel"].Attributes["AI"] != null && iMapInfo["MapInfo"]["Channel"].Attributes["AI"].Value.ToLower()[0] == 't' ? "true" : "false");
                nMapHeadChannel.Attributes.Append(aMapHeadChannelAI);

                nMapHead.AppendChild(nMapHeadChannel);

                System.Xml.XmlNode nMapHeadGameMode = this.Document.CreateElement("GameMode");
                foreach (Base.Enum.Game.Mode iGameMode in Enum.GetValues(typeof(Base.Enum.Game.Mode)))
                {
                    System.Xml.XmlNode nMapHeadGameMode_ModeNode = this.Document.CreateElement("Mode");

                    System.Xml.XmlAttribute nMapHeadGameMode_ModeNode_Type = this.Document.CreateAttribute("Name");
                    nMapHeadGameMode_ModeNode_Type.Value = iGameMode.ToString();
                    nMapHeadGameMode_ModeNode.Attributes.Append(nMapHeadGameMode_ModeNode_Type);

                    System.Xml.XmlAttribute nMapHeadGameMode_ModeNode_Allowed = this.Document.CreateAttribute("Allowed");
                    nMapHeadGameMode_ModeNode_Allowed.Value = "false";

                    if (iGameMode == Base.Enum.Game.Mode.FourVsFour)
                    {
                        if (allowsCQC &&
                            iMapInfo["MapInfo"]["Icon"] != null &&
                            iMapInfo["MapInfo"]["Icon"].Attributes["b4vs4"].Value.ToLower() == "true" &&
                            iMapInfo["MapInfo"]["GameMode"][GameModeTranslator[Base.Enum.Game.Mode.FourVsFour]] != null &&
                            iMapInfo["MapInfo"]["GameMode"][GameModeTranslator[Base.Enum.Game.Mode.FourVsFour]].Attributes["Support"].Value.ToLower() == "true")
                            nMapHeadGameMode_ModeNode_Allowed.Value = "true";
                    }
                    else if (iGameMode == Base.Enum.Game.Mode.FreeForAll)
                    {
                        if (iMapInfo["MapInfo"]["GameMode"][GameModeTranslator[Base.Enum.Game.Mode.FreeForAll]] != null &&
                            int.Parse(iMapInfo["MapInfo"]["GameMode"][GameModeTranslator[Base.Enum.Game.Mode.FreeForAll]].Attributes["Scale"].Value) > 0)
                            nMapHeadGameMode_ModeNode_Allowed.Value = "true";
                    }
                    else
                    {
                        bool oneAllowed = false;
                        foreach (string aGameMode in GameModeTranslator[iGameMode].Split('|'))
                        {
                            if (iMapInfo["MapInfo"]["GameMode"][aGameMode] != null &&
                                iMapInfo["MapInfo"]["GameMode"][aGameMode].Attributes["Support"].Value.ToLower() == "true")
                            {
                                oneAllowed = true;
                                break;
                            }
                        }
                        if(oneAllowed)
                            nMapHeadGameMode_ModeNode_Allowed.Value = "true";
                    }

                    nMapHeadGameMode_ModeNode.Attributes.Append(nMapHeadGameMode_ModeNode_Allowed);
                    nMapHeadGameMode.AppendChild(nMapHeadGameMode_ModeNode);

                }

                nMapHead.AppendChild(nMapHeadGameMode);

                Base.Enum.Premium premiumRestriction = PremiumTranslator[iMapInfo["MapInfo"]["Restriction"].Attributes["PayType"].Value];

                System.Xml.XmlNode nMapHeadPremium = this.Document.CreateElement("Premium");
                System.Xml.XmlAttribute nMapHeadPremiumType = this.Document.CreateAttribute("Type");
                nMapHeadPremiumType.Value = premiumRestriction.ToString();
                nMapHeadPremium.Attributes.Append(nMapHeadPremiumType);

                nMapHead.AppendChild(nMapHeadPremium);

                System.Xml.XmlNode nMapHeadSpecial = this.Document.CreateElement("Special");

                System.Xml.XmlAttribute nMapHeadSpecialNew = this.Document.CreateAttribute("New");
                nMapHeadSpecialNew.Value = ((iMapInfo["MapInfo"]["Icon"] != null && iMapInfo["MapInfo"]["Icon"].Attributes["New"].Value.ToLower()[0] == 't') ? "true" : "false");
                nMapHeadSpecial.Attributes.Append(nMapHeadSpecialNew);

                System.Xml.XmlAttribute nMapHeadSpecialEvent = this.Document.CreateAttribute("Event");
                nMapHeadSpecialEvent.Value = ((iMapInfo["MapInfo"]["Icon"] != null && iMapInfo["MapInfo"]["Icon"].Attributes["Event"].Value.ToLower()[0] == 't') ? "true" : "false");
                nMapHeadSpecial.Attributes.Append(nMapHeadSpecialEvent);

                System.Xml.XmlAttribute nMapHeadSpecialExperience = this.Document.CreateAttribute("Experience");
                double experienceMultiplicator = 1.0;
                if (iMapInfo["MapInfo"]["Icon"] != null && iMapInfo["MapInfo"]["Icon"].Attributes["Exp5Up"].Value.ToLower()[0] == 't') experienceMultiplicator += .05;
                if (iMapInfo["MapInfo"]["Icon"] != null && iMapInfo["MapInfo"]["Icon"].Attributes["Exp10Up"].Value.ToLower()[0] == 't') experienceMultiplicator += .10;
                nMapHeadSpecialExperience.Value = experienceMultiplicator.ToString();
                nMapHeadSpecial.Attributes.Append(nMapHeadSpecialExperience);

                nMapHead.AppendChild(nMapHeadSpecial);

                nMap.AppendChild(nMapHead);

            }

            // Object
            {

                System.Xml.XmlNode nMapObject = this.Document.CreateElement("Object");

                string strControlPointTemplate = System.IO.File.ReadAllText(ControlPointTemplateFile);
                string strStaticObject = System.IO.File.ReadAllText(StaticObjectFile);

                foreach (System.Text.RegularExpressions.Match iMatch in System.Text.RegularExpressions.Regex.Matches(strControlPointTemplate,
                    @"ControlPoint.Name\s+?(?<controlName>[a-zA-Z0-9_\-]+?).*?ControlPoint.Team\s+?(?<controlTeam>(0|1|-1)).*?ControlPoint.Position\s+?(?<controlPositionX>-?[0-9]+?)\.[0-9]+?/(?<controlPositionY>-?[0-9]+?)\.[0-9]+?/(?<controlPositionZ>-?[0-9]+?)\.[0-9]+?",
                    System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.CultureInvariant))
                {

                    System.Xml.XmlNode nMapObjectFlag = this.Document.CreateElement("Flag");

                    System.Xml.XmlAttribute nMapObjectFlagX = this.Document.CreateAttribute("X"),
                        nMapObjectFlagY = this.Document.CreateAttribute("Y"),
                        nMapObjectFlagZ = this.Document.CreateAttribute("Z");
                    nMapObjectFlagX.Value = iMatch.Groups["controlPositionX"].Value;
                    nMapObjectFlagY.Value = iMatch.Groups["controlPositionY"].Value;
                    nMapObjectFlagZ.Value = iMatch.Groups["controlPositionZ"].Value;

                    nMapObjectFlag.Attributes.Append(nMapObjectFlagX);
                    nMapObjectFlag.Attributes.Append(nMapObjectFlagY);
                    nMapObjectFlag.Attributes.Append(nMapObjectFlagZ);

                    System.Xml.XmlAttribute nMapObjectFlagTeam = this.Document.CreateAttribute("Team");
                    Base.Enum.Team aTeam = Base.Enum.Team.None;
                    Base.Types.ParseEnum<Base.Enum.Team>(iMatch.Groups["controlTeam"].Value, out aTeam);
                    nMapObjectFlagTeam.Value = aTeam.ToString();
                    nMapObjectFlag.Attributes.Append(nMapObjectFlagTeam);

                    nMapObject.AppendChild(nMapObjectFlag);

                }

                foreach (System.Text.RegularExpressions.Match iMatch in System.Text.RegularExpressions.Regex.Matches(strStaticObject,
                    @"StandardMesh.Name\s+?bomb.*?StandardMesh.Position\s+?(?<controlPositionX>-?[0-9]+?)\.[0-9]+?/(?<controlPositionY>-?[0-9]+?)\.[0-9]+?/(?<controlPositionZ>-?[0-9]+?)\.[0-9]+?",
                    System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.CultureInvariant))
                {
                    System.Xml.XmlNode nMapObjectBomb = this.Document.CreateElement("Bomb");

                    System.Xml.XmlAttribute nMapObjectBombX = this.Document.CreateAttribute("X"),
                        nMapObjectBombY = this.Document.CreateAttribute("Y"),
                        nMapObjectBombZ = this.Document.CreateAttribute("Z");
                    nMapObjectBombX.Value = iMatch.Groups["controlPositionX"].Value;
                    nMapObjectBombY.Value = iMatch.Groups["controlPositionY"].Value;
                    nMapObjectBombZ.Value = iMatch.Groups["controlPositionZ"].Value;

                    nMapObjectBomb.Attributes.Append(nMapObjectBombX);
                    nMapObjectBomb.Attributes.Append(nMapObjectBombY);
                    nMapObjectBomb.Attributes.Append(nMapObjectBombZ);

                    nMapObject.AppendChild(nMapObjectBomb);
                }

                if (System.IO.File.Exists(ObjectSpawnTemplateFile))
                {
                    string strObjectSpawnTemplate = System.IO.File.ReadAllText(ObjectSpawnTemplateFile);

                    foreach (System.Text.RegularExpressions.Match iMatch in System.Text.RegularExpressions.Regex.Matches(strObjectSpawnTemplate,
                    @"ObjectSpawn.Target\s+(?<objectName>[a-zA-Z0-9_-]+).*?ObjectSpawn.Position\s+?(?<controlPositionX>-?[0-9]+?)\.[0-9]+?/(?<controlPositionY>-?[0-9]+?)\.[0-9]+?/(?<controlPositionZ>-?[0-9]+?)\.[0-9]+?.*?ObjectSpawn.Code\s+?(?<objectCode>[a-zA-Z0-9]{4}).*?ObjectSpawn.SpawnInterval\s+?(?<objectSpawn>[0-9]+)",
                    System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.CultureInvariant))
                    {
                        System.Xml.XmlNode nMapObjectEntity = this.Document.CreateElement("Entity");

                        System.Xml.XmlAttribute nMapObjectEntityX = this.Document.CreateAttribute("X"),
                            nMapObjectEntityY = this.Document.CreateAttribute("Y"),
                            nMapObjectEntityZ = this.Document.CreateAttribute("Z");
                        nMapObjectEntityX.Value = iMatch.Groups["controlPositionX"].Value;
                        nMapObjectEntityY.Value = iMatch.Groups["controlPositionY"].Value;
                        nMapObjectEntityZ.Value = iMatch.Groups["controlPositionZ"].Value;

                        nMapObjectEntity.Attributes.Append(nMapObjectEntityX);
                        nMapObjectEntity.Attributes.Append(nMapObjectEntityY);
                        nMapObjectEntity.Attributes.Append(nMapObjectEntityZ);

                        System.Xml.XmlAttribute nMapObjectEntityCode = this.Document.CreateAttribute("Code"),
                            nMapObjectEntityTarget = this.Document.CreateAttribute("Target"),
                            nMapObjectEntitySpawnInterval = this.Document.CreateAttribute("SpawnInterval");

                        nMapObjectEntityCode.Value = iMatch.Groups["objectCode"].Value;
                        nMapObjectEntityTarget.Value = iMatch.Groups["objectName"].Value;
                        nMapObjectEntitySpawnInterval.Value = iMatch.Groups["objectSpawn"].Value;

                        nMapObjectEntity.Attributes.Append(nMapObjectEntityCode);
                        nMapObjectEntity.Attributes.Append(nMapObjectEntityTarget);
                        nMapObjectEntity.Attributes.Append(nMapObjectEntitySpawnInterval);

                        nMapObject.AppendChild(nMapObjectEntity);
                    }

                }

                nMap.AppendChild(nMapObject);
            }

            this.Document["MapDetails"].AppendChild(nMap);

            return 1;

        }

    }
}
