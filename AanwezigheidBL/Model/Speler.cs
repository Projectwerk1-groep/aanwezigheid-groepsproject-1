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
        public Speler(int spelerID, string naam, int rugNummer, string positie, Team team)
        {
            SpelerID = spelerID;
            Naam = naam;
            RugNummer = rugNummer;
            Positie = positie;
            Team = team;
        }
        public Speler() { }
        public int SpelerID { get; set; }
        private string _naam;
        public string Naam
        {
            get { return _naam; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new SpelerException("De naam van de speler mag niet leeg zijn.");
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
                if (value < 1 || value > 99) { throw new SpelerException("De rugnummer kan enkel tussen 1 en 99 zijn."); }
                _rugNummer = value;
            }
        }
        public string Positie { get; set; }
        public Team Team { get; set; }
    }
}