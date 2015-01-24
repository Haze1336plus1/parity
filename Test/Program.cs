using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Test
{
    class Program
    {

        static void RepaintNames()
        {
            System.Xml.XmlDocument itemsDoc = new System.Xml.XmlDocument();
            itemsDoc.Load(@"E:\dev\visual studio\ParityProject\Release\Data\items.xml");
            Parity.GameDetails.WRBin.ItemsContainer itemInfo = new Parity.GameDetails.WRBin.ItemsContainer(itemsDoc);

            Font painterFont = new Font("Segoe Marker", 9.0f);
            Brush painterBrush = new SolidBrush(System.Drawing.Color.White);

            for (int i = 1; i < 4; i++)
            {
                Bitmap tcrImage = new Bitmap(512, 512);
                Graphics painter = Graphics.FromImage(tcrImage);
                painter.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                painter.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                string tcr_fName = @"E:\WarRock\Parity Client_OLD\texture\UI\Arms\Name\Weapon_name_" + i.ToString().PadLeft(2, '0') + ".tga";
                System.Xml.XmlDocument tcr_document = new System.Xml.XmlDocument();
                tcr_document.Load(tcr_fName + ".xml");
                foreach (System.Xml.XmlElement tcrEntry in tcr_document["ClipInfo"]["ResList"])
                {

                    int tcrWidth = int.Parse(tcrEntry.Attributes["W0"].Value);
                    int tcrHeight = int.Parse(tcrEntry.Attributes["H0"].Value);

                    int tcrX = int.Parse(tcrEntry.Attributes["X0"].Value);
                    int tcrY = int.Parse(tcrEntry.Attributes["Y0"].Value);

                    string itemCode = tcrEntry.Attributes["Name"].Value.Substring(5);
                    var itemRef = itemInfo.Weapons.Find(itemCode);
                    if (itemRef == null)
                        Console.WriteLine("CRITICAL: " + itemCode);
                    else
                    {
                        string paintText = itemRef.BasicInfo.English.Substring(3).Replace('_', ' ');
                        SizeF stringSize = painter.MeasureString(paintText, painterFont);
                        if (stringSize.Width > tcrWidth)
                            Console.WriteLine("itemCode: {0}, width. {1}", itemCode, stringSize.Width - tcrWidth);
                        if (stringSize.Height > tcrHeight)
                            Console.WriteLine("itemCode: {0}, height. {1}", itemCode, stringSize.Height - tcrHeight);
                        painter.DrawString(paintText, painterFont, painterBrush, new PointF(tcrX, tcrY));
                    }
                }
                painter.Dispose();
                tcrImage.Save(@"E:\WarRock\Parity Client_OLD\texture\UI\Arms\Name\Weapon_name_" + i.ToString().PadLeft(2, '0') + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
            Console.ReadLine();
        }

        class example
        {
            byte[] Magie;
            public example(byte[] magie)
            {
                this.Magie = magie;
            }

            public void DealIt()
            {
                this.Magie[0] = 1;
                this.Magie[1] = 3;
                this.Magie[2] = 3;
                this.Magie[3] = 7;
            }

            public void Print()
            {
                Console.WriteLine("Print,example: {0}", BitConverter.ToString(this.Magie));
            }

        }

        static void Main(string[] args)
        {

            byte[] test = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Console.WriteLine("Initial,Main : {0}", BitConverter.ToString(test));
            example test2 = new example(test);
            test2.DealIt();
            Console.WriteLine("After,Main   : {0}", BitConverter.ToString(test));
            test2.Print();
            Console.ReadLine();
        }

    }
}
