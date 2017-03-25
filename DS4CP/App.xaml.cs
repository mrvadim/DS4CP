using System.Threading;
using System.Windows;

namespace DS4CP
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static Mutex mutex = null;

        protected void Main(object sender, StartupEventArgs e)
        {
            //mutex - only one instance of the App

            bool startminimized = false;
            bool createdNew;
            mutex = new Mutex(true, "{cfa30a16-d728-4c11-b076-a0dc4cc1ae1d}", out createdNew);


            //arg keys start App
            for (int i = 0; i != e.Args.Length; ++i)
            {
                if (e.Args[i] == "-minimized")
                {
                    startminimized = true;
                }
            }
            MainWindow mainWindow = new MainWindow();
            if (startminimized)
            {
                mainWindow.WindowState = WindowState.Minimized;
            }

            if (!createdNew)
            {
                MainWindow.WindowState = WindowState.Maximized;
                Current.Shutdown();
                return;
            }



            //Trace.WriteLine( Guid.NewGuid().ToString() );

            MainWindow.Show();
        }


    }
}
