using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HMI_PermanentForm
{
    class Data_A2_2:INotifyPropertyChanged
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

        private double rotateValue;
        public double RotateValue
        {
            get { return rotateValue; }
            set
            {
                if (rotateValue != value)
                {
                    rotateValue = value;
                    OnPropertyChanged("RotateValue");
                }
            }
        }
    }
}
