using System;
using System.Collections.Generic;
using System.Text;

using Automation.BDaq;
using AM_Kernel.Utils;

namespace AM_Kernel.IO
{
    class AdvDAQWrap
    {
        public int DeviceNum = 0;
        public InstantDoCtrl Do = new InstantDoCtrl();
        public InstantDiCtrl Di = new InstantDiCtrl();

        // 讀取IO卡錯誤資訊
        public string DIO_ErrorHandle(ErrorCode Err)
        {
            if (Err != ErrorCode.Success)
                return "DIO Error Happen. Error Code: " + Err.ToString();
            else
                return "";
        }

        // 初始化DO
        // DOCard：DO元件
        // DeviceNum：IOCard 編號
        public string DO_Initial()
        {
            try
            {
                Do.SelectedDevice = new DeviceInformation(DeviceNum);
            }
            catch(Exception e)
            {
                LogHandler.LogException(LogHandler.Formatting(e.ToString(), ""));
            }
                
            if (!Do.Initialized)
                return "No DO device be selected or DO device open fail";
            else
                return "";
        }

        // DO點位輸出
        // PortNum：指定輸出的port
        // bits：指定輸出點位
        // Is_ON： true => 輸出 , false => 關閉
        public string DO_SignalOut(int PortNum, int bits, bool Is_ON)
        {
            byte DoData;
            ErrorCode Err = ErrorCode.Success;
            Err = Do.Read(PortNum, out DoData);
            if (Err != ErrorCode.Success)
                return Err.ToString();

            int tempVal = 0;
            for (int i = 0; i < 8; i++)
            {
                if (Convert.ToBoolean(DoData & (1 << i)))
                    tempVal = tempVal | (1 << i);
            }

            if (Is_ON)
            {
                if (!((tempVal & (1 << bits)) == (1 << bits)))
                    tempVal = tempVal + (1 << bits);
            }
            else
            {
                if ((tempVal & (1 << bits)) == (1 << bits))
                    tempVal = tempVal - (1 << bits);
            }
            Err = Do.Write(PortNum, (byte)tempVal);
            if (Err != ErrorCode.Success)
                return Err.ToString();

            return "";
        }

        //讀取DO狀態
        // PortNum：指定讀取的port
        // bits：指定讀取點位
        // Is_ON： true => 輸出 , false => 關閉
        // Val：讀取到的bit數值
        byte[] DoData = new byte[4];

        public string DO_GetData()
        {
            ErrorCode Err = ErrorCode.Success;
            // Port 0
            Err = Do.Read(0, out DoData[0]);
            if (Err != ErrorCode.Success)
                return Err.ToString() + "Port 0";
            // Port 1
            Err = Do.Read(1, out DoData[1]);
            if (Err != ErrorCode.Success)
                return Err.ToString() + "Port 1";
            // Port 2
            Err = Do.Read(2, out DoData[2]);
            if (Err != ErrorCode.Success)
                return Err.ToString() + "Port 2";
            // Port 3
            Err = Do.Read(3, out DoData[3]);
            if (Err != ErrorCode.Success)
                return Err.ToString() + "Port 3";

            return "";
        }

        public string DO_SignalRead(int PortNum, int bits, ref bool Is_ON, ref int Val)
        {
            Val = (int)DoData[PortNum];
            if (Convert.ToBoolean(DoData[PortNum] & (1 << bits)))
                Is_ON = true;
            else
                Is_ON = false;
            
            return "";
        }

        public string DO_SignalRead(int PortNum, int bits, ref bool Is_ON)
        {
            if (Convert.ToBoolean(DoData[PortNum] & (1 << bits)))
                Is_ON = true;
            else
                Is_ON = false;
            
            return "";
        }

        // 初始化DI
        // DOCard：DI元件
        // DeviceNum：IOCard 編號
        public string DI_Initial()
        {
            try
            {
                Di.SelectedDevice = new DeviceInformation(DeviceNum);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            if (!Di.Initialized)
                return "No DI device be selected or DI device open fail";
            else
                return "";
        }

        //讀取DI狀態
        // PortNum：指定讀取的port
        // bits：指定讀取點位
        // Is_ON： true => 輸出 , false => 關閉
        byte[] DiData = new byte[4];

        public string DI_GetData()
        {
            ErrorCode Err = ErrorCode.Success;
            // Port 0
            Err = Di.Read(0, out DiData[0]);
            if (Err != ErrorCode.Success)
                return Err.ToString() + "Port 0";
            // Port 1
            Err = Di.Read(1, out DiData[1]);
            if (Err != ErrorCode.Success)
                return Err.ToString() + "Port 1";
            // Port 2
            Err = Di.Read(2, out DiData[2]);
            if (Err != ErrorCode.Success)
                return Err.ToString() + "Port 2";
            // Port 3
            Err = Di.Read(3, out DiData[3]);
            if (Err != ErrorCode.Success)
                return Err.ToString() + "Port 3"; ;

            return "";
        }

        public string DI_SignalRead(int PortNum, int bits, ref bool Is_ON)
        {
            if (Convert.ToBoolean((DiData[PortNum] >> bits) & 0x1))
                Is_ON = true;
            else
                Is_ON = false;
            
            return "";
        }
    }

    class ConstVal
    {
        public const int StartPort = 0;
        public const int PortCountShow = 4;
    }
}
