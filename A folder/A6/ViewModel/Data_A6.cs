using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm.A_folder.A6.ViewModel
{
    public class Data_A6 : NotifyBase
    {
        private double _A6_03_TorrDefaultValue;
        public double A6_03_TorrDefaultValue
        {
            get { return _A6_03_TorrDefaultValue; }
            set
            {
                _A6_03_TorrDefaultValue = value;
                OnPropertyChanged("A6_03_TorrDefaultValue");
            }
        }

        private double _A6_03_TorrCurrentValue;
        public double A6_03_TorrCurrentValue
        {
            get { return _A6_03_TorrCurrentValue; }
            set
            {
                _A6_03_TorrCurrentValue = value;
                OnPropertyChanged("A6_03_TorrCurrentValue");
            }
        }

        private double _A6_03_TargetTorrValue;
        public double A6_03_TargetTorrValue
        {
            get { return _A6_03_TargetTorrValue; }
            set
            {
                _A6_03_TargetTorrValue = value;
                OnPropertyChanged("A6_03_TargetTorrValue");
            }
        }

        private double _A6_05_O2Value;
        public double A6_05_O2Value
        {
            get { return _A6_05_O2Value; }
            set
            {
                _A6_05_O2Value = value;
                OnPropertyChanged("A6_05_O2Value");
            }
        }

        private int _flowLevel;
        public int FlowLevel
        {
            get { return _flowLevel; }
            set {
                _flowLevel = value;
                OnPropertyChanged("FlowLevel");
            }
        }

        private bool _isFlowModulationSafe;
        public bool IsFlowModulationSafe
        {
            get { return _isFlowModulationSafe; }
            set
            {
                _isFlowModulationSafe = value;
                OnPropertyChanged("IsFlowModulationSafe");
            }
        }

        private bool _isVacuumSafe;
        public bool IsVacuumSafe
        {
            get { return _isVacuumSafe; }
            set
            {
                _isVacuumSafe = value;
                OnPropertyChanged("IsVacuumSafe");
            }
        }


    }
}
