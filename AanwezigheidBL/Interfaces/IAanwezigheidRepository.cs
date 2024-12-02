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
        List<Speler> LeesSpelersVanTeam(int teamID); //Aanpassen
        bool HeeftTeam(Team team);
        bool HeeftLetsel(Letsel letsel);
        void SchrijfLetsel(Letsel letsel);
        bool HeeftTraining(Training training);
        void SchrijfTraining(Training training);
        void SchrijfWijzigingSpeler(Speler oldSpeler, Speler newSpeler);
        void VerwijderSpelerVanDB(Speler speler);
        double LeesPercentageAanwezigheid(int spelerID); //Aanpassen
        List<Team> LeesTeams();
        List<Team> LeesTeamsPerCoach(int? coachId);
        Training LeesTrainingOmAanwezighedenTeMaken(Training trainingZonderID); //Aanpassen
        bool HeeftSpeler(Speler speler);
        void SchrijfSpeler(Speler speler);
        bool HeeftAanwezigheid(Aanwezigheid aanwezigheid);
        void SchrijfAanwezigheid(Aanwezigheid aanwezigheid);
        void SchrijfWijzigingTraining(Training oldTraining, Training newTraining);
        List<Training> LeesTrainingenVanTeam(int teamId);
        void LeesEnSchrijfAanwezigheidPerTrainingInTXT(Training training, Team team, string filePath);
        Training LeesTraining(int trainingId);

        #region Nog niet gebruiken

        void SchrijfTeam(Team team);
        List<Coach> LeesCoaches();
        void SchrijfCoach(Coach coach);
        bool HeeftCoach(Coach coach);
        List<Speler> LeesSpelers();
        List<Letsel> LeesLetsels();
        List<Training> LeesTrainingen();
        List<Aanwezigheid> LeesAanwezigheden();



        #endregion



    }
}
