using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HMI_PermanentForm
{
    public class ProgressBarStraightData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // 名稱
        private string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }
        }

        // 目前數值
        private float _NowValue;
        public float NowValue
        {
            get
            {
                return _NowValue;
            }
            set
            {
                _NowValue = value;
                OnPropertyChanged("NowValue");
            }
        }

        //  單位
        private string _Unit;
        public string Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                _Unit = value;
                OnPropertyChanged("Unit");
            }
        }

        //  最大值
        private float _MaxValue;
        public float MaxValue
        {
            get
            {
                return _MaxValue;
            }
            set
            {
                _MaxValue = value;
                OnPropertyChanged("MaxValue");
            }
        }

        //  第一個閥值
        private float _ThresholdValue1;
        public float ThresholdValue1
        {
            get
            {             
                return _ThresholdValue1;
            }
            set
            {                
                _ThresholdValue1 = value;                                             
                OnPropertyChanged("ThresholdValue1");
            }
        }
        private float _ChangeValue1;
        public float ChangeValue1
        {
            get
            {
                return _ChangeValue1;
            }
            set
            {
                _ChangeValue1 = StraightProgressBar(NowValue, MaxValue);
                if (NowValue > _ThresholdValue1)
                {
                    _ChangeValue1 = StraightProgressBar(_ThresholdValue1, MaxValue);
                }
                OnPropertyChanged("ChangeValue1");
            }
        }

        //  第二個閥值
        private float _ThresholdValue2;
        public float ThresholdValue2
        {
            get
            {
                return _ThresholdValue2;
            }
            set
            {
                _ThresholdValue2 = value;
                OnPropertyChanged("ThresholdValue2");
            }
        }
        private float _ChangeValue2;
        public float ChangeValue2
        {
            get
            {
                return _ChangeValue2;
            }
            set
            {
                _ChangeValue2 = StraightProgressBar(NowValue, MaxValue);
                if (NowValue > _ThresholdValue2)
                {
                    _ChangeValue2 = StraightProgressBar(_ThresholdValue2, MaxValue);
                }

                OnPropertyChanged("ChangeValue2");
            }
        }

      
        private float _ThresholdValue3;
        public float ThresholdValue3
        {
            get
            {
                return _ThresholdValue3;
            }
            set
            {
                _ThresholdValue3 = value;           
                OnPropertyChanged("ThresholdValue3");
            }
        }
        private float _ChangeValue3;
        public float ChangeValue3
        {
            get
            {
                return _ChangeValue3;
            }
            set
            {
                _ChangeValue3 = StraightProgressBar(NowValue, MaxValue);
                if (NowValue > _ThresholdValue3)
                {
                    _ChangeValue3 = StraightProgressBar(_ThresholdValue3, MaxValue);
                }
               
                OnPropertyChanged("ChangeValue3");
            }
        }

        private float StraightProgressBar(float th, float max)
        {
            float scale = 710 / max;

            th = th * scale;

            return th;
        }
     
    }
}
