using AanwezigheidBL.Model;
using Microsoft.Identity.Client;
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

namespace AanwezigheidProject_WPF
{
    /// <summary>
    /// Interaction logic for WijzigSpelerInputDialog.xaml
    /// </summary>
    public partial class WijzigSpelerInputDialog : Window
    {
        public string Naam { get; private set; }
        public int Rugnummer { get; private set; }
        public Speler Speler { get; private set; }

        public WijzigSpelerInputDialog(Speler speler)
        {
            Speler = speler;
            Naam = speler.Naam;
            Rugnummer = speler.RugNummer;

            InitializeComponent();

            WindowState = WindowState.Normal;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Valideer invoer
            if (string.IsNullOrWhiteSpace(NaamTextBox.Text) ||
                !int.TryParse(RugnummerTextBox.Text, out int rugnummer))
            {
                MessageBox.Show("Voer een geldige naam EN nummer in.");
                return;
            }

            Naam = NaamTextBox.Text;
            Rugnummer = rugnummer;
            DialogResult = true; // Sluit het venster en return een "true"-resultaat.
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Sluit het venster en return een "false"-resultaat.
        }

        private void Text_Loaded(object sender, RoutedEventArgs e)
        {
            NaamTextBox.Text = Naam;
            RugnummerTextBox.Text = Rugnummer.ToString();
        } 
    }
}
