using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HMI_PermanentForm.Utils;
using System.Timers;
using AM_Kernel.IO;
using AM_Kernel.Utils;
using System.Windows.Controls.Primitives;
using HMI_PermanentForm.C_folder.Dict;

namespace HMI_PermanentForm
{
    /// <summary>
    /// Page1.xaml 的互動邏輯
    /// </summary>
    public partial class C2 : Page
    {
        Timer UpdateStatus_Timer = new Timer(500);
        DO_CommandVM commandVM = new DO_CommandVM();

        Dictionary<string, DIOViewModel> _IOCollection = new Dictionary<string, DIOViewModel>()
        {
            {"DO00", new DIOViewModel{Index = 0, IsOn = false, Description = "預留"}},
            {"DO01", new DIOViewModel{Index = 1, IsOn = false, Description = "啟動-真空幫浦電源"}},
            {"DO02", new DIOViewModel{Index = 2, IsOn = false, Description = "啟動-伺服馬達電源"}},
            {"DO03", new DIOViewModel{Index = 3, IsOn = false, Description = "預留"}},
            {"DO04", new DIOViewModel{Index = 4, IsOn = false, Description = "預留"}},
            {"DO05", new DIOViewModel{Index = 5, IsOn = false, Description = "預留"}},
            {"DO06", new DIOViewModel{Index = 6, IsOn = false, Description = "啟動-電熱電源"}},
            {"DO07", new DIOViewModel{Index = 7, IsOn = false, Description = "啟動-氣旋單元"}}, 
            {"DO08", new DIOViewModel{Index = 8, IsOn = false, Description = "啟動-過篩單元"}},
            {"DO09", new DIOViewModel{Index = 9, IsOn = false, Description = "解除-腔體安全鎖"}},
            {"DO10", new DIOViewModel{Index = 10, IsOn = false, Description = "預留"}},
            {"DO11", new DIOViewModel{Index = 11, IsOn = false, Description = "預留"}},
            {"DO12", new DIOViewModel{Index = 12, IsOn = false, Description = "五層指示燈-蜂鳴器"}},
            {"DO13", new DIOViewModel{Index = 13, IsOn = false, Description = "五層指示燈-紅色"}},
            {"DO14", new DIOViewModel{Index = 14, IsOn = false, Description = "五層指示燈-黃色"}},
            {"DO15", new DIOViewModel{Index = 15, IsOn = false, Description = "五層指示燈-綠色"}}, 
            {"DO16", new DIOViewModel{Index = 16, IsOn = false, Description = "變頻器 M0"}},
            {"DO17", new DIOViewModel{Index = 17, IsOn = false, Description = "變頻器 M2"}},
            {"DO18", new DIOViewModel{Index = 18, IsOn = false, Description = "預留"}},
            {"DO19", new DIOViewModel{Index = 19, IsOn = false, Description = "預留"}},
            {"DO20", new DIOViewModel{Index = 20, IsOn = false, Description = "Pump電磁閥"}},
            {"DO21", new DIOViewModel{Index = 21, IsOn = false, Description = "移載平台電磁閥"}},
            {"DO22", new DIOViewModel{Index = 22, IsOn = false, Description = "Filter抽氣電磁閥"}},
            {"DO23", new DIOViewModel{Index = 23, IsOn = false, Description = "Filter抽氣電磁閥"}}, 
            {"DO24", new DIOViewModel{Index = 24, IsOn = false, Description = "腔體氧氣偵測電磁閥"}},
            {"DO25", new DIOViewModel{Index = 25, IsOn = false, Description = "濾器氧氣偵測電磁閥"}},
            {"DO26", new DIOViewModel{Index = 26, IsOn = false, Description = "移動台氧氣偵測電磁閥"}},
            {"DO27", new DIOViewModel{Index = 27, IsOn = false, Description = "大氣測氧氣偵測電磁閥"}}, 
            {"DO28", new DIOViewModel{Index = 28, IsOn = false, Description = "變頻器 M3"}},
            {"DO29", new DIOViewModel{Index = 29, IsOn = false, Description = "變頻器 M4"}},
            {"DO30", new DIOViewModel{Index = 30, IsOn = false, Description = "變頻器 M5"}},
            {"DO31", new DIOViewModel{Index = 31, IsOn = false, Description = "馬達單次下降完成輸入"}}, 
            {"DO32", new DIOViewModel{Index = 32, IsOn = false, Description = "預留"}}, 
            {"DO33", new DIOViewModel{Index = 33, IsOn = false, Description = "Enable"}},
            {"DO34", new DIOViewModel{Index = 34, IsOn = false, Description = "Use External Interface"}},
            {"DO35", new DIOViewModel{Index = 35, IsOn = false, Description = "Target Laser Control"}}, 
            {"DO36", new DIOViewModel{Index = 36, IsOn = false, Description = "Alarm Reset 1(User Level)"}},
            {"DO37", new DIOViewModel{Index = 37, IsOn = false, Description = "Alarm Reset 2(User Level)"}},
            {"DO38", new DIOViewModel{Index = 38, IsOn = false, Description = "OPEN/CLOSE LOOP"}},
            {"DO39", new DIOViewModel{Index = 39, IsOn = false, Description = "PPC/APC MODE"}}, 
            {"DO40", new DIOViewModel{Index = 40, IsOn = false, Description = "自動供粉 STEP"}},
            {"DO41", new DIOViewModel{Index = 41, IsOn = false, Description = "自動供粉 DIR"}},
            {"DO42", new DIOViewModel{Index = 42, IsOn = false, Description = "自動供粉預留"}},
            {"DO43", new DIOViewModel{Index = 43, IsOn = false, Description = "自動供粉預留"}}, 
            {"DO44", new DIOViewModel{Index = 44, IsOn = false, Description = "自動供粉預留"}},
            {"DO45", new DIOViewModel{Index = 45, IsOn = false, Description = "安全門檢知按鍵提示燈"}},
            {"DO46", new DIOViewModel{Index = 46, IsOn = false, Description = "蜂鳴器 有效/無效按鍵指示燈"}},
            {"DO47", new DIOViewModel{Index = 47, IsOn = false, Description = "異常清除按鍵指示燈"}}, 
            {"DO48", new DIOViewModel{Index = 48, IsOn = false, Description = "選擇製程氣體 N2"}},
            {"DO49", new DIOViewModel{Index = 49, IsOn = false, Description = "製程中氣體閥門 N2"}},
            {"DO50", new DIOViewModel{Index = 50, IsOn = false, Description = "腔體回壓充氣閥門 N2"}},
            {"DO51", new DIOViewModel{Index = 51, IsOn = false, Description = "選擇製程氣體 Ar"}}, 
            {"DO52", new DIOViewModel{Index = 52, IsOn = false, Description = "製程中氣體閥門 Ar"}},
            {"DO53", new DIOViewModel{Index = 53, IsOn = false, Description = "腔體回壓充氣閥門 Ar"}},
            {"DO54", new DIOViewModel{Index = 54, IsOn = false, Description = "預留"}},
            {"DO55", new DIOViewModel{Index = 55, IsOn = false, Description = "預留"}},
            {"DO56", new DIOViewModel{Index = 56, IsOn = false, Description = "Laser ON"}},
            {"DO57", new DIOViewModel{Index = 57, IsOn = false, Description = "氧氣濃度到達製程標準"}},
            {"DO58", new DIOViewModel{Index = 58, IsOn = false, Description = "氣旋流速正常"}},
            {"DO59", new DIOViewModel{Index = 59, IsOn = false, Description = "加熱溫度到達"}},
            {"DO60", new DIOViewModel{Index = 60, IsOn = false, Description = "預留"}},
            {"DO61", new DIOViewModel{Index = 61, IsOn = false, Description = "預留"}},
            {"DO62", new DIOViewModel{Index = 62, IsOn = false, Description = "預留"}},
            {"DO63", new DIOViewModel{Index = 63, IsOn = false, Description = "預留"}}
        };

        public Dictionary<string, DIOViewModel> IOCollection
        {
            get { return _IOCollection; }
            set { _IOCollection = value; }
        }

        public C2()
        {
            InitializeComponent();
            DataContext = this;

            UpdateStatus_Timer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateDO_Timer_Tick);
        }

        void UpdateDO_Timer_Tick(object source, System.Timers.ElapsedEventArgs e)
        {
            foreach(var io in IOCollection)
            {
                io.Value.IsOn = DO_Set.employees[io.Key].IsOn;
            }
        }

        private void DO00_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO00"]);
        }

        private void DO00_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO00"]);
        }

        private void DO01_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO01"]);
        }

        private void DO01_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO01"]);
        }

        private void DO02_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO02"]);
        }

        private void DO02_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO02"]);
        }

        private void DO03_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO03"]);
        }

        private void DO03_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO03"]);
        }

        private void DO04_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO04"]);
        }

        private void DO04_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO04"]);
        }

        private void DO05_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO05"]);
        }

        private void DO05_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO05"]);
        }

        private void DO06_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO06"]);
        }

        private void DO06_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO06"]);
        }

        private void DO07_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO07"]);
        }

        private void DO07_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO07"]);
        }

        private void DO08_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO08"]);
        }

        private void DO08_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO08"]);
        }

        private void DO09_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO09"]);
        }

        private void DO09_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO09"]);
        }

        private void DO10_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO10"]);
        }

        private void DO10_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO10"]);
        }

        private void DO11_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO11"]);
        }

        private void DO11_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO11"]);
        }

        private void DO12_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO12"]);
        }

        private void DO12_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO12"]);
        }

        private void DO13_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO13"]);
        }

        private void DO13_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO13"]);
        }

        private void DO14_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO14"]);
        }

        private void DO14_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO14"]);
        }

        private void DO15_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO15"]);
        }

        private void DO15_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO15"]);
        }

        private void DO16_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO16"]);
        }

        private void DO16_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO16"]);
        }

        private void DO17_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO17"]);
        }

        private void DO17_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO17"]);
        }

        private void DO18_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO18"]);
        }

        private void DO18_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO18"]);
        }

        private void DO19_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO19"]);
        }

        private void DO19_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO19"]);
        }

        private void DO20_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO20"]);
        }

        private void DO20_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO20"]);
        }

        private void DO21_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO21"]);
        }

        private void DO21_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO21"]);
        }

        private void DO22_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO22"]);
        }

        private void DO22_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO22"]);
        }

        private void DO23_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO23"]);
        }

        private void DO23_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO23"]);
        }

        private void DO24_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO24"]);
        }

        private void DO24_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO24"]);
        }

        private void DO25_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO25"]);
        }

        private void DO25_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO25"]);
        }

        private void DO26_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO26"]);
        }

        private void DO26_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO26"]);
        }

        private void DO27_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO27"]);
        }

        private void DO27_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO27"]);
        }

        private void DO28_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO28"]);
        }

        private void DO28_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO28"]);
        }

        private void DO29_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO29"]);
        }

        private void DO29_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO29"]);
        }

        private void DO30_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO30"]);
        }

        private void DO30_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO30"]);
        }

        private void DO31_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO31"]);
        }

        private void DO31_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device1, false, DO_Set.employees["DO31"]);
        }

        private void DO32_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO32"]);
        }

        private void DO32_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO32"]);
        }

        private void DO33_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO33"]);
        }

        private void DO33_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO33"]);
        }

        private void DO34_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO34"]);
        }

        private void DO34_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO34"]);
        }

        private void DO35_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO35"]);
        }

        private void DO35_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO35"]);
        }

        private void DO36_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO36"]);
        }

        private void DO36_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO36"]);
        }

        private void DO37_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO37"]);
        }

        private void DO37_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO37"]);
        }

        private void DO38_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO38"]);
        }

        private void DO38_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO38"]);
        }

        private void DO39_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO39"]);
        }

        private void DO39_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO39"]);
        }

        private void DO40_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO40"]);
        }

        private void DO40_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO40"]);
        }

        private void DO41_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO41"]);
        }

        private void DO41_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO41"]);
        }

        private void DO42_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO42"]);
        }

        private void DO42_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO42"]);
        }

        private void DO43_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO43"]);
        }

        private void DO43_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO43"]);
        }

        private void DO44_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO44"]);
        }

        private void DO44_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO44"]);
        }

        private void DO45_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO45"]);
        }

        private void DO45_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO45"]);
        }

        private void DO46_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO46"]);
        }

        private void DO46_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO46"]);
        }

        private void DO47_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO47"]);
        }

        private void DO47_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO47"]);
        }

        private void DO48_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO48"]);
        }

        private void DO48_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO48"]);
        }

        private void DO49_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO49"]);
        }

        private void DO49_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO49"]);
        }

        private void DO50_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO50"]);
        }

        private void DO50_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO50"]);
        }

        private void DO51_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO51"]);
        }

        private void DO51_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO51"]);
        }

        private void DO52_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO52"]);
        }

        private void DO52_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO52"]);
        }

        private void DO53_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO53"]);
        }

        private void DO53_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO53"]);
        }

        private void DO54_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO54"]);
        }

        private void DO54_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO54"]);
        }

        private void DO55_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO55"]);
        }

        private void DO55_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO55"]);
        }

        private void DO56_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO56"]);
        }

        private void DO56_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO56"]);
        }

        private void DO57_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO57"]);
        }

        private void DO57_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO57"]);
        }

        private void DO58_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO58"]);
        }

        private void DO58_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO58"]);
        }

        private void DO59_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO59"]);
        }

        private void DO59_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO59"]);
        }

        private void DO60_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO60"]);
        }

        private void DO60_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO60"]);
        }

        private void DO61_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO61"]);
        }

        private void DO61_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO61"]);
        }

        private void DO62_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO62"]);
        }

        private void DO62_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO62"]);
        }

        private void DO63_Checked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO63"]);
        }

        private void DO63_Unchecked(object sender, RoutedEventArgs e)
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO63"]);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Utils.GlobalCtrlFlag.isIOTimer_Start)
                UpdateStatus_Timer.Start();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            UpdateStatus_Timer.Stop();
        }
    }
}
