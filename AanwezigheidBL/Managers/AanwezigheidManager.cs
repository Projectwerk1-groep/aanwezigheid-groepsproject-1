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

        #region Speler
        public void VoegSpelerToe(Speler speler)
        {
            try
            {
                if (!_aanwezigheidRepository.HeeftSpeler(speler))
                    _aanwezigheidRepository.SchrijfSpeler(speler);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(VoegSpelerToe), ex);
            }
        }
        public void WijzigSpeler(Speler oldSpeler, Speler newSpeler)
        {
            try
            {
                _aanwezigheidRepository.SchrijfWijzigingSpeler(oldSpeler, newSpeler);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(WijzigSpeler), ex);
            }
        }
        public void VerwijderSpeler(Speler speler)
        {
            try
            {
                _aanwezigheidRepository.VerwijderSpelerVanDB(speler);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(VerwijderSpeler), ex);

            }
        }
        public List<Speler> GeefSpelersVanTeam(int teamID)
        {
            try
            {
                return _aanwezigheidRepository.LeesSpelersVanTeam(teamID);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefSpelersVanTeam), ex);
            }
        }

        #endregion

        #region Training

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
                throw new ManagerException(nameof(VoegTrainingToe), ex);
            }
        }
        public void WijzigTraining(Training oldTraining, Training newTraining)
        {
            try
            {
                _aanwezigheidRepository.SchrijfWijzigingTraining(oldTraining, newTraining);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(WijzigTraining), ex);
            }
        }
        public List<Training> GeefTrainingenVanTeam(int teamID)
        {
            try
            {
                return _aanwezigheidRepository.LeesTrainingenVanTeam(teamID);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefTrainingenVanTeam), ex);
            }
        }

        #endregion

        #region Aanwezigheid
        public void VoegAanwezigheidToe(Aanwezigheid aanwezigheid)
        {
            try
            {
                if (!_aanwezigheidRepository.HeeftAanwezigheid(aanwezigheid))
                    _aanwezigheidRepository.SchrijfAanwezigheid(aanwezigheid);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(VoegAanwezigheidToe), ex);
            }
        }
        public void ExportAanwezigheidNaarTXT(Training training, Team team, string filePath)
        {
            try
            {
                _aanwezigheidRepository.LeesEnSchrijfAanwezigheidPerTrainingInTXT(training, team, filePath);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(ExportAanwezigheidNaarTXT), ex);
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
                throw new ManagerException(nameof(GeefPercentageAanwezigheid), ex);
            }

        }

        #endregion

       
        public void VoegTrainingMetAanwezigheidToe(Training training, List<(Speler speler, bool isAanwezig, bool heeftAfwezigheidGemeld, RedenVanAfwezigheid redenAfwezigheid, string letselType, DateTime letselDatum, string notities)> listOmAanwezighedenTeMaken)
        {
            try
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
            catch (Exception ex)
            {
                throw new ManagerException(nameof(VoegTrainingMetAanwezigheidToe), ex);
            }

        }
        public List<Team> GeefTeams()
        {
            try
            {
                return _aanwezigheidRepository.LeesTeams();
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefTeams), ex);
            }
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
                throw new ManagerException(nameof(VoegLetselToe), ex);
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
            catch (Exception ex)
            {
                throw new ManagerException(nameof(VoegCoachToe), ex);
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
            catch (Exception ex)
            {
                throw new ManagerException(nameof(VoegTeamToe), ex);
            }
        }
        #endregion

    }
}
