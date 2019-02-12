using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm
{    
    public class NotifyModels : NotifyBase
    {
        private int CompleteValueI;

        public int CompleteValue
        {
            get { return CompleteValueI; }
            set
            {
                CompleteValueI = value;
                OnPropertyChanged("CompleteValue");
            }
        }

        private double PercentValueI;
        public double PercentValue
        {
            get { return PercentValueI; }
            set
            {
                PercentValueI = value;
                OnPropertyChanged("PercentValue");
            }
        }

        private TimeSpan TimeValueI;
        public TimeSpan TimeValue
        {
            get { return TimeValueI; }
            set
            {
                TimeValueI = value;
                OnPropertyChanged("TimeValue");
            }
        }

        private string FinishedDateValueI;
        public string FinishedDateValue
        {
            get { return FinishedDateValueI; }
            set
            {
                FinishedDateValueI = value;
                OnPropertyChanged("FinishedDateValue");
            }
        }

        private string FinishedTimeValueI;
        public string FinishedTimeValue
        {
            get { return FinishedTimeValueI; }
            set
            {
                FinishedTimeValueI = value;
                OnPropertyChanged("FinishedTimeValue");
            }
        }
        private string A2, Rest1;
        private int A3, A1;
        public string Rest
        {
            get { return Rest1; }
            set
            {
                A3 = value.ToString().Length;
                A1 = 7 - A3;
                A2 = "0";

                String pnum = A2.PadLeft(A1, '0');

                int Aa = Convert.ToInt16(pnum);

                Rest1 = pnum;
                OnPropertyChanged("Rest");
            }
        }

        private int _totalLayer;
        public int TotalLayer
        {
            get { return _totalLayer; }
            set
            {
                _totalLayer = value;
                OnPropertyChanged("TotalLayer");
            }
        }

        private int _currentLayer;
        public int CurrentLayer
        {
            get { return _currentLayer; }
            set
            {
                _currentLayer = value;
                OnPropertyChanged("CurrentLayer");
            }
        }

    }
}
