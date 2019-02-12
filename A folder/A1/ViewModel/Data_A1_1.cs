using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm.A_folder.A1.ViewModel
{
    public class Data_A1_1 : NotifyBase
    {

        double _X_Abs_Pos;
        public double X_Abs_Pos
        {
            get { return _X_Abs_Pos; }
            set
            {
                _X_Abs_Pos = value;
                OnPropertyChanged("X_Abs_Pos");
            }
        }

        double _Z_Abs_Pos;
        public double Z_Abs_Pos
        {
            get { return _Z_Abs_Pos; }
            set
            {
                _Z_Abs_Pos = value;
                OnPropertyChanged("Z_Abs_Pos");
            }

        }

        double _U1_Abs_Pos;
        public double U1_Abs_Pos
        {
            get { return _U1_Abs_Pos; }
            set
            {
                _U1_Abs_Pos = value;
                OnPropertyChanged("U1_Abs_Pos");
            }

        }

        double _U2_Abs_Pos;
        public double U2_Abs_Pos
        {
            get { return _U2_Abs_Pos; }
            set
            {
                _U2_Abs_Pos = value;
                OnPropertyChanged("U2_Abs_Pos");
            }

        }
    }
}
