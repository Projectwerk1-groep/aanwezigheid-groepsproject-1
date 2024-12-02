using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AanwezigheidBL.Exceptions;

namespace AanwezigheidBL.Model
{
    public class Speler
    {
        //We zullen hier een constructor toevoegen met alle eigenschappen van deze klasse, omdat we het nodig hebben om het aanmaken van objecten in de data-laag te vergemakkelijken.
        public Speler(int spelerID, string naam, int rugNummer, Team team)
        {
            SpelerID = spelerID;
            Naam = naam;
            RugNummer = rugNummer;
            Team = team;
        }
        public Speler(string naam, int rugNummer, Team team)
        {
            Naam = naam;
            RugNummer = rugNummer;
            Team = team;
        }
        public int SpelerID;
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
        private int _rugNummer;
        public int RugNummer
        {
            get { return _rugNummer; }
            set
            {
                if (value < 1 || value > 99) { throw new DomeinException("De rugnummer kan enkel tussen 1 en 99 zijn."); }
                _rugNummer = value;
            }
        }
        public Team Team { get; set; }
    }
}