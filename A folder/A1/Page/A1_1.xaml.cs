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
using AM_Kernel.Motor;
using HMI_PermanentForm.A_folder.A1.ViewModel;
using HMI_PermanentForm.Utils;
using AM_Kernel.IO;
using System.Threading;

namespace HMI_PermanentForm
{
    /// <summary>
    /// A1_1.xaml 的互動邏輯
    /// </summary>
    public partial class A1_1 : Page
    {
        public Data_A1_1 AxisCommand { get; set; }
        private System.Timers.Timer Controls_StatusTimer;

        public A1_1()
        {
            InitializeComponent();

            AxisCommand = new Data_A1_1();
            Controls_StatusTimer = new System.Timers.Timer(300) { };
            DataContext = AxisCommand;
            DecimalUD_Increment.DataContext = GlobalChannel.TranlateVM;
    }
        
        private void DecimalUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Return)
            {
                e.Handled = true;
            }
        }

        private void AxisX_Clear_Click(object sender, RoutedEventArgs e)
        {
            MotorFunc4PCI1240.getInstance().AxisResetError(Recoater.getInstance());
            MotorFunc4PCI1240.getInstance().MoveSTOP(Recoater.getInstance().AxisHandle);
        }

        private void AxisZ_Clear_Click(object sender, RoutedEventArgs e)
        {
            MotorFunc4PCI1240.getInstance().AxisResetError(BuildPlatform.getInstance());
            MotorFunc4PCI1240.getInstance().MoveSTOP(BuildPlatform.getInstance().AxisHandle);
        }

        private void AxisU1_Clear_Click(object sender, RoutedEventArgs e)
        {
            MotorFunc4PCI1240.getInstance().AxisResetError(LeftFeeder.getInstance());
            MotorFunc4PCI1240.getInstance().MoveSTOP(LeftFeeder.getInstance().AxisHandle);
        }

        private void AxisU2_Clear_Click(object sender, RoutedEventArgs e)
        {
            MotorFunc4PCI1240.getInstance().AxisResetError(RightFeeder.getInstance());
            MotorFunc4PCI1240.getInstance().MoveSTOP(RightFeeder.getInstance().AxisHandle);
        }

        private void AxisX_Check_Click(object sender, RoutedEventArgs e)
        {
            if (!Utils.GlobalCtrlFlag.isBuildPlatformBeyondLevel)
            {
                if (Recoater.getInstance().Status.isDriving)
                    return;
                else
                {
                    MotorFunc4PCI1240.getInstance().AxisSetSpeed(Recoater.getInstance().AxisHandle,
                        (Recoater.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                    //MotorFunc4PCI1240.getInstance().MoveABS(Recoater.getInstance().AxisHandle,
                    //    Recoater.UnitTransform.MM2Pulse((double)DecimalUD_AxisX_CommandPos.Value));
                    MotorFunc4PCI1240.getInstance().MoveABS(Recoater.getInstance().AxisHandle,
                        Recoater.UnitTransform.MM2Pulse((double)AxisCommand.X_Abs_Pos));
                }
            }
            else
            {
                MessageBox.Show("Z軸高於平面");
                return;
            }
        }

        private void AxisZ_Check_Click(object sender, RoutedEventArgs e)
        {
            if (BuildPlatform.getInstance().Status.isDriving)
                return;
            else
            {
                if(Utils.GlobalCtrlFlag.isRecoaterInScope)
                {
                    if (DecimalUD_AxisZ_CommandPos.Value > 0)
                    {
                        MessageBox.Show("X軸在警戒範圍內");
                        return;
                    }

                }
                MotorFunc4PCI1240.getInstance().AxisSetSpeed(BuildPlatform.getInstance().AxisHandle,
                    (BuildPlatform.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                MotorFunc4PCI1240.getInstance().MoveABS(BuildPlatform.getInstance().AxisHandle,
                    BuildPlatform.UnitTransform.MM2Pulse((double)DecimalUD_AxisZ_CommandPos.Value));
                
            }
        }

        private void AxisU1_Check_Click(object sender, RoutedEventArgs e)
        {
            if (LeftFeeder.getInstance().Status.isDriving)
                return;
            else
            {
                MotorFunc4PCI1240.getInstance().AxisSetSpeed(LeftFeeder.getInstance().AxisHandle,
                    (LeftFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                MotorFunc4PCI1240.getInstance().MoveABS(LeftFeeder.getInstance().AxisHandle,
                    LeftFeeder.UnitTransform.MM2Pulse((double)DecimalUD_AxisU1_CommandPos.Value));
            }
        }

        private void AxisU2_Check_Click(object sender, RoutedEventArgs e)
        {
            if (RightFeeder.getInstance().Status.isDriving)
                return;
            else
            {
                MotorFunc4PCI1240.getInstance().AxisSetSpeed(RightFeeder.getInstance().AxisHandle,
                    (RightFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                MotorFunc4PCI1240.getInstance().MoveABS(RightFeeder.getInstance().AxisHandle,
                    RightFeeder.UnitTransform.MM2Pulse((double)DecimalUD_AxisU2_CommandPos.Value));
            }
        }

        private void AxisX_Home_Click(object sender, RoutedEventArgs e)
        {
            if (!Utils.GlobalCtrlFlag.isBuildPlatformBeyondLevel)
                MotorUtils.GoHomeAsync(Recoater.getInstance());
            else
                MessageBox.Show("Z軸高於平面");
        }

        private void AxisZ_Home_Click(object sender, RoutedEventArgs e)
        {
            MotorUtils.GoHomeAsync(BuildPlatform.getInstance());
        }

        private void AxisU1_Home_Click(object sender, RoutedEventArgs e)
        {
            MotorUtils.GoHomeAsync(LeftFeeder.getInstance());
        }

        private void Axisu2_Home_Click(object sender, RoutedEventArgs e)
        {
            MotorUtils.GoHomeAsync(RightFeeder.getInstance());
        }

        private void A1_1_3_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton RB = sender as RadioButton;

            DIOUtility.DOSignalOut(DIO_DevType.Device2, RB.IsChecked.Value, DO_Set.employees["DO34"]);
            Thread.Sleep(20);
            DIOUtility.DOSignalOut(DIO_DevType.Device2, RB.IsChecked.Value, DO_Set.employees["DO35"]);
        }

        private void A1_1_3_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioButton RB = sender as RadioButton;

            DIOUtility.DOSignalOut(DIO_DevType.Device2, RB.IsChecked.Value, DO_Set.employees["DO35"]);
            Thread.Sleep(20);
            DIOUtility.DOSignalOut(DIO_DevType.Device2, RB.IsChecked.Value, DO_Set.employees["DO34"]);
        }
    }
}
