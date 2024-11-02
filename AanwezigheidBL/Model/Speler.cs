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
        public int Id { get; set; }
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
    }
}