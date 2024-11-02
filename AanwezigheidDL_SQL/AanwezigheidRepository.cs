using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AanwezigheidBL.Interfaces;
using AanwezigheidBL.Model;
using AanwezigheidBL.Exceptions;


namespace AanwezigheidDL_SQL
{
    public class AanwezigheidRepository : IAanwezigheidRepository
    {
        private string _connectionString;

        public AanwezigheidRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        //=======================================================================================================
        //Een methode die de inhoud van de tabel Coach in de database leest en een lijst van objecten van het type Coach retourneert.
        public List<Coach> LeesCoaches()
        {
            string SQL = "SELECT * FROM Coach";
            List<Coach> Coaches = new List<Coach>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Coaches.Add(new Coach((int)reader["coachID"], (string)reader["naam"]));
                    }
                    return Coaches;
                }
                catch (Exception ex)
                {
                    throw new SpelerException("LeesCoaches", ex);
                }
            }
        }
        //=======================================================================================================
        // Een methode die de inhoud van de tabel Team in de database leest en een lijst van objecten van het type Team retourneert.
        // En omdat de tabel Team een object van het type Coach bevat, hebben we de vorige methode gebruikt om deze methode te creëren.
        // We gebruikten hetzelfde patroon met de volgende methodes

        public List<Team> LeesTeams()
        {
            string SQL = "SELECT * FROM Team";
            List<Team> teams = new List<Team>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();

                    List<Coach> coaches = LeesCoaches();
                    Dictionary<int, Coach> dicCoach = new Dictionary<int, Coach>();
                    foreach (Coach coach in coaches)
                    {
                        dicCoach.Add(coach.CoachID, coach);
                    }

                    while (reader.Read())
                    {
                        teams.Add(new Team((int)reader["teamID"], (string)reader["teamNaam"], dicCoach[(int)reader["coachID"]]));
                    }
                    return teams;
                }
                catch (Exception ex)
                {
                    throw new SpelerException("LeesTeams", ex);
                }
            }
        }
        //=======================================================================================================
        public List<Training> LeesTrainingen()
        {
            string SQL = "SELECT * FROM Training";
            List<Training> trainingen = new List<Training>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();

                    List<Team> teams = LeesTeams();
                    Dictionary<int, Team> dicTeams = new Dictionary<int, Team>();

                    foreach (Team team in teams)
                    {
                        dicTeams.Add(team.TeamID, team);
                    }

                    while (reader.Read())
                    {
                        trainingen.Add(new Training((int)reader["trainingID"], (DateTime)reader["datum"], (string)reader["thema"], dicTeams[(int)reader["teamID"]]));
                    }
                    return trainingen;
                }
                catch (Exception ex)
                {
                    throw new SpelerException("LeesTrainingen", ex);
                }
            }
        }
        //=======================================================================================================
        public List<Speler> LeesSpelers()
        {
            string SQL = "SELECT * FROM Speler";
            List<Speler> spelers = new List<Speler>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();

                    List<Team> teams = LeesTeams();
                    Dictionary<int, Team> dicTeams = new Dictionary<int, Team>();

                    foreach (Team team in teams)
                    {
                        dicTeams.Add(team.TeamID, team);
                    }

                    while (reader.Read())
                    {
                        spelers.Add(new Speler((int)reader["spelerID"], (string)reader["naam"], (int)reader["rugNummer"], (string)reader["positie"], dicTeams[(int)reader["teamID"]]));
                    }
                    return spelers;
                }
                catch (Exception ex)
                {
                    throw new SpelerException("LeesSpelers", ex);
                }
            }
        }
        //=======================================================================================================
        public List<Aanwezigheid> LeesAanwezigheden()
        {
            string SQL = "SELECT * FROM Aanwezigheid";
            List<Aanwezigheid> aanwezigheden = new List<Aanwezigheid>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();

                    List<Speler> spelers = LeesSpelers();
                    Dictionary<int, Speler> dicSpelers = new Dictionary<int, Speler>();
                    foreach (Speler speler in spelers)
                    {
                        dicSpelers.Add(speler.SpelerID, speler);
                    }

                    List<Training> trainingen = LeesTrainingen();
                    Dictionary<int, Training> dicTrainingen = new Dictionary<int, Training>();
                    foreach (Training training in trainingen)
                    {
                        dicTrainingen.Add(training.TrainingID, training);
                    }

                    while (reader.Read())
                    {
                        aanwezigheden.Add(new Aanwezigheid(dicSpelers[(int)reader["spelerID"]], dicTrainingen[(int)reader["trainingID"]], (bool)reader["isAanwezig"], (bool)reader["heeftAfwezigheidGemeld"], (string)reader["redenAfwezigheid"]));
                    }
                    return aanwezigheden;
                }
                catch (Exception ex)
                {
                    throw new SpelerException("LeesAanwezigheden", ex);
                }
            }
        }
        //=======================================================================================================
    }
}
