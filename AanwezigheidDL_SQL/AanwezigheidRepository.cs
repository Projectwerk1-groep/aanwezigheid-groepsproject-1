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


        #region Speler

        public List<Speler> LeesSpelersVanTeam(int teamID)
        {
            string SQL = "SELECT * FROM Speler WHERE team_id = @team_id;";
            List<Speler> spelers = new List<Speler>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("team_id", teamID);
                    IDataReader reader = cmd.ExecuteReader();

                    List<Team> teams = LeesTeams();
                    Dictionary<int?, Team> dicTeams = new Dictionary<int?, Team>();

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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesSpelersVanTeam), ex);
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
                    cmd.Parameters.AddWithValue("@naam", speler.Naam);
                    cmd.Parameters.AddWithValue("@rugNummer", speler.RugNummer);
                    cmd.Parameters.AddWithValue("@positie", speler.Positie);
                    cmd.Parameters.AddWithValue("@team_id", speler.Team.TeamID);

                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(HeeftSpeler), ex);
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

                    cmd.Parameters.AddWithValue("@naam", speler.Naam);
                    cmd.Parameters.AddWithValue("@rugNummer", speler.RugNummer);
                    cmd.Parameters.AddWithValue("@positie", speler.Positie);
                    cmd.Parameters.AddWithValue("@team_id", speler.Team.TeamID);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(SchrijfSpeler), ex);
                }
            }
        }
        public void SchrijfWijzigingSpeler(Speler oldSpeler, Speler newSpeler)
        {
            string sql = "UPDATE Speler SET naam = @newNaam, rugNummer = @newRugNummer, team_id = @newTeamID WHERE id = @id";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@newNaam", newSpeler.Naam);
                    cmd.Parameters.AddWithValue("@newRugNummer", newSpeler.RugNummer);
                    cmd.Parameters.AddWithValue("@newPositie", newSpeler.Positie);
                    cmd.Parameters.AddWithValue("@newTeamID", newSpeler.Team.TeamID);
                    cmd.Parameters.AddWithValue("@id", oldSpeler.SpelerID);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(SchrijfWijzigingSpeler), ex);
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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(VerwijderSpelerVanDB), ex);
                }
            }
        }

        #endregion

        #region Training

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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(HeeftTraining), ex);
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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(SchrijfTraining), ex);
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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(SchrijfWijzigingTraining), ex);
                }
            }
        }
        public List<Training> LeesTrainingenVanTeam(int teamId)
        {
            string SQL = "SELECT * FROM Training where team_id = @team_id";
            List<Training> trainingen = new List<Training>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@team_id", teamId);
                    IDataReader reader = cmd.ExecuteReader();

                    List<Team> teams = LeesTeams();
                    Dictionary<int?, Team> dicTeams = new Dictionary<int?, Team>();

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
                    throw new DomeinException(nameof(LeesTrainingenVanTeam), ex);
                }
            }
        }
        public Training LeesTrainingOmAanwezighedenTeMaken(Training trainingZonderID)
        {
            Training trainingMetID = null;
            string SQL = "SELECT id, datum, thema, team_id FROM Training WHERE datum = @datum AND thema = @thema AND team_id = @teamId;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();

                    List<Team> teams = LeesTeams();
                    Dictionary<int?, Team> dicTeams = new Dictionary<int?, Team>();

                    foreach (Team team in teams)
                    {
                        dicTeams.Add(team.TeamID, team);
                    }

                    while (reader.Read())
                    {
                        trainingMetID = new Training((int)reader["id"], (DateTime)reader["datum"], (string)reader["thema"], dicTeams[(int)reader["team_id"]]);
                    }
                    return trainingMetID;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesTrainingOmAanwezighedenTeMaken), ex);
                }
            }
        }

        #endregion

        #region Aanwezigheid

        public bool HeeftAanwezigheid(Aanwezigheid aanwezigheid)
        {
            string sql = "SELECT COUNT(*) FROM Aanwezigheid WHERE speler_id = @speler_id AND training_id = @training_id;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@speler_id", aanwezigheid.Speler.SpelerID);
                    cmd.Parameters.AddWithValue("@training_id", aanwezigheid.Training.TrainingID);

                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(HeeftAanwezigheid), ex);
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
                    cmd.Parameters.AddWithValue("speler_id", aanwezigheid.Speler.SpelerID);
                    cmd.Parameters.AddWithValue("training_id", aanwezigheid.Training.TrainingID);
                    cmd.Parameters.AddWithValue("isAanwezig", aanwezigheid.IsAanwezig);
                    cmd.Parameters.AddWithValue("heeftAfwezigheidGemeld", aanwezigheid.HeeftAfwezigheidGemeld);
                    cmd.Parameters.AddWithValue("redenAfwezigheid", aanwezigheid.RedenAfwezigheid);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(SchrijfAanwezigheid), ex);
                }
            }
        }
        public void LeesEnSchrijfAanwezigheidPerTrainingInTXT(Training training, Team team, string filePath)
        {
            string SQL = @"SELECT 
                                Speler.naam AS 'Speler Naam',
                                Training.datum AS 'Training datum',
                                Training.thema AS 'Training thema',
                                Aanwezigheid.isAanwezig AS 'Was aanwezig',
                                Aanwezigheid.heeftAfwezigheidGemeld AS 'Heeft afwezigheid gemeld',
                                Aanwezigheid.redenAfwezigheid AS 'Reden afwezigheid'
                            FROM 
                                Aanwezigheid
                            INNER JOIN 
                                Speler ON Aanwezigheid.speler_id = Speler.id
                            INNER JOIN 
                                Training ON Aanwezigheid.training_id = Training.id
                            WHERE 
                                Training.id = @training_id AND Training.team_id = @team_id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@training_id", training.TrainingID);
                    cmd.Parameters.AddWithValue("@team_id", team.TeamID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine("Speler Naam | Training Datum | Training Thema | Was Aanwezig | Heeft Afwezigheid Gemeld | Reden Afwezigheid");
                        writer.WriteLine(new string('-', 80));
                        while (reader.Read())
                        {
                            string spelerNaam = reader.GetString(0);
                            DateTime trainingDatum = reader.GetDateTime(1);
                            string trainingThema = reader.GetString(2);
                            bool wasAanwezig = reader.GetBoolean(3);
                            bool heeftAfwezigheidGemeld = reader.GetBoolean(4);
                            string redenAfwezigheid = reader.IsDBNull(5) ? "N/A" : reader.GetString(5);


                            writer.WriteLine($"{spelerNaam} | {trainingDatum} | {trainingThema} | {wasAanwezig} | {heeftAfwezigheidGemeld} | {redenAfwezigheid}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesEnSchrijfAanwezigheidPerTrainingInTXT), ex);
                }
            }
        }
        public double LeesPercentageAanwezigheid(int spelerID)
        {
            string SQL = "SELECT CAST(SUM(CASE WHEN isAanwezig = 1 THEN 1 ELSE 0 END) AS FLOAT) / COUNT(*) * 100 AS aanwezigheidPercentage FROM Aanwezigheid WHERE speler_id = @speler_id;";
            double percentag = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@speler_id", spelerID);
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        percentag = (double)reader["aanwezigheidPercentage"];
                    }
                    return percentag;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesPercentageAanwezigheid), ex);
                }
            }
        }

        #endregion

        #region Team

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
                    Dictionary<int?, Coach> dicCoach = new Dictionary<int?, Coach>();
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
                    throw new DomeinException(nameof(LeesTeams), ex);
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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(HeeftTeam), ex);
                }
            }
        }

        #endregion

        #region Letsel

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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(HeeftLetsel), ex);
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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(SchrijfLetsel), ex);
                }
            }
        }

        #endregion

        

        #region Nog niet gebruiken
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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(SchrijfTeam), ex);
                }
            }
        }
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
                    throw new DomeinException(nameof(LeesCoaches), ex);
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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(HeeftCoach), ex);
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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(SchrijfCoach), ex);
                }
            }
        }
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
                    Dictionary<int?, Team> dicTeams = new Dictionary<int?, Team>();

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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesSpelers), ex);
                }
            }
        }
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
                    Dictionary<int?, Speler> dicSpelers = new Dictionary<int?, Speler>();

                    foreach (Speler speler in spelers)
                    {
                        dicSpelers.Add(speler.SpelerID, speler);
                    }

                    while (reader.Read())
                    {
                        letsels.Add(new Letsel((int)reader["id"], dicSpelers[(int)reader["speler_id"]], (string)reader["letselType"], (DateTime)reader["letselDatum"], (string)reader["notities"]));
                    }
                    return letsels;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesLetsels), ex);
                }
            }
        }
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
                    Dictionary<int?, Team> dicTeams = new Dictionary<int?, Team>();

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
                    throw new DomeinException(nameof(LeesTrainingen), ex);
                }
            }
        }
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
                    Dictionary<int?, Speler> dicSpelers = new Dictionary<int?, Speler>();
                    foreach (Speler speler in spelers)
                    {
                        dicSpelers.Add(speler.SpelerID, speler);
                    }

                    List<Training> trainingen = LeesTrainingen();
                    Dictionary<int?, Training> dicTrainingen = new Dictionary<int?, Training>();
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
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesAanwezigheden), ex);
                }
            }
        }
        #endregion
    }
}
