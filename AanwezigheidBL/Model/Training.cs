using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AanwezigheidBL.Exceptions;


namespace AanwezigheidBL.Model
{
    public class Training
    {
        //We zullen hier een constructor toevoegen met alle eigenschappen van deze klasse, omdat we het nodig hebben om het aanmaken van objecten in de data-laag te vergemakkelijken.
        public Training(int trainingID, DateTime datum, string thema, Team team)
        {
            TrainingID = trainingID;
            Datum = datum;
            Thema = thema;
            Team = team;

        }
        public Training() { }

        public int TrainingID { get; set; }
        public DateTime Datum { get; set; }
        public string Thema { get; set; }
        public Team Team { get; set; }
    }
}
