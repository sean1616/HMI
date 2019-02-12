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
using AM_Kernel.Interpretation;
using HMI_PermanentForm.Utils;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace HMI_PermanentForm
{
    /// <summary>
    /// B1_2.xaml 的互動邏輯
    /// </summary>
    public partial class B1_2 : Page
    {        
        public B1_2()
        {
            InitializeComponent();
                        
            LayerSlider.DataContext = GlobalChannel.SlicingVM;
            
        }               

        private void LayerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SlicingObj.getLayers() == null)
                return;
            else
            {
                GlobalChannel.SlicingVM.Hatch = new Point3DCollection(SlicingObj.getLayers().getLayers()[(int)e.NewValue - 1].getHatch());
                GlobalChannel.SlicingVM.NumOfLayers = SlicingObj.getLayers().getLayers().Count;
            }
        }

        private void LayerSlider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            GC.Collect();
        }        
    }
}
