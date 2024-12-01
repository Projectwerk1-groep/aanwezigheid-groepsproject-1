using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AanwezigheidBL.Model
{
    public class Team
    {
        //We zullen hier een constructor toevoegen met alle eigenschappen van deze klasse, omdat we het nodig hebben om het aanmaken van objecten in de data-laag te vergemakkelijken.
        public Team(int teamID, string teamNaam, Coach coach)
        {
            TeamID = teamID;
            TeamNaam = teamNaam;
            Coach = coach;
        }
        public Team(string teamNaam, Coach coach)
        {
            TeamNaam = teamNaam;
            Coach = coach;
        }

        public int? TeamID;
        public string TeamNaam { get; set; }
        public Coach Coach { get; set; }

        public List<Speler> Spelers { get; set; }
    }
}
