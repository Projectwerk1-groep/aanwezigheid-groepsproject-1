﻿using AanwezigheidBL.Exceptions;
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
        public Training Training;
        public ObservableCollection<Team> Teams = [];
        public ObservableCollection<Speler> SpelersMetPercentages = [];
        public ObservableCollection<Aanwezigheid> AanwezighedenZonderTraining = [];
        public ObservableCollection<Training> Trainingen = [];

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
                RefreshTrainingList(Team);

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
                RefreshTrainingList(Team);
            }
        }

        private void OverzichtSpelers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OverzichtSpelers.SelectedItem is Aanwezigheid aanwezigheid)
            {
                if (aanwezigheid.IsAanwezig || !aanwezigheid.HeeftAfwezigheidGemeld)
                {
                    aanwezigheid.RedenAfwezigheid = ""; // Reset de reden
                }
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
            try
            {
                DateTime datum = DateTime.Today;
                string thema = "";
                if (MyDatePicker.SelectedDate.HasValue)
                { datum = MyDatePicker.SelectedDate.Value; }
                else
                { MessageBox.Show("Geen datum geselecteerd.", "Fout", MessageBoxButton.OK, MessageBoxImage.Warning); }

                if (ThemaTextBox.Text != null)
                { thema = ThemaTextBox.Text; }

                Training nieuweTraining = new(datum, thema, Team);
                _manager.VoegTrainingMetAanwezigheidToe(nieuweTraining, LeesIngegevenAanwezigheden());

                RefreshSpelerList(Team);
                RefreshTrainingList(Team);
                MessageBox.Show("Training is opgeslagen.");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void HistoriekTrainingen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HistoriekTrainingen.SelectedItem != null && HistoriekTrainingen.SelectedItem is Training selectedTraining)
            {
                Training = selectedTraining;
            }
        }

        private void WijzigTraining_Click(object sender, RoutedEventArgs e)
        {
            Training oldTraining = Training;
            try
            {
                if (HistoriekTrainingen.SelectedItem != null)
                {
                    WijzigTrainingInputDialog WijzigDialog = new(_manager, oldTraining);
                    if (WijzigDialog.ShowDialog() == true) // Open de dialog en controleer op OK
                    {
                        List<Aanwezigheid> oldAn = _manager.GeefAanwezighedenVanTraining(Training);
                        //ObservableCollection<Aanwezigheid> newAn = WijzigDialog.AanwezighedenVanTraining;
                        List<Aanwezigheid> newAn = WijzigDialog.NieuweAanwezighedenVanTraining;

                        foreach (Aanwezigheid oldA in oldAn)
                        {
                            foreach (Aanwezigheid newA in newAn)
                            {
                                _manager.WijzigAanwezigheid(oldA, newA);
                            }
                        }

                        RefreshSpelerList(Team);
                        RefreshTrainingList(Team);

                        MessageBox.Show($"De aanwezigheden van de Training van {oldTraining.Datum.Date.ToString("dd/MM/yyyy")} zijn gewijzigd.");
                    }
                }
                else
                {
                    MessageBox.Show("Selecteer eerst een training om te wijzigen.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExporteerTraining_Click(object sender, RoutedEventArgs e)
        {
            string downloadsPath = FolderManager.GetPath(KnownFolder.Downloads);            
            string filepath = $"{downloadsPath}\\TrainingGegevens.txt";

            try
            {
                if (TeamComboBox.SelectedItem != null && TeamComboBox.SelectedItem is Team selectedTeam)
                { Team = selectedTeam; }

                if (HistoriekTrainingen.SelectedItem != null && HistoriekTrainingen.SelectedItem is Training selectedTraining)
                {
                    Training = selectedTraining;
                    _manager.ExportAanwezigheidNaarTXT(Training, Team, filepath);
                    MessageBox.Show($"Een nieuw tekstbestand is aangemaakt in {downloadsPath}");
                }
                else
                {
                    MessageBox.Show("Selecteer eerst een training om te exporteren.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RedenComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.ComboBox comboBox)
            {
                comboBox.ItemsSource = Enum.GetValues(typeof(RedenVanAfwezigheid));
            }
        }

        public void RefreshSpelerList(Team team)
        {
            SpelersMetPercentages.Clear();
            AanwezighedenZonderTraining.Clear();

            List<Speler> spelers = _manager.GeefSpelersVanTeam(team.TeamID).OrderBy(s => s.RugNummer).ToList();
            spelers.ForEach(s =>
            {
                SpelersMetPercentages.Add(
                    new Speler(s.SpelerID, s.Naam, s.RugNummer, s.Team, _manager.GeefPercentageAanwezigheid(s.SpelerID)));
                AanwezighedenZonderTraining.Add(new Aanwezigheid(s, false, false, ""));
            });

            OverzichtSpelers.ItemsSource = AanwezighedenZonderTraining;
            DetailsSpelers.ItemsSource = SpelersMetPercentages;

            AantalSpelersTBl.Text = AanwezighedenZonderTraining.Count.ToString();
            Details_TeamAantalSpelers_TBl.Text = SpelersMetPercentages.Count.ToString();
        }

        public void RefreshTrainingList(Team team)
        {
            Trainingen.Clear();

            List<Training> trainingen = _manager.GeefTrainingenVanTeam(team.TeamID).OrderBy(t => t.Datum).ToList();
            trainingen.ForEach(t =>
            {
                Trainingen.Add(t);
            });

            HistoriekTrainingen.ItemsSource = Trainingen;

            Historiek_AantalTrainingen_TBl.Text = Trainingen.Count.ToString();
        }

        public List<Aanwezigheid> LeesIngegevenAanwezigheden()
        {
            List<Aanwezigheid> aanwezigheden = [];
            try
            {
                if (OverzichtSpelers.ItemsSource is ObservableCollection<Aanwezigheid> overzicht)
                {
                    foreach (Aanwezigheid a in overzicht)
                    {
                        Speler speler = a.Speler;
                        bool isAanwezig = a.IsAanwezig;
                        bool heeftAfwezigheidGemeld = a.HeeftAfwezigheidGemeld;
                        string redenAfwezigheid = "";
                        if (isAanwezig is true)
                            heeftAfwezigheidGemeld = false;
                        if (OverzichtSpelers.ItemContainerGenerator.ContainerFromItem(a) is ListViewItem listViewItem)
                        {
                            System.Windows.Controls.ComboBox redenComboBox = FindVisualChild<System.Windows.Controls.ComboBox>(listViewItem);
                            if (redenComboBox != null)
                            {
                                string geselecteerdeReden = redenComboBox.Text;
                                redenAfwezigheid = geselecteerdeReden;
                            }
                            else { throw new Exception(); }
                        }
                        aanwezigheden.Add(new(speler, isAanwezig, heeftAfwezigheidGemeld, redenAfwezigheid));
                    }
                }
                else
                { throw new Exception(); }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            return aanwezigheden;
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild)
                {
                    return typedChild;
                }

                T result = FindVisualChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

    }
}
