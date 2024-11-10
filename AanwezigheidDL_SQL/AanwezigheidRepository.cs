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
using static System.Runtime.InteropServices.JavaScript.JSType;


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
        public bool HeeftSpeler(Speler speler)
        {
            string sql = "SELECT COUNT(*) FROM Speler WHERE naam = @naam  AND rugNummer = @rugNummer AND positie = @positie AND teamID = @teamID;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@rugNummer", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@positie", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@teamID", SqlDbType.Int));

                    cmd.Parameters["@naam"].Value = speler.Naam;
                    cmd.Parameters["@rugNummer"].Value = speler.RugNummer;
                    cmd.Parameters["@positie"].Value = speler.Positie;
                    cmd.Parameters["@teamID"].Value = speler.Team.TeamID;
                
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("HeeftSpeler", ex);
                }
            }
        }
        //=======================================================================================================
        public void SchrijfSpeler(Speler speler)
        {
            string sql = "INSERT INTO Speler(naam ,rugNummer,positie,teamID) VALUES (@naam,@rugNummer,@positie,@teamID)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@rugNummer", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@positie", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@teamID", SqlDbType.Int));

                    cmd.Parameters["@naam"].Value = speler.Naam;
                    cmd.Parameters["@rugNummer"].Value = speler.RugNummer;
                    cmd.Parameters["@positie"].Value = speler.Positie;
                    cmd.Parameters["@teamID"].Value = speler.Team.TeamID;
                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfSpeler", ex);
                }
            }
        }
        //=======================================================================================================
        public bool HeeftAanwezigheid(Aanwezigheid aanwezigheid)
        {
            string sql = "SELECT COUNT(*) FROM Aanwezigheid WHERE spelerID = @spelerID  AND trainingID = @trainingID AND isAanwezig = @isAanwezig AND heeftAfwezigheidGemeld = @heeftAfwezigheidGemeld AND redenAfwezigheid= @redenAfwezigheid;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@spelerID", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@trainingID", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@isAanwezig", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@heeftAfwezigheidGemeld", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@redenAfwezigheid", SqlDbType.VarChar));

                    cmd.Parameters["@spelerID"].Value = aanwezigheid.Speler.SpelerID;
                    cmd.Parameters["@trainingID"].Value = aanwezigheid.Training.TrainingID;
                    cmd.Parameters["@isAanwezig"].Value = aanwezigheid.IsAanwezig;
                    cmd.Parameters["@heeftAfwezigheidGemeld"].Value = aanwezigheid.HeeftAfwezigheidGemeld;
                    cmd.Parameters["@redenAfwezigheid"].Value = aanwezigheid.RedenAfwezigheid;

                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("HeeftAanwezigheid", ex);
                }
            }
        }
        //=======================================================================================================
        public void SchrijfAanwezigheid(Aanwezigheid aanwezigheid)
        {
            string sql = "INSERT INTO Aanwezigheid(spelerID ,trainingID,isAanwezig,heeftAfwezigheidGemeld,redenAfwezigheid) VALUES (@spelerID ,@trainingID,@isAanwezig,@heeftAfwezigheidGemeld,@redenAfwezigheid)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@spelerID", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@trainingID", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@isAanwezig", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@heeftAfwezigheidGemeld", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@redenAfwezigheid", SqlDbType.VarChar));

                    cmd.Parameters["@spelerID"].Value = aanwezigheid.Speler.SpelerID;
                    cmd.Parameters["@trainingID"].Value = aanwezigheid.Training.TrainingID;
                    cmd.Parameters["@isAanwezig"].Value = aanwezigheid.IsAanwezig;
                    cmd.Parameters["@heeftAfwezigheidGemeld"].Value = aanwezigheid.HeeftAfwezigheidGemeld;
                    cmd.Parameters["@redenAfwezigheid"].Value = aanwezigheid.RedenAfwezigheid;
                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfAanwezigheid", ex);
                }
            }
        }
        //=======================================================================================================
        public bool HeeftCoach(Coach coach)
        {
            string sql = "SELECT COUNT(*) FROM Coach WHERE naam = @naam;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.VarChar));
                    cmd.Parameters["@naam"].Value = coach.Naam;

                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("HeeftCoach", ex);
                }
            }
        }
        //=======================================================================================================
        public void SchrijfCoach(Coach coach)
        {
            string sql = "INSERT INTO Coach(naam) VALUES (@naam)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.VarChar));
                    cmd.Parameters["@naam"].Value = coach.Naam;
                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfCoach", ex);
                }
            }
        }
        //=======================================================================================================
        public bool HeeftTeam(Team team)
        {
            string sql = "SELECT COUNT(*) FROM Team WHERE teamNaam=@teamNaam AND coachID=@coachID";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@teamNaam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@coachID", SqlDbType.Int));
                    cmd.Parameters["@teamNaam"].Value = team.TeamNaam;
                    cmd.Parameters["@coachID"].Value = team.Coach.CoachID;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("HeeftTeam", ex);
                }
            }
        }
        //=======================================================================================================
        public void SchrijfTeam(Team team)
        {
            string sql = "INSERT INTO Team(teamNaam, coachID) VALUES (@teamNaam, @coachID)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@teamNaam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@coachID", SqlDbType.Int));
                    cmd.Parameters["@teamNaam"].Value = team.TeamNaam;
                    cmd.Parameters["@coachID"].Value = team.Coach.CoachID;
                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfTeam", ex);
                }
            }
        }
        //=======================================================================================================
        public bool HeeftTraining(Training training)
        {
            string sql = "SELECT COUNT(*) FROM Training WHERE datum=@datum AND thema=@thema AND teamID=@teamID";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@datum", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@thema", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@teamID", SqlDbType.Int));
                    cmd.Parameters["@datum"].Value = training.Datum;
                    cmd.Parameters["@thema"].Value = training.Thema;
                    cmd.Parameters["@teamID"].Value = training.Team.TeamID;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("HeeftTraining", ex);
                }
            }
        }
        //=======================================================================================================
        public void SchrijfTraining(Training training)
        {
            string sql = "INSERT INTO Training(datum, thema, teamID) VALUES (@datum, @thema, @teamID)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@datum", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@thema", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@teamID", SqlDbType.Int));
                    cmd.Parameters["@datum"].Value = training.Datum;
                    cmd.Parameters["@thema"].Value = training.Thema;
                    cmd.Parameters["@teamID"].Value = training.Team.TeamID;
                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfTeam", ex);
                }
            }
        }
        //=======================================================================================================
        public bool HeeftLetsel(Letsel letsel)
        {
            string sql = "SELECT COUNT(*) FROM Letsel WHERE spelerID=@spelerID AND letselType=@letselType AND letselDatum=@letselDatum AND notities=@notities";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@spelerID", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@letselType", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@letselDatum", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@notities", SqlDbType.VarChar));
                    cmd.Parameters["@spelerID"].Value = letsel.Speler.SpelerID;
                    cmd.Parameters["@letselType"].Value = letsel.LetselType;
                    cmd.Parameters["@letselDatum"].Value = letsel.LetselDatum;
                    cmd.Parameters["@notities"].Value = letsel.Notities;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("HeeftLetsel", ex);
                }
            }
        }
        //=======================================================================================================
        public void SchrijfLetsel(Letsel letsel)
        {
            string sql = "INSERT INTO Letsel(spelerID, letselType, letselDatum, notities) VALUES (@spelerID, @letselType, @letselDatum, @notities)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@spelerID", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@letselType", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@letselDatum", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@notities", SqlDbType.VarChar));

                    cmd.Parameters["@spelerID"].Value = letsel.Speler.SpelerID;
                    cmd.Parameters["@letselType"].Value = letsel.LetselType;
                    cmd.Parameters["@letselDatum"].Value = letsel.LetselDatum;
                    cmd.Parameters["@notities"].Value = letsel.Notities;

                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfLetsel", ex);
                }
            }
        }
        //=======================================================================================================
    }
}
