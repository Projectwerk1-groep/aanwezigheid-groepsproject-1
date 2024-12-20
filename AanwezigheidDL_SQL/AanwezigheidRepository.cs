﻿using System.Data;
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
        //LeesSpelersVanTeam: Deze methode leest alle spelers van een specifiek team uit de database. Deze methode kan worden gebruikt op de details-pagina in de UI en ook op de overzicht-pagina.
        public List<Speler> LeesSpelersVanTeam(int teamID) // getest door Gaith
        {
            string SQL = "SELECT * FROM Speler WHERE team_id = @team_id;";
            List<Speler> spelers = [];
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("team_id", teamID);
                IDataReader reader = cmd.ExecuteReader();

                List<Team> teams = LeesTeams();
                Dictionary<int, Team> dicTeams = new();

                foreach (Team team in teams)
                {
                    dicTeams.Add(team.TeamID, team);
                }

                while (reader.Read())
                {
                    spelers.Add(new Speler((int)reader["id"], (string)reader["naam"], (int)reader["rugNummer"], dicTeams[(int)reader["team_id"]]));
                }
                return spelers;
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(LeesSpelersVanTeam), ex);
            }
        }
        //HeeftSpeler: Deze 2 methodes controleren of deze speler al in de database bestaat voordat we deze aan de database toevoegen.
        // Controle bij toevoeging van speler
        public bool HeeftSpelerBijToevoeging(Speler speler) // getest door Intesar
        {
            string sql = "SELECT COUNT(*) FROM Speler WHERE (naam = @naam AND team_id = @team_id) OR (rugNummer = @rugNummer AND team_id = @team_id);";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@naam", speler.Naam);
                cmd.Parameters.AddWithValue("@rugNummer", speler.RugNummer);
                cmd.Parameters.AddWithValue("@team_id", speler.Team.TeamID);

                int n = (int)cmd.ExecuteScalar();
                if (n > 0) return true; else return false;
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(HeeftSpelerBijToevoeging), ex);
            }
        }
        // Controle bij wijziging van speler
        public bool HeeftSpelerBijWijziging(Speler speler) // getest door Intesar
        {
            string sql = "SELECT COUNT(*) FROM Speler WHERE naam = @naam AND rugNummer = @rugNummer AND team_id = @team_id;";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@naam", speler.Naam);
                cmd.Parameters.AddWithValue("@rugNummer", speler.RugNummer);
                cmd.Parameters.AddWithValue("@team_id", speler.Team.TeamID);

                int n = (int)cmd.ExecuteScalar();
                if (n > 0) return true; else return false;
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(HeeftSpelerBijWijziging), ex);
            }
        }

        //SchrijfSpeler: Deze methode voegt een speler toe aan de database en kan worden gebruikt op beide pagina's in de UI.
        public void SchrijfSpeler(Speler speler) // getest door Intesar
        {
            string sql = "INSERT INTO Speler(naam ,rugNummer,team_id) VALUES (@naam,@rugNummer,@team_id)";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@naam", speler.Naam);
                cmd.Parameters.AddWithValue("@rugNummer", speler.RugNummer);
                cmd.Parameters.AddWithValue("@team_id", speler.Team.TeamID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(SchrijfSpeler), ex);
            }
        }
        //SchrijfWijzigingSpeler: Deze methode neemt een Speler-object vóór de wijziging en een nieuw Speler-object (na de wijziging) om de gegevens van de speler aan te passen, met uitzondering van het ID. En kan worden gebruikt op de overzicht-pagina in de UI.
        public void SchrijfWijzigingSpeler(Speler oldSpeler, Speler newSpeler) // getest door Gaith
        {
            string sql = "UPDATE Speler SET naam = @newNaam, rugNummer = @newRugNummer, team_id = @newTeamID WHERE id = @id;";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@newNaam", newSpeler.Naam);
                cmd.Parameters.AddWithValue("@newRugNummer", newSpeler.RugNummer);
                cmd.Parameters.AddWithValue("@newTeamID", newSpeler.Team.TeamID);
                cmd.Parameters.AddWithValue("@id", oldSpeler.SpelerID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(SchrijfWijzigingSpeler), ex);
            }
        }
        //VerwijderSpelerVanDB: Deze methode verwijdert de speler uit de database. Het kan worden gebruikt op beide pagina's in de UI.
        public void VerwijderSpelerVanDB(Speler speler) // getest door Gaith
        {
            string sql = "DELETE FROM Speler WHERE id = @id;";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", speler.SpelerID);
                //cmd.Parameters.AddWithValue("@naam", speler.Naam);
                //cmd.Parameters.AddWithValue("@rugNummer", speler.RugNummer);
                //cmd.Parameters.AddWithValue("@team_id", speler.Team.TeamID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(VerwijderSpelerVanDB), ex);
            }
        }

        #endregion

        #region Training
        //HeeftTraining: Deze methode controleert of deze training al in de database bestaat voordat we deze aan de database toevoegen.
        public bool HeeftTraining(Training training) // getest door Gaith
        {
            string sql = "SELECT COUNT(*) FROM Training WHERE datum=@datum AND thema=@thema AND team_id=@team_id";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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


        //SchrijfTraining: Deze methode voegt een training toe aan de database en kan worden gebruikt op beide pagina's in de UI.
        public void SchrijfTraining(Training training) // getest door Gaith
        {
            string sql = "INSERT INTO Training(datum, thema, team_id) VALUES (@datum, @thema, @team_id)";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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
        //SchrijfWijzigingTraining: Deze methode neemt een Training-object vóór de wijziging en een bijgewerkt Training-object (na de wijziging) en vervangt het oude object door het nieuwe in de database.
        public void SchrijfWijzigingTraining(Training oldTraining, Training newTraining)
        {
            string sql = "UPDATE Training SET id=@newId, datum =@newDatum,thema = @newThema,team_id =@newTeamID WHERE id = @oldId";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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
        //LeesTrainingenVanTeam: Deze methode retourneert alle eerdere trainingen van een specifiek team en kan worden gebruikt op de details-pagina.
        public List<Training> LeesTrainingenVanTeam(int teamId) // getest door Intesar
        {
            string SQL = "SELECT * FROM Training WHERE team_id = @team_id";
            List<Training> trainingen = [];
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@team_id", teamId);
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
                throw new DomeinException(nameof(LeesTrainingenVanTeam), ex);
            }
        }
        
        //LeesTrainingOmAanwezighedenTeMaken: Deze methode retourneert dezelfde training die in de parameter wordt meegegeven, maar vult de eigenschap ID in vanuit de database.
        public Training LeesTrainingOmAanwezighedenTeMaken(Training trainingZonderID)
        {
            Training trainingMetID = null;
            string SQL = "SELECT id, datum, thema, team_id FROM Training WHERE datum = @datum AND thema = @thema AND team_id = @teamId;";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@datum", trainingZonderID.Datum);
                cmd.Parameters.AddWithValue("@thema", trainingZonderID.Thema);
                cmd.Parameters.AddWithValue("@teamId", trainingZonderID.Team.TeamID);
                IDataReader reader = cmd.ExecuteReader();

                List<Team> teams = LeesTeams();
                Dictionary<int, Team> dicTeams = [];

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

        //LeesTraining: Deze methode retourneert één specifieke training op basis van het ID.
        public Training LeesTraining(int trainingId) // getest door Gaith
        {
            Training training = null;
            string SQL = "SELECT * FROM Training WHERE id = @training_id";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@training_id", trainingId);
                IDataReader reader = cmd.ExecuteReader();

                List<Team> teams = LeesTeams();
                Dictionary<int, Team> dicTeams = new();

                foreach (Team team in teams)
                {
                    dicTeams.Add(team.TeamID, team);
                }

                while (reader.Read())
                {
                    training = new Training((int)reader["id"], (DateTime)reader["datum"], (string)reader["thema"], dicTeams[(int)reader["team_id"]]);
                }
                return training;
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(LeesTraining), ex);
            }
        }

        #endregion

        #region Aanwezigheid

        //HeeftAanwezigheid: Deze methode controleert of deze aanwezigheid al in de database bestaat voordat we deze aan de database toevoegen.
        public bool HeeftAanwezigheid(Aanwezigheid aanwezigheid) // getest door Intesar
        {
            string sql = "SELECT COUNT(*) FROM Aanwezigheid WHERE speler_id = @speler_id AND training_id = @training_id;";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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

        public bool HeeftAanwezigheidOpSpeler(Speler speler) // getest door Orlando
        {
            string sql = "SELECT COUNT(*) FROM Aanwezigheid WHERE speler_id = @speler_id;";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@speler_id", speler.SpelerID);

                int n = (int)cmd.ExecuteScalar();
                if (n > 0) return true; else return false;
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(HeeftAanwezigheidOpSpeler), ex);
            }
        }

        //SchrijfAanwezigheid: Deze methode voegt de aanwezigheid toe aan de database.
        public void SchrijfAanwezigheid(Aanwezigheid aanwezigheid) // getest door Intesar
        {
            string sql = "INSERT INTO Aanwezigheid(speler_id ,training_id,isAanwezig,heeftAfwezigheidGemeld,redenAfwezigheid) VALUES (@speler_id ,@training_id,@isAanwezig,@heeftAfwezigheidGemeld,@redenAfwezigheid)";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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
        
        //LeesEnSchrijfAanwezigheidPerTrainingInTXT: Deze methode exporteert de aanwezigheidsgegevens van elke speler, gebaseerd op het team en de training, naar een tekstbestand
        public void LeesEnSchrijfAanwezigheidPerTrainingInTXT(Training training, Team team, string filePath) // getest door Intesar
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

            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@training_id", training.TrainingID);
                cmd.Parameters.AddWithValue("@team_id", team.TeamID);
                SqlDataReader reader = cmd.ExecuteReader();
                using StreamWriter writer = new(filePath);
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
            catch (Exception ex)
            {
                throw new DomeinException(nameof(LeesEnSchrijfAanwezigheidPerTrainingInTXT), ex);
            }
        }
        
        //LeesPercentageAanwezigheid: Deze methode berekent het aanwezigheidspercentage van een speler bij de trainingendoor het aantal trainingen dat hij heeft bijgewoond te delen door het totale aantal trainingen dat hij had kunnen bijwonen. Deze methode kan worden gebruikt op de details-pagina in de UI.
        public double LeesPercentageAanwezigheid(int spelerID) // getest door Gaith
        {
            string SQL = "SELECT CAST(SUM(CASE WHEN isAanwezig = 1 THEN 1 ELSE 0 END) AS FLOAT) / NULLIF(COUNT(*), 0) * 100 AS aanwezigheidPercentage FROM Aanwezigheid WHERE speler_id = @speler_id;";
            //double percentage = 0;
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@speler_id", spelerID);
                IDataReader reader = cmd.ExecuteReader();
                //while (reader.Read())
                //{
                //    percentage = (double)reader["aanwezigheidPercentage"];
                //}
                //return percentage;
                if (reader.Read())
                {
                    object value = reader["aanwezigheidPercentage"];
                    return value == DBNull.Value ? 0 : Convert.ToDouble(value);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(LeesPercentageAanwezigheid), ex);
            }
        }

        // SchrijfWijzigingAanwezigheid: Deze methode wijzigt de aan-/afwezigheid van een speler van een bepaalde training.
        public void SchrijfWijzigingAanwezigheid(Aanwezigheid oldA, Aanwezigheid newA) // getest door Orlando
        {
            string sql = "UPDATE Aanwezigheid SET isAanwezig = @newIsAanwezig, " +
                "heeftAfwezigheidGemeld = @newHeeftAfwezigheidGemeld, " +
                "redenAfwezigheid = @newRedenAfwezigheid " +
                "WHERE (speler_id = @speler_id AND training_id = @training_id);";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@newIsAanwezig", newA.IsAanwezig);
                cmd.Parameters.AddWithValue("@newHeeftAfwezigheidGemeld", newA.HeeftAfwezigheidGemeld);
                cmd.Parameters.AddWithValue("@newRedenAfwezigheid", newA.RedenAfwezigheid);
                cmd.Parameters.AddWithValue("@speler_id", newA.Speler.SpelerID);
                cmd.Parameters.AddWithValue("@training_id", newA.Training.TrainingID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(SchrijfWijzigingAanwezigheid), ex);
            }
        }

        public void VerwijderAanwezigheidVanDB(Speler speler) // getest door Orlando
        {
            string sql = "DELETE FROM Aanwezigheid WHERE speler_id = @speler_id;";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@speler_id", speler.SpelerID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(VerwijderAanwezigheidVanDB), ex);
            }
        }

        // LeesAanwezighedenVanTraining: Deze methode retourneert de aan-/afwezigheden van alle spelers van de gegeven training.
        public List<Aanwezigheid> LeesAanwezighedenVanTraining(int trainingID) // Getest door Orlando
        {
            string SQL = "SELECT * FROM Aanwezigheid WHERE training_id = @training_id";
            List<Aanwezigheid> aanwezigheden = [];
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("training_id", trainingID);
                IDataReader reader = cmd.ExecuteReader();

                List<Speler> spelers = LeesSpelers();
                Dictionary<int, Speler> dicSpelers = [];
                foreach (Speler speler in spelers)
                {
                    dicSpelers.Add(speler.SpelerID, speler);
                }

                List<Training> trainingen = LeesTrainingen();
                Dictionary<int, Training> dicTrainingen = [];
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
                throw new DomeinException(nameof(LeesAanwezighedenVanTraining), ex);
            }
        }

        #endregion

        #region Team

        //LeesTeams: Deze methode retourneert een lijst van teams die in de database staan en kan worden gebruikt op beide pagina's in de UI.
        public List<Team> LeesTeams()
        {
            string SQL = "SELECT * FROM Team";
            List<Team> teams = [];
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = SQL;
                IDataReader reader = cmd.ExecuteReader();

                List<Coach> coaches = LeesCoaches();
                Dictionary<int, Coach> dicCoach = new();
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

        //LeesTeamsPerCoach: Deze methode retourneert een lijst van teams in de database die tot een bepaalde coach behoren en kan worden gebruikt op beide pagina's in de UI.
        public List<Team> LeesTeamsPerCoach(int coachId)
        {
            string SQL = "SELECT * FROM Team WHERE coach_id=@coach_id";
            List<Team> teams = [];
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            try
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@coach_id", coachId);
                IDataReader reader = cmd.ExecuteReader();

                List<Coach> coaches = LeesCoaches();
                Dictionary<int, Coach> dicCoach = new();
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
                throw new DomeinException(nameof(LeesTeamsPerCoach), ex);
            }
        }

        //HeeftTeam: Deze methode controleert of dit team al in de database bestaat voordat we details eraan toevoegen.
        public bool HeeftTeam(Team team) // getest door Gaith
        {
            string sql = "SELECT COUNT(*) FROM Team WHERE naam=@naam AND coach_id=@coach_id";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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

        #endregion

        #region Letsel

        //HeeftLetsel: Deze methode controleert of dit letsel al in de database bestaat voordat we een nieuw letsel toevoegen.
        public bool HeeftLetsel(Letsel letsel)
        {
            string sql = "SELECT COUNT(*) FROM Letsel WHERE speler_id=@speler_id AND letselType=@letselType AND letselDatum=@letselDatum AND notities=@notities";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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
        //SchrijfLetsel: Deze methode voegt een letsel toe aan een speler in de database.
        public void SchrijfLetsel(Letsel letsel)
        {
            string sql = "INSERT INTO Letsel(speler_id, letselType, letselDatum, notities) VALUES (@speler_id, @letselType, @letselDatum, @notities)";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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

        #endregion


        public void SchrijfTeam(Team team) // getest door Gaith
        {
            string sql = "INSERT INTO Team(naam, coach_id) VALUES (@naam, @coach_id)";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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

        public List<Coach> LeesCoaches() // getest door Intesar
        {
            string SQL = "SELECT * FROM Coach";
            List<Coach> Coaches = [];
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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

        public bool HeeftCoach(Coach coach) // getest door Intesar
        {
            string sql = "SELECT COUNT(*) FROM Coach WHERE naam = @naam;";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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
        
        public void SchrijfCoach(Coach coach) // getest door Intesar
        {
            string sql = "INSERT INTO Coach(naam) VALUES (@naam)";
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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
        
        public List<Speler> LeesSpelers()
        {
            string SQL = "SELECT * FROM Speler";
            List<Speler> spelers = new();
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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
                    spelers.Add(new Speler((int)reader["id"], (string)reader["naam"], (int)reader["rugNummer"], dicTeams[(int)reader["team_id"]]));
                }
                return spelers;
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(LeesSpelers), ex);
            }
        }
        
        public List<Letsel> LeesLetsels()
        {
            string SQL = "SELECT * FROM Letsel";
            List<Letsel> letsels = new();
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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
                    letsels.Add(new Letsel((int)reader["id"], dicSpelers[(int)reader["speler_id"]], (string)reader["letselType"], (DateTime)reader["letselDatum"], (string)reader["notities"]));
                }
                return letsels;
            }
            catch (Exception ex)
            {
                throw new DomeinException(nameof(LeesLetsels), ex);
            }
        }
        
        public List<Training> LeesTrainingen()
        {
            string SQL = "SELECT * FROM Training";
            List<Training> trainingen = new();
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
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
                throw new DomeinException(nameof(LeesTrainingen), ex);
            }
        }
            
    }
}
