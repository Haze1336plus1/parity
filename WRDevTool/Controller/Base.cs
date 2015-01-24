using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.WRDevTool.Controller
{
    public class Basic
    {

        public Config Configuration { get; private set; }

        public Basic()
        {
            this.Configuration = new Config("wrdevtool.xml");
            this.Configuration.Load();
        }

        protected void _doUpdateFLCD_createRecursive(FCLD.FileCodeListDocument iDocument, string path, ref int fileCount)
        {
            string relativePath = path.Substring(Environment.CurrentDirectory.Length);

            // check for exceptions to exclude
            /*  not required. :D    */
            /*string lowerPath = relativePath.ToLower();
            foreach (FCLD.FCLDException iEx in this.Configuration.FCLD.Exceptions)
            {
                if (iEx.Type == FCLD.FCLDException.eType.StartsWith && lowerPath.StartsWith(iEx.Value)) goto skipFile;
                else if (iEx.Type == FCLD.FCLDException.eType.EndsWith && lowerPath.EndsWith(iEx.Value)) goto skipFile;
                else if (iEx.Type == FCLD.FCLDException.eType.Equals && lowerPath.Equals(iEx.Value)) goto skipFile;
            }*/

            // go thru all directories
            foreach (string subDirectory in System.IO.Directory.GetDirectories(path))
                this._doUpdateFLCD_createRecursive(iDocument, subDirectory, ref fileCount);

            // go thru all files
            foreach (string file in System.IO.Directory.GetFiles(path))
            {
                string relativeFile = file.Substring(Environment.CurrentDirectory.Length);

                // read the file
                byte[] fileData;
                using(System.IO.BinaryReader reader = new System.IO.BinaryReader(System.IO.File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)))
                {
                    fileData = new byte[reader.BaseStream.Length];
                    reader.Read(fileData, 0, fileData.Length);
                }
                // make an entry
                iDocument.RegisterEntry(
                    new FCLD.FCLDEntry(
                        relativeFile,
                        new System.IO.FileInfo(file).LastWriteTime,
                        (uint)fileData.Length,
                        FCLD.FileCodeListDocument.ChecksumData(fileData)
                    )
                );

                fileCount ++;
                if ((fileCount + 100) % 100 == 0)
                    Base.IO.GetInstance().SetHeading("Rebuilding FCLD [" + fileCount.ToString() + "] ...");
            }
        }

        protected int _doUpdateFLCD_update(FCLD.FileCodeListDocument iDocument)
        {
            int changedFiles = 0,
                processedFiles = 0;
            foreach (FCLD.FCLDEntry iEntry in iDocument.Entrys)
            {
                string fullPath = Environment.CurrentDirectory + iEntry.FileName;
                var fileInfo = new System.IO.FileInfo(fullPath);
                
                if(fileInfo.LastWriteTime != iEntry.ModifyTime ||
                    fileInfo.Length != iEntry.FileSize)
                {

                    // read the file
                    byte[] fileData;
                    using(System.IO.BinaryReader reader = new System.IO.BinaryReader(System.IO.File.Open(fullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)))
                    {
                        fileData = new byte[reader.BaseStream.Length];
                        reader.Read(fileData, 0, fileData.Length);
                    }

                    uint newChecksum = FCLD.FileCodeListDocument.ChecksumData(fileData);

                    FCLD.FCLDEntry newEntry = new FCLD.FCLDEntry
                    (
                        iEntry.FileName, fileInfo.LastWriteTime, (uint)fileInfo.Length, newChecksum
                    );

                    changedFiles++;
                    iDocument.Entrys[processedFiles] = newEntry;
                    Base.IO.Notice("File Changed: " + iEntry.FileName);

                }

                processedFiles++;
                if ((processedFiles + 100) % 100 == 0)
                    Base.IO.GetInstance().SetHeading("Updating FCLD [" + processedFiles.ToString() + " / " + iDocument.Entrys.Length.ToString() + "]");
            }
            return changedFiles;
        }

        protected bool _doUpdateFCLD()
        {
            Base.IO.GetInstance().SetHeading("Processing ...");
            string fcldFileName = System.IO.Path.Combine(Environment.CurrentDirectory, "data\\Global.FCL");

            if (System.IO.File.Exists(fcldFileName))
            {
                Base.IO.Notice("[U]pdate or [R]ebuild FCLD? (default: Update)");
                string message = Console.ReadLine().ToLower();
                if (message.Fallback("u")[0] == 'u')
                {
                    // load FCLD
                    Base.IO.GetInstance().SetHeading("Updating FCLD ...");
                    FCLD.FileCodeListDocument iDocument = new FCLD.FileCodeListDocument(fcldFileName);
                    iDocument.Process();
                    Base.IO.Notice("FCLD File found. Version: " + iDocument.Header.Version.ToString());
                    int changedFiles = this._doUpdateFLCD_update(iDocument);
                    Base.IO.GetInstance().SetHeading("FCLD updated!");
                    Base.IO.Informational("FCLD updated - " + changedFiles.ToString() + " files updated. Save now? Y/N");
                    // save updated FCLD
                    if ((Console.ReadLine() ?? "y").ToLower()[0] == 'n')
                        return true;
                    else
                    {
                        var iFileInfo = new System.IO.FileInfo(fcldFileName);
                        if (!iFileInfo.Directory.Exists)
                            iFileInfo.Directory.Create();
                        if (iFileInfo.Exists)
                            iFileInfo.Delete();

                        byte[] writeData = iDocument.Serialize();
                        using(System.IO.BinaryWriter writer = new System.IO.BinaryWriter(System.IO.File.Open(fcldFileName, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None)))
                        {
                            writer.Write(writeData);
                        }
                        Base.IO.Informational("Saved FCLD Document!");
                        return false;
                    }

                }
            }
            else
            {
                Base.IO.Warning("FCLD File does not exist! Create now? Y/N");
                if (Console.ReadLine().ToLower()[0] == 'n')
                    return true;
            }

            FCLD.FileCodeListDocument newDocument = new FCLD.FileCodeListDocument(this.Configuration.FCLD.Template);
            newDocument.Process();
            Base.IO.GetInstance().SetHeading("Creating FCLD ... this may takes a bit");
            int fileCount = 0;
            this._doUpdateFLCD_createRecursive(newDocument, Environment.CurrentDirectory, ref fileCount);
            Base.IO.Informational("FCLD created - " + newDocument.Entrys.Length.ToString() + " entries created. Save now? Y/N");
            // create new FCLD
            if (Console.ReadLine().ToLower()[0] == 'n')
                return true;
            else
            {
                var iFileInfo = new System.IO.FileInfo(fcldFileName);
                if (!iFileInfo.Directory.Exists)
                    iFileInfo.Directory.Create();
                if (iFileInfo.Exists)
                    iFileInfo.Delete();

                byte[] writeData = newDocument.Serialize();
                using(System.IO.BinaryWriter writer = new System.IO.BinaryWriter(System.IO.File.Open(fcldFileName, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None)))
                {
                    writer.Write(writeData);
                }
                Base.IO.Informational("Saved FCLD Document!");
                return false;
            }

        }

        protected bool _doUpdateFiles()
        {
            FileDB.FileDB iFDB = new FileDB.FileDB();
            iFDB.ReloadDatabase();
            iFDB.StartScan();
            iFDB.SaveDatabase();
            if (iFDB.statusCreated > 0 || iFDB.statusUpdated > 0)
            {
                Base.IO.Notice("Do you want to copy new/updated Files? Y/N");
                if ((Console.ReadLine() ?? "y")[0] == 'y')
                    iFDB.CopyUpdated(@"FileDB\");
            }
            else
                Base.IO.Informational("Nothing has changes at all. So there's nothing to do anymore!");
            return false;
        }

        protected bool _doProcessMaps()
        {
            Base.IO.GetInstance().SetHeading("Processing Maps ...");
            string mapsFolder = System.IO.Path.Combine(Environment.CurrentDirectory, @"maps\");
            if (!System.IO.Directory.Exists(mapsFolder))
            {
                Base.IO.Critical("Maps folder does not exist!");
                return true;
            }
            GameDetails.Packer iDetailPacker = new GameDetails.Packer();
            if (!iDetailPacker.ProcessMapsFolder(mapsFolder))
            {
                Base.IO.Critical("Something went wrong!");
                return true;
            }
            else
            {
                Base.IO.GetInstance().SetHeading("Map Details processed to XML !");

                var xmlSettings = new System.Xml.XmlWriterSettings();
                xmlSettings.Indent = true;
                xmlSettings.IndentChars = "\t";
                xmlSettings.NewLineChars = "\r\n";

                var xmlWriter = System.Xml.XmlWriter.Create("GameDetails.xml", xmlSettings);
                iDetailPacker.Document.Save(xmlWriter);
                Base.IO.Informational("XML File has been saved (GameDetails.xml)");
            }
            return false;
        }

        protected bool _doConvertBinXML()
        {
            Base.IO.GetInstance().SetHeading("Convert BIN to XML. Enter Filename");
            Base.IO.Notice("Enter Binary Filename:");
            string fileName = Console.ReadLine();
            var fileInfo = new System.IO.FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                Base.IO.Error("File does not exist :(");
            }
            else
            {
                Base.IO.Informational("Let's get it on!");
                Base.App.XmlRewriter iRewriter = new Base.App.XmlRewriter();
                try
                {
                    //return false;
                    Base.IO.GetInstance().SetHeading("Loading Binary file ...");
                    string fileContent = LowLevel.WRBin.Decrypt(System.IO.File.ReadAllText(fileInfo.FullName));
                    Base.IO.GetInstance().SetHeading("Processing Binary file ...");
                    iRewriter.Process(fileContent);
                    Base.IO.GetInstance().SetHeading("Saving XML ...");
                    iRewriter.Document.Save(fileInfo.FullName + ".xml");
                    Base.IO.GetInstance().SetHeading("BIN to XML converted!");
                    Base.IO.Informational("You've got it. File has been saved as .xml");
                    return false;
                }
                catch
                {
                    Base.IO.Error("Corrupted / invalid BIN file!");
                }
            }
            return true;
        }

        protected bool _doBuildItemIndex()
        {
            Base.IO.GetInstance().SetHeading("Loading Data [items.bin] ...");
            Base.IO.Informational("Loading Data ...");
            string itemsBin = System.IO.Path.Combine(Environment.CurrentDirectory, @"data\items.bin");
            string soundsTxt = System.IO.Path.Combine(Environment.CurrentDirectory, @"data\Weapons.txt");
            if (!System.IO.File.Exists(itemsBin) ||
                !System.IO.File.Exists(soundsTxt))
            {
                Base.IO.Error("Either data\\items.bin or data\\Weapons.txt does not exist!");
                return true;
            }
            string strSounds = System.IO.File.ReadAllText(soundsTxt);
            Base.App.XmlRewriter iRewriter = new Base.App.XmlRewriter();
            string itemsBinContent = LowLevel.WRBin.Decrypt(System.IO.File.ReadAllText(itemsBin));
            Base.IO.GetInstance().SetHeading("Loading Data [bin -> xml] ...");
            iRewriter.Process(itemsBinContent);
            System.Xml.XmlDocument iDocument = iRewriter.Document;
            Base.IO.GetInstance().SetHeading("Translating Items ...");
            Base.IO.Informational("Translating Items ...");
            System.Xml.XmlDocument translationDocument = GameDetails.Processor.ItemTranslation(iDocument, false);
            //new sound.*?index\s(?<weaponIndex>[0-9]+).*?//tado\s+?(?<weaponName>[a-zA-Z0-9_-]+).*?path\s+(?<soundLink>[a-zA-Z0-9_\-/\\]+)\s
            Base.IO.GetInstance().SetHeading("Fetching Sound Table ...");
            Base.IO.Informational("Fetching Sound Table ...");
            var iDict = new Dictionary<int, string[]>();
            foreach(System.Text.RegularExpressions.Match iMatch in System.Text.RegularExpressions.Regex.Matches(
                strSounds,
                @"new[\s|\t]+?sound.*?index[\s|\t]+?(?<weaponIndex>[0-9]+).*?//ta[d|t]o[\s|\t]+?.*?path[\s|\t]+(?<soundLink>[a-zA-Z0-9_\-/\\]+?)[\s|\t]+?(?<soundPath>[a-zA-Z0-9_-]+).*?radius[\s|\t]+?(?<soundRadius>[0-9]+?)[\r|\n]",
                System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.CultureInvariant))
            {
                int weaponIndex = int.Parse(iMatch.Groups["weaponIndex"].Value);
                iDict.Add(weaponIndex, new string[] { iMatch.Groups["soundLink"].Value, iMatch.Groups["soundPath"].Value, iMatch.Groups["soundRadius"].Value });
            }
            Base.IO.GetInstance().SetHeading("Finishing Document ...");
            Base.IO.Informational("Finishing Document ...");
            var iActiveSoundList = new List<int>();
            foreach(System.Xml.XmlNode iNode in translationDocument["ItemTranslation"])
            {
                if (iNode.NodeType != System.Xml.XmlNodeType.Element)
                    continue;
                int currentID = int.Parse(iNode.Attributes["ID"].Value);
                System.Xml.XmlAttribute iSoundAttribute = translationDocument.CreateAttribute("Sound");
                iSoundAttribute.Value = (iDict.ContainsKey(currentID) ? iDict[currentID][0] : "undefined");
                iNode.Attributes.Append(iSoundAttribute);

                System.Xml.XmlAttribute iSoundPathAttribute = translationDocument.CreateAttribute("SoundPath");
                iSoundPathAttribute.Value = (iDict.ContainsKey(currentID) ? iDict[currentID][1] : "undefined");
                iNode.Attributes.Append(iSoundPathAttribute);

                System.Xml.XmlAttribute iSoundRadiusAttribute = translationDocument.CreateAttribute("SoundRadius");
                iSoundRadiusAttribute.Value = (iDict.ContainsKey(currentID) ? iDict[currentID][2] : "undefined");
                iNode.Attributes.Append(iSoundRadiusAttribute);

                iActiveSoundList.Add(currentID);
            }
            foreach (KeyValuePair<int, string[]> iPayloadSound in iDict)
            {
                if (iActiveSoundList.Contains(iPayloadSound.Key))
                    continue;
                System.Xml.XmlElement iElement = translationDocument.CreateElement("Sound");

                System.Xml.XmlAttribute iID = translationDocument.CreateAttribute("ID");
                iID.Value = iPayloadSound.Key.ToString();
                iElement.Attributes.Append(iID);

                System.Xml.XmlAttribute iSoundAttribute = translationDocument.CreateAttribute("Sound");
                iSoundAttribute.Value = iPayloadSound.Value[0];
                iElement.Attributes.Append(iSoundAttribute);

                System.Xml.XmlAttribute iSoundPathAttribute = translationDocument.CreateAttribute("SoundPath");
                iSoundPathAttribute.Value = iPayloadSound.Value[1];
                iElement.Attributes.Append(iSoundPathAttribute);

                System.Xml.XmlAttribute iSoundRadiusAttribute = translationDocument.CreateAttribute("SoundRadius");
                iSoundRadiusAttribute.Value = iPayloadSound.Value[2];
                iElement.Attributes.Append(iSoundRadiusAttribute);

                translationDocument["ItemTranslation"].AppendChild(iElement);
            }
            translationDocument.Save("ItemIndex.xml");
            Base.IO.GetInstance().SetHeading("Job: Finished :]");
            Base.IO.Notice("Item/Sound Index has been saved to ItemIndex.xml");

            return false;
        }

        protected bool _doRebuildSounds()
        {
            Base.IO.GetInstance().SetHeading("Processing sounds ...");
            Base.IO.Informational("This shouldn't take that long ...");
            string itemIndexFile = System.IO.Path.Combine(Environment.CurrentDirectory, @"ItemIndex.xml");
            string soundFile = System.IO.Path.Combine(Environment.CurrentDirectory, @"data\Weapons.txt");
            if (!System.IO.File.Exists(itemIndexFile))
            {
                Base.IO.Informational("ItemIndex.xml does not exist! Build now? Y/N");
                if ((Console.ReadLine() ?? "y").ToLower()[0] == 'y')
                    return this._doBuildItemIndex();
                else
                    return true;
            }
            System.Xml.XmlDocument iItemIndex = new System.Xml.XmlDocument();
            iItemIndex.Load(itemIndexFile);
            string newSounds = "// automatic sounds file [wrdevtool]\r\n\r\n";
            foreach (System.Xml.XmlNode iEntry in iItemIndex["ItemTranslation"])
            {
                if (iEntry.NodeType != System.Xml.XmlNodeType.Element)
                    continue;
                if (iEntry.Name == "Entry")
                {
                    if (iEntry.Attributes["Sound"].Value == "undefined")
                        newSounds += String.Format("// skipping item {0}: undefined sound\r\n\r\n", iEntry.Attributes["Translation"].Value);
                    else
                    {
                        newSounds += String.Format("new sound\r\nindex {0}\r\n//tado {1}\r\npath {2} {3}\r\nradius {4}\r\n\r\n",
                            iEntry.Attributes["ID"].Value,
                            iEntry.Attributes["Translation"].Value,
                            iEntry.Attributes["Sound"].Value,
                            iEntry.Attributes["SoundPath"].Value,
                            iEntry.Attributes["SoundRadius"].Value);
                    }
                }
                else if (iEntry.Name == "Sound")
                {
                    newSounds += String.Format("new sound\r\nindex {0}\r\n//tado unknown-payload\r\npath {1} {2}\r\nradius {3}\r\n\r\n",
                            iEntry.Attributes["ID"].Value,
                            iEntry.Attributes["Sound"].Value,
                            iEntry.Attributes["SoundPath"].Value,
                            iEntry.Attributes["SoundRadius"].Value);
                }
            }
            newSounds += "\r\n//sound end";
            if (System.IO.File.Exists(soundFile))
                System.IO.File.Delete(soundFile);
            System.IO.File.AppendAllText(soundFile, newSounds);
            Base.IO.GetInstance().SetHeading("Sound Index saved");
            Base.IO.Informational("Saved Sound Index to data\\Weapons.txt");
            return false;
        }

        public bool _doProcessXXML()
        {
            Base.IO.GetInstance().SetHeading("Feed me with Input");
            Base.IO.Informational("Enter (X)XML Filename (XXML <-> XML):");
            string fileName = Console.ReadLine();
            var fileInfo = new System.IO.FileInfo(fileName);
            if (!fileInfo.Exists)
                Base.IO.Error("File does not exist :(");
            else
            {
                if (fileInfo.Extension == ".xxml" || 
                    fileInfo.Extension == ".xml")
                {
                    Base.IO.GetInstance().SetHeading("Processing File");
                    Base.IO.Informational("Processing File ...");
                    byte[] fileBytes = new byte[] { };
                    using (System.IO.BinaryReader iReader = new System.IO.BinaryReader(System.IO.File.Open(fileInfo.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read)))
                    {
                        fileBytes = iReader.ReadBytes((int)iReader.BaseStream.Length);
                        for (int iIndex = 0; iIndex < fileBytes.Length; iIndex++)
                            fileBytes[iIndex] ^= 0x7F;
                        iReader.Close();
                    }
                    string newFileName = fileInfo.FullName.Substring(0, fileInfo.FullName.Length - fileInfo.Extension.Length) +
                        (fileInfo.Extension == ".xml" ? ".xxml" : ".xml");
                    var newFileInfo = new System.IO.FileInfo(newFileName);
                    if (newFileInfo.Exists)
                        newFileInfo.Delete();
                    using (System.IO.BinaryWriter iWriter = new System.IO.BinaryWriter(System.IO.File.Open(newFileInfo.FullName, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write)))
                    {
                        iWriter.Write(fileBytes);
                        iWriter.Flush();
                        iWriter.Close();
                    }

                    Base.IO.GetInstance().SetHeading("That's it");
                    Base.IO.Notice("(X)XML processed successfully");
                    return true;

                }
                else
                {
                    Base.IO.Error("Extension '" + fileInfo.Extension + "' not supported.");
                }
            }
            return false;
        }

        public bool _doEncryptFile()
        {
            Base.IO.GetInstance().SetHeading("Feed me with Input");
            Base.IO.Informational("Enter the filename of file to be encrypted");
            string fileName = Console.ReadLine();
            if (!System.IO.File.Exists(fileName))
            {
                Base.IO.Error("File does not exist :(");
                return false;
            }
            Base.IO.Informational("And now enter the Key for encryption (0x00-0xFF)");
            Console.Write("0x");
            byte key = byte.Parse(Console.ReadLine(), System.Globalization.NumberStyles.HexNumber);
            Base.IO.GetInstance().SetHeading("Encrypting file ...");
            byte[] data = System.IO.File.ReadAllBytes(fileName);
            Array.Resize(ref data, data.Length + 1);
            Array.Copy(data, 0, data, 1, data.Length - 1);
            data[0] = key;
            for (int i = 1; i < data.Length - 1; i++)
                data[i] ^= key;
            string targetFile = fileName + ".enc";
            if (System.IO.File.Exists(targetFile))
                System.IO.File.Delete(targetFile);
            System.IO.File.WriteAllBytes(targetFile, data);
            Base.IO.Notice("File saved");
            Base.IO.GetInstance().SetHeading("Done!");
            return true;
        }


        public bool Run()
        {
            var _assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo _fileVersionInfo = null;
            try { _fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(_assembly.Location); }
            catch
            {
            }
            Base.IO.SetupWindow("WarRock Development Tool");
            Base.IO.GetInstance().SetHeading("Starting ...");
            if(_fileVersionInfo != null)
                Base.IO.Informational("Starting WRDevTool v" + _fileVersionInfo.FileVersion + " | " + _fileVersionInfo.LegalCopyright);
            Base.IO.GetInstance().SetHeading("Take a decision ...");
            Base.IO.Informational("Take a decision, what to do:");
            Base.IO.Notice("You are in " + Environment.CurrentDirectory);
            Base.IO.Notice("1 > Update FCLD         (File Code List/Game CRC)");
            Base.IO.Notice("2 > Update FileDB       (Local File Database)");
            Base.IO.Notice("3 > Process Maps to XML (Process all Maps to a single XML Doc)");
            Base.IO.Notice("4 > Convert BIN to XML  (Process WR-BIN file to valid XML Doc)");
            Base.IO.Notice("5 > Build Item Index    (Pack Item Translation / Sounds)");
            Base.IO.Notice("6 > Re-Build Sounds from Item Index (Create new Weapons.txt)");
            Base.IO.Notice("7 > Process (X)XML      (Converts XXML <-> XML)");
            Base.IO.Notice("8 > Encrypt File        (Encrypts a file for release by given key)");
doAgain:
            string _strChoice = Console.ReadLine();
            int _intChoice = -1;
            if (!int.TryParse(_strChoice, out _intChoice))
                goto idiot;

            if (_intChoice == 1)
                return this._doUpdateFCLD();
            else if (_intChoice == 2)
                return this._doUpdateFiles();
            else if (_intChoice == 3)
                return this._doProcessMaps();
            else if (_intChoice == 4)
                return this._doConvertBinXML();
            else if (_intChoice == 5)
                return this._doBuildItemIndex();
            else if (_intChoice == 6)
                return this._doRebuildSounds();
            else if (_intChoice == 7)
                return this._doProcessXXML();
            else if (_intChoice == 8)
                return this._doEncryptFile();
            else
                goto idiot;

idiot:
            Base.IO.Error("Nope. Invalid choice! Do it again ...");
            goto doAgain;
        }

    }
}
