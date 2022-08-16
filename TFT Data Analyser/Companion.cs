using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFT_Data_Analyser
{
    public class Companion
    {
        public string content_ID;
        public int skin_ID;
        public string species;

        public Companion(string content_ID, int skin_ID, string species)
        {
            this.content_ID = content_ID;
            this.skin_ID = skin_ID;
            this.species = species;
        }
    }
}
