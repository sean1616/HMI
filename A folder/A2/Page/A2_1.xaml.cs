using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using Newtonsoft.Json;
using AM_Kernel.Procedure;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm
{
    /// <summary>
    /// A2_1.xaml 的互動邏輯
    /// </summary>
    public partial class A2_1 : Page
    {
        Data_A2_1 _WorkOrderVM = new Data_A2_1() { A2_1_07_value = true, A2_1_08_value = false, A2_1_05_value = false };
        string filePath = string.Empty;
        
        public A2_1()
        {
            InitializeComponent();
            DataContext = _WorkOrderVM;
            
        }

        private void A2_1_01_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "製程工單(*.json*)|*.json*";

            if (dlg.ShowDialog() == true)
            {
                filePath = dlg.FileName;

                AMProcedure.GetInstance().ReadConfig(filePath);

                #region set vm
                _WorkOrderVM.A2_1_10_value = (AMProcedure.GetInstance().GetConfig() as ProcessParam).RecoaterInitialPos;
                _WorkOrderVM.A2_1_12_value = (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderRouteDist;
                _WorkOrderVM.A2_1_15_value = (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFeedingSpeed;
                _WorkOrderVM.A2_1_17_value = (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledPos;
                _WorkOrderVM.A2_1_19_value = (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledSpeed;
                _WorkOrderVM.A2_1_21_value = (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledDuration;
                _WorkOrderVM.A2_1_24_value = (AMProcedure.GetInstance().GetConfig() as ProcessParam).BuildPlatformInitialPos;
                _WorkOrderVM.A2_1_26_value = (AMProcedure.GetInstance().GetConfig() as ProcessParam).BuildPlatformInchDist;
                _WorkOrderVM.A2_1_07_value = true;   // bi-direction
                _WorkOrderVM.A2_1_05_value = false;
                #endregion

                GlobalCtrlFlag.isPowderSupplyStatusCheck = false;
                
            }
            else
                return;
        }

        // 儲存工單
        private void A2_1_02_Click(object sender, RoutedEventArgs e)
        {
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).RecoaterInitialPos = _WorkOrderVM.A2_1_10_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderRouteDist = _WorkOrderVM.A2_1_12_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFeedingSpeed = _WorkOrderVM.A2_1_15_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledPos = _WorkOrderVM.A2_1_17_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledSpeed = _WorkOrderVM.A2_1_19_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledDuration = _WorkOrderVM.A2_1_21_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).BuildPlatformInitialPos = _WorkOrderVM.A2_1_24_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).BuildPlatformInchDist = _WorkOrderVM.A2_1_26_value;

            if(File.Exists(filePath))
            {
                AMProcedure.GetInstance().SaveConfig(filePath);
            }
            else
            {
                SaveAsConfig();
            }
                       
        }

        private void A2_1_03_Click(object sender, RoutedEventArgs e)
        {
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).RecoaterInitialPos = _WorkOrderVM.A2_1_10_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderRouteDist = _WorkOrderVM.A2_1_12_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFeedingSpeed = _WorkOrderVM.A2_1_15_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledPos = _WorkOrderVM.A2_1_17_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledSpeed = _WorkOrderVM.A2_1_19_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledDuration = _WorkOrderVM.A2_1_21_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).BuildPlatformInitialPos = _WorkOrderVM.A2_1_24_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).BuildPlatformInchDist = _WorkOrderVM.A2_1_26_value;


            SaveAsConfig();
        }

        void SaveAsConfig()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "製程工單(*.json*)|*.json*";

            if (dlg.ShowDialog() == true)
            {
                filePath = dlg.FileName + ".json";

                AMProcedure.GetInstance().SaveConfig(filePath);
            }
            else
                return;
        }

        private void A2_1_05_Click(object sender, RoutedEventArgs e)
        {
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).RecoaterInitialPos = _WorkOrderVM.A2_1_10_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderRouteDist = _WorkOrderVM.A2_1_12_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFeedingSpeed = _WorkOrderVM.A2_1_15_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledPos = _WorkOrderVM.A2_1_17_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledSpeed = _WorkOrderVM.A2_1_19_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderFilledDuration = _WorkOrderVM.A2_1_21_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).BuildPlatformInitialPos = _WorkOrderVM.A2_1_24_value;
            (AMProcedure.GetInstance().GetConfig() as ProcessParam).BuildPlatformInchDist = _WorkOrderVM.A2_1_26_value;

            GlobalCtrlFlag.isPowderSupplyStatusCheck = true;
        }
    }
}
