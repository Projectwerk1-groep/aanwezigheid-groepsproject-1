using AanwezigheidBL.Exceptions;
using AanwezigheidBL.Managers;
using AanwezigheidBL.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;


namespace AanwezigheidProject_WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CreateTraining : Window
    {
        public AanwezigheidManager _manager;
        public Team Team;
        public Speler Speler;
        public ObservableCollection<Team> Teams = [];
        public ObservableCollection<Speler> Spelers = [];
        public ObservableCollection<Speler> SpelersMetPercentages = [];

        public CreateTraining(AanwezigheidManager manager)
        {
            _manager = manager;
            InitializeComponent();

            WindowState = WindowState.Maximized;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void TeamComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Trainer inlezen
            Coach coach = _manager.GeefCoaches().First();
            CoachNameTBl.Text = coach.Naam;

            //Teams PER trainer inlezen
            Teams.Clear();
            List<Team> teams = _manager.GeefTeamsPerCoach(coach.CoachID).ToList();
            teams.ForEach(t => { Teams.Add(t); });

            TeamComboBox.ItemsSource = teams;

            // Stel een standaardwaarde in
            TeamComboBox.SelectedItem = teams.FirstOrDefault();

            //Toon de namen van de teams
            TeamComboBox.DisplayMemberPath = "TeamNaam";

            if (TeamComboBox.SelectedItem != null && TeamComboBox.SelectedItem is Team selectedTeam)
            {
                Team = selectedTeam;

                RefreshSpelerList(Team);

                OverzichtSpelers.ItemsSource = Spelers;
                DetailsSpelers.ItemsSource = SpelersMetPercentages;
            }
        }

        private void TeamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Stel het geselecteerde item in als de nieuwe standaardwaarde
            if (TeamComboBox.SelectedItem != null && TeamComboBox.SelectedItem is Team selectedTeam)
            {
                Team = selectedTeam;

                TeamComboBox.SelectedItem = Team;

                Details_TeamNaam_TBl.Text = Team.TeamNaam.ToString();

                Historiek_TeamNaam_TBl.Text = Team.TeamNaam.ToString();

                RefreshSpelerList(Team);
            }
        }

        private void OverzichtSpelers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OverzichtSpelers.SelectedItem != null && OverzichtSpelers.SelectedItem is Speler selectedSpeler)
            {
                Speler = selectedSpeler;
            }
        }

        private void DetailsSpelers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DetailsSpelers.SelectedItem != null && DetailsSpelers.SelectedItem is Speler selectedSpeler)
            {
                Speler = selectedSpeler;
            }
        }

        private void VoegSpelerToe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddSpelerInputDialog addDialog = new();
                if (addDialog.ShowDialog() == true) // Open de dialog en controleer op OK
                {
                    string naam = addDialog.Naam;
                    int rugnummer = addDialog.Rugnummer;

                    Speler newSpeler = new(naam, rugnummer, Team);

                    _manager.VoegSpelerToe(newSpeler);

                    RefreshSpelerList(Team);

                    // Voeg hier de logica toe om de speler aan de lijst toe te voegen
                    MessageBox.Show($"Nieuwe speler: {naam}, Nr.{rugnummer} is toegevoegd.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("In deze team bestaat er al een speler met dezelfde naam of nummer.");
            }
        }

        private void WijzigSpeler_Click(object sender, RoutedEventArgs e)
        {
            Speler oldSpeler = Speler;
            try
            {
                if (DetailsSpelers.SelectedItem != null)
                {
                    WijzigSpelerInputDialog WijzigDialog = new(oldSpeler);
                    if (WijzigDialog.ShowDialog() == true) // Open de dialog en controleer op OK
                    {
                        string naam = WijzigDialog.Naam;
                        int rugnummer = WijzigDialog.Rugnummer;

                        Speler newSpeler = new(naam, rugnummer, Team);
                        _manager.WijzigSpeler(oldSpeler, newSpeler);

                        RefreshSpelerList(Team);

                        MessageBox.Show($"Deze speler is gewijzigd naar {naam}, Nr.{rugnummer}.");
                    }
                }
                else
                {
                    MessageBox.Show("Selecteer eerst een speler om te wijzigen.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("In deze team bestaat er al een speler met dezelfde naam of nummer.");
            }
        }

        private void VerwijderSpeler_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DetailsSpelers.SelectedItem != null)
                {
                    _manager.VerwijderSpeler(Speler);

                    RefreshSpelerList(Team);

                    MessageBox.Show("Deze speler is verwijderd.");
                }
                else
                {
                    MessageBox.Show("Selecteer eerst een speler om te verwijderen.");
                }
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(_manager.VerwijderSpeler), ex);
            }
        }

        private void TrainingOpslaan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToonTraining_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExporteerTraining_Click(object sender, RoutedEventArgs e)
        {

        }

        public void RefreshSpelerList(Team team)
        {
            Spelers.Clear();
            SpelersMetPercentages.Clear();

            List<Speler> spelers = _manager.GeefSpelersVanTeam(team.TeamID);
            spelers.ForEach(s =>
            {
                Spelers.Add(s);
                SpelersMetPercentages.Add(
                    new Speler(s.SpelerID, s.Naam, s.RugNummer, s.Team, _manager.GeefPercentageAanwezigheid(s.SpelerID)));
            });

            AantalSpelersTBl.Text = Spelers.Count.ToString();
            Details_TeamAantalSpelers_TBl.Text = SpelersMetPercentages.Count.ToString();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            textBlock.Visibility = Visibility.Collapsed;

            var textBox = (System.Windows.Controls.TextBox)VisualTreeHelper.GetParent(textBlock);
            textBox.Visibility = Visibility.Visible;
            textBox.Focus();
        }

        private void ShowSelectedDate_Click(object sender, RoutedEventArgs e)
        {
            // Controleer of er een datum is geselecteerd
            if (MyDatePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = MyDatePicker.SelectedDate.Value;
                MessageBox.Show($"Geselecteerde datum: {selectedDate.ToString("dd-MM-yyyy")}", "Datum");
            }
            else
            {
                MessageBox.Show("Geen datum geselecteerd!", "Waarschuwing");
            }
        }
    }
}
