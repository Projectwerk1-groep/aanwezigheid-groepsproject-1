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
        public Speler Speler { get; set; }
        public Training Training { get; set; }
        public bool IsAanwezig { get; set; }
        public bool HeeftAfwezigheidGemeld { get; set; }
        public string RedenAfwezigheid { get; set; }
    }
}