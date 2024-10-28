using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AanwezigheidProject_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Dummy inloggegevens
            string correctUsername = "admin";
            string correctPassword = "password";

            // Gebruikersinvoer
            string enteredUsername = UsernameTextBox.Text;
            string enteredPassword = PasswordBox.Password;

            // Controleren of gebruikersnaam en wachtwoord kloppen
            if (enteredUsername == correctUsername && enteredPassword == correctPassword)
            {
                MessageBox.Show("Succesvol ingelogd!", "Ingelogd", MessageBoxButton.OK, MessageBoxImage.Information);
                // Open volgende scherm of sluit huidig scherm
                this.Close();
            }
            else
            {
                ErrorTextBlock.Text = "Onjuiste gebruikersnaam of wachtwoord!";
            }
        }
    }
}