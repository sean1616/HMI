using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace HMI_PermanentForm
{
    public class Data_A1_2 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private double translateValue;
        public double TranslateValue
        {
            get { return translateValue; }
            set
            {
                if (translateValue != value)
                {
                    translateValue = value;
                    OnPropertyChanged("TranslateValue");

                }
            }
        }

        private double translateValue_Arrow_Y;
        public double TranslateValue_Arrow_Y
        {
            get { return translateValue_Arrow_Y; }
            set
            {
                if (translateValue_Arrow_Y != value)
                {
                    translateValue_Arrow_Y = value;
                    OnPropertyChanged("TranslateValue_Arrow_Y");

                }
            }
        }

        private double rotateValue1;
        public double RotateValue1
        {
            get { return rotateValue1; }
            set
            {
                if (rotateValue1 != value)
                {
                    rotateValue1 = value;
                    OnPropertyChanged("RotateValue1");
                }
            }
        }

        private double rotateValue2;
        public double RotateValue2
        {
            get { return rotateValue2; }
            set
            {
                if (rotateValue2 != value)
                {
                    rotateValue2 = value;
                    OnPropertyChanged("RotateValue2");
                }
            }
        }

        private Visibility _Jog_visibility = Visibility.Hidden;
        public Visibility Jog_visibility
        {
            get { return _Jog_visibility; }
            set
            {
                _Jog_visibility = value;
                OnPropertyChanged("Jog_visibility");
            }
        }

        private bool _Jog_U1_enable = false;
        public bool Jog_U1_enable
        {
            get { return _Jog_U1_enable; }
            set
            {
                _Jog_U1_enable = value;
                OnPropertyChanged("Jog_U1_enable");
            }
        }

        private bool _Jog_U2_enable = false;
        public bool Jog_U2_enable
        {
            get { return _Jog_U2_enable; }
            set
            {
                _Jog_U2_enable = value;
                OnPropertyChanged("Jog_U2_enable");
            }
        }

        private bool _Jog_Up_down_enable = false;
        public bool Jog_Up_down_enable
        {
            get { return _Jog_Up_down_enable; }
            set
            {
                _Jog_Up_down_enable = value;
                OnPropertyChanged("Jog_Up_down_enable");
            }
        }

        private bool _Jog_X_enable = false;
        public bool Jog_X_enable
        {
            get { return _Jog_X_enable; }
            set
            {
                _Jog_X_enable = value;
                OnPropertyChanged("Jog_X_enable");
            }
        }

        private Visibility _U1_visibility = Visibility.Hidden;
        public Visibility U1_visibility
        {
            get { return _U1_visibility; }
            set
            {
                _U1_visibility = value;
                OnPropertyChanged("U1_visibility");
            }
        }

        private Visibility _U2_visibility = Visibility.Hidden;
        public Visibility U2_visibility
        {
            get { return _U2_visibility; }
            set
            {
                _U2_visibility = value;
                OnPropertyChanged("U2_visibility");
            }
        }

        private double increment;
        public double Increment
        {
            get { return increment; }
            set
            {
                increment = value;
                OnPropertyChanged("Increment");
            }
        }

        private GridLength _JogRegionSize = new GridLength(20, GridUnitType.Auto);
        public GridLength JogRegionSize
        {
            get
            {
                return _JogRegionSize;
            }
            set
            {
                _JogRegionSize = value;
                OnPropertyChanged("JogRegionSize");
            }
        }
    }
}
