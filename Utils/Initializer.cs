using AM_Kernel.Motor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM_Kernel.IO;
using AM_Kernel.Periphery;
using AM_Kernel.Utils;
using AM_Kernel.LaserCtrl;

namespace HMI_PermanentForm.Utils
{
    public enum Initial_Step
    {
        Step_MotorReadingConfig = 1,
        Step_Motor_Writing = 2,
        Step_DigitalIO = 3,
        Step_HeaterOpen = 4
    }

    public class Initializer
    {
        public Initial_Step StepInfo;

        public Initializer()
        {
            Periphery_Register();
        }

        public string IsMotorConfigReady()
        {
            Recoater.getInstance().ReadConfig("../../../TT_AM_Kernel/Config/Recoater.json");
            BuildPlatform.getInstance().ReadConfig("../../../TT_AM_Kernel/Config/BuildPlatform.json");
            LeftFeeder.getInstance().ReadConfig("../../../TT_AM_Kernel/Config/Left_Feeder.json");
            RightFeeder.getInstance().ReadConfig("../../../TT_AM_Kernel/Config/Right_Feeder.json");
            StepInfo = Initial_Step.Step_MotorReadingConfig;

            string str = string.Empty;

            str = MotorFunc4PCI1240.getInstance().InitialDevice();
            if (str != "")
            {
                LogHandler.LogDebug(LogHandler.Formatting(str, "Motor初始化失敗"));
                return str;
            }
            GlobalCtrlFlag.isMotorTimer_start = true;

            StepInfo = Initial_Step.Step_MotorReadingConfig;

            return "Motor initialization......OK.";
        }

        private void Periphery_Register()
        {
            try
            {
                PeripherySetting.GetInstance().ReadConfig("../../../TT_AM_Kernel/Config/PeripheryConfig.json");
            }
            catch(Exception ex)
            {
                LogHandler.LogException(LogHandler.Formatting(ex.ToString(), ""));
            }
        }

        public string IsDIOReady()
        {
            
            StepInfo = Initial_Step.Step_DigitalIO;
            
            string str = DIOUtility.DIO_DeviceInitial(PeripherySetting.GetInstance().IO_Card[0],
                PeripherySetting.GetInstance().IO_Card[1]);
            if(str != "")
            {
                LogHandler.LogDebug(LogHandler.Formatting(str, "IO初始化失敗"));

                return str;
            }
            GlobalCtrlFlag.isIOTimer_Start = true;

            return "Digital I/O initialization.....OK.";
        }

        public string IsLaserCtrlReady()
        {
            string ErrorCode = ScanSystem.InitialSystem();

            if (ErrorCode != "0")
            {
                LogHandler.LogDebug(LogHandler.Formatting(ErrorCode, "雷射初始化失敗"));
                return "RTC5 isn't initialization...";
            }
            else
            {
                ScanSystem.LoadCorrectionFile(ScanSystem.correctionFilePath);
                return "RTC5 initialize.....OK";
            }
                
        }

        public string WritingMotor()
        {
            StepInfo = Initial_Step.Step_Motor_Writing;
            string str = string.Empty;

            str = _writeMotorWholeParam(Recoater.getInstance());
            
            str = _writeMotorWholeParam(BuildPlatform.getInstance());
           
            str = _writeMotorWholeParam(LeftFeeder.getInstance());

            str = _writeMotorWholeParam(RightFeeder.getInstance());

            if (str != "")
            {
                LogHandler.LogDebug(LogHandler.Formatting(str, "馬達寫入失敗"));
                return str;
            }
            return "Writing parameters into Motor initialization.....OK.";
            
        }

        private string _writeMotorWholeParam(MotorBase src)
        {
            string str = string.Empty;

            MotorPara tmp = (MotorPara)src.GetConfig();
            str = MotorFunc4PCI1240.getInstance().SetParameter_CFG_Motion(src.AxisHandle, tmp.paraMotion);
            if (str != "")
                return str;
            str = MotorFunc4PCI1240.getInstance().SetParameter_Hardware(src.AxisHandle, tmp.paraHardware);
            if (str != "")
                return str;
            str = MotorFunc4PCI1240.getInstance().SetParameter_Motion(src.AxisHandle, tmp.paraMotion);
            if (str != "")
                return str;
            str = MotorFunc4PCI1240.getInstance().SetParameter_HomeSearch(src.AxisHandle, tmp.paraHome);
            if (str != "")
                return str;

            return "";
        }
    }
}
