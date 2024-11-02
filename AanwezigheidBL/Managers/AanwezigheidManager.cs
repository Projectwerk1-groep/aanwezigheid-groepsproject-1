using AanwezigheidBL.Interfaces;
using AanwezigheidBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AanwezigheidBL.Managers
{
    public class AanwezigheidManager
    {
        private IAanwezigheidRepository _aanwezigheidRepository;

        public AanwezigheidManager(IAanwezigheidRepository aanwezigheidRepository)
        {
            _aanwezigheidRepository = aanwezigheidRepository;
        }

        public double BereekAanwezigheidsPercentage(Speler speler)
        {
            List<Aanwezigheid> alleTrainingen = _aanwezigheidRepository.LeesAanwezigheden();
            List<Aanwezigheid> alleTrainingenVanEenSpeler = alleTrainingen.Where(a => a.Speler == speler).ToList();
            int aantalDeelnames = alleTrainingenVanEenSpeler.Where(a => a.IsAanwezig).ToList().Count;

            return aantalDeelnames / alleTrainingenVanEenSpeler.Count * 100;
        }


    }
}
