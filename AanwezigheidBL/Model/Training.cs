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
        public Training()
        {
            Spelers = new List<Speler>();
        }
        public int Id { get; set; }
        public DateTime Datum{ get; set; }
        public string Thema { get; set; }
        public List<Speler> Spelers { get; set; }
    }
}
