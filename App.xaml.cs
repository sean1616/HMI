#define OfflineMode
//#define OnlineMode

using HMI_PermanentForm.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AM_Kernel.Periphery;
using AM_Kernel.IO;
using AM_Kernel.Motor;
using System.IO;
using Newtonsoft.Json;
using AM_Kernel.LaserCtrl;


namespace HMI_PermanentForm
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        private Initializer m_initializer = new Initializer();

        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            //var dialog = new LoadingWidnow();

            //if (dialog.ShowDialog() == false)
            //{
            //    var mainWindow = new MainWindow();
            //    //Re-enable normal shutdown mode.
            //    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            //    Current.MainWindow = mainWindow;
            //    mainWindow.Show();
            //}

            //呼叫出Console視窗，這邊記得要先呼叫出來，再針對它來做其他的設定，以免出錯喔!!
            ConsoleHelper.Show();

            //設定Console視窗的大小，注意，這邊的寬和高指的是文字的行數和列數，而不是畫面上的點喔!!
            Console.SetWindowSize(80, 24);

            //設定Console視窗的起始位置
            Console.SetWindowPosition(0, 0);

            //設定Console視窗的標題
            Console.Title = "Tongtai AM250 Configuration";

            //可以再次設定前景色，之後的文字就會以新的顏色列印
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Welcome to Tongtai AM250 machine control system!");
            
            string result = string.Empty;
            string input = string.Empty;

            #region Peripheral Configuration
#if (OnlineMode)
            result = m_initializer.IsMotorConfigReady();
            Console.WriteLine(result);

            result = m_initializer.IsDIOReady();
            Console.WriteLine(result);

            result = m_initializer.WritingMotor();
            Console.WriteLine(result);

            result = m_initializer.IsLaserCtrlReady();
            Console.WriteLine(result);

            // 啟動伺服馬達電源
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO02"]);
            MotorFunc4PCI1240.getInstance().ServoON();

            // 啟動加熱器電源
            DIOUtility.DOSignalOut(DIO_DevType.Device1, true, DO_Set.employees["DO06"]);
            
#endif
#endregion

            // A timer that reads the data on whole system begins.
            if(Utils.GlobalCtrlFlag.isIOTimer_Start)
                TimerHandler.m_DIO_Timer.GetTimer().Start();


            // set-up the device below.
            while (input.ToLower() != "open")
            {
                Console.Write("請輸入open開啟程式:");

                input = Console.ReadLine();
            }
            
            var mainWindow = new MainWindow();

            //Re-enable normal shutdown mode.
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Current.MainWindow = mainWindow;
            mainWindow.Show();

            ConsoleHelper.Toggle();
            
        }
 

    }
}
