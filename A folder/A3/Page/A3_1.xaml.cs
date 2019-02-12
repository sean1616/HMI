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
using AM_Kernel.Periphery;
using AM_Kernel.IO;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm
{
    /// <summary>
    /// A3_1.xaml 的互動邏輯
    /// </summary>
    public partial class A3_1 : Page
    {
        private System.Timers.Timer _HeaterTimer = new System.Timers.Timer(600);
        int Count = 0;
        public Data_A3_1 vm { get; set; }

        public A3_1()
        {
            InitializeComponent();

            DataContext = this;

            vm = new Data_A3_1()
            {
                A3_03_TemperatureValue = 25.0,
                A3_03_TargetTemperatureValue = 150.0,
                A3_10_TemperatureValue = 25.0,
                A3_10_TargetTemperatureValue = 150.0,
                A3_17_TemperatureValue = 25.0,
                A3_17_TargetTemperatureValue = 150.0,
            };

            _HeaterTimer.Elapsed += new System.Timers.ElapsedEventHandler(Heater_Timer_Elapsed);

            if(DO_Set.employees["DO06"].IsOn)
            {               
                _HeaterTimer.Start();
            }

        }

        void Heater_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string tmp_PV = string.Empty, tmp_SV = string.Empty;

            if(GlobalChannel.Heater_Provider.GetInstance().isOpen)
            {
                (GlobalChannel.Heater_Provider.GetInstance() as HeaterMonitor).GetTempCmd(Count);

                (GlobalChannel.Heater_Provider.GetInstance() as HeaterMonitor).GetHeaterVal(Count);

                
                if ((GlobalChannel.Heater_Provider.GetInstance() as HeaterMonitor).DTA_Heaters[Count].SV != "")
                {
                    tmp_SV = (GlobalChannel.Heater_Provider.GetInstance() as HeaterMonitor).DTA_Heaters[Count].SV;
                }

                if ((GlobalChannel.Heater_Provider.GetInstance() as HeaterMonitor).DTA_Heaters[Count].PV != "")
                {
                    tmp_PV = (GlobalChannel.Heater_Provider.GetInstance() as HeaterMonitor).DTA_Heaters[Count].PV;
                }

                switch(Count)
                {
                    case 0:
                        vm.A3_03_TemperatureValue = Convert.ToDouble(tmp_PV);
                        vm.A3_03_TargetTemperatureValue = Convert.ToDouble(tmp_SV);
                        break;
                    case 1:
                        vm.A3_10_TemperatureValue = Convert.ToDouble(tmp_PV);
                        vm.A3_10_TargetTemperatureValue = Convert.ToDouble(tmp_SV);
                        break;
                    case 2:
                        vm.A3_17_TemperatureValue = Convert.ToDouble(tmp_PV);
                        vm.A3_17_TargetTemperatureValue = Convert.ToDouble(tmp_SV);
                        break;

                }

                if ((GlobalChannel.Heater_Provider.GetInstance() as HeaterMonitor).isReading)
                    Count = (Count + 1) % 3;

                
            }


        }

        private void Btn_Heater01_Check_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalChannel.Heater_Provider.GetInstance().isOpen)
            {
                double setVal = vm.A3_03_TargetTemperatureValue;
                (GlobalChannel.Heater_Provider.GetInstance() as HeaterMonitor).SetTempCmd(0, setVal);

                Btn_Heater01_Check.IsEnabled = false;
                DUP_Heater01_SetVal.IsEnabled = false;
            }

        }

        private void Btn_Heater01_Clear_Click(object sender, RoutedEventArgs e)
        {
            DUP_Heater01_SetVal.IsEnabled = true;
            Btn_Heater01_Check.IsEnabled = true;
        }

        private void Btn_Heater02_Check_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalChannel.Heater_Provider.GetInstance().isOpen)
            {
                double setVal = vm.A3_10_TargetTemperatureValue;
                (GlobalChannel.Heater_Provider.GetInstance() as HeaterMonitor).SetTempCmd(1, setVal);

                Btn_Heater02_Check.IsEnabled = false;
                DUP_Heater02_SetVal.IsEnabled = false;
            }
        }

        private void Btn_Heater02_Clear_Click(object sender, RoutedEventArgs e)
        {
            DUP_Heater02_SetVal.IsEnabled = true;
            Btn_Heater02_Check.IsEnabled = true;
        }

        private void Btn_Heater03_Check_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalChannel.Heater_Provider.GetInstance().isOpen)
            {
                double setVal = vm.A3_17_TargetTemperatureValue;
                (GlobalChannel.Heater_Provider.GetInstance() as HeaterMonitor).SetTempCmd(2, setVal);

                Btn_Heater03_Check.IsEnabled = false;
                DUP_Heater03_SetVal.IsEnabled = false;
            }
        }

        private void Btn_Heater03_Clear_Click(object sender, RoutedEventArgs e)
        {
            DUP_Heater02_SetVal.IsEnabled = true;
            Btn_Heater02_Check.IsEnabled = true;
        }
    }
}
