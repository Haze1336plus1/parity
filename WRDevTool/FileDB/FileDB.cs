using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Parity.WRDevTool.FileDB
{
    public class FileDB
    {

        public Dictionary<string, fdbEntry> Database { get; private set; }
        public List<fdbEntryDiff> UpdatedDatabase { get; private set; }
        public List<FCLD.FCLDException> Exceptions { get; private set; }
        public string BasePath { get; private set; }
        public string DatabaseDate { get; private set; }
        public CultureInfo Culture { get; private set; }

        public int statusDeepness { get; private set; }
        public int statusCreated { get; private set; }
        public int statusUpdated { get; private set; }
        public int statusScanned { get; private set; }

        public void ReloadDatabase()
        {
            Base.IO.GetInstance().SetHeading("Reloading Database ...");
            this.Database.Clear();
            this.UpdatedDatabase.Clear();
            string DatabaseFile = System.IO.Path.Combine(this.BasePath, "fileDB.dat");
            if (!System.IO.File.Exists(DatabaseFile))
                System.IO.File.AppendAllText(DatabaseFile, "");
            foreach (string iLine in System.IO.File.ReadAllLines(DatabaseFile))
            {
                string[] split = iLine.Split('?');
                string FileName = split[0];
                if (!FileName.StartsWith(this.BasePath))
                    FileName = System.IO.Path.Combine(this.BasePath, split[0].Substring(1));
                Database.Add(FileName, new fdbEntry(FileName, split[1], split[2], split[3]));
            }
            Base.IO.Notice("Database reloaded");
        }
        public string GetFileChecksum(string FileName)
        {
            // Not sure if BufferedStream should be wrapped in using block
            using (var stream = new System.IO.BufferedStream(System.IO.File.Open(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read), 120000))
            //using (var stream = System.IO.File.Open(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))//, 1200000))
            {
                var sha = new System.Security.Cryptography.SHA256Managed();
                byte[] checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty).PadLeft(64, '0');
            }
        }
        public string GetRelative(string Path)
        {
            return Path.Substring(this.BasePath.Length - 1);
        }
        /*public string GetFileChecksumB(string FileName)
        {
            // Not sure if BufferedStream should be wrapped in using block
            //using (var stream = new System.IO.BufferedStream(System.IO.File.Open(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read), 1200000))
            using (var stream = System.IO.File.Open(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))//, 1200000))
            {
                var sha = new System.Security.Cryptography.SHA256Managed();
                byte[] checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty).PadLeft(64, '0');
            }
        }*/
        private void Scan(string currentPath)
        {
            foreach (string newPath in System.IO.Directory.GetDirectories(currentPath))
            {
                this.statusDeepness++;
                Scan(newPath);
            }
            foreach (string newFile in System.IO.Directory.GetFiles(currentPath))
            {
                System.IO.FileInfo iInfo = new System.IO.FileInfo(newFile);
                string relativeFileName = this.GetRelative(iInfo.FullName);
                string lowerPath = relativeFileName.ToLower();
                foreach (FCLD.FCLDException iEx in Program.Controller.Configuration.FileDB.Exceptions)
                {
                    if (iEx.Type == FCLD.FCLDException.eType.StartsWith && lowerPath.StartsWith(iEx.Value)) goto skipFile;
                    else if (iEx.Type == FCLD.FCLDException.eType.EndsWith && lowerPath.EndsWith(iEx.Value)) goto skipFile;
                    else if (iEx.Type == FCLD.FCLDException.eType.Equals && lowerPath.Equals(iEx.Value)) goto skipFile;
                }

                this.statusScanned++;

                fdbEntry iFileEntry = new fdbEntry(iInfo.FullName, iInfo.LastWriteTime.ToString(this.Culture), iInfo.CreationTime.ToString(this.Culture), string.Empty);
                if (Database.ContainsKey(iFileEntry.FileName))
                {
                    fdbEntry iDbEntry = Database[iFileEntry.FileName];
                    if (iDbEntry.LastChanged != iFileEntry.LastChanged) // || iDbEntry.CreatedAt != iFileEntry.CreatedAt
                    {
                        // did it really change?
                        iFileEntry = new fdbEntry(iFileEntry.FileName, iFileEntry.LastChanged, iFileEntry.CreatedAt, GetFileChecksum(iInfo.FullName));
                        if (iFileEntry.Checksum != iDbEntry.Checksum)
                        {
                            this.Database.Remove(iFileEntry.FileName);
                            Database.Add(iFileEntry.FileName, iFileEntry);
                            UpdatedDatabase.Add(new fdbEntryDiff(iDbEntry, iFileEntry, "changed"));
                            this.statusUpdated++;
                        }
                    }
                }
                else
                {
                    iFileEntry = new fdbEntry(iFileEntry.FileName, iFileEntry.LastChanged, iFileEntry.CreatedAt, GetFileChecksum(iInfo.FullName));
                    Database.Add(iFileEntry.FileName, iFileEntry);
                    UpdatedDatabase.Add(new fdbEntryDiff(iFileEntry, iFileEntry, "created")); // new one :D
                    this.statusCreated++;
                }
                
            skipFile:
                continue;
            }
            if ((this.statusScanned + 100) % 100 == 0)
                Base.IO.GetInstance().SetHeading("FileDB Status: " + this.statusDeepness.ToString() + "F | " + this.statusUpdated.ToString() + "U | " + this.statusCreated.ToString() + "C | " + this.statusScanned.ToString() + " total ...");
        }
        public void StartScan()
        {
            Base.IO.Informational("Starting Database update ...");
            foreach (string newPath in System.IO.Directory.GetDirectories(this.BasePath))
                Scan(newPath);
            Base.IO.Notice("Database updated!");
        }
        public void SaveDatabase()
        {
            this.DatabaseDate = DateTime.Now.ToString("d_M_y\\-h_m_s");
            Base.IO.GetInstance().SetHeading("Saving Database ...");
            string DatabaseFile = System.IO.Path.Combine(this.BasePath, "fileDB.dat");
            if (System.IO.File.Exists(DatabaseFile))
                System.IO.File.Delete(DatabaseFile);
            using (System.IO.StreamWriter iWriter = new System.IO.StreamWriter(System.IO.File.Open(DatabaseFile, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None)))
            {
                foreach (KeyValuePair<string, fdbEntry> iPair in Database)
                    iWriter.WriteLine(GetRelative(iPair.Value.FileName) + "?" + iPair.Value.LastChanged + "?" + iPair.Value.CreatedAt + "?" + iPair.Value.Checksum);
                iWriter.Flush();
                iWriter.Close();
            }

            /*string DatabaseChangedFile = System.IO.Path.Combine(this.BasePath, "fileDB-changed-" + this.DatabaseDate + ".dat");
            using (System.IO.StreamWriter iWriter = new System.IO.StreamWriter(System.IO.File.Open(DatabaseChangedFile, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None)))
            {
                foreach (fdbEntryDiff iEntry in UpdatedDatabase)
                    iWriter.WriteLine("File " + iEntry.Action + ": " + iEntry.Difference.FileName + " [last change: " + iEntry.Difference.LastChanged + " | created at: " + iEntry.Difference.CreatedAt + "]");
                iWriter.Flush();
                iWriter.Close();
            }*/
            Base.IO.Notice("Database saved!");
        }
        public void CopyUpdated(string Destination)
        {
            int updatedFiles = 0;
            foreach(fdbEntryDiff File in this.UpdatedDatabase)
            {
                System.IO.FileInfo iFile = new System.IO.FileInfo(System.IO.Path.Combine(this.BasePath, @"FileDB\", this.DatabaseDate, new System.IO.FileInfo(File.Original.FileName).FullName.Substring(this.BasePath.Length)));
                if (!System.IO.Directory.Exists(iFile.DirectoryName))
                    System.IO.Directory.CreateDirectory(iFile.DirectoryName);
                string DestinationFileName = System.IO.Path.Combine(Destination, iFile.FullName);
                System.IO.File.Copy(File.Original.FileName, DestinationFileName);
                if ((updatedFiles + 5) % 5 == 0)
                    Base.IO.GetInstance().SetHeading("Copying files [" + updatedFiles.ToString() + "]");
            }
        }

        public FileDB()
        {
            this.Database = new Dictionary<string, fdbEntry>();
            this.UpdatedDatabase = new List<fdbEntryDiff>();
            this.BasePath = Environment.CurrentDirectory + "\\";
            //this.BasePath = @"E:\WarRock\WarRock_Client_New\";
            this.Culture = new CultureInfo("en-US");
        }

    }
}
