using System.Collections.Generic;
using System.Windows.Media;

namespace DS4CP.Classes
{
    public class Controllers : MyNotifyPropChanged
    {
        private string id { get; set; }
        public string Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string status { get; set; }
        public string Status
        {
            get { return status; }
            set
            {
                if (value != status)
                {
                    status = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int battery { get; set; }
        public int Battery
        {
            get { return battery; }
            set
            {
                if (value != battery)
                {
                    battery = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private List<string> profile { get; set; }
        public List<string> Profile
        {
            get { return profile; }
            set
            {
                if (value != profile)
                {
                    profile = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Brush color { get; set; }
        public Brush Color
        {
            get { return color; }
            set
            {
                if (value != color)
                {
                    color = value;
                    NotifyPropertyChanged();
                }
            }
        }




        /*public Controllers(string Id, string Status, int Battery, Brush Color, List<string> Profile)
        {
            this.Id = Id;
            this.Status = Status;
            this.Battery = Battery;
            this.Color = Color;
            this.Profile = Profile;
        }*/


    }
}
