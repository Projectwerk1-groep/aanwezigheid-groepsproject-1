using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AanwezigheidBL.Model
{
    public class Letsel
    {
        //We zullen hier een constructor toevoegen met alle eigenschappen van deze klasse,
        //omdat we het nodig hebben om het aanmaken van objecten in de data-laag te vergemakkelijken.
        public Letsel(int letselID, Speler speler, string letselType, DateTime letselDatum, string notities)
        {
            LetselID = letselID;
            Speler = speler;
            LetselType = letselType;
            LetselDatum = letselDatum;
            Notities = notities;
        }
        public Letsel(Speler speler, string letselType, DateTime letselDatum, string notities)
        {
            Speler = speler;
            LetselType = letselType;
            LetselDatum = letselDatum;
            Notities = notities;
        }

        public int? LetselID;
        public Speler Speler { get; set; }
        public string? LetselType { get; set; }
        public DateTime? LetselDatum { get; set; }
        public string? Notities { get; set; }

    }
}
