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
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm.WebCam
{
    /// <summary>
    /// View_Cam.xaml 的互動邏輯
    /// </summary>
    public partial class View_Cam : Page
    {
        public View_Cam()
        {
            InitializeComponent();

            image.DataContext = GlobalChannel.ImageVM;
        }

    }
}
