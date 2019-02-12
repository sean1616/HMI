using AM_Kernel.Interpretation;
using AM_Kernel.Utils;
using HMI_PermanentForm.Utils;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Newtonsoft.Json;
using LinearAlgebraAPI;
using System.Collections.Generic;
using AM_Kernel.LaserCtrl;

namespace HMI_PermanentForm
{
    /// <summary>
    /// Dict_Loading_Page.xaml 的互動邏輯
    /// </summary>
    public partial class B : Page
    {
        public SlicingViewModel SlicingVM { get; set; }               

        public B()
        {
            InitializeComponent();

            /// Initialize
            SlicingVM = GlobalChannel.SlicingVM;

            B1_2 Page_B1_2 = new B1_2();

            B_View3D.NavigationService.Navigate(GlobalChannel.view3D);
            B_View3D.Refresh();
            Frame_B1_2.NavigationService.Navigate(Page_B1_2);

            DataContext = this;
                       
        }
               
        private void HatchObj_SettingData(object sender, DataObjectSettingDataEventArgs e)
        {

        }

        private async void B1_btn_Checked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            Stopwatch watcher = new Stopwatch();
            Loading_B LdWin = new Loading_B();
            watcher.Reset();

            dlg.Filter = "切層檔(*.aff;*.iff)|*.aff;*.iff|材料搜尋圖檔(*.json*)|*.json*";

            if (dlg.ShowDialog() == true)
            {
                string fileName = dlg.FileName;

                if (fileName.EndsWith("aff") || fileName.EndsWith("iff") || fileName.EndsWith("tsf"))
                {
                    

                    LdWin.Show(); //Show loading window

                    await Task.Run(() =>
                    {
                        watcher.Start(); //Timing begin
                        SlicingObj.readAff(fileName);
                        watcher.Stop();
                    });

                    LdWin.Close(); //Close loading window            

                    MessageBox.Show(string.Format("Time consuming: {0}", watcher.ElapsedMilliseconds / 1000.0));
                    ////NumOfLayers = SlicingObj.getLayers().Count;
                    SlicingVM.Hatch = new Point3DCollection(SlicingObj.getLayers().getLayers()[0].getHatch());
                    ScanSystem.PreloadPenLibrary(SlicingObj.getLayers().getPenLibrary());
                    GlobalChannel.AM_Processor.CurrentScanLayerIndex = 0;


                }
                else if (fileName.EndsWith("json"))
                {
                    List<LineSegment> paths = JsonConvert.DeserializeObject<List<LineSegment>>(System.IO.File.ReadAllText(fileName));

                    Point3DCollection pt3dCollection = new Point3DCollection();

                    foreach (LineSegment seg in paths)
                    {
                        pt3dCollection.Add(new Point3D(seg.startPt.X, seg.startPt.Y, 0.0));
                        pt3dCollection.Add(new Point3D(seg.endPt.X, seg.endPt.Y, 0.0));
                    }

                    SlicingVM.Hatch = pt3dCollection;
                }


                //Matrix3D mat = new Matrix3D();
                //mat.Translate(new Vector3D(125.0, -125.0, 0));
                //mat.Scale(new Vector3D(1, -1, 1));

                #region output contour
                //Task.Factory.StartNew(() =>
                //{
                //    StringBuilder sb = new StringBuilder();
                //    for(int i = 0; i != 1; i++)
                //    {
                //        foreach(Polygon poly in SlicingObj.getLayers()[i].getPolygons().getPolygons())
                //        {
                //            foreach (Point3D pt in poly.getPoints())
                //            {
                //                //Point3D.Multiply(pt, mat);
                //                sb.AppendLine(string.Format("{0},{1}", pt.X, pt.Y));
                //            }

                //            sb.AppendLine("\n");
                //        }
                //        Utility.OuputPolygonData(string.Format("Layer_{0}.txt", i), sb.ToString());
                //    }

                //    sb.Clear();
                //    for (int i = 0; i != 1; i++)
                //    {
                //        foreach (Polygon poly in SlicingObj.getLayers()[i].getPolygons().getPolygons())
                //        {
                //            foreach (Point3D pt in poly.getPoints())
                //            {
                //                Point3D tmp = Point3D.Multiply(pt, mat);
                //                sb.AppendLine(string.Format("{0},{1}", tmp.X, tmp.Y));
                //            }

                //            sb.AppendLine("\n");
                //        }
                //        Utility.OuputPolygonData(string.Format("Layer_{0}_transform.txt", i), sb.ToString());
                //    }

                //});
                #endregion

                //Rebuild page to reorganize slider
                B1_2 Page_B1_2 = new B1_2();
                Frame_B1_2.NavigationService.Navigate(Page_B1_2);
            }

            else
            {
                return;
            }
        }

        private void B2_btn_Checked(object sender, RoutedEventArgs e)
        {
            if (SlicingObj.getLayers() == null && SlicingVM.Hatch == null)
            {
                return;
            }
            else
            {
                SlicingObj.Dispose();
                GlobalChannel.SlicingVM.NumOfLayers = 1;
                GlobalChannel.SlicingVM.Hatch = new Point3DCollection();
                E.lastLayerIndex = 0;
                //Polygon = new Point3DCollection();

            }
        }

        private void B3_btn_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void B4_btn_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void B5_btn_Click(object sender, RoutedEventArgs e)
        {
            Vector3D origin_vector=new Vector3D(0, 134, -335);
            Point3D origin_point = new Point3D(0, -148, 330);
            Vector3D origin_updirection = new Vector3D(0, 0, 1);
            
            GlobalChannel.view3D.ViewPort.Camera.LookDirection = origin_vector;
            GlobalChannel.view3D.ViewPort.Camera.Position = origin_point;
            GlobalChannel.view3D.ViewPort.Camera.UpDirection = origin_updirection;
        }

        private void B6_btn_Checked(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
