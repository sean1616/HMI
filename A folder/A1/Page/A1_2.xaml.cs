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
using AM_Kernel.Motor;
using AM_Kernel.Utils;

namespace HMI_PermanentForm
{
    /// <summary>
    /// A2_2.xaml 的互動邏輯
    /// </summary>
    public partial class A1_2 : Page
    {
        //Data_A1_2 viewmodel = new Data_A1_2();
        Data_A1_2 viewmodel = GlobalChannel.TranlateVM;

        public A1_2()
        {
            InitializeComponent();
        }

        // The main object model group.        
        //指定匯入物件        
        private Model3DGroup Plate_model3Dgroup = new Model3DGroup();
        private Model3DGroup Plate_U_model3Dgroup = new Model3DGroup();
        private Model3DGroup Plate_D_model3Dgroup = new Model3DGroup();
        private Model3DGroup Blade_model3Dgroup = new Model3DGroup();
        private Model3DGroup Blade_R_Arrow_model3Dgroup = new Model3DGroup();
        private Model3DGroup Blade_L_Arrow_model3Dgroup = new Model3DGroup();
        //private Model3DGroup Blade_R_Jog_Arrow_model3Dgroup = new Model3DGroup();
        //private Model3DGroup Blade_L_Jog_Arrow_model3Dgroup = new Model3DGroup();
        private Model3DGroup U1_model3Dgroup = new Model3DGroup();
        private Model3DGroup U2_model3Dgroup = new Model3DGroup();

        // Materials used for normal and selected models.
        private Material NormalMaterial = new DiffuseMaterial(Brushes.Gray); //Define NormalMaterial color
        private Material SelectedMaterial = new DiffuseMaterial(Brushes.LightGray);
        private Material Arrow_Normal_Material = new DiffuseMaterial(Brushes.LightBlue);
        private Material Arrow_Selected_Material = new DiffuseMaterial(Brushes.SkyBlue);

        // The list of selectable models.
        private List<GeometryModel3D> SelectableModels = new List<GeometryModel3D>();
        private List<GeometryModel3D> SelectableModels_Arrows = new List<GeometryModel3D>();

        //SelectedModels & Arrows
        private GeometryModel3D SelectedModel = null;
        private GeometryModel3D SelectedModel_Arrows = null;

        private GeometryModel3D GM_Plate, GM_Plate_U, GM_Plate_D, GM_Plate_U_Jog, GM_Plate_D_Jog = new GeometryModel3D();
        private GeometryModel3D GM_Blade, GM_Blade_R, GM_Blade_L, GM_Blade_R_Jog, GM_Blade_L_Jog = new GeometryModel3D();
        private GeometryModel3D GM_U1, GM_U2 = new GeometryModel3D();

        //load the stl file to edit
        string Blade_path = @"../../../TT_AM_Kernel/3D Model/Blade_v2.stl";
        string Blade_R_Arrow_path = @"../../../TT_AM_Kernel/3D Model/Arrow_R_v4.stl";
        string Blade_L_Arrow_path = @"../../../TT_AM_Kernel/3D Model/Arrow_L_v4.stl";
        string Plate_path = @"../../../TT_AM_Kernel/3D Model/Plate_v2.stl";
        string Plate_U_path = @"../../../TT_AM_Kernel/3D Model/Arrow_U_v4.stl";
        string Plate_D_path = @"../../../TT_AM_Kernel/3D Model/Arrow_D_v4.stl";
        string U1_path = @"../../../TT_AM_Kernel/3D Model/U1_v2.stl";
        string U2_path = @"../../../TT_AM_Kernel/3D Model/U2_v2.stl";

        bool check = false;

        private void view1_Loaded(object sender, RoutedEventArgs e)
        {
            //因Slider與transform 繫結不同步，因此需在此指定DataContext！！
            this.DataContext = viewmodel;

            //viewmodel.TranslateValue = -145; //預設x方向位移量  

            DefineModel_Blade(Blade_path);

            DefineModel_Plate(Plate_path);

            DefineModel_Plate_U_Arrow(Plate_U_path);
            DefineModel_Plate_D_Arrow(Plate_D_path);
            DefineModel_Blade_R_Arrow(Blade_R_Arrow_path);
            DefineModel_Blade_L_Arrow(Blade_L_Arrow_path);
            DefineModel_U1(U1_path);
            DefineModel_U2(U2_path);

            // Add the group of models to a ModelVisual3D.                
            Blade.Content = Blade_model3Dgroup; //將Model匯入view中
            Plate.Content = Plate_model3Dgroup;
            U1.Content = U1_model3Dgroup;
            U2.Content = U2_model3Dgroup;

        }

        private MeshGeometry3D Load_Path(string path)
        {
            var mi = new ModelImporter();

            Model3DGroup Model = mi.Load(path, System.Windows.Threading.Dispatcher.CurrentDispatcher);

            MeshBuilder Model_meshbuilder = new MeshBuilder(true, false);

            //convert model to geometry3D //判斷物件是否為空
            foreach (var n in Model.Children)
            {
                var mGeo = n as GeometryModel3D;
                var tri = (MeshGeometry3D)mGeo.Geometry;

                if (tri != null)
                {
                    Model_meshbuilder.Append(tri);
                }
            }

            //Generate meshgeometry3D model
            MeshGeometry3D Model_meshgeometry = Model_meshbuilder.ToMesh();

            return Model_meshgeometry;
        }

        private void DefineModel_Blade(string model_path)
        {
            GM_Blade = new GeometryModel3D(Load_Path(model_path), NormalMaterial);

            //Add GeometryModel into Model_group            
            Blade_model3Dgroup.Children.Add(GM_Blade);

            // Remember that this model is selectable.
            //將物件放入"可選擇清單中"
            SelectableModels.Add(GM_Blade);
        }

        private void DefineModel_Blade_R_Arrow(string model_path)
        {
            GM_Blade_R = new GeometryModel3D(Load_Path(model_path), Arrow_Normal_Material); //指定模型顏色

            //Add GeometryModel into Model_group            
            Blade_R_Arrow_model3Dgroup.Children.Add(GM_Blade_R);

            // Remember that this model is selectable.
            //將物件放入"可選擇清單中"
            SelectableModels_Arrows.Add(GM_Blade_R);
        }

        private void DefineModel_Blade_L_Arrow(string model_path)
        {
            GM_Blade_L = new GeometryModel3D(Load_Path(model_path), Arrow_Normal_Material); //指定模型顏色

            //Add GeometryModel into             
            Blade_L_Arrow_model3Dgroup.Children.Add(GM_Blade_L);

            // Remember that this model is selectable.
            //將物件放入"可選擇清單中"
            SelectableModels_Arrows.Add(GM_Blade_L);

        }

        private void DefineModel_Plate(string model_path)
        {
            GM_Plate = new GeometryModel3D(Load_Path(model_path), NormalMaterial);

            //Add GeometryModel into Model_group            
            Plate_model3Dgroup.Children.Add(GM_Plate);

            // Remember that this model is selectable.
            //將物件放入"可選擇清單中"
            SelectableModels.Add(GM_Plate);

        }

        private void DefineModel_Plate_U_Arrow(string model_path)
        {
            GeometryModel3D Plate_geometrymodel = new GeometryModel3D(Load_Path(model_path), Arrow_Normal_Material);

            GM_Plate_U = Plate_geometrymodel;

            //Add GeometryModel into Model_group
            Plate_U_model3Dgroup.Children.Add(GM_Plate_U);

            // Remember that this model is selectable.
            //將物件放入"可選擇清單中"
            SelectableModels_Arrows.Add(GM_Plate_U);
        }

        private void DefineModel_Plate_D_Arrow(string model_path)
        {
            GeometryModel3D Plate_geometrymodel = new GeometryModel3D(Load_Path(model_path), Arrow_Normal_Material);

            GM_Plate_D = Plate_geometrymodel;

            //Add GeometryModel into Model_group
            Plate_D_model3Dgroup.Children.Add(GM_Plate_D);

            // Remember that this model is selectable.
            //將物件放入"可選擇清單中"
            SelectableModels_Arrows.Add(GM_Plate_D);
        }

        private void DefineModel_U1(string model_path)
        {
            GM_U1 = new GeometryModel3D(Load_Path(model_path), NormalMaterial);

            //Add GeometryModel into Model_group
            U1_model3Dgroup.Children.Add(GM_U1);

            // Remember that this model is selectable.
            //將物件放入"可選擇清單中"
            SelectableModels.Add(GM_U1);
        }

        #region Recoater Jog
        private void Left_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            if (!Utils.GlobalCtrlFlag.isBuildPlatformBeyondLevel)
            {
                if (Recoater.getInstance().Status.isDriving)
                    return;
                else
                {
                    string ret = MotorFunc4PCI1240.getInstance().JOG_N(Recoater.getInstance().AxisHandle, (Recoater.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Jog);
                    if (ret != "")
                        LogHandler.LogError(LogHandler.Formatting(ret, "Recoater Jog Negative Error"));
                }
            }
            else
            {
                MessageBox.Show("Z軸高於平面");
                return;
            }
        }

        private void Left_MouseUp(object sender, MouseButtonEventArgs e)
        {
                    string ret = MotorFunc4PCI1240.getInstance().JOG_Stop(Recoater.getInstance().AxisHandle, (Recoater.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                    if (ret != "")
                        LogHandler.LogError(LogHandler.Formatting(ret, "Recoater Jog Stop Error"));
            
            
        }

        private void Right_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Utils.GlobalCtrlFlag.isBuildPlatformBeyondLevel)
            {
                if (Recoater.getInstance().Status.isDriving)
                    return;
                else
                {
                    string ret = MotorFunc4PCI1240.getInstance().JOG_P(Recoater.getInstance().AxisHandle, (Recoater.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Jog);
                    if (ret != "")
                        LogHandler.LogError(LogHandler.Formatting(ret, "Recoater Jog Positive Error"));
                }
            }
            else
            {
                MessageBox.Show("Z軸高於平面");
                return;
            }
        }

        private void Right_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
                    string ret = MotorFunc4PCI1240.getInstance().JOG_Stop(Recoater.getInstance().AxisHandle, (Recoater.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                    if (ret != "")
                        LogHandler.LogError(LogHandler.Formatting(ret, "Recoater Jog Stop Error"));
            
        }
        #endregion

        #region BuildPlatform Jog
        private void Up_MouseDown(object sender, MouseButtonEventArgs e)
        {    
            if (Utils.GlobalCtrlFlag.isRecoaterInScope)
            {
                MessageBox.Show("X軸在警戒範圍");
                return;
            }
            else
            {
                if (BuildPlatform.getInstance().Status.isDriving)
                    return;
                else
                {
                    string ret = MotorFunc4PCI1240.getInstance().JOG_P(BuildPlatform.getInstance().AxisHandle, (BuildPlatform.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Jog);
                    if (ret != "")
                        LogHandler.LogError(LogHandler.Formatting(ret, "BuildPlatform Jog Positive Error"));

                }
            }
        }

        private void Up_MouseUp(object sender, MouseButtonEventArgs e)
        {
                    string ret = MotorFunc4PCI1240.getInstance().JOG_Stop(BuildPlatform.getInstance().AxisHandle, (BuildPlatform.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                    if (ret != "")
                        LogHandler.LogError(LogHandler.Formatting(ret, "BuildPlatform Jog Stop Error"));

        }

        private void Down_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Utils.GlobalCtrlFlag.isRecoaterInScope)
            {
                MessageBox.Show("X軸在警戒範圍");
                return;
            }
            else
            {
                if (BuildPlatform.getInstance().Status.isDriving)
                    return;
                else
                {
                    string ret = MotorFunc4PCI1240.getInstance().JOG_N(BuildPlatform.getInstance().AxisHandle, (BuildPlatform.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Jog);
                    if (ret != "")
                        LogHandler.LogError(LogHandler.Formatting(ret, "BuildPlatform Jog Negative Error"));

                }
            }
        }

        private void Down_MouseUp(object sender, MouseButtonEventArgs e)
        {
                    string ret = MotorFunc4PCI1240.getInstance().JOG_Stop(BuildPlatform.getInstance().AxisHandle, (BuildPlatform.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                    if (ret != "")
                        LogHandler.LogError(LogHandler.Formatting(ret, "BuildPlatform Jog Stop Error"));
     
        }
        #endregion

        #region U1 Jog
        private void U1_up_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LeftFeeder.getInstance().Status.isDriving)
                return;
            else
            {
                string ret = MotorFunc4PCI1240.getInstance().JOG_P(LeftFeeder.getInstance().AxisHandle, (LeftFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Jog);
                if (ret != "")
                    LogHandler.LogError(LogHandler.Formatting(ret, "LeftFeeder Jog Positive Error"));

            }

        }

        private void U1_up_MouseUp(object sender, MouseButtonEventArgs e)
        {
                string ret = MotorFunc4PCI1240.getInstance().JOG_Stop(LeftFeeder.getInstance().AxisHandle, (LeftFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                if (ret != "")
                    LogHandler.LogError(LogHandler.Formatting(ret, "LeftFeeder Jog Stop Error"));

        }

        private void U1_down_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LeftFeeder.getInstance().Status.isDriving)
                return;
            else
            {
                string ret = MotorFunc4PCI1240.getInstance().JOG_N(LeftFeeder.getInstance().AxisHandle, (LeftFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Jog);
                if (ret != "")
                    LogHandler.LogError(LogHandler.Formatting(ret, "LeftFeeder Jog negative Error"));

            }
        }

        private void U1_down_MouseUp(object sender, MouseButtonEventArgs e)
        {
                string ret = MotorFunc4PCI1240.getInstance().JOG_Stop(LeftFeeder.getInstance().AxisHandle, (LeftFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                if (ret != "")
                    LogHandler.LogError(LogHandler.Formatting(ret, "LeftFeeder Jog Stop Error"));
        }
        #endregion

        #region U2 Jog
        private void U2_up_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (RightFeeder.getInstance().Status.isDriving)
                return;
            {
                string ret = MotorFunc4PCI1240.getInstance().JOG_P(RightFeeder.getInstance().AxisHandle, (RightFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Jog);
                if (ret != "")
                    LogHandler.LogError(LogHandler.Formatting(ret, "RightFeeder Jog positive Error"));
            }
        }

        private void U2_up_MouseUp(object sender, MouseButtonEventArgs e)
        {
                string ret = MotorFunc4PCI1240.getInstance().JOG_Stop(RightFeeder.getInstance().AxisHandle, (RightFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                if (ret != "")
                    LogHandler.LogError(LogHandler.Formatting(ret, "RightFeeder Jog Stop Error"));
        }

        private void U2_down_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (RightFeeder.getInstance().Status.isDriving)
                return;
            else
            {
                string ret = MotorFunc4PCI1240.getInstance().JOG_N(RightFeeder.getInstance().AxisHandle, (RightFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Jog);
                if (ret != "")
                    LogHandler.LogError(LogHandler.Formatting(ret, "RightFeeder Jog negative Error"));

            }
        }

        private void U2_down_MouseUp(object sender, MouseButtonEventArgs e)
        {
                string ret = MotorFunc4PCI1240.getInstance().JOG_Stop(RightFeeder.getInstance().AxisHandle, (RightFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                if (ret != "")
                    LogHandler.LogError(LogHandler.Formatting(ret, "RightFeeder Jog Stop Error"));

        }
        #endregion


        private void U2_up_btn_Click(object sender, RoutedEventArgs e)
        {
            MotorFunc4PCI1240.getInstance().AxisSetSpeed(RightFeeder.getInstance().AxisHandle,
                (RightFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
            string ret = MotorFunc4PCI1240.getInstance().MoveINC(RightFeeder.getInstance().AxisHandle, RightFeeder.UnitTransform.MM2Pulse(viewmodel.Increment));
            if (ret != "")
                LogHandler.LogError(LogHandler.Formatting(ret, "Right Feeder move incrementally up Error"));
        }

        private void U2_Down_btn_Click(object sender, RoutedEventArgs e)
        {
            MotorFunc4PCI1240.getInstance().AxisSetSpeed(RightFeeder.getInstance().AxisHandle,
                (RightFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
            string ret = MotorFunc4PCI1240.getInstance().MoveINC(RightFeeder.getInstance().AxisHandle, -RightFeeder.UnitTransform.MM2Pulse(viewmodel.Increment));
            if (ret != "")
                LogHandler.LogError(LogHandler.Formatting(ret, "Right Feeder move incrementally down Error"));
        }

        private void DefineModel_U2(string model_path)
        {
            GM_U2 = new GeometryModel3D(Load_Path(model_path), NormalMaterial);

            //Add GeometryModel into Model_group
            U2_model3Dgroup.Children.Add(GM_U2);

            // Remember that this model is selectable.
            //將物件放入"可選擇清單中"
            SelectableModels.Add(GM_U2);
        }

        #region Hit Testing Code

        // See what was clicked. //滑鼠點擊事件
        private void view1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Get the mouse's position relative to the viewport. //取得游標位置
            Point mouse_pos = e.GetPosition(view1);

            // Perform the hit test. //回傳點擊結果
            HitTestResult result =
                VisualTreeHelper.HitTest(view1, mouse_pos);


            // See if we hit a model.
            RayMeshGeometry3DHitTestResult mesh_result =
                result as RayMeshGeometry3DHitTestResult;

            if (mesh_result != null)
            {
                //gets the GeometryModel3D object that was hit
                GeometryModel3D model = (GeometryModel3D)mesh_result.ModelHit;
                if (SelectableModels.Contains(model)) //if點選元件在"模型選單中"
                {
                    viewmodel.Jog_visibility = Visibility.Visible;
                    Jog_Unable();

                    if (SelectedModel != null)
                    {
                        SelectedModel.Material = NormalMaterial; //回復原色
                        SelectedModel = null;
                        check = true; //判斷是否點到模型                      
                    }

                    if (model == GM_Plate) //點到平台時
                    {
                        Content_null();
                        viewmodel.Jog_Up_down_enable = true;
                        Plate_U.Content = Plate_U_model3Dgroup;
                        Plate_D.Content = Plate_D_model3Dgroup;
                    }
                    else if (model == GM_Blade) //點到刮刀時
                    {
                        Content_null();
                        viewmodel.Jog_X_enable = true;
                        Arrow_R.Content = Blade_R_Arrow_model3Dgroup;
                        Arrow_L.Content = Blade_L_Arrow_model3Dgroup;
                    }
                    else
                    {
                        check = false;
                        Content_null();
                    }

                    if (model == GM_U1) //點到U1時
                    {
                        viewmodel.Jog_U1_enable = true;
                        viewmodel.U1_visibility = Visibility.Visible;
                        check = true;
                    }
                    else
                    {
                        viewmodel.U1_visibility = Visibility.Hidden;
                    }

                    if (model == GM_U2) //點到U2時
                    {
                        viewmodel.Jog_U2_enable = true;
                        viewmodel.U2_visibility = Visibility.Visible;
                        check = true;
                    }
                    else
                    {
                        viewmodel.U2_visibility = Visibility.Hidden;
                    }

                    SelectedModel = model;
                    SelectedModel.Material = SelectedMaterial;

                }
                else if (SelectableModels_Arrows.Contains(model)) //if點選元件在"鍵頭選單中"
                {
                    ArrowRecover();

                    SelectedModel_Arrows = model;
                    SelectedModel_Arrows.Material = Arrow_Selected_Material;

                    if (model == GM_Blade_R)
                    {
                        viewmodel.TranslateValue += viewmodel.Increment;

                        if (!Utils.GlobalCtrlFlag.isBuildPlatformBeyondLevel)
                        {
                            if (Recoater.getInstance().Status.isDriving)
                                return;
                            else
                            {
                                MotorFunc4PCI1240.getInstance().AxisSetSpeed(Recoater.getInstance().AxisHandle,
                                    (Recoater.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                                string ret = MotorFunc4PCI1240.getInstance().MoveINC(Recoater.getInstance().AxisHandle, Recoater.UnitTransform.MM2Pulse(viewmodel.Increment));
                                if (ret != "")
                                    LogHandler.LogError(LogHandler.Formatting(ret, "Recoater move incrementally right Error"));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Z軸高於平面");
                            
                        }
                        ArrowRecover();
                    }

                    else if (model == GM_Blade_L)
                    {
                        viewmodel.TranslateValue -= viewmodel.Increment;
                        if (!Utils.GlobalCtrlFlag.isBuildPlatformBeyondLevel)
                        {
                            if (Recoater.getInstance().Status.isDriving)
                                return;
                            else
                            {
                                MotorFunc4PCI1240.getInstance().AxisSetSpeed(Recoater.getInstance().AxisHandle,
                                    (Recoater.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                                string ret = MotorFunc4PCI1240.getInstance().MoveINC(Recoater.getInstance().AxisHandle, -Recoater.UnitTransform.MM2Pulse(viewmodel.Increment));
                                if (ret != "")
                                    LogHandler.LogError(LogHandler.Formatting(ret, "Recoater move incrementally left Error"));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Z軸高於平面");

                        }
                        ArrowRecover();
                    }
                    else if (model == GM_Blade_R_Jog)
                    {
                        MessageBox.Show("R btn");
                        ArrowRecover();
                    }
                    else if (model == GM_Blade_L_Jog)
                    {
                        MessageBox.Show("L btn");
                        ArrowRecover();
                    }
                    else if (model == GM_Plate_U)
                    {
                        if (Utils.GlobalCtrlFlag.isRecoaterInScope)
                        {
                            MessageBox.Show("X軸在警戒範圍");
                            return;
                        }
                        else
                        {
                            if (BuildPlatform.getInstance().Status.isDriving)
                                return;
                            else
                            {
                                MotorFunc4PCI1240.getInstance().AxisSetSpeed(BuildPlatform.getInstance().AxisHandle,
                                    (BuildPlatform.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                                string ret = MotorFunc4PCI1240.getInstance().MoveINC(BuildPlatform.getInstance().AxisHandle, BuildPlatform.UnitTransform.MM2Pulse(viewmodel.Increment));
                                if (ret != "")
                                    LogHandler.LogError(LogHandler.Formatting(ret, "BuildPlatform move incrementally up Error"));

                            }
                        }
                        ArrowRecover();
                    }
                    else if (model == GM_Plate_D)
                    {
                        if (Utils.GlobalCtrlFlag.isRecoaterInScope)
                        {
                            MessageBox.Show("X軸在警戒範圍");
                            return;
                        }
                        else
                        {
                            if (BuildPlatform.getInstance().Status.isDriving)
                                return;
                            else
                            {
                                MotorFunc4PCI1240.getInstance().AxisSetSpeed(BuildPlatform.getInstance().AxisHandle,
                                    (BuildPlatform.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
                                string ret = MotorFunc4PCI1240.getInstance().MoveINC(BuildPlatform.getInstance().AxisHandle, -BuildPlatform.UnitTransform.MM2Pulse(viewmodel.Increment));
                                if (ret != "")
                                    LogHandler.LogError(LogHandler.Formatting(ret, "BuildPlatform move incrementally down Error"));

                            }
                        }
                        ArrowRecover();
                    }
                }
            }
            else
            {
                check = false;//設定是否點到模型
                viewmodel.Jog_visibility = Visibility.Hidden;

                if (SelectedModel != null)
                {
                    SelectedModel.Material = NormalMaterial; //回復原色
                    SelectedModel = null;
                    Content_null();
                    viewmodel.U1_visibility = Visibility.Hidden;
                    viewmodel.U2_visibility = Visibility.Hidden;
                }
            }
        }
        #endregion Hit Testing Code 


        //滑鼠移動事件
        #region Mouse Over Testing Code

        private void view1_MouseMove(object sender, MouseEventArgs e)
        {
            Point mouse_pos = e.GetPosition(view1);

            // Perform the hit test. //回傳點擊(位置)結果
            HitTestResult result = VisualTreeHelper.HitTest(view1, mouse_pos);

            // See if we hit a model.
            RayMeshGeometry3DHitTestResult mesh_result = result as RayMeshGeometry3DHitTestResult;

            if (mesh_result != null)
            {
                //gets the GeometryModel3D object that was hit
                GeometryModel3D model = (GeometryModel3D)mesh_result.ModelHit;
                if (SelectableModels.Contains(model) & check == false) //if滑鼠所在元件於"模型選單中"且無點選到模型
                {
                    if (SelectedModel != null)
                    {
                        SelectedModel.Material = NormalMaterial; //回復原色
                        SelectedModel = null;
                    }
                    SelectedModel = model;
                    SelectedModel.Material = SelectedMaterial;
                }
            }
            else
            {
                if (SelectedModel != null & check == false)
                {
                    SelectedModel.Material = NormalMaterial; //回復原色
                    SelectedModel = null;
                }
            }
        }

        private void U1_up_btn_Click(object sender, RoutedEventArgs e)
        {
            MotorFunc4PCI1240.getInstance().AxisSetSpeed(LeftFeeder.getInstance().AxisHandle,
                (LeftFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
            string ret = MotorFunc4PCI1240.getInstance().MoveINC(LeftFeeder.getInstance().AxisHandle, LeftFeeder.UnitTransform.MM2Pulse(viewmodel.Increment));
            if (ret != "")
                LogHandler.LogError(LogHandler.Formatting(ret, "Left Feeder move incrementally up Error"));
        }

        private void U1_Down_btn_Click(object sender, RoutedEventArgs e)
        {
            MotorFunc4PCI1240.getInstance().AxisSetSpeed(LeftFeeder.getInstance().AxisHandle,
                (LeftFeeder.getInstance().GetConfig() as MotorPara).paraMotion.Speed_Driving);
            string ret = MotorFunc4PCI1240.getInstance().MoveINC(LeftFeeder.getInstance().AxisHandle, -LeftFeeder.UnitTransform.MM2Pulse(viewmodel.Increment));
            if (ret != "")
                LogHandler.LogError(LogHandler.Formatting(ret, "Left Feeder move incrementally down Error"));
        }

        #endregion Mouse Over Testing Code


        //模型回原色
        private void ModelRecover()
        {
            if (SelectedModel != null & check == false)
            {
                SelectedModel.Material = NormalMaterial;
                SelectedModel = null;
            }
        }

        //鍵頭回原色
        private void ArrowRecover()
        {
            if (SelectedModel_Arrows != null)
            {
                SelectedModel_Arrows.Material = Arrow_Normal_Material;
                SelectedModel_Arrows = null;
            }
        }

        //Delete all models
        private void Content_null()
        {
            Arrow_R.Content = null;
            Arrow_L.Content = null;
            Arrow_R_Jog.Content = null;
            Arrow_L_Jog.Content = null;
            Plate_D.Content = null;
            Plate_U.Content = null;
        }

        //Unable all Jog buttons
        private void Jog_Unable()
        {
            viewmodel.Jog_U1_enable = false;
            viewmodel.Jog_U2_enable = false;
            viewmodel.Jog_Up_down_enable = false;
            viewmodel.Jog_X_enable = false;
        }

    }
}
