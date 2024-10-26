using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AanwezigheidProject
{
    public class Aanwezigheid
    {  
        public int SpelerID { get; set; }
        public int TrainingID { get; set; }
        public bool IsAanwezig { get; set; }
        public bool HeeftAfwezigheidGemeld { get; set; }
        public string RedenAfwezigheid { get; set; }
    }
}
