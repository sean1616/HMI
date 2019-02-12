using HMI_PermanentForm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HMI_PermanentForm.WebCam
{
    public class CamViewModel : NotifyBase
    {
        private BitmapSource _CameraImage;
        public BitmapSource CameraImage
        {
            get
            {
                return _CameraImage;
            }
            set
            {
                _CameraImage = value;
                OnPropertyChanged("CameraImage");
            }
        }

    }
}
