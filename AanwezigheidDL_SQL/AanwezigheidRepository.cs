using System.Data;
using AanwezigheidBL.Interfaces;
using AanwezigheidBL.Model;
using AanwezigheidBL.Exceptions;
using Microsoft.Data.SqlClient;


namespace AanwezigheidDL_SQL
{
    public class AanwezigheidRepository : IAanwezigheidRepository
    {
        private string _connectionString;

        public AanwezigheidRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Coache
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
                        Coaches.Add(new Coach((int)reader["id"], (string)reader["naam"]));
                    }
                    return Coaches;
                }
                catch (Exception ex)
                {
                    throw new SpelerException("LeesCoaches", ex);
                }
            }
        }
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
        #endregion

        #region Team
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
                        teams.Add(new Team((int)reader["id"], (string)reader["naam"], dicCoach[(int)reader["coach_id"]]));
                    }
                    return teams;
                }
                catch (Exception ex)
                {
                    throw new SpelerException("LeesTeams", ex);
                }
            }
        }
        public bool HeeftTeam(Team team)
        {
            string sql = "SELECT COUNT(*) FROM Team WHERE naam=@naam AND coach_id=@coach_id";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@naam", team.TeamNaam);
                    cmd.Parameters.AddWithValue("@coach_id", team.Coach.CoachID);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("HeeftTeam", ex);
                }
            }
        }
        public void SchrijfTeam(Team team)
        {
            string sql = "INSERT INTO Team(naam, coach_id) VALUES (@naam, @coach_id)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@naam", team.TeamNaam);
                    cmd.Parameters.AddWithValue("@coach_id", team.Coach.CoachID);
                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfTeam", ex);
                }
            }
        }
        #endregion

        #region Training
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
                        trainingen.Add(new Training((int)reader["id"], (DateTime)reader["datum"], (string)reader["thema"], dicTeams[(int)reader["team_id"]]));
                    }
                    return trainingen;
                }
                catch (Exception ex)
                {
                    throw new SpelerException("LeesTrainingen", ex);
                }
            }
        }
        public bool HeeftTraining(Training training)
        {
            string sql = "SELECT COUNT(*) FROM Training WHERE datum=@datum AND thema=@thema AND team_id=@team_id";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@datum", training.Datum);
                    cmd.Parameters.AddWithValue("@thema", training.Thema);
                    cmd.Parameters.AddWithValue("@team_id", training.Team.TeamID);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("HeeftTraining", ex);
                }
            }
        }
        public void SchrijfTraining(Training training)
        {
            string sql = "INSERT INTO Training(datum, thema, team_id) VALUES (@datum, @thema, @team_id)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@datum", training.Datum);
                    cmd.Parameters.AddWithValue("@thema", training.Thema);
                    cmd.Parameters.AddWithValue("@team_id", training.Team.TeamID);
                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfTraining", ex);
                }
            }
        }
        public void SchrijfWijzigingTraining(Training oldTraining, Training newTraining)
        {
            string sql = "UPDATE Training SET id=@newId, datum =@newDatum,thema = @newThema,team_id =@newTeamID WHERE id = @oldId";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@newId", newTraining.TrainingID);
                    cmd.Parameters.AddWithValue("@newDatum", newTraining.Datum);
                    cmd.Parameters.AddWithValue("@newThema", newTraining.Thema);
                    cmd.Parameters.AddWithValue("@newTeamID", newTraining.Team.TeamID);
                    
                    cmd.Parameters.AddWithValue("@oldId", oldTraining.TrainingID);

                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfWijzigingTraining", ex);
                }
            }
        }
        #endregion

        #region Speler
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
                        spelers.Add(new Speler((int)reader["id"], (string)reader["naam"], (int)reader["rugNummer"], (string)reader["positie"], dicTeams[(int)reader["team_id"]]));
                    }
                    return spelers;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("LeesSpelers", ex);
                }
            }
        }
        public bool HeeftSpeler(Speler speler)
        {
            string sql = "SELECT COUNT(*) FROM Speler WHERE naam = @naam  AND rugNummer = @rugNummer AND positie = @positie AND team_id = @team_id;";
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
                    cmd.Parameters.Add(new SqlParameter("@team_id", SqlDbType.Int));

                    cmd.Parameters["@naam"].Value = speler.Naam;
                    cmd.Parameters["@rugNummer"].Value = speler.RugNummer;
                    cmd.Parameters["@positie"].Value = speler.Positie;
                    cmd.Parameters["@team_id"].Value = speler.Team.TeamID;

                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("HeeftSpeler", ex);
                }
            }
        }
        public void SchrijfSpeler(Speler speler)
        {
            string sql = "INSERT INTO Speler(naam ,rugNummer,positie,team_id) VALUES (@naam,@rugNummer,@positie,@team_id)";
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
                    cmd.Parameters.Add(new SqlParameter("@team_id", SqlDbType.Int));

                    cmd.Parameters["@naam"].Value = speler.Naam;
                    cmd.Parameters["@rugNummer"].Value = speler.RugNummer;
                    cmd.Parameters["@positie"].Value = speler.Positie;
                    cmd.Parameters["@team_id"].Value = speler.Team.TeamID;
                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfSpeler", ex);
                }
            }
        }
        public void SchrijfWijzigingSpeler(Speler oldSpeler, Speler newSpeler)
        {
            string sql = "UPDATE Speler SET id = @newId, naam = @newNaam, rugNummer = @newRugNummer, team_id = @newTeamID WHERE id = @oldId";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@newId", newSpeler.SpelerID);
                    cmd.Parameters.AddWithValue("@newNaam", newSpeler.Naam);
                    cmd.Parameters.AddWithValue("@newRugNummer", newSpeler.RugNummer);
                    cmd.Parameters.AddWithValue("@newPositie", newSpeler.Positie);
                    cmd.Parameters.AddWithValue("@newTeamID", newSpeler.Team.TeamID);

                    cmd.Parameters.AddWithValue("@oldId", oldSpeler.SpelerID);

                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfWijzigingSpeler", ex);
                }
            }
        }
        public void VerwijderSpelerVanDB(Speler speler)
        {
            string sql = "DELETE FROM Speler WHERE id = @id;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id", speler.SpelerID);
                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("VerwijderSpeler", ex);
                }
            }
        }
        #endregion

        #region Aanwezighed
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
                        aanwezigheden.Add(new Aanwezigheid(dicSpelers[(int)reader["speler_id"]], dicTrainingen[(int)reader["training_id"]], (bool)reader["isAanwezig"], (bool)reader["heeftAfwezigheidGemeld"], (string)reader["redenAfwezigheid"]));
                    }
                    return aanwezigheden;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("LeesAanwezigheden", ex);
                }
            }
        }
        public bool HeeftAanwezigheid(Aanwezigheid aanwezigheid)
        {
            string sql = "SELECT COUNT(*) FROM Aanwezigheid WHERE speler_id = @speler_id AND training_id = @training_id AND isAanwezig = @isAanwezig AND heeftAfwezigheidGemeld = @heeftAfwezigheidGemeld AND redenAfwezigheid= @redenAfwezigheid;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@speler_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@training_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@isAanwezig", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@heeftAfwezigheidGemeld", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@redenAfwezigheid", SqlDbType.VarChar));

                    cmd.Parameters["@speler_id"].Value = aanwezigheid.Speler.SpelerID;
                    cmd.Parameters["@training_id"].Value = aanwezigheid.Training.TrainingID;
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
        public void SchrijfAanwezigheid(Aanwezigheid aanwezigheid)
        {
            string sql = "INSERT INTO Aanwezigheid(speler_id ,training_id,isAanwezig,heeftAfwezigheidGemeld,redenAfwezigheid) VALUES (@speler_id ,@training_id,@isAanwezig,@heeftAfwezigheidGemeld,@redenAfwezigheid)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@speler_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@training_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@isAanwezig", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@heeftAfwezigheidGemeld", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@redenAfwezigheid", SqlDbType.VarChar));

                    cmd.Parameters["@speler_id"].Value = aanwezigheid.Speler.SpelerID;
                    cmd.Parameters["@training_id"].Value = aanwezigheid.Training.TrainingID;
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
        #endregion

        #region Letsel
        public List<Letsel> LeesLetsels()
        {
            string SQL = "SELECT * FROM Letsel";
            List<Letsel> letsels = new List<Letsel>();
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

                    while (reader.Read())
                    {
                        letsels.Add(new Letsel((int)reader["id"], dicSpelers[(int)reader["speler_id"]], (string)reader["letselType"],(DateTime)reader["letselDatum"], (string)reader["notities"]));
                    }
                    return letsels;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("LeesLetsels", ex);
                }
            }
        }
        public bool HeeftLetsel(Letsel letsel)
        {
            string sql = "SELECT COUNT(*) FROM Letsel WHERE speler_id=@speler_id AND letselType=@letselType AND letselDatum=@letselDatum AND notities=@notities";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@speler_id", letsel.Speler.SpelerID);
                    cmd.Parameters.AddWithValue("@letselType", letsel.LetselType);
                    cmd.Parameters.AddWithValue("@letselDatum", letsel.LetselDatum);
                    cmd.Parameters.AddWithValue("@notities", letsel.Notities);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("HeeftLetsel", ex);
                }
            }
        }
        public void SchrijfLetsel(Letsel letsel)
        {
            string sql = "INSERT INTO Letsel(speler_id, letselType, letselDatum, notities) VALUES (@speler_id, @letselType, @letselDatum, @notities)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@speler_id", letsel.Speler.SpelerID);
                    cmd.Parameters.AddWithValue("@letselType", letsel.LetselType);
                    cmd.Parameters.AddWithValue("@letselDatum", letsel.LetselDatum);
                    cmd.Parameters.AddWithValue("@notities", letsel.Notities);
                    cmd.ExecuteNonQuery();
                }
                catch (SpelerException ex)
                {
                    throw new SpelerException("SchrijfLetsel", ex);
                }
            }
        }
        #endregion
    }
}
