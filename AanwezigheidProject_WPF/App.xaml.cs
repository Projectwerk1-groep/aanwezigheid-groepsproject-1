using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using AanwezigheidDL_SQL;
using AanwezigheidBL.Managers;
using AanwezigheidBL.Interfaces;


namespace AanwezigheidProject_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string connectionstring = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB_Aanwezigheid;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

            IAanwezigheidRepository AanwezigheidRepo = new AanwezigheidRepository(connectionstring);

            AanwezigheidManager manager = new(AanwezigheidRepo);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: "
                            + e.Exception.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);

            e.Handled = true;
        }

    }
}
