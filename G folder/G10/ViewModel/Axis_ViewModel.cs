using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm.G_folder.G10.ViewModel
{
    public class Axis_ViewModel : NotifyBase
    {
        #region Encoder
        private string _encoderX;

        public string EncoderX
        {
            get { return _encoderX; }
            set
            {
                _encoderX = value;
                OnPropertyChanged("EncoderX");
            }
        }

        private string _encoderZ;

        public string EncoderZ
        {
            get { return _encoderZ; }
            set
            {
                _encoderZ = value;
                OnPropertyChanged("EncoderZ");
            }
        }

        private string _encoderU1;

        public string EncoderU1
        {
            get { return _encoderU1; }
            set
            {
                _encoderU1 = value;
                OnPropertyChanged("EncoderU1");
            }
        }

        private string _encoderU2;

        public string EncoderU2
        {
            get { return _encoderU2; }
            set
            {
                _encoderU2 = value;
                OnPropertyChanged("EncoderU2");
            }
        }
        #endregion

        #region Limit
        private string _limitX;
        public string LimitX
        {
            get{return _limitX;}
            set
            {
                _limitX = value;
                OnPropertyChanged("LimitX");
            }
        }

        private string _limitZ;
        public string LimitZ
        {
            get { return _limitZ; }
            set
            {
                _limitZ = value;
                OnPropertyChanged("LimitZ");
            }
        }

        private string _limitU1;
        public string LimitU1
        {
            get { return _limitU1; }
            set
            {
                _limitU1 = value;
                OnPropertyChanged("LimitU1");
            }
        }

        private string _limitU2;
        public string LimitU2
        {
            get { return _limitU2; }
            set
            {
                _limitU2 = value;
                OnPropertyChanged("LimitU2");
            }
        }
        #endregion

        #region Alarm
        private string _alarmX;
        public string AlarmX
        {
            get { return _alarmX; }
            set
            {
                _alarmX = value;
                OnPropertyChanged("AlarmX");
            }
        }

        private string _alarmZ;
        public string AlarmZ
        {
            get { return _alarmZ; }
            set
            {
                _alarmZ = value;
                OnPropertyChanged("AlarmZ");
            }
        }
        #endregion

        #region INP
        private string _INPX;
        public string INPX
        {
            get { return _INPX; }
            set
            {
                _INPX = value;
                OnPropertyChanged("INPX");
            }
        }

        private string _INPZ;
        public string INPZ
        {
            get { return _INPZ; }
            set
            {
                _INPZ = value;
                OnPropertyChanged("INPZ");
            }
        }
        #endregion

        #region State
        private string _stateU1;
        public string StateU1
        {
            get { return _stateU1; }
            set
            {
                _stateU1 = value;
                OnPropertyChanged("StateU1");
            }
        }

        private string _stateU2;
        public string StateU2
        {
            get { return _stateU2; }
            set
            {
                _stateU2 = value;
                OnPropertyChanged("StateU2");
            }
        }
        #endregion
    }
}
