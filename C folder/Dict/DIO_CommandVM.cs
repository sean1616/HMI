using AM_Kernel.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HMI_PermanentForm.C_folder.Dict
{
    class DIO_Command : ICommand
    {
        public Action<object> ExecuteCommand { get; set; }
        public Func<object, bool> CanExecuteCommand { get; set; }

        public DIO_Command(Action<object> execute, Func<object, bool> canexecute)
        {
            ExecuteCommand = execute;
            CanExecuteCommand = canexecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteCommand(parameter);
        }
    }

    class DO_CommandVM : Utils.NotifyBase
    {
        public ICommand DO_Check { get; set; }
        public ICommand DO_UnCheck { get; set; }

        public DO_CommandVM()
        {
            DO_Check = new DIO_Command(DO_CheckEvent, Can_DO_CheckEvent);    
        }

        private void DO_CheckEvent(object param)
        {
            DIO_DevType type = DIO_DevType.Device1;

            if (Convert.ToInt16(param.ToString().Substring(2)) > 32)
                type = DIO_DevType.Device2;

            DIOUtility.DOSignalOut(type, true, DO_Set.employees[param.ToString()]);
            MessageBox.Show("Hi");
        }

        private bool Can_DO_CheckEvent(object param)
        {
            return true;
        }

        private void DO_UnCheckEvent(object param)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees[param.ToString()]);
        }

        private bool Can_DO_UnCheckEvent(object param)
        {
            return true;
        }
    }
}
