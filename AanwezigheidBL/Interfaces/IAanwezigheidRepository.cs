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

        public List<Speler> LeesSpelersVanTeam(int teamID); //Aanpassen
        public bool HeeftTeam(Team team);
        public bool HeeftLetsel(Letsel letsel);
        public void SchrijfLetsel(Letsel letsel);
        public bool HeeftTraining(Training training);
        public void SchrijfTraining(Training training);
        public void SchrijfWijzigingSpeler(Speler oldSpeler, Speler newSpeler);
        public void VerwijderSpelerVanDB(Speler speler);
        public double LeesPercentageAanwezigheid(int spelerID); //Aanpassen
        public List<Team> LeesTeams();

        public bool HeeftSpeler(Speler speler);
        public void SchrijfSpeler(Speler speler);
        public bool HeeftAanwezigheid(Aanwezigheid aanwezigheid);
        public void SchrijfAanwezigheid(Aanwezigheid aanwezigheid);
        public void SchrijfWijzigingTraining(Training oldTraining, Training newTraining);
        #region Nog niet gebruiken

        public void SchrijfTeam(Team team);
        public List<Coach> LeesCoaches();
        public void SchrijfCoach(Coach coach);
        public bool HeeftCoach(Coach coach);
        public List<Speler> LeesSpelers();
        public List<Letsel> LeesLetsels();
        public List<Training> LeesTrainingen();
        public List<Aanwezigheid> LeesAanwezigheden();



        #endregion



    }
}
