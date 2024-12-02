using AanwezigheidBL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AanwezigheidBL.Model
{
    public class Coach
    {
        //We zullen hier een constructor toevoegen met alle eigenschappen van deze klasse, omdat we het nodig hebben om het aanmaken van objecten in de data-laag te vergemakkelijken.
        public Coach(int coachID, string naam)
        {
            CoachID = coachID;
            Naam = naam;
        }
        public Coach(string naam)
        {
            Naam = naam;
        }
        public int CoachID;

        private string _naam;

        public string Naam
        {
            get { return _naam; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new DomeinException("De naam van de speler mag niet leeg zijn.");
                }
                _naam = value;
            }
        }
    }
}
