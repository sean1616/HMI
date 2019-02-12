using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM_Kernel.IO;
using System.Threading;

namespace HMI_PermanentForm.TT_AM_Kernel.Utils
{
    enum TaskLevel
    { }

    public class ReloaderHandler
    {
        private ReloaderHandler instance = new ReloaderHandler();

        private ReloaderHandler()
        {

        }

        public ReloaderHandler GetInstance()
        {
            return instance;
        }

        private void GoHome()
        {
            if(DI_Set.employees["DI29"].IsOn)
            {
                DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO38"]);
                Thread.Sleep(50);
                DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO38"]);
            }
        }

        public Task GoHomeAsync()
        {
            return Task.Run( () => GoHome() );
        }

        private void Move(int level)
        {
            if (DI_Set.employees["DI29"].IsOn)
            {
                TaskClassify(level);

                DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO40"]);
                Thread.Sleep(50);
                DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO40"]);
            }
        }

        public Task MoveAsync(int level)
        {
            return Task.Run(() => Move(level));
        }

        private void MovingStop()
        {
            TaskClassify(0);

            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO43"]);
            Thread.Sleep(50);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO43"]);
        }

        public Task MovingStopAsync()
        {
            return Task.Run(() => MovingStop());
        }

        private void Reset()
        {
            TaskClassify(0);

            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO44"]);
            Thread.Sleep(50);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO44"]);
        }

        public Task ResetAsync()
        {
            return Task.Run(() => Reset());
        }

        private void TaskClassify(int level)
        {
            string tmp = Convert.ToString(level, 2).PadLeft(2, '0');

            bool[] bits = new bool[2] {Convert.ToBoolean(Char.GetNumericValue(tmp[1])),
                Convert.ToBoolean(Char.GetNumericValue(tmp[0])) };

            DIOUtility.DOSignalOut(DIO_DevType.Device1, bits[0], DO_Set.employees["DO41"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, bits[1], DO_Set.employees["DO42"]);
        }
    }
}
