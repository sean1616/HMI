using System;
using System.Windows;
using System.Timers;
using HMI_PermanentForm.Utils;
using AM_Kernel.IO;
using AM_Kernel.Motor;
using AM_Kernel.Utils;
using HMI_PermanentForm.G_folder.G10.ViewModel;
using AM_Kernel.Periphery;
using AM_Kernel.Interpretation;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;

namespace HMI_PermanentForm
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public NotifyModels models { get; set; }
        public Axis_ViewModel axisStatusVM { get; set; }
        public ErrorMsgVM errorText { get; set; }
        public StatusViewModel statusVM { get; set; }
         
        private System.Timers.Timer timer;

        //double remaintime = 1000;
        private Timer ReadTimer = new Timer(300);

        private System.Timers.Timer AxisStatusTimer = new Timer(500);
        private Timer EnviroDataTimer = new Timer(500);
        float i;

        #region Page instance
        A Page_A = new A();
        B Page_B = new B();
        C Page_C = new C();
        D Page_D = new D();
        E Page_E = new E();
        #endregion
        
        public ProgressBarStraightData Data_ProgressBarStraight { get; set; }

        public ProgressBarStraightData Data_ProgressBarStraight2 { get; set; }

        public ProgressBarStraightData Data_ProgressBarStraight3 { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            // Defualt Page          
            Change_Page.NavigationService.Navigate(Page_E);

            models = new NotifyModels() {CompleteValue = 100, TotalLayer = 100, CurrentLayer = 0, PercentValue = 360};
            axisStatusVM = new Axis_ViewModel() {EncoderX = "0.0", EncoderZ = "0.0", EncoderU1 = "0.0", EncoderU2 = "0.0" };
            errorText = GlobalChannel.errorMsgVM;
            statusVM = new StatusViewModel() { IsSystemStatusCheck = false, IsProcessStatusCheck = false };

            #region Timer setting
            timer = new System.Timers.Timer(200);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;

            ReadTimer.Elapsed += new System.Timers.ElapsedEventHandler(ReadTimerEvent);
            ReadTimer.Start();

            AxisStatusTimer.Elapsed += new System.Timers.ElapsedEventHandler(GetAxisStatus_Tick);
            if(Utils.GlobalCtrlFlag.isMotorTimer_start)
                AxisStatusTimer.Start();

            EnviroDataTimer.Elapsed += new ElapsedEventHandler(GetEnviromentData_Tick);
            EnviroDataTimer.Start();
            #endregion

            #region straightProgressBar
            Data_ProgressBarStraight = new ProgressBarStraightData() { Title = "腔體溫度", NowValue = 0, Unit = "°C", ChangeValue1 = 0, ChangeValue2 = 0, ChangeValue3 = 0, ThresholdValue1 = 40, ThresholdValue2 = 80, ThresholdValue3 = 120, MaxValue = 120 };
            Data_ProgressBarStraight2 = new ProgressBarStraightData() { Title = "腔體濕度", NowValue = 0, Unit = "%", ChangeValue1 = 0, ChangeValue2 = 0, ChangeValue3 = 0, ThresholdValue1 = 30, ThresholdValue2 = 60, ThresholdValue3 = 100, MaxValue = 100 };
            Data_ProgressBarStraight3 = new ProgressBarStraightData() { Title = "雷射狀態", NowValue = 0, Unit = "W", ChangeValue1 = 0, ChangeValue2 = 0, ChangeValue3 = 0, ThresholdValue1 = 200, ThresholdValue2 = 400, ThresholdValue3 = 500, MaxValue = 500 };
            Data_ProgressBarStraight3.NowValue = 500;
            Data_ProgressBarStraight3.ChangeValue1 = i;                                       // 多餘的程式碼 -- 寫法待討論
            Data_ProgressBarStraight3.ChangeValue2 = Data_ProgressBarStraight3.ChangeValue1;   // 多餘的程式碼 -- 寫法待討論
            Data_ProgressBarStraight3.ChangeValue3 = Data_ProgressBarStraight3.ChangeValue2;   // 多餘的程式碼 -- 寫法待討論 

            #endregion
        }
           
        #region Page Switch button
        private void Process_Page_btn_Click(object sender, RoutedEventArgs e)
        {
            //A Page_A = new A();
            Change_Page.NavigationService.Navigate(Page_A);
            //EnviroDataTimer.Close();
            GC.Collect();
        }
        private void Loading_Page_btn_Click(object sender, RoutedEventArgs e)
        {
            //B Page_B = new B();
            Change_Page.NavigationService.Navigate(Page_B);
        }
        private void System_Page_btn_Click(object sender, RoutedEventArgs e)
        {
            //C Page_C = new C();
            Change_Page.NavigationService.Navigate(Page_C);
            GC.Collect();
        }
        private void History_Page_btn_Click(object sender, RoutedEventArgs e)
        {
            Change_Page.NavigationService.Navigate(Page_D);
            GC.Collect();
        }
        private void Home_Page_btn_Click(object sender, RoutedEventArgs e)
        {
            Change_Page.NavigationService.Navigate(Page_E);
            GC.Collect();
        }
        private void Exit_btn_Click(object sender, RoutedEventArgs e)
        {
            string str = MotorFunc4PCI1240.getInstance().CloseDevice();
            if (str != "")
            {
                LogHandler.LogDebug(LogHandler.Formatting(str, "軸卡尚未連接"));
            }

            Application.Current.Shutdown();
        }
        #endregion

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (GlobalCtrlFlag.isAutoProcessing)
                GlobalChannel.SlicingVM.LastLayerIndex = GlobalChannel.AM_Processor.CurrentScanLayerIndex;

            if (SlicingObj.getLayers() != null)
            {
                models.TotalLayer = SlicingObj.getLayers().getLayers().Count;
                models.CurrentLayer = GlobalChannel.SlicingVM.LastLayerIndex + 1 ;
                models.PercentValue = (Convert.ToDouble(models.CurrentLayer) / Convert.ToDouble(models.TotalLayer)) * 360;
                models.CompleteValue = Convert.ToInt16(models.PercentValue / 360 * 100);
            }


            if (models.TotalLayer == models.CurrentLayer)
            {
                GlobalCtrlFlag.isAutoProcessing = false;        
                GlobalChannel.AM_Processor.ClearTrigger();
                models.FinishedTimeValue = DateTime.Now.Date.ToString("hh:mm:ss");
                models.FinishedDateValue = DateTime.Now.Date.ToString("yy/mm/dd");
            }

        }

        public void ReadTimerEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            GetData();
        }

        void GetAxisStatus_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
           #region 編碼器
            MotorFunc4PCI1240.getInstance().EncoderStatus(Recoater.getInstance().AxisHandle,
                ref Recoater.getInstance().Status.CommandEncoder,
                ref Recoater.getInstance().Status.ActualEncoder);
            MotorFunc4PCI1240.getInstance().EncoderStatus(BuildPlatform.getInstance().AxisHandle,
                ref BuildPlatform.getInstance().Status.CommandEncoder,
                ref BuildPlatform.getInstance().Status.ActualEncoder);
            MotorFunc4PCI1240.getInstance().EncoderStatus(LeftFeeder.getInstance().AxisHandle,
                ref LeftFeeder.getInstance().Status.CommandEncoder,
                ref LeftFeeder.getInstance().Status.ActualEncoder);
            MotorFunc4PCI1240.getInstance().EncoderStatus(RightFeeder.getInstance().AxisHandle,
                ref RightFeeder.getInstance().Status.CommandEncoder,
                ref RightFeeder.getInstance().Status.ActualEncoder);

           axisStatusVM.EncoderX =
                Recoater.UnitTransform.Pulse2MM(Recoater.getInstance().Status.CommandEncoder).ToString("0.00");
           axisStatusVM.EncoderZ =
               BuildPlatform.UnitTransform.Pulse2MM(BuildPlatform.getInstance().Status.CommandEncoder).ToString("0.000");
           axisStatusVM.EncoderU1 =
                           LeftFeeder.UnitTransform.Pulse2MM(LeftFeeder.getInstance().Status.CommandEncoder).ToString("0.00");
           axisStatusVM.EncoderU2 =
                           RightFeeder.UnitTransform.Pulse2MM(RightFeeder.getInstance().Status.CommandEncoder).ToString("0.00");

            if (Convert.ToDouble(axisStatusVM.EncoderZ) > GlobalChannel.BuildPlatform_UpperLimit)
                GlobalCtrlFlag.isBuildPlatformBeyondLevel = true;
            else
                GlobalCtrlFlag.isBuildPlatformBeyondLevel = false;

            if (GlobalChannel.RecoaterCordon_Left < Convert.ToDouble(axisStatusVM.EncoderX) && 
                Convert.ToDouble(axisStatusVM.EncoderX) < GlobalChannel.RecoaterCordon_Right)
                GlobalCtrlFlag.isRecoaterInScope = true;
            else
                GlobalCtrlFlag.isRecoaterInScope = false;

            #endregion

           #region Alarm, INP
            bool servoRDY_X = false, servoRDY_Z = false;
           MotorFunc4PCI1240.getInstance().ALMStatus(Recoater.getInstance().AxisHandle, ref Recoater.getInstance().Status.isAlarm);
           MotorFunc4PCI1240.getInstance().ServoRDY(Recoater.getInstance().AxisHandle, ref servoRDY_X);
           MotorFunc4PCI1240.getInstance().ALMStatus(BuildPlatform.getInstance().AxisHandle, ref BuildPlatform.getInstance().Status.isAlarm);
           MotorFunc4PCI1240.getInstance().ServoRDY(BuildPlatform.getInstance().AxisHandle, ref servoRDY_Z);

           axisStatusVM.AlarmX = Recoater.getInstance().Status.isAlarm ? "Alarm" : (servoRDY_X ? "RDY" : "");
           axisStatusVM.AlarmZ = BuildPlatform.getInstance().Status.isAlarm ? "Alarm" : (servoRDY_Z ? "RDY" : "");

           MotorFunc4PCI1240.getInstance().INPStatus(Recoater.getInstance().AxisHandle, ref Recoater.getInstance().Status.isINP);
           MotorFunc4PCI1240.getInstance().INPStatus(BuildPlatform.getInstance().AxisHandle, ref BuildPlatform.getInstance().Status.isINP);

           axisStatusVM.INPX = Recoater.getInstance().Status.isINP ? "INP" : "";
           axisStatusVM.INPZ = BuildPlatform.getInstance().Status.isINP ? "INP" : "";
           #endregion

           #region U1, U2狀態
           string stateU1 = "", stateU2 = "";
           MotorFunc4PCI1240.getInstance().AxisState(LeftFeeder.getInstance().AxisHandle, ref stateU1);
           MotorFunc4PCI1240.getInstance().AxisState(RightFeeder.getInstance().AxisHandle, ref stateU2);
           axisStatusVM.StateU1 = stateU1;
           axisStatusVM.StateU2 = stateU2;
           #endregion

           #region 極限
           MotorFunc4PCI1240.getInstance().LimtiStatus(Recoater.getInstance().AxisHandle, ref Recoater.getInstance().Status.P_Limit,
               ref Recoater.getInstance().Status.N_Limit);
           MotorFunc4PCI1240.getInstance().LimtiStatus(BuildPlatform.getInstance().AxisHandle, ref BuildPlatform.getInstance().Status.P_Limit,
               ref BuildPlatform.getInstance().Status.N_Limit);
           MotorFunc4PCI1240.getInstance().LimtiStatus(LeftFeeder.getInstance().AxisHandle, ref LeftFeeder.getInstance().Status.P_Limit,
               ref LeftFeeder.getInstance().Status.N_Limit);
           MotorFunc4PCI1240.getInstance().LimtiStatus(RightFeeder.getInstance().AxisHandle, ref RightFeeder.getInstance().Status.P_Limit,
               ref RightFeeder.getInstance().Status.N_Limit);

           MotorFunc4PCI1240.getInstance().HomeStatus(Recoater.getInstance().AxisHandle, ref Recoater.getInstance().Status.H_Limit);
           MotorFunc4PCI1240.getInstance().HomeStatus(BuildPlatform.getInstance().AxisHandle, ref BuildPlatform.getInstance().Status.H_Limit);
           //MotorFunc4PCI1240.getInstance().HomeStatus(LeftFeeder.getInstance().AxisHandle, ref LeftFeeder.getInstance().Status.H_Limit);
           //MotorFunc4PCI1240.getInstance().HomeStatus(RightFeeder.getInstance().AxisHandle, ref RightFeeder.getInstance().Status.H_Limit);

           axisStatusVM.LimitX = Recoater.getInstance().Status.P_Limit ? "正極限" : (Recoater.getInstance().Status.N_Limit ? "負極限" : (Recoater.getInstance().Status.H_Limit ? "原點" : ""));
           axisStatusVM.LimitZ = BuildPlatform.getInstance().Status.P_Limit ? "正極限" : (BuildPlatform.getInstance().Status.N_Limit ? "負極限" : (BuildPlatform.getInstance().Status.H_Limit ? "原點" : ""));
           axisStatusVM.LimitU1 = LeftFeeder.getInstance().Status.P_Limit ? "正極限" : (LeftFeeder.getInstance().Status.N_Limit ? "負極限" : "");
           axisStatusVM.LimitU2 = RightFeeder.getInstance().Status.P_Limit ? "正極限" : (RightFeeder.getInstance().Status.N_Limit ? "負極限" : "");
           #endregion

           #region Driving
            MotorFunc4PCI1240.getInstance().DrivingStatus(Recoater.getInstance().AxisHandle, ref Recoater.getInstance().Status.isDriving);
            MotorFunc4PCI1240.getInstance().DrivingStatus(BuildPlatform.getInstance().AxisHandle, ref BuildPlatform.getInstance().Status.isDriving);
            MotorFunc4PCI1240.getInstance().DrivingStatus(LeftFeeder.getInstance().AxisHandle, ref LeftFeeder.getInstance().Status.isDriving);
            MotorFunc4PCI1240.getInstance().DrivingStatus(RightFeeder.getInstance().AxisHandle, ref RightFeeder.getInstance().Status.isDriving);
            
            #endregion
        }

        void GetEnviromentData_Tick(object sender, ElapsedEventArgs e)
        {
            if (GlobalCtrlFlag.isAxisStatusCheck && GlobalCtrlFlag.isPowderSupplyStatusCheck && GlobalCtrlFlag.isHeaterStatusCheck)
                statusVM.IsProcessStatusCheck = true;


            if (GlobalChannel.ADAM4117_Provider.GetInstance().isOpen)
            {
                string[] data = (GlobalChannel.ADAM4117_Provider.GetInstance() as EnviroDectector).ExportEnviroData();

                #region 腔體壓力
                double pressure_A = Convert.ToDouble(data[1]);;
                double pressure = (pressure_A - 4) / 16 * 750;;
                GlobalChannel.envriomentModel.Pressure = pressure;
                #endregion

                #region Cavity pressure
                double cavityGas_I = Convert.ToDouble(data[3]);
                double cavityGas = (cavityGas_I - 1.1) / 0.3;
                GlobalChannel.envriomentModel.CavityGasPressure = Math.Round(cavityGas, 2);
                #endregion

                #region 流速
                double circulatoryFlow = Convert.ToDouble(data[5]) * 2.5 - 10;
                double flow_Hz = Convert.ToDouble(data[6]) * 5.58;
                GlobalChannel.envriomentModel.FlowRate = Math.Round(circulatoryFlow, 2);
                #endregion

                #region 氧氣含量
                double oxygen_I = Convert.ToDouble(data[0]);
                double oxygen = (oxygen_I - 4 / 16) * 10000;
                GlobalChannel.envriomentModel.Oxygen = oxygen;
                #endregion
            }
            else
                return;
        }

        private void GetData()
        {
            i = i + (float)0.1;
            Data_ProgressBarStraight.NowValue = (float)30+i;

            Data_ProgressBarStraight.ChangeValue1 = i;                                       // 多餘的程式碼 -- 寫法待討論
            Data_ProgressBarStraight.ChangeValue2 = Data_ProgressBarStraight.ChangeValue1;   // 多餘的程式碼 -- 寫法待討論
            Data_ProgressBarStraight.ChangeValue3 = Data_ProgressBarStraight.ChangeValue2;   // 多餘的程式碼 -- 寫法待討論 

            Data_ProgressBarStraight2.NowValue = (float)59.2 + i;
            Data_ProgressBarStraight2.ChangeValue1 = i;                                       // 多餘的程式碼 -- 寫法待討論
            Data_ProgressBarStraight2.ChangeValue2 = Data_ProgressBarStraight2.ChangeValue1;   // 多餘的程式碼 -- 寫法待討論
            Data_ProgressBarStraight2.ChangeValue3 = Data_ProgressBarStraight2.ChangeValue2;   // 多餘的程式碼 -- 寫法待討論 

            if (i > 0.3)
            {
                i = (float)-0.3;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MotorFunc4PCI1240.getInstance().ServoOFF();
            MotorFunc4PCI1240.getInstance().CloseDevice();
            TimerHandler.m_DIO_Timer.GetTimer().Stop();
            TimerHandler.m_DIO_Timer.GetTimer().Dispose();
            AxisStatusTimer.Stop();
            AxisStatusTimer.Dispose();
            EnviroDataTimer.Stop();
            EnviroDataTimer.Dispose();     
        }

        private void Loading_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (f.ShowDialog() == true)
            {
                using (StreamReader r = new StreamReader(f.OpenFile()))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        ListBoxItem itm = new ListBoxItem();
                        itm.Content = line;
                        listbox1.Items.Add(itm);
                    }
                }
            }
        }
               
        private void Delete_message_Click(object sender, RoutedEventArgs e)
        {
            listbox1.Items.Clear();
        }
    }
}
