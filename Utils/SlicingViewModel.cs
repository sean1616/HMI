using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace HMI_PermanentForm.Utils
{
    public class SlicingViewModel : NotifyModels
    {
        private int _numOfLayers;
        public int NumOfLayers
        {
            get
            {
                return _numOfLayers;
            }

            set
            {
                this._numOfLayers = value;
                OnPropertyChanged("NumOfLayers");
            }
        }
        private int _lastLayerIndex;
        public int LastLayerIndex
        {
            get
            {
                return _lastLayerIndex;
            }

            set
            {
                this._lastLayerIndex = value;
                OnPropertyChanged("LastLayerIndex");
            }
        }

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

        #region Point3D
        //private Point3D _Position = new Point3D(0,-100,250);
        //public Point3D Position
        //{
        //    get
        //    {
        //        return this._Position;
        //    }
        //    set
        //    {
        //        this._Position = value;
        //        OnPropertyChanegd("Position");
        //    }
        //}
        
        //private Point3D _LookDirection = new Point3D(0, 100, -250);
        //public Point3D LookDirection
        //{
        //    get
        //    {
        //        return this._LookDirection;
        //    }
        //    set
        //    {
        //        this._LookDirection = value;
        //        OnPropertyChanegd("LookDirection");
        //    }
        //}
        
        //    private Point3D _UpDirection = new Point3D(0, 0, 1);
        //public Point3D UpDirection
        //{
        //    get
        //    {
        //        return this._UpDirection;
        //    }
        //    set
        //    {
        //        this._UpDirection = value;
        //        OnPropertyChanegd("UpDirection");
        //    }
        //}
        #endregion

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
