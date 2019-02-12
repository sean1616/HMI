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
using AM_Kernel.Interpretation;
using System.Timers;
using System.Threading;
using HMI_PermanentForm.A_folder.A4.ViewModel;
using HMI_PermanentForm.Utils;
using System.Data.SQLite;

namespace HMI_PermanentForm
{
    /// <summary>
    /// A5_1.xaml 的互動邏輯
    /// </summary>
    public partial class A4_1 : Page
    {
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;

        bool isTaskShutdown = true;

        public A4_1()
        {
            InitializeComponent();

            DataContext = GlobalChannel.StatusCheckVM;

            sqlite_conn = new SQLiteConnection("Data source=Job.sqlite");
            sqlite_cmd = sqlite_conn.CreateCommand();

        }

        private void A4_1_07_Click(object sender, RoutedEventArgs e)
        {
            if (SlicingObj.getLayers() == null)
                return;

            Utils.GlobalCtrlFlag.isAutoProcessing = true;
            Utils.GlobalChannel.AM_Processor.CurrentScanLayerIndex = Utils.GlobalChannel.SlicingVM.LastLayerIndex;
            Utils.GlobalChannel.AM_Processor.Trigger();

            if(isTaskShutdown)
            {
                sqlite_conn.Open();

                string[] data = SlicingObj.GetJobInfo();
            
                sqlite_cmd.CommandText = string.Format("INSERT INTO Template VALUES ('{0:T}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), data[0], data[4], data[2], data[1], data[3]);
                sqlite_cmd.ExecuteNonQuery();

                sqlite_conn.Close();
            }

            isTaskShutdown = false;

        }

        private void A4_1_08_Click(object sender, RoutedEventArgs e)
        {
            Utils.GlobalCtrlFlag.isAutoProcessing = false;
            Utils.GlobalChannel.AM_Processor.PauseTrigger();
        }

        private void A4_1_09_Click(object sender, RoutedEventArgs e)
        {
            Utils.GlobalCtrlFlag.isAutoProcessing = false;
            Utils.GlobalChannel.AM_Processor.ClearTrigger();
            isTaskShutdown = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalChannel.CheckSetting();
        }
    }
}
