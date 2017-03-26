using DS4CP.Classes;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DS4CP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string message;
        List<string> mProfiles = new List<string>();
        Controllers ctrl = new Controllers();
        //private TaskbarIcon tb;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //tb = (TaskbarIcon)FindResource("/Resources/NotifyIcon");

            MProfiles.Add("DEFAULT");
            MProfiles.Add("NEW");

            ctrl.Id = "00:00:00:00:00:00";
            ctrl.Status = "CONNECTED";
            ctrl.Battery = 90;
            ctrl.Color = Brushes.Aqua;
            ctrl.Profile = MProfiles;

            mControllers.Add(ctrl);

            ReadSettings();
        }

        ObservableCollection<Controllers> mControllers = new ObservableCollection<Controllers>();
        public ObservableCollection<Controllers> Controllers
        {
            get { return mControllers; }
        }

        public string Message
        {
            get { return message; }
        }

        public List<string> MProfiles
        {
            get { return mProfiles; }
            set
            {
                mProfiles = value;
            }
        }

        public void ReadSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                if (appSettings.Count == 0)
                {
                    Console.WriteLine("config empty");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        Console.WriteLine("key: {0} value: {1}", key, appSettings[key]);
                    }
                }

            }
            catch
            {
                Console.WriteLine("error read config");
            }

            message = ConfigurationManager.AppSettings["setting1"];

        }

        private void OnClickAbout(object sender, RoutedEventArgs e)
        {
            ctrl.Id = "ma:c1:23:45:67:89";
            ctrl.Status = "disconnected";
            ShowStandardBalloon("hello");

        }

        private void ShowStandardBalloon(string text)
        {
            string title = "DS4 Control Panel";
            

            //show balloon with built-in icon
            MyNotifyIcon.ShowBalloonTip(title, text, BalloonIcon.Info);
            //MyNotifyIcon.ShowBalloonTip(title, text, BalloonIcon.Warning);
            //MyNotifyIcon.ShowBalloonTip(title, text, BalloonIcon.Error);

            //hide balloon
            //MyNotifyIcon.HideBalloonTip();

        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized) Show();
            WindowState = WindowState.Normal;
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
            Hide();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            Console.WriteLine("OnStateChanged");
            if (WindowState == WindowState.Minimized) Hide();

        }

        

    }


    public class ShowMessageCommand : ICommand
    {
        public void Execute(object parameter)
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }


}
