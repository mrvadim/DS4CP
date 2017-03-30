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
        private string _message = "message";
        string _log = "log";
        AppConfig _config = new AppConfig();

        public MainWindow()
        {
            InitializeComponent();
            _config.InitSettings();
            DataContext = this;

            _profiles.Add("DEFAULT");
            _profiles.Add("NEW");

            ctrl.Id = "00:00:00:00:00:00";
            ctrl.Status = "CONNECTED";
            ctrl.Battery = 90;
            ctrl.Color = Brushes.Aqua;
            ctrl.Profile = _profiles;

            controllers.Add(ctrl);

            //Console.WriteLine(_config.ReadSetting("cbCloseMinimize"));

            Message = _config.InitSettings();
            cbCloseMinimize.IsChecked = Convert.ToBoolean(_config.ReadSetting("cbCloseMinimize"));
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

        private void OnClickAbout(object sender, RoutedEventArgs e)
        {
            ctrl.Id = "ma:c1:23:45:67:89";
            ctrl.Status = "disconnected";
            ShowStandardBalloon("hello");
            _profiles.Add("new item");
            Message = "fdsfdsfsfd";
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

            Console.WriteLine("Window_Closed");

            WindowState = WindowState.Minimized;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("Window_Closing");
            if (cbCloseMinimize.IsChecked ?? true)
            {
                Console.WriteLine("cbCloseMinimize");
                e.Cancel = true;
                WindowState = WindowState.Minimized;
                Hide();
            }
            else
            {
                Application.Current.Shutdown();
            }
            
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

        private void cbCloseMinChecked(object sender, RoutedEventArgs e)
        {
            Message = _config.AddUpdateAppSettings("cbCloseMinimize", "true");
        }

        private void cbCloseMinUnChecked(object sender, RoutedEventArgs e)
        {
            Message = _config.AddUpdateAppSettings("cbCloseMinimize", "false");
        }
    }


    public class ShowMessageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.Activate();
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
    }

    public class AppConfig
    {
        public string InitSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            string message = "settings exist";
            if (appSettings.Count == 0)
            {
                message = "init settings";
                AddUpdateAppSettings("cbCloseMinimize", "false");
            }
            return message;
        }
        public void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                if (appSettings.Count == 0)
                {
                    Console.WriteLine("empty");
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
                Console.WriteLine("error");
            }
        }
        public string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "error";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                return "error read settings";
            }
        }
        public string AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                string result;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                    result = "settings added";

                }
                else
                {
                    settings[key].Value = value;
                    result = "settings updated";
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("error");
                return "error to update settings";
            }
        }
    }
}