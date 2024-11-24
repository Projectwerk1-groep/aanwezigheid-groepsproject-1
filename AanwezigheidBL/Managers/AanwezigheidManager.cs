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

        public void VoegLetselToe(Letsel letsel)
        {
            try
            {
                if (!_aanwezigheidRepository.HeeftLetsel(letsel))
                {
                    _aanwezigheidRepository.SchrijfLetsel(letsel);
                }
            }
            catch (Exception ex)
            {
                throw new SpelerException(nameof(VoegLetselToe), ex);
            }
        }
        public void VoegTrainingToe(Training training)
        {
            try
            {
                if (!_aanwezigheidRepository.HeeftTraining(training))
                {
                    _aanwezigheidRepository.SchrijfTraining(training);
                }
            }
            catch (Exception ex)
            {
                throw new SpelerException(nameof(VoegTrainingToe), ex);
            }
        }
        public void WijzigSpeler(Speler oldSpeler, Speler newSpeler)
        {
            try
            {
                _aanwezigheidRepository.SchrijfWijzigingSpeler(oldSpeler, newSpeler);
            }
            catch (SpelerException ex)
            {

            }
        }
        public void VerwijderSpeler(Speler speler)
        {
            try
            {
                _aanwezigheidRepository.VerwijderSpelerVanDB(speler);
            }
            catch (SpelerException ex)
            {

            }
        }
        public double GeefPercentageAanwezigheid(int spelerID)
        {
            try
            {
                return _aanwezigheidRepository.LeesPercentageAanwezigheid(spelerID);
            }
            catch (Exception ex)
            {
                throw new SpelerException(nameof(GeefPercentageAanwezigheid), ex);
            }

        }
        public void VoegTrainingMetZijnAanwezigheidToe(Training training, List<(Speler speler, bool isAanwezig, bool heeftAfwezigheidGemeld, RedenVanAfwezigheid redenAfwezigheid, string letselType, DateTime letselDatum, string notities)> listOmAanwezighedenTeMaken)
        {
            VoegTrainingToe(training);
            Training trainingMetID = _aanwezigheidRepository.LeesTrainingOmAanwezighedenTeMaken(training);
            foreach (var lijn in listOmAanwezighedenTeMaken)
            {
                string reden = "";
                switch (lijn.redenAfwezigheid)
                {
                    case RedenVanAfwezigheid.Letsel: reden = "Letsel"; break;
                    case RedenVanAfwezigheid.Ziekte: reden = "Ziekte"; break;
                    case RedenVanAfwezigheid.Andere: reden = "Andere"; break;
                }
                Aanwezigheid aanwezigheid = new Aanwezigheid(lijn.speler, trainingMetID, lijn.isAanwezig, lijn.heeftAfwezigheidGemeld, reden);
                VoegAanwezigheidToe(aanwezigheid);


                if (lijn.redenAfwezigheid == RedenVanAfwezigheid.Letsel)
                {
                    Letsel letsel = new Letsel(lijn.speler, lijn.letselType, lijn.letselDatum, lijn.notities);
                    VoegLetselToe(letsel);
                }
            }
        }
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
        public void WijzigTraining(Training oldTraining, Training newTraining)
        {
            try
            {
                _aanwezigheidRepository.SchrijfWijzigingTraining(oldTraining, newTraining);
            }
            catch (SpelerException ex)
            {

            }
        }

        #region Nog niet gebruiken methodes
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
        public void VoegTeamToe(Team team)
        {
            try
            {
                if (!_aanwezigheidRepository.HeeftTeam(team))
                {
                    _aanwezigheidRepository.SchrijfTeam(team);
                }
            }
            catch (SpelerException ex)
            {

            }
        }
        #endregion






    }
}
