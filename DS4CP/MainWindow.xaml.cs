using DS4CP.Classes;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DS4CP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        List<string> _profiles = new List<string>();
        Controllers ctrl = new Controllers();
        ObservableCollection<Controllers> controllers = new ObservableCollection<Controllers>();
        string _message = "message";
        string _log = "log";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            _profiles.Add("DEFAULT");
            _profiles.Add("NEW");

            ctrl.Id = "00:00:00:00:00:00";
            ctrl.Status = "CONNECTED";
            ctrl.Battery = 90;
            ctrl.Color = Brushes.Aqua;
            ctrl.Profile = _profiles;

            controllers.Add(ctrl);
            cbCloseMin.IsChecked = Convert.ToBoolean(ReadSetting("cbCloseMinimize"));
        }


        public ObservableCollection<Controllers> Controllers
        {
            get { return controllers; }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Log
        {
            get { return _log; }
            set
            {
                if (_log != value)
                {
                    _log = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void ReadAllSettings()
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
        }

        public string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                return "false";
            }
            
            
        }

        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch(ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private void OnClickAbout(object sender, RoutedEventArgs e)
        {
            ctrl.Id = "ma:c1:23:45:67:89";
            ctrl.Status = "disconnected";
            ShowStandardBalloon("hello");
            _profiles.Add("new item");
            this.Message = "fdsfdsfsfd";
            //AddUpdateAppSettings("cbCloseMinimize", "true");
            Console.WriteLine(_profiles.Count);
        }

        private void ShowStandardBalloon(string text)
        {
            string title = "DS4 Control Panel";
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


    public class ShowMessageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }


}
