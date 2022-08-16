namespace TFT_Data_Analyser
{
    public class PlayerProfile
    {
        public string summonerId { get; set; }
        public string summonerName { get; set; }
        public int leaguePoints { get; set; }
        public string rank { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public bool inactive { get; set; }
        public bool freshBlood { get; set; }
        public bool hotStreak { get; set; }
        public string puuid { get; set; }
        public List<String> matchids = new List<String>();

        public PlayerProfile(string summonerId, string summonerName, int leaguePoints, string rank, int wins, int losses, bool inactive, bool freshBlood, bool hotStreak)
        {
            this.summonerId = summonerId;
            this.summonerName = summonerName;
            this.leaguePoints = leaguePoints;
            this.rank = rank;
            this.wins = wins;
            this.losses = losses;
            this.inactive = inactive;
            this.freshBlood = freshBlood;
            this.hotStreak = hotStreak;
        }

    }
}
