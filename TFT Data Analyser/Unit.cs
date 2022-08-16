using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFT_Data_Analyser
{
    public class Unit
    {
        public string character_id { get; set; }
        public List<int> items { get; set; }
        public string name { get; set; }
        public int rarity { get; set; }
        public int tier { get; set; }

        public Unit(string character_id, List<int> items, string name, int rarity, int tier)
        {
            this.character_id = character_id;
            this.items = items;
            this.name = name;
            this.rarity = rarity;
            this.tier = tier;
        }
    }
}
