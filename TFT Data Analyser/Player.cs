using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFT_Data_Analyser
{
    public class Player
    {
        public List<string> augments = new List<string>();
        public Companion companion { get; set; }
        public int gold_left { get; set; }
        public int last_round { get; set; }
        public int level { get; set; }
        public int partner_group_id { get; set; }
        public int placement { get; set; }
        public int players_eliminated { get; set; }
        public string puuid { get; set; }
        public double time_eliminated { get; set; }
        public int total_damage_to_players { get; set; }
        public List<Trait> traits;
        public List<Unit> units;

        public Player(List<string> augments, Companion companion, int gold_left, int last_round, int level, int partner_group_id,int placement,int players_eliminated, string puuid, double time_eliminated, int total_damage_to_players, List<Trait> traits, List<Unit> units)
        {
            this.augments = augments;
            this.companion = companion;
            this.gold_left = gold_left;
            this.last_round = last_round;
            this.level = level;
            this.partner_group_id = partner_group_id;
            this.placement = placement;
            this.players_eliminated = players_eliminated;
            this.puuid = puuid;
            this.time_eliminated = time_eliminated;
            this.total_damage_to_players = total_damage_to_players;
            this.traits = traits;
            this.units = units;
        }
    }
}
