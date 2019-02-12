using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HMI_PermanentForm
{
    public class Data_A5_1:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private double num;
        public double Num
        {
            get
            {
                return num;
            }
            set
            {
                num = value;
                OnPropertyChanged("Num");
            }
        }
    }
}
