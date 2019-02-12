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
using HelixToolkit.Wpf;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm
{
    /// <summary>
    /// A2_2.xaml 的互動邏輯
    /// </summary>
    public partial class A2_2 : Page
    {
        //private const string MODEL_PATH = "D:/Download/bladev3.obj"; //匯入檔案路徑
        private const string MODEL_PATH = "../../../A folder/model/Blade_v1.obj"; //匯入檔案路徑
        private const string MODEL_PATH_2 = "../../../A folder/model/platev1.obj"; //匯入檔案路徑

        //Data_A1_2 viewmodel = new Data_A1_2();
        Data_A1_2 viewmodel = GlobalChannel.TranlateVM;

        public A2_2()
        {
            InitializeComponent();

            ModelVisual3D device3D = new ModelVisual3D();
            ModelVisual3D device3D_2 = new ModelVisual3D();

            device3D.Content = Display3d(MODEL_PATH);
            device3D_2.Content = Display3d(MODEL_PATH_2);
            // Add to view port
            blade1.Children.Add(device3D);
            plate1.Children.Add(device3D_2);
            //view1.Children.Add(device3D); //直接將object匯入view中

        }

        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                //Adding a gesture here
                view1.RotateGesture = new MouseGesture(MouseAction.LeftClick);

                //Import 3D model file
                ModelImporter import = new ModelImporter();

                Material material = new DiffuseMaterial(new SolidColorBrush(Colors.DarkGray)); //這裡換顏色
                import.DefaultMaterial = material;

                //Load the 3D model file
                device = import.Load(model);
            }
            catch (Exception e)
            {
                // Handle exception in case can not file 3D model
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }

        private void view1_Loaded(object sender, RoutedEventArgs e)
        {
            //因Slider與transform 繫結不同步，需在此指定DataContext！！
            this.DataContext = viewmodel;                       

            //rotation.Value = 90; //預設旋轉角度
            
            //translate_X.Value = 0; //預設x方向位移量            
        }        
    }
}
