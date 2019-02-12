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
using AMEntity;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm
{
    /// <summary>
    /// A5_1.xaml 的互動邏輯
    /// </summary>
    public partial class A5_1 : Page
    {
        Data_A5_1 viewmodel = new Data_A5_1();

        public A5_1()
        {
            InitializeComponent();

            this.DataContext = viewmodel;

            A5_1_02_Img.DataContext = GlobalChannel.StatusCheckVM;
            A5_1_03_Img.DataContext = GlobalChannel.StatusCheckVM;
            A5_1_04_Img.DataContext = GlobalChannel.StatusCheckVM;

            viewmodel.Num = 0;
        }

        #region BuildPlatform decline
        private void A5_1_05_b_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.Num -= 120;
            GlobalChannel.AM_Processor.ExecuteSpecifiedOrders((int)AMEntity.AMProcedureDef.BUILDPLATFORM_DECLINE);
        }
        #endregion

        #region Laser melting
        private void A5_1_05_c_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.Num -= 120;

            GlobalChannel.AM_Processor.ExecuteSpecifiedOrders((int)AMEntity.AMProcedureDef.LASER_SCANNING);
            
        }
        #endregion

        #region Powder supply
        private void A5_1_05_d_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.Num -= 120;
            GlobalChannel.AM_Processor.ExecuteSpecifiedOrders((int)AMEntity.AMProcedureDef.POWDER_SUPPLY);
        }
        #endregion

        private void Reload_btn_Click(object sender, RoutedEventArgs e)
        {
           GlobalChannel.AM_Processor.ExecuteSpecifiedOrders((int)AMEntity.AMProcedureDef.POWDER_FILLING);
        }

        private void Feed_btn_Click(object sender, RoutedEventArgs e)
        {
            GlobalChannel.AM_Processor.ExecuteSpecifiedOrders((int)AMEntity.AMProcedureDef.POWDER_SUPPLY);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalChannel.CheckSetting();
        }
    }
}
