using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Timers;
using mainHMI.gaugedata;
using HMI_PermanentForm.Utils;
using AM_Kernel.Interpretation;
using System.Windows.Media.Media3D;
using HMI_PermanentForm.WebCam;

namespace HMI_PermanentForm
{
    /// <summary>
    /// Dict_Home_Page.xaml 的互動邏輯
    /// </summary>
    public partial class E : Page
    {
        public CircleGaugeData Data_PressureGauge { get; set; }
        public CircleGaugeData Data_CavityGasPressureGauge{ get; set; }
        public CircleGaugeData Data_FlowRateGauge { get; set; }
        public CircleGaugeData Data_OxygenGauge { get; set; }
        
        // 紀錄最後一層層數，考慮使用VM單向繫結到Slider的value上
        static public int lastLayerIndex;

        CamCapturer webcam = new CamCapturer();
        // 更新數據的timer
        private Timer ReadTimer = new Timer(300);

        int i = 0;
        

        public E()
        {
            InitializeComponent();

            #region CricleGauge1
            SolidColorBrush brush_Red1 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 35, 35));
            SolidColorBrush brush_Green1 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(56, 180, 23));
            SolidColorBrush brush_Yellow1 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 83));
            Data_PressureGauge = new CircleGaugeData() { Title = "目前壓力", Title2 = "目標壓力", Title3 = "腔體壓力", Percentage = 0, GoalPercentage = 0, Unit = "torr", TickNumberAmount = 10, Color1 = brush_Green1, Color2 = brush_Yellow1, Color3 = brush_Red1, ColorDivideAngle1 = ColorDivideAngleCalculate(CircleGauge1, 250, false), ColorDivideAngle2 = ColorDivideAngleCalculate(CircleGauge1, 500, false) };
            #endregion

            #region CricleGauge2
            SolidColorBrush brush_Red2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 35, 35));
            SolidColorBrush brush_Green2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(56, 180, 23));
            SolidColorBrush brush_Yellow2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 83));
            Data_CavityGasPressureGauge = new CircleGaugeData() { Title = "目前壓力", Title2 = "目標壓力", Title3 = "氮氣壓力", Percentage = 0, GoalPercentage = 0, Unit = "kg/cm", TickNumberAmount = 10, Color1 = brush_Green2, Color2 = brush_Yellow2, Color3 = brush_Red2, ColorDivideAngle1 = ColorDivideAngleCalculate(CircleGauge2, 3, false), ColorDivideAngle2 = ColorDivideAngleCalculate(CircleGauge2, 6, false) };
            #endregion

            #region CricleGauge3
            SolidColorBrush brush_Red3 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 35, 35));
            SolidColorBrush brush_Green3 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(56, 180, 23));
            SolidColorBrush brush_Yellow3 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 83));
            Data_FlowRateGauge = new CircleGaugeData() { Title = "目前流量", Title2 = "目標流量", Title3 = "氣旋流量", Percentage = 0, GoalPercentage = 0, Unit = "m/s", TickNumberAmount = 10, Color1 = brush_Green3, Color2 = brush_Yellow3, Color3 = brush_Red3, ColorDivideAngle1 = ColorDivideAngleCalculate(CircleGauge3, 8, false), ColorDivideAngle2 = ColorDivideAngleCalculate(CircleGauge3, 16, false) };
            #endregion

            #region CricleGauge4
            SolidColorBrush brush_Red4 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 35, 35));
            SolidColorBrush brush_Green4 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(56, 180, 23));
            SolidColorBrush brush_Yellow4 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 83));
            Data_OxygenGauge = new CircleGaugeData() { Title = "目前含量", Title2 = "目標含量", Title3 = "氧氣含量", Percentage = 0, GoalPercentage = 0, Unit = "ppm", TickNumberAmount = 10, Color1 = brush_Green4, Color2 = brush_Yellow4, Color3 = brush_Red4, ColorDivideAngle1 = ColorDivideAngleCalculate(CircleGauge4, 2500, false), ColorDivideAngle2 = ColorDivideAngleCalculate(CircleGauge4, 5000, false) };
            #endregion


            DataContext = this;

            E_View3D.NavigationService.Navigate(GlobalChannel.view3D);
            E_View3D.Refresh();
            //E_View3D.NavigationService.Navigate(GlobalChannel.CameraStream);
            // translate into xaml language.
            LayerSlider.DataContext = GlobalChannel.SlicingVM;

            DisplayObj();

            // timer開始- 讀取資料
            ReadTimer.Elapsed += new System.Timers.ElapsedEventHandler(ReadTimerEvent);
            ReadTimer.Start();
        }               

        private int ColorDivideAngleCalculate(ProgressBar progressbar, int DivideValue, bool IsInverse)
        {
            int Angle = 0;

            if (IsInverse)
            {
                Angle = (int)(110 - 220 * DivideValue / (progressbar.Maximum - progressbar.Minimum));
            }
            else
            {
                Angle = (int)(-110 + 220 * DivideValue / (progressbar.Maximum - progressbar.Minimum));
            }
            return Angle;
        }

        public void ReadTimerEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            i = i + 3;

            //Data_CircleGauge1.Percentage = Math.Round(748 + 3 * Math.Cos(Math.PI * i / 180), 2);      //demo 用 model 可調整
            Data_PressureGauge.Percentage = GlobalChannel.envriomentModel.Pressure;
            Data_PressureGauge.GoalPercentage = 750;

            //Data_CircleGauge2.Percentage = Math.Round(5 + 3 * Math.Cos(Math.PI * i / 180), 2);      //demo 用 model 可調整
            Data_CavityGasPressureGauge.Percentage = GlobalChannel.envriomentModel.CavityGasPressure;
            Data_CavityGasPressureGauge.GoalPercentage = 10;

            //Data_CircleGauge3.Percentage = Math.Round(10 + 3 * Math.Cos(Math.PI * i / 180), 2);      //demo 用 model 可調整
            Data_FlowRateGauge.Percentage = GlobalChannel.envriomentModel.FlowRate;
            Data_FlowRateGauge.GoalPercentage = 20;

            //Data_CircleGauge4.Percentage = Math.Round(1000 + 3 * Math.Cos(Math.PI * i / 180), 2);      //demo 用 model 可調整
            Data_OxygenGauge.Percentage = GlobalChannel.envriomentModel.Oxygen;
            Data_OxygenGauge.GoalPercentage = 10000;
        }

        private void DisplayObj()
        {
            if (SlicingObj.getLayers() == null)
            {
                return;
            }
            else
            {
                GlobalChannel.SlicingVM.Hatch = new Point3DCollection(SlicingObj.getLayers().getLayers()[GlobalChannel.SlicingVM.LastLayerIndex].getHatch());
                GlobalChannel.SlicingVM.NumOfLayers = SlicingObj.getLayers().getLayers().Count;
            }
        }

        private void Page_E_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayObj();
            //LayerSlider.Value = lastLayerIndex + 1; //這行會使Page_B_Slider無法改變Page_E_Slider
        }

        private void LayerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SlicingObj.getLayers() == null)
                return;
            else
            {
                GlobalChannel.SlicingVM.Hatch = new Point3DCollection(SlicingObj.getLayers().getLayers()[(int)e.NewValue - 1].getHatch());
                //GlobalChannel.SlicingVM.NumOfLayers = SlicingObj.getLayers().Count;            
            }
        }

        private void LayerSlider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            GC.Collect();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
           lastLayerIndex = (int)(LayerSlider.Value - 1);
        }

        private void RB_VideoWatcher_Checked(object sender, RoutedEventArgs e)
        {
            E_View3D.NavigationService.Navigate(GlobalChannel.CameraStream);
            webcam.WebCamTrigger(true);
            MessageBox.Show("Check");
        }

        private void RB_VideoWatcher_Unchecked(object sender, RoutedEventArgs e)
        {
            webcam.WebCamTrigger(false);
            MessageBox.Show("UnCheck");
        }

        private void RB_3DView_Checked(object sender, RoutedEventArgs e)
        {
            E_View3D.NavigationService.Navigate(GlobalChannel.view3D);
        }

        private void RB_3DView_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
