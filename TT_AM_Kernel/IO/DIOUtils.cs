using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.BDaq;
using System.IO.Ports;
using AM_Kernel.Utils;

namespace AM_Kernel.IO
{
    public enum DIO_DevType
    {
        Device1 = 1,
        Device2 = 2
    }

    class DIOUtility
    {
		public static AdvDAQWrap DIO_Dev_1 = new AdvDAQWrap();
        public static AdvDAQWrap DIO_Dev_2 = new AdvDAQWrap();

        public static bool is_DIODev_1 = false;
        public static bool is_DIODev_2 = false;

        static string g_ErrorMsg = "No available Device.";

        public static string DIO_DeviceInitial(params int[] src)
        {
            /// Every computer define different serial com port number;
            DIO_Dev_1.DeviceNum = src[0];
            DIO_Dev_2.DeviceNum = src[1];

            string Error = "";
            Error = DIO_Dev_1.DO_Initial();

            if (Error != "")
                return "Warning" + " - " + "07" + " - " + Error + " - " + "<IO Dev1 Do Initial>";
            Error = DIO_Dev_1.DI_Initial();
            if (Error != "")
                return "Warning" + " - " + "07" + " - " + Error + " - " + "<IO Dev1 Di Initial>";

            is_DIODev_1 = true;

            Error = DIO_Dev_2.DO_Initial();
            if (Error != "")
                return "Warning" + " - " + "07" + " - " + Error + " - " + "<IO Dev2 Do Initial>";
            Error = DIO_Dev_2.DI_Initial();
            if (Error != "")
                return "Warning" + " - " + "07" + " - " + Error + " - " + "<IO Dev2 Di Initial>";

            is_DIODev_2 = true;

            return "";
        }

        public static string DIORead(DIO_DevType devType, DigitalType DIOType)
        {
            if (devType == DIO_DevType.Device1)
            {
                if (is_DIODev_1)
                {
                    if (DIOType == DigitalType.DO)
                    {
                        string result = "";
                        result = DIO_Dev_1.DO_GetData();
                        if (result != "")
                            return "Warning" + "-" + "07" + "-" + result + "-" + "DIO_Dev" + devType + "Get Do Data";
                    }

                    else if (DIOType == DigitalType.DI)
                    {
                        string result = "";
                        result = DIO_Dev_1.DI_GetData();
                        if (result != "")
                            return "Warning" + "-" + "07" + "-" + result + "-" + "DIO_Dev" + devType + "Get DI Data";
                    }
                }
                else
                    return g_ErrorMsg;
            }

            if(devType == DIO_DevType.Device2)
            {
                if (is_DIODev_2)
                {
                    if (DIOType == DigitalType.DO)
                    {
                        string result = "";
                        result = DIO_Dev_2.DO_GetData();
                        if (result != "")
                            return "Warning" + "-" + "07" + "-" + result + "-" + "DIO_Dev" + devType + "Get Do Data";
                    }
                    else if (DIOType == DigitalType.DI)
                    {
                        string result = "";
                        result = DIO_Dev_2.DI_GetData();
                        if (result != "")
                            return "Warning" + "-" + "07" + "-" + result + "-" + "DIO_Dev" + devType + "Get DI Data";
                    }
                }
                else
                    return g_ErrorMsg;
            }

            return "";

        }

        public static string DOSignalOut(DIO_DevType devType, bool isOn, DIO_Core src)
        {
            /// need refactoring
            string result = string.Empty;
            switch(devType)
            {
                case DIO_DevType.Device1:
                    if (is_DIODev_1)
                    {
                        result = DIO_Dev_1.DO_SignalOut(src.PortNum, src.Bits, isOn);
                        src.IsOn = isOn;
                    }
                    break;

                case DIO_DevType.Device2:
                    if (is_DIODev_2)
                    {
                        result = DIO_Dev_2.DO_SignalOut(src.PortNum, src.Bits, isOn);
                        src.IsOn = isOn;
                    }
                    break;
            }

            if (result != "")
            {
                LogHandler.LogError(LogHandler.Formatting(string.Format("Warning - 07 - {0} - <Do{1}_Port{2}_{3}>"
                , result, devType, src.PortNum, src.Bits), ""));

                return result;
            }

            return "";
        }

        public static string DOSignalIn(DIO_DevType devType, ref bool isOn, DIO_Core src)
        {
            string result = string.Empty;
            bool signal = false;

            switch (devType)
            {
                case DIO_DevType.Device1:
                    if (is_DIODev_1)
                    {
                        result = DIO_Dev_1.DO_SignalRead(src.PortNum, src.Bits, ref signal);
                    }
                    break;

                case DIO_DevType.Device2:
                    if (is_DIODev_1)
                    {
                        result = DIO_Dev_2.DO_SignalRead(src.PortNum, src.Bits, ref signal);
                    }

                    break;
            }
            
            if (result != "")
            {
                LogHandler.LogError(LogHandler.Formatting(string.Format("{0} <Get_Do{1}_Port{2}_{3}>", result, 
                    devType, src.PortNum, src.Bits), ""));
                return result;
            }
            else
            {
                isOn = signal;
                return "";
            }

        }

        public static string DISignalIn(DIO_DevType devType, ref bool isOn, DIO_Core src)
        {
            string result = "";
            bool signal = false;
            switch (devType)
            {
                case DIO_DevType.Device1:
                    if (is_DIODev_1)
                    {
                        result = DIO_Dev_1.DI_SignalRead(src.PortNum, src.Bits, ref signal);
                    }
                    break;

                case DIO_DevType.Device2:
                    if (is_DIODev_1)
                    {
                        result = DIO_Dev_2.DI_SignalRead(src.PortNum, src.Bits, ref signal);
                    }
                    break;
            }

            if (result != "")
            {
                LogHandler.LogError(LogHandler.Formatting(string.Format("{0} <DI{1}_Port{2}_{3}>", result, 
                    devType, src.PortNum, src.Bits), ""));
                return result;
            }
            else
            {
                isOn = signal;
                return "";
            }
        }

    }

    class DO_Set
    {
        public static Dictionary<string, DIO_Core> employees = new DIOFactory().GetProduct(DigitalType.DO);
    }
    
    class DI_Set
    {
        //public static DIO_Core DI01 = new DIO_Core(0, 0, "DI00");
		public static Dictionary<string, DIO_Core> employees = new DIOFactory().GetProduct(DigitalType.DI);
    }


}
