using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin
{
    public class ItemsContainer
    {

        public Items.Resource[] Resources;
        public Items.Character[] Characters;
        public Items.Etc[] Etc;
        public Items.Weapon[] Weapons;
        protected Dictionary<string, ItemInformation> _itemInformationCollection;
        public ItemInformation GetItemInfo(string code)
        {
            return (this._itemInformationCollection.ContainsKey(code) ? this._itemInformationCollection[code] : null);
        }

        public ItemsContainer(System.Xml.XmlDocument itemsDocument)
        {
            int id = 0;
            List<Items.Resource> resourcesList = new List<Items.Resource>();
            foreach (System.Xml.XmlNode resourceNode in itemsDocument["Document"]["ITEM_DATA"]["RESOURCE"])
            {
                if (resourceNode.NodeType == System.Xml.XmlNodeType.Element &&
                    resourceNode.Name == "Entry")
                    resourcesList.Add(new Items.Resource(id ++, resourceNode));
            }

            id = 0;
            List<Items.Character> charactersList = new List<Items.Character>();
            foreach (System.Xml.XmlNode characterNode in itemsDocument["Document"]["ITEM_DATA"]["CHARACTER"])
            {
                if (characterNode.NodeType == System.Xml.XmlNodeType.Element &&
                    characterNode.Name == "Entry")
                    charactersList.Add(new Items.Character(id ++, characterNode));
            }

            id = 0;
            List<Items.Etc> etcList = new List<Items.Etc>();
            foreach (System.Xml.XmlNode etcNode in itemsDocument["Document"]["ITEM_DATA"]["ETC"])
            {
                if (etcNode.NodeType == System.Xml.XmlNodeType.Element &&
                    etcNode.Name == "Entry")
                    etcList.Add(new Items.Etc(id ++, etcNode));
            }

            id = 0;
            List<Items.Weapon> weaponsList = new List<Items.Weapon>();
            foreach (System.Xml.XmlNode weaponNode in itemsDocument["Document"]["ITEM_DATA"]["WEAPON"])
            {
                if (weaponNode.NodeType == System.Xml.XmlNodeType.Element &&
                    weaponNode.Name == "Entry")
                    weaponsList.Add(new Items.Weapon(id ++, weaponNode));
            }

            this.Resources = resourcesList.ToArray();
            this.Characters = charactersList.ToArray();
            this.Etc = etcList.ToArray();
            this.Weapons = weaponsList.ToArray();

            /*
            /* linq at its costs. below code takes 16 msec! omg * /
            this._itemInformationCollection =
                this.Resources.ToDictionary(x => x.BasicInfo.Code, y => new ItemInformation(y.BasicInfo.Code)).Concat(
                this.Characters.ToDictionary(x => x.BasicInfo.Code, y => new ItemInformation(y.BasicInfo.Code))).Concat(
                this.Etc.ToDictionary(x => x.BasicInfo.Code, y => new ItemInformation(y.BasicInfo.Code))).Concat(
                this.Weapons.ToDictionary(x => x.BasicInfo.Code, y => new ItemInformation(y.BasicInfo.Code)))
                .ToDictionary(x => x.Key, y => y.Value); // finalize this
            //*/

            this._itemInformationCollection = new Dictionary<string, ItemInformation>();
            foreach (var l in this.Resources)
                this._itemInformationCollection.Add(l.BasicInfo.Code, new ItemInformation(l.BasicInfo.Code));
            foreach (var l in this.Characters)
                this._itemInformationCollection.Add(l.BasicInfo.Code, new ItemInformation(l.BasicInfo.Code));
            foreach (var l in this.Etc)
                this._itemInformationCollection.Add(l.BasicInfo.Code, new ItemInformation(l.BasicInfo.Code));
            foreach (var l in this.Weapons)
                this._itemInformationCollection.Add(l.BasicInfo.Code, new ItemInformation(l.BasicInfo.Code));
        }
    
    }
}
