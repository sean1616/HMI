using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;

namespace HMI_PermanentForm
{
    /// <summary>
    /// LDpage.xaml 的互動邏輯
    /// </summary>
    public partial class Loading_B : Window
    {
        public Loading_B()
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
                    SpinWait.SpinUntil(() => false, 60000);//等待空轉時間(ms)
                }
                
            };
            worker.RunWorkerCompleted += (o, a) =>
            {
                BusyIndicator.IsBusy = false;
                this.Close();
            };
            worker.RunWorkerAsync();

            
        }
    }
}
