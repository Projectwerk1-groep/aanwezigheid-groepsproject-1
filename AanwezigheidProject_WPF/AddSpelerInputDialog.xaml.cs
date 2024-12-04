using System.Windows;

namespace AanwezigheidProject_WPF
{
    public partial class AddSpelerInputDialog : Window
    {
        public string Naam { get; private set; }
        public int Rugnummer { get; private set; }

        public AddSpelerInputDialog()
        {
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
    }        
}

