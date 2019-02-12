using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMI_PermanentForm.Utils;
using System.Windows.Media.Media3D;

namespace HMI_PermanentForm.A_folder.A4.ViewModel
{
    class A4_2_ViewModel : NotifyBase
    {

        //private int _numOfLayers;
        //public int NumOfLayers
        //{
        //    get
        //    {
        //        return _numOfLayers;
        //    }

        //    set
        //    {
        //        this._numOfLayers = value;
        //        OnPropertyChanegd("NumOfLayers");
        //    }
        //}

        private Point3D _currentPosition;

        public Point3D CurrentPosition
        {
            get
            {
                return this._currentPosition;
            }
            set
            {
                this._currentPosition = value;
                OnPropertyChanged("CurrentPosition");
            }
        }

        private Point3DCollection _polygon;

        public Point3DCollection Polygon
        {
            get
            {
                return this._polygon;
            }

            set
            {
                this._polygon = value;
                OnPropertyChanged("Polygon");
            }
        }

        private Point3DCollection _hatch;

        public Point3DCollection Hatch
        {
            get
            {
                return this._hatch;
            }

            set
            {
                this._hatch = value;
                OnPropertyChanged("Hatch");
            }
        }

    }
}
