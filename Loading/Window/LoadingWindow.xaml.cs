using System;
using System.Windows;
using System.ComponentModel;
using System.Threading;
using AM_Kernel.Motor;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm
{
    /// <summary>
    /// LDpage.xaml 的互動邏輯
    /// </summary>
    public partial class LoadingWidnow : Window
    {
        public LoadingWidnow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BusyIndicator.IsBusy = true;
            BusyIndicator.BusyContent = "Loading...";
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, a) =>
            {
                for (int index = 0; index < 1; index++)
                {
                    //Dispatcher.Invoke(new Action(() =>
                    //{
                    //    BusyIndicator.BusyContent = "Loop : " + index;
                    //}), null);
                    //Thread.Sleep(new TimeSpan(0, 0, 1));
                    
                    SpinWait.SpinUntil(() => false, 3000);//Waiting time(ms)
                }

                IsMotorReady();
                
            };
            worker.RunWorkerCompleted += (o, a) =>
            {
                BusyIndicator.IsBusy = false;
                this.Close();
            };
            worker.RunWorkerAsync();
        }

        private static string IsMotorReady()
        {
            Recoater.getInstance().ReadConfig("../../../TT_AM_Kernel/Config/Recoater.json");

            string str = MotorFunc4PCI1240.getInstance().InitialDevice();

            if (str != "")
                return str;
            else
                Console.WriteLine("Device initial Done!");

            return str;
        }
    }
}
