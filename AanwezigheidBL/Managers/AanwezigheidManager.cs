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

        //VoegSpelerToe: Deze methode voegt een speler toe aan de database.
        public void VoegSpelerToe(Speler speler) // getest door Intesar
        {
            try
            {
                if (_aanwezigheidRepository.HeeftSpelerBijToevoeging(speler))
                    throw new Exception(); 
                
                _aanwezigheidRepository.SchrijfSpeler(speler);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(VoegSpelerToe), ex);
            }
        }
        //WijzigSpeler: Deze methode neemt een Speler-object vóór de wijziging en een nieuw Speler-object (na de wijziging) om de gegevens van de speler aan te passen, met uitzondering van het ID. En kan worden gebruikt op de overzicht-pagina in de UI.
        public void WijzigSpeler(Speler oldSpeler, Speler newSpeler) // getest door Gaith
        {
            try
            {
                if (_aanwezigheidRepository.HeeftSpelerBijWijziging(newSpeler))
                    throw new Exception();

                _aanwezigheidRepository.SchrijfWijzigingSpeler(oldSpeler, newSpeler);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(WijzigSpeler), ex);
            }
        }
        //VerwijderSpeler: Deze methode verwijdert de speler uit de database. Het kan worden gebruikt op beide pagina's in de UI.
        public void VerwijderSpeler(Speler speler) // getest door Gaith
        {
            try
            {
                if (_aanwezigheidRepository.HeeftAanwezigheidOpSpeler(speler))
                    _aanwezigheidRepository.VerwijderAanwezigheidVanDB(speler); // getest door Orlando

                _aanwezigheidRepository.VerwijderSpelerVanDB(speler);               
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(VerwijderSpeler), ex);

            }
        }
        //GeefSpelersVanTeam: Deze methode retourneert een lijst met spelers van een specifiek team.
        public List<Speler> GeefSpelersVanTeam(int teamID) // getest door Gaith
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

        //VoegTrainingToe: Deze methode voegt een training toe aan de database nadat eerst wordt gecontroleerd of deze al bestaat. Als er al een training met dezelfde datum, thema en details bestaat, voorkomt deze methode dat deze aan de database wordt toegevoegd.
        public void VoegTrainingToe(Training training) // getest door Gaith
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
        //WijzigTraining: Deze methode neemt een Training-object vóór de wijziging en een bijgewerkt Training-object (na de wijziging) en vervangt het oude object door het nieuwe in de database.
        public void WijzigTraining(Training oldTraining, Training newTraining) // getest door Intesar
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
        //GeefTrainingenVanTeam: Deze methode retourneert alle eerdere trainingen van een specifiek team en kan worden gebruikt op de details-pagina.
        public List<Training> GeefTrainingenVanTeam(int teamID) // getest door Intesar
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
        //LeesTraining: Deze methode retourneert één specifieke training op basis van het ID.
        public Training GeefTraining(int trainingId) // getest door Gaith
        {
            try
            {
                return _aanwezigheidRepository.LeesTraining(trainingId);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefTraining), ex);
            }
        }

        #endregion

        #region Aanwezigheid
        //VoegAanwezigheidToe: Deze methode voegt de aanwezigheid toe aan de database.
        public void VoegAanwezigheidToe(Aanwezigheid aanwezigheid) // getest door Intesar
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
        //ExportAanwezigheidNaarTXT:Deze methode exporteert de aanwezigheidsgegevens van elke speler, gebaseerd op het team en de training, naar een tekstbestand
        public void ExportAanwezigheidNaarTXT(Training training, Team team, string filePath) // getest door Intesar
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
        //GeefPercentageAanwezigheid: Deze methode berekent het aanwezigheidspercentage van een speler bij de trainingendoor het aantal trainingen dat hij heeft bijgewoond te delen door het totale aantal trainingen dat hij had kunnen bijwonen. Deze methode kan worden gebruikt op de details-pagina in de UI.
        public double GeefPercentageAanwezigheid(int spelerID) // getest door Gaith
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

        /* VoegTrainingMetAanwezigheidToe:Deze methode wordt gebruikt op de overzicht-pagina in de UI. Ze voegt één training en meerdere aanwezigheden toe (afhankelijk van het aantal spelers in het geselecteerde team) aan de database.
                                              Nadat de training is toegevoegd, wordt deze opgehaald uit de database om het ID te verkrijgen dat door de database wordt gegenereerd.
                                              Met deze training, inclusief het verkregen ID, kunnen we de aanwezigheid voor elke speler afzonderlijk toevoegen, gebaseerd op de informatie die beschikbaar is in de regels van de ListBox "Overzicht van spelers".
                                              Daarom leest deze methode de inhoud van deze ListBox en zet deze over naar de database.*/
        public void VoegTrainingMetAanwezigheidToe(Training training, List<Aanwezigheid> listOmAanwezighedenTeMaken)
        {
            try
            {
                VoegTrainingToe(training);
                Training trainingMetID = _aanwezigheidRepository.LeesTrainingen().Last();
                foreach (var lijn in listOmAanwezighedenTeMaken)
                {
                    //string reden = "";
                    //switch (lijn.RedenAfwezigheid)
                    //{
                    //    case "Letsel": reden = "Letsel"; break;
                    //    case "Ziekte": reden = "Ziekte"; break;
                    //    case "Andere": reden = "Andere"; break;
                    //}
                    Aanwezigheid aanwezigheid = new(lijn.Speler, trainingMetID, lijn.IsAanwezig, lijn.HeeftAfwezigheidGemeld, lijn.RedenAfwezigheid);
                    VoegAanwezigheidToe(aanwezigheid);


                    //if (lijn.redenAfwezigheid == RedenVanAfwezigheid.Letsel)
                    //{
                    //    Letsel letsel = new(lijn.speler, lijn.letselType, lijn.letselDatum, lijn.notities);
                    //    VoegLetselToe(letsel);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(VoegTrainingMetAanwezigheidToe), ex);
            }

        }
        //GeefTeams: Deze methode controleert of dit team al in de database bestaat voordat we details eraan toevoegen.
        public List<Team> GeefTeams() // getest door Intesar
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

        public List<Team> GeefTeamsPerCoach(int coachId) // getest door Intesar
        {
            try
            {
                return _aanwezigheidRepository.LeesTeamsPerCoach(coachId);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefTeamsPerCoach), ex);
            }
        }

        public List<Coach> GeefCoaches() // getest door Intesar
        {
            try
            {
                return _aanwezigheidRepository.LeesCoaches();
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefCoaches), ex);
            }
        }

        //VoegLetselToe:Deze methode voegt een letsel toe aan een speler in de database.
        public void VoegLetselToe(Letsel letsel) // getest door Gaith
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

        public void VoegCoachToe(Coach coach) // getest door Intesar
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
        public void VoegTeamToe(Team team) // getest door Gaith
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

    }
}
