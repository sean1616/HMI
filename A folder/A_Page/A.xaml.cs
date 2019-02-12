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
using System.Windows.Media.Media3D;
using AM_Kernel.Interpretation;
using Microsoft.Win32;
using HMI_PermanentForm.Utils;


namespace HMI_PermanentForm
{
    /// <summary>
    /// Dict_Process_Page.xaml 的互動邏輯
    /// </summary>
    public partial class A : Page
    {
        #region Page instance
        A1_1 Page_A1_1 = new A1_1();
        A1_2 Page_A1_2 = new A1_2();
        
        A2_1 Page_A2_1 = new A2_1();
        //A2_2 Page_A2_2 = new A2_2();

        A3_1 Page_A3_1 = new A3_1();

        A4_1 Page_A4_1 = new A4_1(); 

        A5_1 Page_A5_1 = new A5_1();

        A6_1 Page_A6_1 = new A6_1();
        
        A_grayblock Grayblock = new A_grayblock();
        #endregion
        
        
        public A()
        {
            InitializeComponent();
          
            DataContext = this;

            
            //Defualt Page     
            Left_State_1.NavigationService.Navigate(Page_A1_1);

             //若要直接用btn產生此頁面(無到有)，則定要在此產生物件！！
            view_A1_2.NavigationService.Navigate(Page_A1_2);
                        
        }
        private void Set_Axis_btn_Checked(object sender, RoutedEventArgs e)
        {
            Page_Navigation();
            Left_State_1.NavigationService.Navigate(Page_A1_1);
        }
        private void Powder_Feeding_btn_Checked(object sender, RoutedEventArgs e)
        {
            Page_Navigation();
            Left_State_1.NavigationService.Navigate(Page_A2_1);
        }
        private void Plate_Temperature_btn_Checked(object sender, RoutedEventArgs e)
        {
            Page_Navigation();
            A_GrayBlock.NavigationService.Navigate(Grayblock);
            State_Page.NavigationService.Navigate(Page_A3_1);
        }
        private void Auto_Process_btn_Checked(object sender, RoutedEventArgs e)
        {
            Page_Navigation();
            A_GrayBlock.NavigationService.Navigate(Grayblock);
            //A5_2.NavigationService.Navigate(GlobalChannel.view3D);
            A5_2.NavigationService.Navigate(GlobalChannel.view3D);
            Left_State_2.NavigationService.Navigate(Page_A4_1);
        }
        private void Single_Motion_btn_Checked(object sender, RoutedEventArgs e)
        {
            Page_Navigation();
            Left_State_2.NavigationService.Navigate(Page_A5_1);
            A5_2.NavigationService.Navigate(GlobalChannel.view3D);
            //A5_2.Refresh();
            A_GrayBlock.NavigationService.Navigate(Grayblock);
        }

        private void Set_Axis_btn_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void Environment_btn_Checked(object sender, RoutedEventArgs e)
        {
            Page_Navigation();            
            A_GrayBlock.NavigationService.Navigate(Grayblock);
            State_Page.NavigationService.Navigate(Page_A6_1);
        }

        private void Page_Navigation()
        {
            Left_State_1.NavigationService.Navigate(null);
            A_GrayBlock.NavigationService.Navigate(null);
            Left_State_2.NavigationService.Navigate(null);
            A5_2.NavigationService.Navigate(null);
            State_Page.NavigationService.Navigate(null);
        }
    }
}
