﻿using DS4CP.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            MProfiles.Add("DEFAULT");
            MProfiles.Add("NEW");

            ctrl.Id = "00:00:00:00:00:00";
            ctrl.Status = "CONNECTED";
            ctrl.Battery = 80;
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

        public List<string> MProfiles { get => mProfiles; set => mProfiles = value; }

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

        }
    }
}