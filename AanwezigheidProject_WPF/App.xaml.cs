using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;

namespace AanwezigheidProject_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: "
                            + e.Exception.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);

            e.Handled = true;
        }

    }

}
