using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Management
{
    public class WarRock
    {

        public readonly GameDetails.WRBin.ItemsContainer ItemsContainer;
        public readonly GameDetails.WRBin.BranchContainer BranchContainer;
        public readonly GameDetails.Map.DetailContainer MapDetailContainer;

        public WarRock()
        {

            Base.IO.GetInstance().SetHeading("Preparing Game Information...");
            bool fancyOutput = (bool)Base.IO.Configuration["FancyOutput"];
            Base.IO.Configuration["FancyOutput"] = false;

            // Filename variables
            string itemsXml = Base.Compile.FileNames["Game.ItemsXml"];
            string branchXml = Base.Compile.FileNames["Game.BranchXml"];

            string itemsBin = Base.Compile.FileNames["Game.ItemsBin"];
            string branchBin = Base.Compile.FileNames["Game.BranchBin"];

            // Caching stuff
            {
                // Check for file existence
                /*
                if (!System.IO.File.Exists(itemsBin))
                {
                    Server.Core.GetInstance().Stop();
                    Base.IO.Error("File: items.bin file does not exist!");
                    System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
                }
                if (!System.IO.File.Exists(branchBin))
                {
                    Server.Core.GetInstance().Stop();
                    Base.IO.Error("File: branch.bin file does not exist!");
                    System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
                }
                */

                // Create XML from bin and rename bin to a new file
                if (System.IO.File.Exists(itemsBin))
                {
                    Base.IO.GetInstance().SetHeading("Creating items.xml from bin...");
                    Base.IO.Informational("Creating items.xml");
                    Base.App.XmlRewriter itemsRewriter = new Base.App.XmlRewriter();
                    itemsRewriter.Process(LowLevel.WRBin.Decrypt(System.IO.File.ReadAllText(itemsBin)));
                    itemsRewriter.Document.Save(itemsXml);
                    string oldFile = itemsBin + ".old";
                    if (System.IO.File.Exists(oldFile))
                        System.IO.File.Delete(oldFile);
                    System.IO.File.Move(itemsBin, oldFile);
                }

                if (System.IO.File.Exists(branchBin))
                {
                    Base.IO.GetInstance().SetHeading("Creating branch.xml from bin...");
                    Base.IO.Informational("Creating branch.xml");
                    Base.App.XmlRewriter itemsRewriter = new Base.App.XmlRewriter();
                    itemsRewriter.Process(LowLevel.WRBin.Decrypt(System.IO.File.ReadAllText(branchBin)));
                    itemsRewriter.Document.Save(branchXml);
                    string oldFile = branchBin + ".old";
                    if (System.IO.File.Exists(oldFile))
                        System.IO.File.Delete(oldFile);
                    System.IO.File.Move(branchBin, oldFile);
                }
            }

            Base.IO.GetInstance().SetHeading("Loading Information...");

            // ItemsContainer
            {
                Base.IO.Notice("Loading items.xml");
                System.Xml.XmlDocument itemsDocument = new System.Xml.XmlDocument();
                itemsDocument.Load(itemsXml);
                this.ItemsContainer = new GameDetails.WRBin.ItemsContainer(itemsDocument);
            }

            // BranchContainer
            {
                Base.IO.Notice("Loading branch.xml");
                System.Xml.XmlDocument branchDocument = new System.Xml.XmlDocument();
                branchDocument.Load(branchXml);
                this.BranchContainer = new GameDetails.WRBin.BranchContainer(branchDocument);
            }

            // MapDetailContainer
            {
                Base.IO.Notice("Loading GameDetails.xml");
                System.Xml.XmlDocument mapDetailXml = new System.Xml.XmlDocument();
                if (!System.IO.File.Exists(Base.Compile.FileNames["Game.DetailsXml"]))
                {
                }
                mapDetailXml.Load(Base.Compile.FileNames["Game.DetailsXml"]);
                this.MapDetailContainer = GameDetails.Processor.MapDetails(mapDetailXml);
            }

            Base.IO.Configuration["FancyOutput"] = fancyOutput;

        }

    }
}
