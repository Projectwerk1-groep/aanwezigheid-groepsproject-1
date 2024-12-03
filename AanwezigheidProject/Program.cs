using AanwezigheidBL.Interfaces;
using AanwezigheidBL.Managers;
using AanwezigheidBL.Model;
using AanwezigheidDL_SQL;


namespace AanwezigheidProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            string conn = "Data Source=Gaith\\SQLEXPRESS01;Initial Catalog=DB_Aanwezigheid;Integrated Security=True;Trust Server Certificate=True";
            IAanwezigheidRepository aanwezigheidRepository = new AanwezigheidRepository(conn);
            AanwezigheidManager aanwezigheidManager = new AanwezigheidManager(aanwezigheidRepository);

            //================================================================================
            //VoegCoachToe
            Coach coach = new Coach("Intesar");
            aanwezigheidManager.VoegCoachToe(coach);
            Console.WriteLine($"{nameof(aanwezigheidManager.VoegCoachToe)} is getest");

            //================================================================================
            //GeefCoaches
            List<Coach> coaches = aanwezigheidManager.GeefCoaches();
            foreach (Coach c in coaches)
            {
                //Console.WriteLine($"CoachNaam: {c.Naam} , CoachID: {c.CoachID}");
            }
            Console.WriteLine($"{nameof(aanwezigheidManager.GeefCoaches)} is getest");


            //================================================================================
            // VoegTeamToe
            Team realMadrid = new Team("Real Madrid", aanwezigheidManager.GeefCoaches()[0]);
            aanwezigheidManager.VoegTeamToe(realMadrid);
            Console.WriteLine($"{nameof(aanwezigheidManager.VoegTeamToe)} is getest");

            //================================================================================
            //GeefTeams
            List<Team> teams = aanwezigheidManager.GeefTeams();
            foreach (Team t in teams)
            {
                //Console.WriteLine($"TeamNaam: {t.TeamNaam} , CoachNaam: {t.Coach.Naam}");
            }
            Console.WriteLine($"{nameof(aanwezigheidManager.GeefTeams)} is getest");

            //================================================================================
            // VoegSpelerToe
            Speler speler = new Speler("Gaith", 7, aanwezigheidManager.GeefTeams()[0]);
            aanwezigheidManager.VoegSpelerToe(speler);
            Console.WriteLine($"{nameof(aanwezigheidManager.VoegSpelerToe)} is getest");

            //================================================================================
            //GeefSpelersVanTeam
            List<Speler> spelers = aanwezigheidManager.GeefSpelersVanTeam(aanwezigheidManager.GeefTeams()[0].TeamID);
            foreach (Speler s in spelers)
            {
                Console.WriteLine($"SpelerID: {s.SpelerID} , Naam: {s.Naam} , RugNummer: {s.RugNummer} , TeamNaam: {s.Team.TeamNaam}");
            }
            Console.WriteLine($"{nameof(aanwezigheidManager.GeefSpelersVanTeam)} is getest");


            //================================================================================
            // WijzigSpeler
            Speler newSpeler = new Speler("Orlando", 9, aanwezigheidManager.GeefTeams()[0]);
            aanwezigheidManager.WijzigSpeler(spelers[0], newSpeler);
            foreach (Speler s in spelers)
            {
                Console.WriteLine($"SpelerID: {s.SpelerID} , Naam: {s.Naam} , RugNummer: {s.RugNummer} , TeamNaam: {s.Team.TeamNaam}");
            }
            Console.WriteLine($"{nameof(aanwezigheidManager.WijzigSpeler)} is getest");


            //================================================================================
            // VerwijderSpeler
            //aanwezigheidManager.VerwijderSpeler(spelers[0]);
            //foreach (Speler s in aanwezigheidManager.GeefSpelersVanTeam(aanwezigheidManager.GeefTeams()[0].TeamID))
            //{
            //    Console.WriteLine($"SpelerID: {s.SpelerID} , Naam: {s.Naam} , RugNummer: {s.RugNummer} , TeamNaam: {s.Team.TeamNaam}");
            //}
            //Console.WriteLine($"{nameof(aanwezigheidManager.VerwijderSpeler)} is getest");

            //================================================================================
            // VoegTrainingToe
            Training training = new Training(DateTime.Now, "Man to man", teams[0]);
            aanwezigheidManager.VoegTrainingToe(training);
            Console.WriteLine($"{nameof(aanwezigheidManager.VoegTrainingToe)} is getest");

            //================================================================================
            // GeefTrainingenVanTeam
            List<Training> trainingen = aanwezigheidManager.GeefTrainingenVanTeam(1);
            foreach (Training t in trainingen)
            {
                //Console.WriteLine($"TrainingID: {t.TrainingID} , Datum: {t.Datum.Date} , Thema: {t.Thema} , TeamNaam: {t.Team.TeamNaam}");
            }
            Console.WriteLine($"{nameof(aanwezigheidManager.GeefTrainingenVanTeam)} is getest");

            //================================================================================
            // GeefTraining
            Training training1 = aanwezigheidManager.GeefTraining(1);
            Console.WriteLine($"TrainingID: {training1.TrainingID} , Datum: {training1.Datum.Date} , Thema: {training1.Thema} , TeamNaam: {training1.Team.TeamNaam}");
            Console.WriteLine($"{nameof(aanwezigheidManager.GeefTraining)} is getest");

            //================================================================================
            //VoegAanwezigheidToe
            Aanwezigheid aanwezigheid = new Aanwezigheid(spelers[1], training1, true, false, "");
            aanwezigheidManager.VoegAanwezigheidToe(aanwezigheid);
            Console.WriteLine($"{nameof(aanwezigheidManager.VoegAanwezigheidToe)} is getest");

            //================================================================================
            //ExportAanwezigheidNaarTXT
            aanwezigheidManager.ExportAanwezigheidNaarTXT(training1, teams[0], "C:\\Users\\Gaith Alsahaf\\Desktop\\HoGent\\Graduaat\\2de jaar\\Sem 1\\Projectwerk1\\Project Aanwezigheden\\txt.txt");
            Console.WriteLine($"{nameof(aanwezigheidManager.ExportAanwezigheidNaarTXT)} is getest");

            //================================================================================
            //GeefPercentageAanwezigheid
            Console.WriteLine(aanwezigheidManager.GeefPercentageAanwezigheid(7));
            Console.WriteLine($"{nameof(aanwezigheidManager.GeefPercentageAanwezigheid)} is getest");

            //================================================================================
            //GeefTeamsPerCoach
            List<Team> teams1 = aanwezigheidManager.GeefTeamsPerCoach(coaches[0].CoachID);
            foreach (Team t in teams)
            {
                //Console.WriteLine($"TeamNaam: {t.TeamNaam} , CoachNaam: {t.Coach.Naam}");
            }
            Console.WriteLine($"{nameof(aanwezigheidManager.GeefTeamsPerCoach)} is getest");

            //================================================================================
            //GeefCoaches
            List<Coach> coaches1 = aanwezigheidManager.GeefCoaches();
            foreach (Coach c in coaches1)
            {
                Console.WriteLine($"CoachID: {c.CoachID} , CoachNaam: {c.Naam}");
            }
            Console.WriteLine($"{nameof(aanwezigheidManager.GeefCoaches)} is getest");

            //================================================================================
            //VoegLetselToe
            Letsel letsel = new Letsel(aanwezigheidManager.GeefSpelersVanTeam(1)[0], "kruisbandblessure", DateTime.Now, "geen");
            aanwezigheidManager.VoegLetselToe( letsel );
            Console.WriteLine($"{nameof(aanwezigheidManager.VoegLetselToe)} is getest");

            //================================================================================
            //VoegTrainingMetAanwezigheidToe
            Training training2 = new Training (DateTime.Now, "attack", aanwezigheidManager.GeefTeams()[0]);
            (Speler speler, bool isAanwezig, bool heeftAfwezigheidGemeld, RedenVanAfwezigheid redenAfwezigheid, string letselType, DateTime letselDatum, string notities) aanwezigheden1 = (aanwezigheidManager.GeefSpelersVanTeam(1)[0],false, true, RedenVanAfwezigheid.Ziekte,null,DateTime.Now,null);
            List<(Speler speler, bool isAanwezig, bool heeftAfwezigheidGemeld, RedenVanAfwezigheid redenAfwezigheid, string letselType, DateTime letselDatum, string notities)> listAanwezigheden = new();
            listAanwezigheden.Add(aanwezigheden1);
            aanwezigheidManager.VoegTrainingMetAanwezigheidToe(training2, listAanwezigheden);
            //================================================================================












        }
    }
}
