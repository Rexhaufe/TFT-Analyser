using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFT_Data_Analyser
{
    public class Trait
    {
        public string name { get; set; }
        public int numi_units { get; set; }
        public int style { get; set; }
        public int tier_current { get; set; }
        public int tier_total { get; set; }

        public Trait(string name, int numi_units, int style, int tier_current, int tier_total)
        {
            this.name = name;
            this.numi_units = numi_units;
            this.style = style;
            this.tier_current = tier_current;
            this.tier_total = tier_total;
        }
    }
}
