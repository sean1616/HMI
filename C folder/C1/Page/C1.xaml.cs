using AM_Kernel.IO;
using HMI_PermanentForm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HMI_PermanentForm
{
    /// <summary>
    /// Page1.xaml 的互動邏輯
    /// </summary>
    public partial class C1 : Page
    {
        Timer UpdateStatus_Timer = new Timer(300);

        Dictionary<string, DIOViewModel> _IOCollection = new Dictionary<string, DIOViewModel>()
        {
            {"DI00", new DIOViewModel{Index = 0, IsOn = false, Description = "非常停止確認信號"}},
            {"DI01", new DIOViewModel{Index = 1, IsOn = false, Description = "腔體正壓壓力-到達"}},
            {"DI02", new DIOViewModel{Index = 2, IsOn = false, Description = "非常停止開關 1(右)"}},
            {"DI03", new DIOViewModel{Index = 3, IsOn = false, Description = "非常停止開關 2(左)"}},
            {"DI04", new DIOViewModel{Index = 4, IsOn = false, Description = "加熱器1 Alarm#1"}},
            {"DI05", new DIOViewModel{Index = 5, IsOn = false, Description = "加熱器1 Alarm#2"}},
            {"DI06", new DIOViewModel{Index = 6, IsOn = false, Description = "加熱器2 Alarm#1"}},
            {"DI07", new DIOViewModel{Index = 7, IsOn = false, Description = "加熱器2 Alarm#2"}}, 
            {"DI08", new DIOViewModel{Index = 8, IsOn = false, Description = "加熱器3 Alarm#1"}},
            {"DI09", new DIOViewModel{Index = 9, IsOn = false, Description = "加熱器3 Alarm#2"}},
            {"DI10", new DIOViewModel{Index = 10, IsOn = false, Description = "氧氣偵測器上限"}},
            {"DI11", new DIOViewModel{Index = 11, IsOn = false, Description = "氧氣偵測器下陷"}},
            {"DI12", new DIOViewModel{Index = 12, IsOn = false, Description = "預留"}},
            {"DI13", new DIOViewModel{Index = 13, IsOn = false, Description = "預留"}},
            {"DI14", new DIOViewModel{Index = 14, IsOn = false, Description = "腔體安全鎖 - 鎖附"}},
            {"DI15", new DIOViewModel{Index = 15, IsOn = false, Description = "循環過濾器飽和"}}, 
            {"DI16", new DIOViewModel{Index = 16, IsOn = false, Description = "循環過濾器故障"}},
            {"DI17", new DIOViewModel{Index = 17, IsOn = false, Description = "冰水冷卻機故障"}},
            {"DI18", new DIOViewModel{Index = 18, IsOn = false, Description = "PUMP過負載Alarm"}},
            {"DI19", new DIOViewModel{Index = 19, IsOn = false, Description = "PUMP角閥Sensor關"}},
            {"DI20", new DIOViewModel{Index = 20, IsOn = false, Description = "PUMP角閥Sensor開"}},
            {"DI21", new DIOViewModel{Index = 21, IsOn = false, Description = "移載平台角閥Sensor關"}},
            {"DI22", new DIOViewModel{Index = 22, IsOn = false, Description = "移載平台角閥Sensor開"}},
            {"DI23", new DIOViewModel{Index = 23, IsOn = false, Description = "Filter抽氣角閥Sensor關"}}, 
            {"DI24", new DIOViewModel{Index = 24, IsOn = false, Description = "Filter抽氣角閥Sensor開"}},
            {"DI25", new DIOViewModel{Index = 25, IsOn = false, Description = "Filter吹氣角閥Sensor關"}},
            {"DI26", new DIOViewModel{Index = 26, IsOn = false, Description = "Filter吹氣角閥Sensor開"}},
            {"DI27", new DIOViewModel{Index = 27, IsOn = false, Description = "預留"}}, 
            {"DI28", new DIOViewModel{Index = 28, IsOn = false, Description = "預留"}},
            {"DI29", new DIOViewModel{Index = 29, IsOn = false, Description = "預留"}},
            {"DI30", new DIOViewModel{Index = 30, IsOn = false, Description = "預留"}},
            {"DI31", new DIOViewModel{Index = 31, IsOn = false, Description = "SAM3D單次燒結完成輸出"}}, 
            {"DI32", new DIOViewModel{Index = 32, IsOn = false, Description = "Alarm Indicator"}}, 
            {"DI33", new DIOViewModel{Index = 33, IsOn = false, Description = "預留"}},
            {"DI34", new DIOViewModel{Index = 34, IsOn = false, Description = "Laser Enable Indicator"}},
            {"DI35", new DIOViewModel{Index = 35, IsOn = false, Description = "Target Laser Status"}}, 
            {"DI36", new DIOViewModel{Index = 36, IsOn = false, Description = "Status 1"}},
            {"DI37", new DIOViewModel{Index = 37, IsOn = false, Description = "Status 2"}},
            {"DI38", new DIOViewModel{Index = 38, IsOn = false, Description = "Status 3"}},
            {"DI39", new DIOViewModel{Index = 39, IsOn = false, Description = "Status 4"}}, 
            {"DI40", new DIOViewModel{Index = 40, IsOn = false, Description = "WARNING"}},
            {"DI41", new DIOViewModel{Index = 41, IsOn = false, Description = "預留"}},
            {"DI42", new DIOViewModel{Index = 42, IsOn = false, Description = "預留"}},
            {"DI43", new DIOViewModel{Index = 43, IsOn = false, Description = "預留"}}, 
            {"DI44", new DIOViewModel{Index = 44, IsOn = false, Description = "預留"}},
            {"DI45", new DIOViewModel{Index = 45, IsOn = false, Description = "預留"}},
            {"DI46", new DIOViewModel{Index = 46, IsOn = false, Description = "CDA 壓力檢知"}},
            {"DI47", new DIOViewModel{Index = 47, IsOn = false, Description = "N2 壓力檢知"}}, 
            {"DI48", new DIOViewModel{Index = 48, IsOn = false, Description = "Ar 壓力檢知"}},
            {"DI49", new DIOViewModel{Index = 49, IsOn = false, Description = "U1&U2 LOCK"}},
            {"DI50", new DIOViewModel{Index = 50, IsOn = false, Description = "蜂鳴器 有效/無效 選擇按鍵"}},
            {"DI51", new DIOViewModel{Index = 51, IsOn = false, Description = "安全門檢按鍵"}}, 
            {"DI52", new DIOViewModel{Index = 52, IsOn = false, Description = "異常清除按鍵"}},
            {"DI53", new DIOViewModel{Index = 53, IsOn = false, Description = "Pitch UP"}},
            {"DI54", new DIOViewModel{Index = 54, IsOn = false, Description = "Pitcp Down"}},
            {"DI55", new DIOViewModel{Index = 55, IsOn = false, Description = "Roll Up"}},
            {"DI56", new DIOViewModel{Index = 56, IsOn = false, Description = "Roll Down"}},
            {"DI57", new DIOViewModel{Index = 57, IsOn = false, Description = "預留"}},
            {"DI58", new DIOViewModel{Index = 58, IsOn = false, Description = "預留"}},
            {"DI59", new DIOViewModel{Index = 59, IsOn = false, Description = "環境安全氧氣檢知"}},
            {"DI60", new DIOViewModel{Index = 60, IsOn = false, Description = "預留"}},
            {"DI61", new DIOViewModel{Index = 61, IsOn = false, Description = "預留"}},
            {"DI62", new DIOViewModel{Index = 62, IsOn = false, Description = "預留"}},
            {"DI63", new DIOViewModel{Index = 63, IsOn = false, Description = "預留"}}
        };

        public Dictionary<string, DIOViewModel> IOCollection
        {
            get { return _IOCollection; }
            set { _IOCollection = value; }
        }

        public C1()
        {
            InitializeComponent();
            DataContext = this;

            UpdateStatus_Timer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateDI_Timer_Tick);
        }

        void UpdateDI_Timer_Tick(object source, System.Timers.ElapsedEventArgs e)
        {
            foreach (var io in IOCollection)
            {
                io.Value.IsOn = DI_Set.employees[io.Key].IsOn;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            UpdateStatus_Timer.Stop();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Utils.GlobalCtrlFlag.isIOTimer_Start)
                UpdateStatus_Timer.Start();
        }
    }
}
