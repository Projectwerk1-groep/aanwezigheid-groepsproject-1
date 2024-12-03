using AanwezigheidBL.Managers;
using AanwezigheidBL.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            teams.ForEach(t => {Teams.Add(t);});

            TeamComboBox.ItemsSource = teams;

            // Stel een standaardwaarde in
            TeamComboBox.SelectedItem = teams.FirstOrDefault();

            //Toon de namen van de teams
            TeamComboBox.DisplayMemberPath = "TeamNaam";

            if (TeamComboBox.SelectedItem != null)
            {
                Team? selectedTeam = TeamComboBox.SelectedItem as Team;

                RefreshSpelerList(selectedTeam);

                OverzichtSpelers.ItemsSource = Spelers;
                DetailsSpelers.ItemsSource = SpelersMetPercentages;
            }
        }

        private void TeamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Stel het geselecteerde item in als de nieuwe standaardwaarde
            if (TeamComboBox.SelectedItem != null)
            {
                Team? selectedTeam = TeamComboBox.SelectedItem as Team;

                TeamComboBox.SelectedItem = selectedTeam;

                Details_TeamNaam_TBl.Text = selectedTeam.TeamNaam.ToString();

                Historiek_TeamNaam_TBl.Text = selectedTeam.TeamNaam.ToString();

                RefreshSpelerList(selectedTeam);
            }
        }

        private void OverzichtSpelers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DetailsSpelers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void VoeSpelerToe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VerwijderSpeler_Click(object sender, RoutedEventArgs e)
        {

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
            List<Speler> spelers = _manager.GeefSpelersVanTeam(team.TeamID);
            spelers.ForEach(s => { Spelers.Add(s); });
            AantalSpelersTBl.Text = Spelers.Count.ToString();

            SpelersMetPercentages.Clear();
            spelers.ForEach(s => { SpelersMetPercentages.Add(
                new Speler(s.Naam, s.RugNummer, s.Team, _manager.GeefPercentageAanwezigheid(s.SpelerID))); });
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
