using HMI_PermanentForm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_PermanentForm.G_folder.G10.ViewModel
{
    public class ErrorMsgVM : NotifyBase
    {
        private string _msg;
        public string Msg
        {
            get { return _msg; }
            set
            {
                _msg = value;
                OnPropertyChanged("Msg");
            }
        }
    }
}
