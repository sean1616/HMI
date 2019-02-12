using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm
{
    public class Data_A3_1:NotifyBase
    {
        private double _A3_03_TemperatureValue;
        public double A3_03_TemperatureValue
        {
            get { return _A3_03_TemperatureValue; }
            set
            {
                _A3_03_TemperatureValue = value;
                OnPropertyChanged("A3_03_TemperatureValue");
            }
        }

        private double _A3_03_TargetTemperatureValue;
        public double A3_03_TargetTemperatureValue
        {
            get
            {
                return _A3_03_TargetTemperatureValue;
            }
            set
            {
                _A3_03_TargetTemperatureValue = value;
                OnPropertyChanged("A3_03_TargetTemperatureValue");
            }
        }

        private double _A3_10_TemperatureValue;
        public double A3_10_TemperatureValue
        {
            get { return _A3_10_TemperatureValue; }
            set
            {
                _A3_10_TemperatureValue = value;
                OnPropertyChanged("A3_10_TemperatureValue");
            }
        }

        private double _A3_10_TargetTemperatureValue;
        public double A3_10_TargetTemperatureValue
        {
            get
            {
                return _A3_10_TargetTemperatureValue;
            }
            set
            {
                _A3_10_TargetTemperatureValue = value;
                OnPropertyChanged("A3_10_TargetTemperatureValue");
            }
        }

        private double _A3_17_TemperatureValue;
        public double A3_17_TemperatureValue
        {
            get { return _A3_17_TemperatureValue; }
            set
            {
                _A3_17_TemperatureValue = value;
                OnPropertyChanged("A3_17_TemperatureValue");
            }
        }

        private double _A3_17_TargetTemperatureValue;
        public double A3_17_TargetTemperatureValue
        {
            get
            {
                return _A3_17_TargetTemperatureValue;
            }
            set
            {
                _A3_17_TargetTemperatureValue = value;
                OnPropertyChanged("A3_17_TargetTemperatureValue");
            }
        }

        ////溫度數值
        //private double _A3_TemperatureValue;
        //public double A3_TemperatureValue
        //{
        //    get
        //    {
        //        return _A3_TemperatureValue;
        //    }
        //    set
        //    {
        //        _A3_TemperatureValue = value;
        //        OnPropertyChanged("A3_TemperatureValue");
        //    }
        //}

        ////目標溫度
        //private double _A3_TargetTemperatureValue;
        //public double A3_TargetTemperatureValue
        //{
        //    get
        //    {
        //        return _A3_TargetTemperatureValue;
        //    }
        //    set
        //    {
        //        _A3_TargetTemperatureValue = value;
        //        OnPropertyChanged("A3_TargetTemperatureValue");
        //    }
        //}

       
    }
}
