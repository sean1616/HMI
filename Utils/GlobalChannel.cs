using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMI_PermanentForm.G_folder.G10.ViewModel;
using AM_Kernel.Periphery;
using AM_Kernel.Motor;
using HMI_PermanentForm.WebCam;
using HMI_PermanentForm.A_folder.A4.ViewModel;

namespace HMI_PermanentForm.Utils
{
    public class GlobalChannel
    {
        #region ViewModel channel
        public static SlicingViewModel SlicingVM = new SlicingViewModel();
        public static View3D view3D = new View3D();
        //public static View3D A_view3D = new View3D();
        //public static View3D B_view3D = new View3D();
        //public static View3D E_view3D = new View3D();
        public static CamViewModel ImageVM = new CamViewModel();
        public static View_Cam CameraStream = new View_Cam();
        public static Data_A1_2 TranlateVM = new Data_A1_2();
        public static ErrorMsgVM errorMsgVM = new ErrorMsgVM();
        public static EnviromentStatus envriomentModel = new EnviromentStatus();
        public static A4_1_ViewModel StatusCheckVM = new A4_1_ViewModel()
        {
            IsAxisCheckImgSrc = "../Img/A4_1_2_warning.png",
            IsPowderSupplyCheckImgSrc = "../Img/A4_1_3_warning.png",
            IsHeaterCheckImgSrc = "../Img/A4_1_4_warning.png"
        };

        #endregion


        public static double BuildPlatform_UpperLimit = 60.0;     
        public static double RecoaterCordon_Left = 20.0;
        public static double RecoaterCordon_Right = 260.0;
 
        public static COMStore ADAM4117_Provider = new COMStore(new ADAM4117Factory(PeripherySetting.GetInstance().COM_Port[0]));
        public static COMStore Heater_Provider = new COMStore(new HeaterFactory(PeripherySetting.GetInstance().COM_Port[1]));
        public static AM_Kernel.Procedure.AM_CycleWorker AM_Processor = new AM_Kernel.Procedure.AM_CycleWorker();

        public static void CheckSetting()
        {
            if (GlobalCtrlFlag.isAxisStatusCheck)
                GlobalChannel.StatusCheckVM.IsAxisCheckImgSrc = "../Img/A4_1_2_normal.png";

            if (GlobalCtrlFlag.isPowderSupplyStatusCheck)
                GlobalChannel.StatusCheckVM.IsPowderSupplyCheckImgSrc = "../Img/A4_1_3_normal.png";

            if (GlobalCtrlFlag.isHeaterStatusCheck)
                GlobalChannel.StatusCheckVM.IsHeaterCheckImgSrc = "../Img/A4_1_4_normal.png";
        }

    }

    public class GlobalCtrlFlag
    {
        public static bool isIOTimer_Start = false;
        public static bool isMotorTimer_start = false;
        public static bool isBuildPlatformBeyondLevel = false;
        public static bool isRecoaterInScope = false;
        public static bool isAutoProcessing = false;
        public static bool isAxisStatusCheck = false;
        public static bool isPowderSupplyStatusCheck = false;
        public static bool isHeaterStatusCheck = false;
        public static bool isHeaterRunning = false;
    }
}
