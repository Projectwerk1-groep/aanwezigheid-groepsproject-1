using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AanwezigheidBL.Model;



namespace AanwezigheidBL.Interfaces
{
    public interface IAanwezigheidRepository
    {
        public List<Coach> LeesCoaches();
        public List<Team> LeesTeams();
        public List<Training> LeesTrainingen();
        public List<Speler> LeesSpelers();
        public List<Aanwezigheid> LeesAanwezigheden();
    }
}
