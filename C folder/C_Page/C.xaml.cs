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

namespace HMI_PermanentForm
{
    /// <summary>
    /// Dict_System_Page.xaml 的互動邏輯
    /// </summary>
    public partial class C : Page
    {
        C1 C1_page = new C1();
        C2 C2_page = new C2();

        Lang lang = new Lang();
                
        public C()
        {
            InitializeComponent();

            //lang.En_to_Ch();
            
            if (Utils.GlobalCtrlFlag.isIOTimer_Start) 
                Utils.TimerHandler.m_DIO_Timer.GetTimer().Start();
        }

        private void C1_btn_Checked(object sender, RoutedEventArgs e)
        {
            Change_Page.NavigationService.Navigate(C1_page);
        }

        private void C2_btn_Checked(object sender, RoutedEventArgs e)
        {
            Change_Page.NavigationService.Navigate(C2_page);
        }

        private void C4_btn_Click(object sender, RoutedEventArgs e)
        {
            C5 Login_win = new C5();
            Login_win.ShowDialog();//顯示Alarm dialog，並鎖住mainwindow
        }

        private void C5_btn_Click(object sender, RoutedEventArgs e)
        {
            C5 Login_win = new C5();
            Login_win.ShowDialog();
        }

        private void Lang1_btn_Checked(object sender, RoutedEventArgs e)
        {
            //Lang_ResourceDictionary.Clear();
            //Lang_ResourceDictionary.Source = new Uri("C folder/Language/Lang_C.xaml", UriKind.RelativeOrAbsolute);
            //Lang_ResourceDictionary_B.Source = new Uri("B folder/Language/Lang_B.xaml", UriKind.RelativeOrAbsolute);          
            //Application.Current.Resources.MergedDictionaries.Add(Lang_ResourceDictionary);
            //Application.Current.Resources.MergedDictionaries.Add(Lang_ResourceDictionary_B);
            lang.En_to_Ch();

        }

        private void Lang2_btn_Checked(object sender, RoutedEventArgs e)
        {
            //Lang_ResourceDictionary.Clear();
            //Lang_ResourceDictionary.Source = new Uri("C folder/Language/Lang_Eng_C.xaml", UriKind.RelativeOrAbsolute);
            //Lang_ResourceDictionary_B.Source = new Uri("B folder/Language/Lang_En_B.xaml", UriKind.RelativeOrAbsolute);
            //Application.Current.Resources.MergedDictionaries.Add(Lang_ResourceDictionary);
            //Application.Current.Resources.MergedDictionaries.Add(Lang_ResourceDictionary_B);
            lang.Ch_to_En();
        }
    }
}
