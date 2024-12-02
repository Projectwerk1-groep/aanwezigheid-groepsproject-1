using AanwezigheidBL.Managers;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CreateTraining: Window
    {
        public AanwezigheidManager _manager;

        public CreateTraining(AanwezigheidManager manager)
        {
            _manager = manager;
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
