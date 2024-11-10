using AanwezigheidBL.Exceptions;
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
        //=======================================================================================================
        public void VoegSpelerToe(Speler speler)
        {
            try
            {
                if (!_aanwezigheidRepository.HeeftSpeler(speler))
                    _aanwezigheidRepository.SchrijfSpeler(speler);
            }
            catch (SpelerException ex)
            {

            }
        }
        //=======================================================================================================
        public void VoegAanwezigheidToe(Aanwezigheid aanwezigheid)
        {
            try
            {
                if (!_aanwezigheidRepository.HeeftAanwezigheid(aanwezigheid))
                    _aanwezigheidRepository.SchrijfAanwezigheid(aanwezigheid);
            }
            catch (SpelerException ex)
            {

            }
        }
        //=======================================================================================================
        public void VoegCoachToe(Coach coach)
        {
            try
            {
                if (!_aanwezigheidRepository.HeeftCoach(coach))
                    _aanwezigheidRepository.SchrijfCoach(coach);
            }
            catch (SpelerException ex)
            {

            }
        }
    }
}
