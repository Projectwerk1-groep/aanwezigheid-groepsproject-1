using AanwezigheidBL.Managers;
using AanwezigheidBL.Model;
using System;
using System.Collections.Generic;
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
            List<string> teams = _manager.GeefTeamsPerCoach(coach.CoachID).Select(x => x.TeamNaam).ToList();
            TeamComboBox.ItemsSource = teams;

            // Stel een standaardwaarde in
            TeamComboBox.SelectedItem = teams[0];
        }

        private void TeamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Stel het geselecteerde item in als de nieuwe standaardwaarde
            if (TeamComboBox.SelectedItem != null)
            {
                TeamComboBox.SelectedItem = TeamComboBox.SelectedItem;

                Team_Naam_TextBlock.Text = TeamComboBox.SelectedItem.ToString();

                Historiek_Team_Naam_TextBlock.Text = TeamComboBox.SelectedItem.ToString();
            }
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
