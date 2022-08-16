using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFT_Data_Analyser
{
    public class Match
    {
        public string matchId { get; set; }
        public int dateTime { get; set; }
        public int gameTime { get; set; }
        public string gameVersion { get; set; }
        public List<Player> participants = new List<Player>();

        public Match(string matchId, int dateTime, int gameTime, string gameVersion, List<Player> participants)
        {
            this.matchId = matchId;
            this.dateTime = dateTime;
            this.gameTime = gameTime;
            this.gameVersion = gameVersion;
            this.participants = participants;
        }
    }
}
