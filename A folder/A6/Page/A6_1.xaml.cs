using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HMI_PermanentForm.A_folder.A6.ViewModel;
using HMI_PermanentForm.Utils;
using AM_Kernel.IO;

namespace HMI_PermanentForm
{
    /// <summary>
    /// A3_1.xaml 的互動邏輯
    /// </summary>
    public partial class A6_1 : Page
    {
        private System.Timers.Timer _EnviromentTimer = new System.Timers.Timer(600);
        public Data_A6 vm { get; set; }

        bool isFlowModulationSafe = true;
        bool isVacuumSafe = true;
        bool isInertGasN2 = false;
        bool isInertGasAr = false;

        bool isInertGasInStart = false;

        public A6_1()
        {
            InitializeComponent();

            DataContext = this;

            vm = new Data_A6()
            {
                A6_03_TargetTorrValue = 300.0,
                A6_03_TorrDefaultValue = 760.0,
                A6_03_TorrCurrentValue = 450,
                A6_05_O2Value = 1200.0,
                FlowLevel = 3,

            };

            _EnviromentTimer.Elapsed += new System.Timers.ElapsedEventHandler(Enviroment_Timer_Elapsed);
            _EnviromentTimer.Start();

        }

        void Enviroment_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            vm.A6_03_TorrCurrentValue = GlobalChannel.envriomentModel.Pressure;
            vm.A6_05_O2Value = GlobalChannel.envriomentModel.Oxygen;

        }

        private void Btn_FlowRateStart_Click(object sender, RoutedEventArgs e)
        {
            if (isFlowModulationSafe)
            {
                FlowRateLevelSetting(vm.FlowLevel);
                DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO16"]);
                A6_1_Utility.Cyclone_ON();

                Btn_FlowRateStart.IsEnabled = false;
                Btn_VacuumStart.IsEnabled = false;
                Btn_VacuumStop.IsEnabled = false;
            }
            else
                return;
        }

        private void Btn_FlowRateStop_Click(object sender, RoutedEventArgs e)
        {
            FlowRateLevelSetting(0);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO16"]);
            A6_1_Utility.Cyclone_OFF();

            Btn_FlowRateStart.IsEnabled = true;
            Btn_VacuumStart.IsEnabled = true;
            Btn_VacuumStop.IsEnabled = true;

            isVacuumSafe = true;
        }

        void FlowRateLevelSetting(int level)
        {
            string tmp = Convert.ToString(level, 2).PadLeft(3, '0');

            bool[] bits = new bool[3] {Convert.ToBoolean(Char.GetNumericValue(tmp[2])),
                Convert.ToBoolean(Char.GetNumericValue(tmp[1])),
                Convert.ToBoolean(Char.GetNumericValue(tmp[0])) };

            DIOUtility.DOSignalOut(DIO_DevType.Device1, bits[0], DO_Set.employees["DO28"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, bits[1], DO_Set.employees["DO29"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, bits[2], DO_Set.employees["DO30"]);
        }

        private void Btn_GasInStart_Click(object sender, RoutedEventArgs e)
        {
            Btn_GasInStart.IsEnabled = false;

            DIOUtility.DOSignalOut(DIO_DevType.Device1, isInertGasN2, DO_Set.employees["DO48"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, isInertGasN2, DO_Set.employees["DO49"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, isInertGasN2, DO_Set.employees["DO50"]);


            DIOUtility.DOSignalOut(DIO_DevType.Device1, isInertGasAr, DO_Set.employees["DO51"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, isInertGasAr, DO_Set.employees["DO52"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, isInertGasAr, DO_Set.employees["DO53"]);

        }

        private void Btn_GasInStop_Click(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO48"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO49"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO50"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO51"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO52"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO53"]);

            Btn_GasInStart.IsEnabled = true;
        }

        private void Btn_VacuumStart_Click(object sender, RoutedEventArgs e)
        {
            if (isVacuumSafe)
            {
                A6_1_Utility.Vacuum_ON();

                Btn_VacuumStart.IsEnabled = false;
                Btn_FlowRateStart.IsEnabled = false;
                Btn_FlowRateStop.IsEnabled = false;
            }
            else
                return;
        }

        private void Btn_VacuumStop_Click(object sender, RoutedEventArgs e)
        {
            A6_1_Utility.Vacuum_OFF();

            Btn_VacuumStart.IsEnabled = true;
            Btn_FlowRateStart.IsEnabled = true;
            Btn_FlowRateStop.IsEnabled = true;

            isFlowModulationSafe = true;
        }


        private void RB_N2_Checked(object sender, RoutedEventArgs e)
        {
            isInertGasN2 = true;
        }

        private void RB_N2_Unchecked(object sender, RoutedEventArgs e)
        {
            isInertGasN2 = false;
        }

        private void RB_Ar_Checked(object sender, RoutedEventArgs e)
        {
            isInertGasAr = true;
        }

        private void RB_Ar_Unchecked(object sender, RoutedEventArgs e)
        {
            isInertGasAr = false;
        }

    }


    class A6_1_Utility
    {
        public static void Vacuum_ON()
        {
            #region Step 0
                DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO07"]);
                DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO22"]);
                DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO23"]);
                DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO24"]);
                DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO25"]);
                DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO26"]);
                DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO27"]);
            #endregion

            #region Step 1
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO20"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO21"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO48"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO51"]);

            #endregion

            #region Step 2
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO01"]);

            #endregion
        }

        public static void Vacuum_OFF()
        {
            #region Step 0
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO07"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO22"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO23"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO24"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO25"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO26"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO27"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO48"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO51"]);

            #endregion

            #region Step 1
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO20"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO21"]);


            #endregion

            #region Step 2
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO01"]);

            #endregion
        }

        public static void Cyclone_ON()
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO01"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO20"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO21"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO22"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO23"]);

            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO07"]);

        }

        public static void Cyclone_OFF()
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO01"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO20"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO21"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO22"]);
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO23"]);

            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO07"]);

        }
    }

}
