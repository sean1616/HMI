using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm.Utils
{
    public class StatusViewModel : NotifyBase
    {
      
        private bool _isSystemStatusCheck;
        public bool IsSystemStatusCheck
        {
            get { return _isSystemStatusCheck; }
            set
            {
                _isSystemStatusCheck = value;
                OnPropertyChanged("IsSystemStatusCheck");
            }
        }

        private bool _isProcessStatusCheck;
        public bool IsProcessStatusCheck
        {
            get { return _isProcessStatusCheck; }
            set
            {
                _isProcessStatusCheck = value;
                OnPropertyChanged("IsProcessStatusCheck");
            }
        }


    }
}
