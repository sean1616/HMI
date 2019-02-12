using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm.A_folder.A4.ViewModel
{
    public class A4_1_ViewModel : NotifyBase
    {
        private string _isAxisCheck_ImgSrc;
        public string IsAxisCheckImgSrc
        {
            get { return _isAxisCheck_ImgSrc; }
            set { _isAxisCheck_ImgSrc = value;
                OnPropertyChanged("IsAxisCheckImgSrc");
            }
        }

        private string _isPowderSupplyCheck_ImgSrc;
        public string IsPowderSupplyCheckImgSrc
        {
            get { return _isPowderSupplyCheck_ImgSrc; }
            set
            {
                _isPowderSupplyCheck_ImgSrc = value;
                OnPropertyChanged("IsPowderSupplyCheckImgSrc");
            }
        }

        private string _isHeaterCheck_ImgSrc;
        public string IsHeaterCheckImgSrc
        {
            get { return _isHeaterCheck_ImgSrc; }
            set
            {
                _isHeaterCheck_ImgSrc = value;
                OnPropertyChanged("IsHeaterCheckImgSrc");
            }
        }
    }
}
