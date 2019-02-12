using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HMI_PermanentForm.Utils
{
    public class DIOViewModel : NotifyBase
    {
        private bool _isOn;
        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                this._isOn = value;
                OnPropertyChanged("IsOn");
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set 
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged("Index");

            }
        }

    }
}
