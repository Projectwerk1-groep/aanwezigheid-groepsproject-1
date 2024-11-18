﻿using System;
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
        public bool HeeftSpeler(Speler speler);
        public void SchrijfSpeler(Speler speler);
        public bool HeeftAanwezigheid(Aanwezigheid aanwezigheid);
        public void SchrijfAanwezigheid(Aanwezigheid aanwezigheid);
        public bool HeeftCoach(Coach coach);
        public void SchrijfCoach(Coach coach);
        public bool HeeftTeam(Team team);
        public void SchrijfTeam(Team team);
        public bool HeeftTraining(Training training);
        public void SchrijfTraining(Training training);
        public List<Letsel> LeesLetsels();
        public bool HeeftLetsel(Letsel letsel);
        public void SchrijfLetsel(Letsel letsel);
        public void SchrijfWijzigingTraining(Training oldTraining, Training newTraining);
        public void SchrijfWijzigingSpeler(Speler oldSpeler, Speler newSpeler);
        public void VerwijderSpelerVanDB(Speler speler);

    }
}
