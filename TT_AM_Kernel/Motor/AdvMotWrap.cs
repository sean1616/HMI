using System;
using System.Collections.Generic;
using System.Text;

using Advantech.Motion;

namespace AM_Kernel.Motor
{
    /// <summary>
    /// 將PCI-1240運動控制卡所提供的方法另外包成一個"AdvMotWrap"類別
    /// </summary>
	class AdvMotWrap : MotorFuncSupplier
    {
        public AdvMotWrap()
        {

        }

		uint _Result;
		bool _is_HardwareConnected;

		public bool is_HardwareConnected
		{
			get
			{
				return _is_HardwareConnected;
			}

			set
			{
				_is_HardwareConnected = value;
			}
		}

		public uint Result
		{
			get
			{
				return _Result;
			}

			set
			{
				_Result = value;
			}
		}

		//uint Result = 0;           ///< 儲存錯誤訊息

		/// <summary>
		/// 取得裝置的資訊
		/// </summary>
		/// <param name="List">裝置清單(方法執行完畢，取得IPC上連接到的所有裝置)</param>
		/// <param name="Count">裝置數量(方法執行完畢，取得IPC上連接到的裝置數量)</param>
		/// <returns>錯誤訊息</returns>
		public string GetAvailableDevs(DEV_LIST[] List, ref uint Count)
        {
            /// Motion.MAX_DEVICES預設為100，表示最大有100個裝置連接
            Result = (uint)Motion.mAcm_GetAvailableDevs(List, Motion.MAX_DEVICES, ref Count);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得錯誤資訊
        /// </summary>
        /// <param name="ErrorCode">研華API回傳的Error Code</param>
        /// <returns>錯誤資訊</returns>
        public string GetErrorMsg(uint ErrorCode)
        {
            StringBuilder Msg = new StringBuilder();
            bool Result;
            
            /// Msg 回傳Error code所對應的錯誤資訊; 500表示錯誤資訊的回傳字串最大長度
            Result = Motion.mAcm_GetErrorMessage(ErrorCode, Msg, 500);
            if (Result)
                return Msg.ToString();
            else
                return "Can NOT get ErrorMsg";
        }

        /// <summary>
        /// Open device
        /// </summary>
        /// <param name="DevNum">裝置的編號</param>
        /// <param name="DevHandle">裝置的控點</param>
        /// <returns>錯誤訊息</returns>
        public string OpenDevice(uint DevNum, ref IntPtr DevHandle)
        {
            Result = Motion.mAcm_DevOpen(DevNum, ref DevHandle);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// Close device
        /// </summary>
        /// <param name="DevHandle">裝置的控點</param>
        /// <returns>錯誤訊息</returns>
        public string CloseDevice(IntPtr DevHandle)
        {
            Result = Motion.mAcm_DevClose(ref DevHandle);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// Open/Close axis
        /// </summary>
        /// <param name="DevHandle">裝置(軸卡?)控點</param>
        /// <param name="Axis">物理上運動軸編號(ex: PCI-1240: 0, 1, 2, 3)</param>
        /// <param name="AxisHandle">欲取得的運動軸控點</param>
        /// <returns>錯誤訊息</returns>
        public string OpenAxis(IntPtr DevHandle, ushort Axis, ref IntPtr AxisHandle)
        {
            Result = Motion.mAcm_AxOpen(DevHandle, Axis, ref AxisHandle);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 關閉運動軸
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <returns>錯誤訊息</returns>
        public string CloseAxis(IntPtr AxisHandle)
        {
            Result = (uint)Motion.mAcm_AxClose(ref AxisHandle);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 設定Pulse OUT Property
        /// Pulse OUT Mode:
        ///         Type 1: OUT/DIR
        ///         Type 2: OUT/DIR, OUT negative logic
        ///         Type 3: OUT/DIR, DIR negative logic
        ///         Type 4: OUT/DIR, OUT&DIR negative logic
        ///         Type 5: CW/CCW
        ///         Type 6: CW/CCW, CW&CCW negative logic
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Pulse_OUT_Type">欲設定之Pulse Out模式</param>
        /// <returns>錯誤訊息</returns>
        public string SetPulseOut(IntPtr AxisHandle, int Pulse_OUT_Type)
        {
            int SettingValue = 0;
            switch (Pulse_OUT_Type)
            {
                case 1:
                    SettingValue = 1;            ///< type 1
                    break;
                case 2:
                    SettingValue = 2;            ///< type 2
                    break;
                case 3:
                    SettingValue = 4;            ///< type 3
                    break;
                case 4:
                    SettingValue = 8;            ///< type 4
                    break;
                case 5:
                    SettingValue = 16;           ///< type 5
                    break;
                case 6:
                    SettingValue = 32;           ///< type 6
                    break;
            }

            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxPulseOutMode, ref SettingValue, 4);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得Pulse OUT Property
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Pulse_OUT_Type">欲取得之Pulse Out模式</param>
        /// <returns>錯誤訊息</returns>
        public string GetPulseOut(IntPtr AxisHandle, ref int Pulse_OUT_Type)
        {
            uint GettingValue = 0;
            uint bufferlength = 4;

            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxPulseOutMode, ref GettingValue, ref bufferlength);

            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                switch (GettingValue)
                {
                    case 1:
                        Pulse_OUT_Type = 1;
                        break;
                    case 2:
                        Pulse_OUT_Type = 2;
                        break;
                    case 4:
                        Pulse_OUT_Type = 3;
                        break;
                    case 8:
                        Pulse_OUT_Type = 4;
                        break;
                    case 16:
                        Pulse_OUT_Type = 5;
                        break;
                    case 32:
                        Pulse_OUT_Type = 6;
                        break;
                }
                return "";
            }
        }

        /// <summary>
        /// 設定Pulse IN Property
        /// Pulse IN Mode:
        ///         Type 1: 1X A/B
        ///         Type 2: 2X A/B
        ///         Type 3: 4X A/B
        ///         Type 4: CW/CCW
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Pulse_IN_Type">欲設定的Pulse In 模式</param>
        /// <returns>錯誤訊息</returns>
        public string SetPulseIn(IntPtr AxisHandle, int Pulse_IN_Type)
        {
            int SettingValue = 0;
            switch (Pulse_IN_Type)
            {
                case 1:
                    SettingValue = 0;
                    break;
                case 2:
                    SettingValue = 1;
                    break;
                case 3:
                    SettingValue = 2;
                    break;
                case 4:
                    SettingValue = 3;
                    break;
            }
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxPulseInMode, ref SettingValue, 4);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得Pulse IN Property
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Pulse_IN_Type">欲取得的Pulse In 模式</param>
        /// <returns>錯誤訊息</returns>
        public string GetPulseIn(IntPtr AxisHandle, ref int Pulse_IN_Type)
        {
            uint GettingValue = 0;
            uint bufferlength = 4;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxPulseInMode, ref GettingValue, ref bufferlength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                switch (GettingValue)
                {
                    case 0:
                        Pulse_IN_Type = 1;
                        break;
                    case 1:
                        Pulse_IN_Type = 2;
                        break;
                    case 2:
                        Pulse_IN_Type = 3;
                        break;
                    case 3:
                        Pulse_IN_Type = 4;
                        break;
                }
            return "";
        }

        /// <summary>
        /// 設定初始驅動速度
        /// Unit:PPU/s
        ///     Max:CFG_AxMaxVel/8000
        ///     Min:CFG_AxMaxVel
        ///     Default:2000
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="IniSpeed">欲設定得初始速度</param>
        /// <returns>錯誤訊息</returns>
        public string SetInitialSpeed(IntPtr AxisHandle, double IniSpeed)
        {
			uint BufferLength = 8;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.PAR_AxVelLow, ref IniSpeed, BufferLength);
            //double temp = IniSpeed;
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得初始驅動速度
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="IniSpeed">欲取得之初始速度</param>
        /// <returns>錯誤訊息</returns>
        public string GetInitialSpeed(IntPtr AxisHandle, ref double IniSpeed)
        {
            uint BufferLength = 8;
            double GettingValue = 0;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.PAR_AxVelLow, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                IniSpeed = GettingValue;
            return "";
        }

        /// <summary>
        /// 設定馬達驅動速度
        /// Unit:PPU/s
        ///     smaller than CFG_AxMaxVel
        ///     Default: 8000
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="DrivingSpeed">欲設定之驅動速度</param>
        /// <returns>錯誤訊息</returns>
        public string SetDrivingSpeed(IntPtr AxisHandle, double DrivingSpeed)
        {
            uint BufferLength = 8;   ///< double的位元大小
            //double temp;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.PAR_AxVelHigh, ref DrivingSpeed, BufferLength);
            //temp = DrivingSpeed;
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得馬達驅動速度
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="DrivingSpeed">欲取得的驅動速度</param>
        /// <returns>錯誤訊息</returns>
        public string GetDrivingSpeed(IntPtr AxisHandle, ref double DrivingSpeed)
        {
            uint BufferLength = 8;
            double GettingValue = 0;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.PAR_AxVelHigh, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                DrivingSpeed = GettingValue;
            return "";
        }

        /// <summary>
        /// 設定馬達加速度
        /// Unit:PPU/s^2
        ///     smaller than or equal to CFG_AxMaxAcc
        ///     Min: CFG_AxMaxVel/64
        ///     Default:10000
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Acc">欲設定的加速度</param>
        /// <returns>錯誤訊息</returns>
        public string SetAcceleration(IntPtr AxisHandle, double Acc)
        {
            uint BufferLength = 8;
            //double temp;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.PAR_AxAcc, ref Acc, BufferLength);
            //temp = Acc;
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得馬達加速度
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Acc">欲取得的加速度</param>
        /// <returns>錯誤訊息</returns>
        public string GetAcceleration(IntPtr AxisHandle, ref double Acc)
        {
            uint BufferLength = 8;
            double GettingValue = 0;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.PAR_AxAcc, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                Acc = GettingValue;
            return "";
        }

        /// <summary>
        /// 設定馬達減速度
        /// Unit:PPU/s^2
        ///      smaller than or equal to CFG_AxMaxDec
        ///      Min: CFG_AxMaxVel/64
        ///      Default:10000
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Dec">欲設定之減速度</param>
        /// <returns>錯誤訊息</returns>
        public string SetDeceleration(IntPtr AxisHandle, double Dec)
        {
            uint BufferLength = 8;
            //double temp;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.PAR_AxDec, ref Dec, BufferLength);
            //temp = Dec;
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得馬達減速度
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Dec">欲取得之減速度</param>
        /// <returns>錯誤訊息</returns>
        public string GetDeceleration(IntPtr AxisHandle, ref double Dec)
        {
            uint BufferLength = 8;
            double GettingValue = 0;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.PAR_AxDec, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                Dec = GettingValue;
            return "";
        }

        /// <summary>
        /// 設定T/S曲線
        /// 0      : T curve(Default)
        /// 1(!=0) : S curve
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="CurveType">欲設定的曲線類型</param>
        /// <returns>錯誤訊息</returns>
        public string SetCurve(IntPtr AxisHandle, int CurveType)
        {
            uint SettingValue = 0;
            uint BufferLength = 8;
            switch (CurveType)
            {
                case 1:
                    SettingValue = 0;     ///< T curve
                    break;
                case 2:
                    SettingValue = 1;     ///< S curve
                    break;
            }

            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.PAR_AxJerk, ref SettingValue, BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得T/S曲線
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="CurveType">欲取得的曲線類型</param>
        /// <returns>錯誤訊息</returns>
        public string GetCurve(IntPtr AxisHandle, ref int CurveType)
        {
            int GettingValue = 0;
            uint BufferLength = 8;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.PAR_AxJerk, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                switch (GettingValue)
                {
                    case 0:
                        CurveType = 1;
                        break;
                    case 1:
                        CurveType = 2;
                        break;
                }
                return "";
            }
        }

        /// <summary>
        /// 設定硬體極限訊號(Ax_ElLogic)的旗標
        ///     Type 0: Low Active
        ///     Type 1: High Active
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ActiveType">判斷ON/OFF旗標</param>
        /// <returns>錯誤資訊</returns>
        public string SetLimit(IntPtr AxisHandle, int ActiveType)
        {
            int SettingValue = ActiveType;

            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxElLogic, ref SettingValue, 4);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得硬體極限訊號的旗標
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ActiveType">判斷ON/OFF旗標</param>
        /// <returns>錯誤資訊</returns>
        public string GetLimit(IntPtr AxisHandle, ref int ActiveType)
        {
            int GettingValue = 0;
            uint BufferLength = 4;

            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxElLogic, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                ActiveType = (int)GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 設定硬體極限訊號(ELReact)的反應模式
        ///     Type 1: Immediately stop
        ///     Type 0: Deceleration stop
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ReactingType">反應類型</param>
        /// <returns>錯誤資訊</returns>
        public string SetReactingMode(IntPtr AxisHandle, int ReactingType)
        {
            int SettingValue = 0;
            switch (ReactingType)
            {
                case 1:
                    SettingValue = 0;
                    break;
                case 2:
                    SettingValue = 1;
                    break;
            }
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxElReact, ref SettingValue, 4);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得硬體極限訊號的反應模式
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ReactingType">反應類型</param>
        /// <returns>錯誤資訊</returns>
        public string GetReactingMode(IntPtr AxisHandle, ref int ReactingType)
        {
            uint BufferLength = 4;
            int GettingValue = 0;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxElReact, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                switch (GettingValue)
                {
                    case 0:
                        ReactingType = 1;
                        break;
                    case 1:
                        ReactingType = 2;
                        break;
                }
                return "";
            }
        }

        /// <summary>
        /// 設定DO function啟動與否
        ///     Type 0:Disabled
        ///     Type 1:Enabled (Default)
        /// If property CFG_AxGenDoEnable is enabled, the OUT4~OUT7 will be used as general output. 
        /// The property CFG_AxCmpEnable is disabled automatically. 
        /// Functions Acm_AxSetCmpData, Acm_AxSetCmpTable, Acm_AxSetCmpAuto will not be able to output signal.
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ActiveType">判斷啟動與否旗標</param>
        /// <returns>錯誤資訊</returns>
        public string SetDOEnable(IntPtr AxisHandle, int ActiveType)
        {
            uint SettingValue = (uint)ActiveType;

            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxGenDoEnable, ref SettingValue, 4);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 設定伺服馬達伺服ON/OFF
        ///         Active = 1 ---> ServeON
        ///         Active = 0 ---> ServoOFF
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Active">ON/OFF控制旗標</param>
        /// <returns>錯誤訊息</returns>
        public string SetServoOn(IntPtr AxisHandle, int Active)
        {
            uint SettingValue = (uint)Active;

            Result = Motion.mAcm_AxSetSvOn(AxisHandle, SettingValue);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得Servo Ready的訊號
        ///         Active = 0 ---> not active
        ///         Active = 1 ---> active
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Active">欲取得的Servo ready旗標</param>
        /// <returns>錯誤訊息</returns>
        public string GetServoRDY(IntPtr AxisHandle, ref int Active)
        {
            byte BitData = 0;  ///< 儲存欲回傳的DI值(true or false)

            /// 取得第2腳位的DI值
            Result = Motion.mAcm_AxDiGetBit(AxisHandle, 2, ref BitData);
            if (Result != (uint)ErrorCode.SUCCESS)
                return "";
            else
            {
                int temp = 0;
                temp = (int)BitData;

                /// DI input is open.
                if (temp == 1)
                    Active = 0;

                /// DI input is connected to ground.
                else if (temp == 0)
                    Active = 1;

                return "";
            }
        }

        /// <summary>
        /// 設定INP function Enable   (in position)
        ///     Type 0: Disable (Default)
        ///     Type 1: Enable
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="EnableType">判斷啟動旗標</param>
        /// <returns>錯誤訊息</returns>
        public string SetINPEnable(IntPtr AxisHandle, int EnableType)
        {
            int SettingValue = EnableType;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxInpEnable, ref SettingValue, 4);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得INP function啟動與否
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="EnableType">判斷啟動旗標</param>
        /// <returns>錯誤訊息</returns>
        public string GetINPEnable(IntPtr AxisHandle, ref int EnableType)
        {
            uint BufferLength = 4;
            int GettingValue = 0;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxInpEnable, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                EnableType = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 設定INP訊號啟動邏輯
        ///     Type 0: Low active
        ///     Type 1: High active
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ActiveType">判斷啟動旗標</param>
        /// <returns>錯誤訊息</returns>
        public string SetINPLogic(IntPtr AxisHandle, int ActiveType)
        {
            int SettingValue = ActiveType;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxInpLogic, ref SettingValue, 4);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得INP訊號啟動邏輯
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ActiveType">判斷啟動旗標</param>
        /// <returns>錯誤訊息</returns>
        public string GetINPLogic(IntPtr AxisHandle, ref int ActiveType)
        {
            uint BufferLength = 4;
            int GettingValue = 0;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxInpLogic, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                ActiveType = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 設定Motion alarm function enable/disable
        ///     Type 0: Disable
        ///     Type 1: Enable
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="EnableType">判斷啟動旗標</param>
        /// <returns>錯誤訊息</returns>
        public string SetAlarmEnable(IntPtr AxisHandle, int EnableType)
        {
            int SettingValue = EnableType;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxAlmEnable, ref SettingValue, 4);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得Motion alarm function enable/disable
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="EnableType">判斷啟動旗標</param>
        /// <returns>錯誤訊息</returns>
        public string GetAlarmEnable(IntPtr AxisHandle, ref int EnableType)
        {
            uint BufferLength = 4;
            int GettingValue = 0;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxAlmEnable, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                EnableType = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 設定Alarm 訊號啟動邏輯
        ///     Type 0: Low active
        ///     Type 1: High active
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ActiveType">判斷啟動旗標</param>
        /// <returns>錯誤訊息</returns>
        public string SetAlarmLogic(IntPtr AxisHandle, int ActiveType)
        {
            int SettingValue = ActiveType;

            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxAlmLogic, ref SettingValue, 4);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得Alarm訊號啟動邏輯
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ActiveType">判斷啟動旗標</param>
        /// <returns>錯誤訊息</returns>
        public string GetAlarmLogic(IntPtr AxisHandle, ref int ActiveType)
        {
            uint BufferLength = 4;
            int GettingValue = 0;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxAlmLogic, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                ActiveType = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 取得極限狀態
        ///     Value = 1 ---> Active
        ///     Value = 0 ---> not active
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="PLimit">運動軸正極限</param>
        /// <param name="NLimit">運動軸負極限</param>
        /// <returns>錯誤訊息</returns>
        public string GetLimitStatus(IntPtr AxisHandle, ref int PLimit, ref int NLimit)
        {
            uint Status = 0;      ///< 初始化Limit狀態為0
            uint temp;
            
            /// 取得運動軸的狀態
            Result = Motion.mAcm_AxGetMotionIO(AxisHandle, ref Status);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                /// temp = staus與0x04作AND運算
                temp = Status & 0x04;
                if (temp == 0x04)
                    PLimit = 1;
                else
                    PLimit = 0;
                temp = Status & 0x08;
                if (temp == 0x08)
                    NLimit = 1;
                else
                    NLimit = 0;

                return "";
            }
        }

        /// <summary>
        /// Get Home status
        ///     Value = 1 ---> Active
        ///     Value = 0 ---> Not active
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="HomeLimit">Home極限</param>
        /// <returns>錯誤訊息</returns>
        public string GetHomeStatus(IntPtr AxisHandle, ref int HomeLimit)
        {
            uint Status = 0;
            uint temp;
            Result = Motion.mAcm_AxGetMotionIO(AxisHandle, ref Status);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                temp = Status & (1 << 4);
                if (temp == (1 << 4))
                    HomeLimit = 1;
                else
                    HomeLimit = 0;

                return "";
            }
        }

        /// <summary>
        /// 取得INP狀態
        ///     Value = 1 ---> Actibe
        ///     Value = 0 ---> not active
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Is_INP">判斷INP ON/OFF 旗標</param>
        /// <returns>錯誤訊息</returns>
        public string GetINPStatus(IntPtr AxisHandle, ref int Is_INP)
        {
            uint Status = 0;
            uint temp = 0;
            Result = Motion.mAcm_AxGetMotionIO(AxisHandle, ref Status);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                temp = Status & 0x2000;
                if (temp == 0x2000)
                    Is_INP = 1;
                else
                    Is_INP = 0;

                return "";
            }
        }

        /// <summary>
        /// 取得Alarm狀態
        ///     Value = 1 ---> Active
        ///     Value = 0 ---> not active
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Is_Alarm">判斷Alarm ON/OFF旗標</param>
        /// <returns>錯誤資訊</returns>
        public string GetAlarmStatus(IntPtr AxisHandle, ref int Is_Alarm)
        {
            uint Status = 0;
            uint temp = 0;
            Result = Motion.mAcm_AxGetMotionIO(AxisHandle, ref Status);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                temp = Status & 0x02;
                if (temp == 0x02)
                    Is_Alarm = 1;
                else
                    Is_Alarm = 0;

                return "";
            }
        }

        /// <summary>
        /// 取得EMC狀態
        ///     Value = 1 ---> Active
        ///     Value = 0 ---> Not active
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Is_EMC">判斷EMC ON/OFF旗標</param>
        /// <returns>錯誤資訊</returns>
        public string GetEMCStatus(IntPtr AxisHandle, ref int Is_EMC)
        {
            uint Status = 0;
            uint temp = 0;
            Result = Motion.mAcm_AxGetMotionIO(AxisHandle, ref Status);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                temp = Status & 0x40;
                if (temp == 0x40)
                    Is_EMC = 1;
                else
                    Is_EMC = 0;

                return "";
            }
        }

        /// <summary>
        /// 取得驅動狀態
        ///     Value = 1 ---> Is Driving
        ///     Value = 0 ---> Stop
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Is_Driving">判動Driving旗標</param>
        /// <returns>錯誤資訊</returns>
        public string GetDrivingState(IntPtr AxisHandle, ref int Is_Driving)
        {
            uint Status = 0;
            uint temp_bit8 = 0, temp_bit9 = 0, temp_bit10 = 0;
            Result = Motion.mAcm_AxGetMotionStatus(AxisHandle, ref Status);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                temp_bit8 = Status & 0x100;  // Accelerating
                temp_bit9 = Status & 0x200;  // Feeding in MAX Speed
                temp_bit10 = Status & 0x400; // Decelerating

                if (temp_bit8 == 0x100 | temp_bit9 == 0x200 | temp_bit10 == 0x400)
                    Is_Driving = 1;
                else
                    Is_Driving = 0;

                return "";
            }

        }

        /// <summary>
        /// 取得驅動狀態
        /// 增加AxGetMotionStatus方法取得的Status值的可讀性
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="state">目前馬達狀態(i.e. 停止、加速...)</param>
        /// <param name="Status_num">AxGetMotionStatus方法取得之Status的值</param>
        /// <returns>錯誤訊息</returns>
        public string GetDrivingState(IntPtr AxisHandle, ref string state, ref uint Status_num)
        {
            uint Status = 0;
            uint temp_bit8 = 0, temp_bit9 = 0, temp_bit10 = 0, temp_bit0 = 0;
            Result = Motion.mAcm_AxGetMotionStatus(AxisHandle, ref Status);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                temp_bit0 = Status & 0x01;   // Stop
                temp_bit8 = Status & 0x100;  // Accelerating
                temp_bit9 = Status & 0x200;  // Feeding in MAX Speed
                temp_bit10 = Status & 0x400; // Decelerating

                if (temp_bit0 == 0x01)
                    state = "Stop";
                else if (temp_bit8 == 0x100)
                    state = "Acclerating";
                else if (temp_bit9 == 0x200)
                    state = " MAX Speed";
                else if (temp_bit10 == 0x400)
                    state = "Decelerating";
                Status_num = Status;

                return "";
            }
        }

        /// <summary>
        /// 設定Command Position
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Command_Position">欲設定的Command position</param>
        /// <returns>錯誤資訊</returns>
        public string SetCommandEncoder(IntPtr AxisHandle, double Command_Position)
        {
            Result = Motion.mAcm_AxSetCmdPosition(AxisHandle, Command_Position);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 設定Actual position
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Actual_Position">欲設定的Actual position</param>
        /// <returns>錯誤資訊</returns>
        public string SetActualEncoder(IntPtr AxisHandle, double Actual_Position)
        {
            Result = Motion.mAcm_AxSetActualPosition(AxisHandle, Actual_Position);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得Command position
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Command_Encoder">欲取得之Command position</param>
        /// <returns>錯誤資訊</returns>
        public string GetCommandEncoder(IntPtr AxisHandle, ref double Command_Encoder)
        {
            Result = Motion.mAcm_AxGetCmdPosition(AxisHandle, ref Command_Encoder);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得Actual position
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Actual_Encoder">欲取得之Actual position</param>
        /// <returns>錯誤資訊</returns>
        public string GetActualEncoder(IntPtr AxisHandle, ref double Actual_Encoder)
        {
            Result = Motion.mAcm_AxGetActualPosition(AxisHandle, ref Actual_Encoder);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 運動軸作絕對位置移動
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Position">欲移動到之絕對座標</param>
        /// <returns>錯誤資訊</returns>
        public string MoveABS(IntPtr AxisHandle, double Position)
        {
            Result = Motion.mAcm_AxMoveAbs(AxisHandle, Position);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        //Relative Motion
        /// <summary>
        /// 運動軸作增量移動
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Distance">欲移動的增量</param>
        /// <returns>錯誤資訊</returns>
        public string MoveINC(IntPtr AxisHandle, double Distance)
        {
            Result = Motion.mAcm_AxMoveRel(AxisHandle, Distance);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 停止運動軸移動
        ///     StopType = 1 ---> Deceleration stop
        ///     StopType = 2 ---> EMC Stop
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="StopType">停止類型</param>
        /// <returns>錯誤資訊</returns>
        public string MoveStop(IntPtr AxisHandle, int StopType)
        {
            if (StopType == 1)
                Result = Motion.mAcm_AxStopDec(AxisHandle);    ///< 命令運動軸慢慢停止
            else
                Result = Motion.mAcm_AxStopEmg(AxisHandle);    ///< 命令運動軸緊急停止

            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 運動軸連續移動
        ///     Dir = 1 ---> Positive direction
        ///     Dir = 2 ---> Negative direction
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Dir">轉動方向</param>
        /// <returns>錯誤資訊</returns>
        public string MoveContinuous(IntPtr AxisHandle, int Dir)
        {
            ushort Direction = 0;
            switch (Dir)
            {
                case 1:// Positive Direction
                    Direction = 0;
                    break;
                case 2:// Negative Direction
                    Direction = 1;
                    break;
            }
            Result = Motion.mAcm_AxMoveVel(AxisHandle, Direction);    ///< 命令運動軸作一個不會停止的運動
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 將一個運動軸加入到指定的Group
        /// 如果集合為空，就自行創建一個新集合
        /// </summary>
        /// <param name="GpHandle">運動軸集合控點</param>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <returns>錯誤資訊</returns>
        public string Multi_SetAxisGroup(ref IntPtr GpHandle, IntPtr AxisHandle)
        {
            Result = Motion.mAcm_GpAddAxis(ref GpHandle, AxisHandle);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 刪除運動軸集合中所有運動軸，並關閉此集合的控點
        /// </summary>
        /// <param name="GpHandle">運動軸集合控點</param>
        /// <returns>錯誤資訊</returns>
        public string Multi_CloseGroup(IntPtr GpHandle)
        {
            Result = Motion.mAcm_GpClose(ref GpHandle);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 刪除一組運動軸集合中特定的運動軸
        /// </summary>
        /// <param name="GpHandle">運動軸集合控點</param>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <returns></returns>
        public string Multi_GroupRemoveAxis(IntPtr GpHandle, IntPtr AxisHandle)
        {

            Result = Motion.mAcm_GpRemAxis(GpHandle, AxisHandle);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 命令運動軸以典型的運動方式回到home點
        ///     Mode:0~15
        ///     Dir:Search Direction
        ///     Dir = 0 ---> Position
        ///     Dir = 1 ---> Negative
        /// </summary>
        /// <param name="AxisHandle">運動軸控建</param>
        /// <param name="Mode">欲回home的模式</param>
        /// <param name="Dir">回home的轉動方向</param>
        /// <returns></returns>
        public string HomeSearch(IntPtr AxisHandle, int Mode, int Dir)
        {
            switch (Dir)
            {
                case 1:
                    Dir = 0;
                    break;
                case 2:
                    Dir = 1;
                    break;
            }

            Result = Motion.mAcm_AxHome(AxisHandle, (uint)Mode, (uint)Dir);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// Set Home reset encoder 
        ///     Active = 0 ---> Disable
        ///     Active = 1 ---> Enable
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ActiveType">作動旗標</param>
        /// <returns>錯誤訊息</returns>
        public string HomeSetResetEncoder(IntPtr AxisHandle, int ActiveType)
        {
            int SettingValue = ActiveType;
            uint BufferLength = 4;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxHomeResetEnable, ref SettingValue, BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得Home reset encoder
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ActiveType">作動旗標</param>
        /// <returns>錯誤訊息</returns>
        public string HomeGetResetEncoder(IntPtr AxisHandle, ref int ActiveType)
        {
            uint GettingValue = 0;
            uint BufferLength = 4;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxHomeResetEnable, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                ActiveType = (int)GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 設定Home Shift distance
        /// Unit:Pulse
        /// 在PCI-1240卡中，這個值必須大於0，預設為100
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ShiftValue">欲移動的距離</param>
        /// <returns>錯誤訊息</returns>
        public string HomeShiftSet(IntPtr AxisHandle, double ShiftValue)
        {
            double SettingValue = ShiftValue;
            uint bufferLength = 8;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.PAR_AxHomeCrossDistance, ref SettingValue, bufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得Home Shift distance
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ShiftValue">欲取得的移動距離</param>
        /// <returns>錯誤訊息</returns>
        public string HomeShiftGet(IntPtr AxisHandle, double ShiftValue)
        {
            double GettingValue = 0;
            uint BufferLength = 0;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.PAR_AxHomeCrossDistance, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                ShiftValue = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 判斷現在是否正在回到home的位置
        ///     Is_Search = 1 ---> Searching Home
        ///     Is_Search = 0 ---> Axis is Ready
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="Is_Search">判斷尋找home旗標</param>
        /// <returns>錯誤資訊</returns>
        public string IsHomeSearch(IntPtr AxisHandle, ref int Is_Search)
        {
            ushort State = 0;
            Result = Motion.mAcm_AxGetState(AxisHandle, ref State);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                if (State == 4)
                    Is_Search = 1;
                else if (State == 1)
                    Is_Search = 0;

                return "";
            }
        }

        /// <summary>
        /// 取得CFG_AxPPU的值
        /// Pulse per unit(PPU)
        /// CFG_AxPPU的值必須要大於0，一般預設為10
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="PPU">欲取得的PPU</param>
        /// <returns>錯誤訊息</returns>
        public string Get_CFG_PPU(IntPtr AxisHandle, ref uint PPU)
        {
            uint GettingValue = 0;
            uint BufferLength = 4;     ///< uint的位元數
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxPPU, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                PPU = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 設定CFG_PPU的值
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="PPU">欲設定的PPU</param>
        /// <returns>錯誤訊息</returns>
        public string Set_CFG_PPU(IntPtr AxisHandle, uint PPU)
        {
            uint SettingValue = PPU;
            uint BufferLength = 4;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxPPU, ref SettingValue, BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得CFG_AxMaxVel的值
        /// Unit: PPU/s
        /// 對PCI-1240來說，最大值: FT_AxMaxVel / CFG_AxPPU；最小值：8000/ CFG_AxPPU
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="MaxVel">欲取得的最大速度</param>
        /// <returns>錯誤訊息</returns>
        public string Get_CFG_MaxVel(IntPtr AxisHandle, ref double MaxVel)
        {
            double GettingValue = 0;
            uint BufferLength = 8;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxMaxVel, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                MaxVel = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 設定CFG_MaxVel的值
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="MaxVel">欲設定的最大速度</param>
        /// <returns>錯誤訊息</returns>
        public string Set_CFG_MaxVel(IntPtr AxisHandle, double MaxVel)
        {
            double SettingValue = MaxVel;
            //double temp = 0;
            uint BufferLength = 8;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxMaxVel, ref SettingValue, BufferLength);
            //temp = SettingValue;
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得CFG_AxMaxAcc的值
        /// Unit: PPU/s^2
        /// 對PCI-1240來說，最大值：FT_AxMaxAcc / CFG_AxPPU；最小值：125/ CFG_AxPPU
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="MaxAcc">欲取得的最大加速度</param>
        /// <returns>錯誤訊息</returns>
        public string Get_CFG_MaxAcc(IntPtr AxisHandle, ref double MaxAcc)
        {
            double GettingValue = 0;
            uint BufferLength = 8;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxMaxAcc, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                MaxAcc = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 設定CFG_AxMaxAcc的值
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="MaxAcc">欲設定的最大加速度</param>
        /// <returns>錯誤訊息</returns>
        public string Set_CFG_MaxAcc(IntPtr AxisHandle, double MaxAcc)
        {
            double SettingValue = MaxAcc;
            //double temp = 0;
            uint BufferLength = 8;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxMaxAcc, ref SettingValue, BufferLength);
            //temp = SettingValue;
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        // Get/Set CFG_AxMaxDec
        /// <summary>
        /// 取得CFG_AxMaxDec的值
        /// Unit:PPU/s^2
        /// 對PCI-1240來說，最大值：FT_AxMaxDec/CFG_AxPPU；最小值：125/ CFG_AxPPU
        /// </summary>
        /// <param name="AxisHandle"></param>
        /// <param name="MaxDec"></param>
        /// <returns>錯誤訊息</returns>
        public string Get_CFG_MaxDec(IntPtr AxisHandle, ref double MaxDec)
        {
            double GettingValue = 0;
            uint BufferLength = 8;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxMaxDec, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                MaxDec = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 設定CFG_AxMaxDec的值
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="MaxDec">欲設定的最大減速度</param>
        /// <returns>錯誤訊息</returns>
        public string Set_CFG_MaxDec(IntPtr AxisHandle, double MaxDec)
        {
            double SettingValue = MaxDec;
            uint BufferLength = 8;
            //double temp = 0;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxMaxDec, ref SettingValue, BufferLength);
            //temp = SettingValue;
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 取得FT_AxMaxVel(最大速度)的數值
        /// Unit:pulse/s
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="FT_MaxVel">欲取得之最大速度</param>
        /// <returns>錯誤訊息</returns>
        public string Get_FT_MaxVel(IntPtr AxisHandle, ref double FT_MaxVel)
        {
            double GettingValue = 0;
            uint BufferLength = 8;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.FT_AxMaxVel, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                FT_MaxVel = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 取得FT_AxMaxACC(最大加速度)的值
        /// Unit:pulse/s^2
        /// </summary>
        /// <param name="AxisHandle">運動軸的控點</param>
        /// <param name="FT_MaxAcc">欲取得之最大加速度</param>
        /// <returns>錯誤訊息</returns>
        public string Get_FT_MaxAcc(IntPtr AxisHandle, ref double FT_MaxAcc)
        {
            double GettingValue = 0;
            uint BufferLength = 8;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.FT_AxMaxAcc, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                FT_MaxAcc = GettingValue;
                return "";
            }
        }

        // Get FT_AxMaxDec
        /// <summary>
        /// 取得FT_AxMaxDec(最大減速度)的值
        /// Unit:pulse/s^2
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="FT_MaxDec">欲取得的最大減速度</param>
        /// <returns>錯誤訊息</returns>
        public string Get_FT_MaxDec(IntPtr AxisHandle, ref double FT_MaxDec)
        {
            double GettingValue = 0;
            uint BufferLength = 8;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.FT_AxMaxDec, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                FT_MaxDec = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 取得運動軸的狀態
        /// </summary>
        /// <param name="AxisHandle">運動軸的控點</param>
        /// <param name="State">儲存目前傳入之運動軸的狀態</param>
        /// <returns>錯誤訊息</returns>
        public string Get_Axis_State(IntPtr AxisHandle, ref string State)
        {
            UInt16 AxState = new UInt16();
            Result = Motion.mAcm_AxGetState(AxisHandle, ref AxState);
            if (Result != (uint)ErrorCode.SUCCESS)
            {
                return GetErrorMsg(Result);
            }
            else
            {
                switch (AxState)
                {
                    case 0:
                        State = "STA_AX_DISABLE";      ///< Axis is disabled, you can open it to active it
                        break;
                    case 1:
                        State = "STA_AX_READY";       ///< Axis is ready and waiting for new command
                        break;
                    case 2:
                        State = "STA_AX_STOPPING";    ///< Axis is stopping
                        break;
                    case 3:
                        State = "STA_AX_ERROR_STOP";  ///< Axis has stopped because of error
                        break;
                    case 4:
                        State = "STA_AX_HOMING";      ///< Axis is executing home motion
                        break;
                    case 5:
                        State = "STA_AX_PTP_MOT";     ///< Axis is executing PTP motion
                        break;
                    case 6:
                        State = "STA_AX_CONTI_MOT";   ///< Axis is executing continuous motion
                        break;
                    case 7:
                        State = "STA_AX_SYNC_MOT";    ///< Axis is in one group and the group is executing  interpolation motion
                        break;
                    default:
                        break;
                }
                return "";
            }
        }

        /// <summary>
        /// 取得原點訊號的旗標
        ///     Type 0: Low active
        ///     Type 1: High active
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ActiveType">判斷旗標</param>
        /// <returns>錯誤資訊</returns>
        public string GetHomeLogic(IntPtr AxisHandle, ref int ActiveType)
        {
            uint BufferLength = 4;
            int GettingValue = 0;
            Result = Motion.mAcm_GetProperty(AxisHandle, (uint)PropertyID.CFG_AxOrgLogic, ref GettingValue, ref BufferLength);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
            {
                ActiveType = GettingValue;
                return "";
            }
        }

        /// <summary>
        /// 設定原點訊號的旗標
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <param name="ActiveType">判斷旗標</param>
        /// <returns>錯誤資訊</returns>
        public string SetHomeLogic(IntPtr AxisHandle, int ActiveType)
        {
            int SettingValue = ActiveType;
            Result = Motion.mAcm_SetProperty(AxisHandle, (uint)PropertyID.CFG_AxOrgLogic, ref SettingValue, 4);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 若運動軸的狀態處於"STA_AxErrorStop"，呼叫此方法後會重設運動軸使其狀態變為"STA_AxReady"
        /// </summary>
        /// <param name="AxisHandle">運動軸控點</param>
        /// <returns>錯誤訊息</returns>
        public string ErrorReset(IntPtr AxisHandle)
        {
            Result = Motion.mAcm_AxResetError(AxisHandle);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }

        /// <summary>
        /// 重設伺服機警告(Motion Card DO07)
        /// </summary>
        /// <param name="AxisHandle">運動軸的控點</param>
        /// <param name="Is_ON">判斷伺服馬德Servo ON/OFF的旗標</param>
        /// <returns>錯誤訊息</returns>
        public string ResetServoAlarm(IntPtr AxisHandle, bool Is_ON)
        {
            byte InputValue;

            /// Is_ON == true  --->  InputValue = 1;
            /// Is_ON == false --->  InputValue = 0;
            if (Is_ON)
                InputValue = 1;
            else
                InputValue = 0;

            /// 設定數位輸出通道7為ON/OFF
            Result = Motion.mAcm_AxDoSetBit(AxisHandle, 7, InputValue);
            if (Result != (uint)ErrorCode.SUCCESS)
                return GetErrorMsg(Result);
            else
                return "";
        }


	}
}
