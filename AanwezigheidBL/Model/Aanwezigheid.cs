using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AanwezigheidBL.Exceptions;


namespace AanwezigheidBL.Model
{
    public class Aanwezigheid
    {
        //We zullen hier een constructor toevoegen met alle eigenschappen van deze klasse, omdat we het nodig hebben om het aanmaken van objecten in de data-laag te vergemakkelijken.
        public Aanwezigheid(Speler speler, Training training, bool isAanwezig, bool heeftAfwezigheidGemeld, string redenAfwezigheid)
        {
            Speler = speler;
            Training = training;
            IsAanwezig = isAanwezig;
            HeeftAfwezigheidGemeld = heeftAfwezigheidGemeld;
            RedenAfwezigheid = redenAfwezigheid;
        }
        public Aanwezigheid() { }

        public Speler Speler { get; set; }
        public Training Training { get; set; }
        public bool IsAanwezig { get; set; }
        public bool? HeeftAfwezigheidGemeld { get; set; }
        public string? RedenAfwezigheid { get; set; }
    }
}