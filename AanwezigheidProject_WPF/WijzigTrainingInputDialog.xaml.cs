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
using AanwezigheidBL.Model;
using AanwezigheidBL.Managers;
using Microsoft.IdentityModel.Tokens;

namespace AanwezigheidProject_WPF
{
    /// <summary>
    /// Interaction logic for WijzigTrainingInputDialog.xaml
    /// </summary>
    public partial class WijzigTrainingInputDialog : Window
    {
        public AanwezigheidManager _manager;

        public Speler Speler { get; private set; }

        public Training Training { get; private set; }

        public DateTime Datum { get; private set; }

        public ObservableCollection<Aanwezigheid> AanwezighedenVanTraining = [];
        public List<Aanwezigheid> NieuweAanwezighedenVanTraining = [];


        public WijzigTrainingInputDialog(AanwezigheidManager manager, Training training)
        {
            _manager = manager;
            Training = training;
            Datum = training.Datum;

            InitializeComponent();

            RefreshSpelerList();
            TrainingDatum.Text = Datum.Date.ToString();
            OverzichtAanwezigheden.ItemsSource = AanwezighedenVanTraining;

            WindowState = WindowState.Maximized;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        //private void OverzichtAanwezigheden_Loaded(object sender, RoutedEventArgs e)
        //{
        //    TrainingDatum.Text = Datum.Date.ToString();

        //    OverzichtAanwezigheden.ItemsSource = AanwezighedenVanTraining;
        //}

        private void OverzichtAanwezigheden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OverzichtAanwezigheden.SelectedItem is Aanwezigheid aanwezigheid)
            {
                if (aanwezigheid.IsAanwezig || !aanwezigheid.HeeftAfwezigheidGemeld)
                {
                    aanwezigheid.RedenAfwezigheid = ""; // Reset de reden
                }
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Valideer invoer
            NieuweAanwezighedenVanTraining = LeesIngegevenAanwezigheden();

            DialogResult = true; // Sluit het venster en return een "true"-resultaat.
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Sluit het venster en return een "false"-resultaat.
        }

        public void RefreshSpelerList()
        {
            AanwezighedenVanTraining.Clear();

            List<Aanwezigheid> aanwezigheden = _manager.GeefAanwezighedenVanTraining(Training);
            aanwezigheden.ForEach(a =>
            {
                AanwezighedenVanTraining.Add(new(a.Speler, a.Training, a.IsAanwezig, a.HeeftAfwezigheidGemeld, a.RedenAfwezigheid));
            });
        }

        public List<Aanwezigheid> LeesIngegevenAanwezigheden()
        {
            List<Aanwezigheid> aanwezigheden = [];
            try
            {
                if (OverzichtAanwezigheden.ItemsSource is ObservableCollection<Aanwezigheid> overzicht)
                {
                    foreach (Aanwezigheid a in overzicht)
                    {
                        Speler speler = a.Speler;
                        bool isAanwezig = a.IsAanwezig;
                        bool heeftAfwezigheidGemeld;
                        string redenAfwezigheid = a.RedenAfwezigheid;

                        if (isAanwezig is true)
                        {
                            heeftAfwezigheidGemeld = false;
                            redenAfwezigheid = "";
                        }
                        else
                        {
                            heeftAfwezigheidGemeld = a.HeeftAfwezigheidGemeld;
                        }

                        if (heeftAfwezigheidGemeld)
                        {
                            if (OverzichtAanwezigheden.ItemContainerGenerator.ContainerFromItem(a) is ListViewItem listViewItem && redenAfwezigheid.IsNullOrEmpty())
                            {
                                System.Windows.Controls.ComboBox redenComboBox = FindVisualChild<System.Windows.Controls.ComboBox>(listViewItem);

                                if (redenComboBox != null)
                                {
                                    string geselecteerdeReden = redenComboBox.Text;
                                    redenAfwezigheid = geselecteerdeReden;
                                }
                                else { throw new Exception(); }
                            }
                        }
                        else { redenAfwezigheid = ""; }

                        aanwezigheden.Add(new(speler, Training, isAanwezig, heeftAfwezigheidGemeld, redenAfwezigheid));
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

        private void RedenComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.ComboBox comboBox)
            {
                comboBox.ItemsSource = Enum.GetValues(typeof(RedenVanAfwezigheid));
            }
        }
    }
}
