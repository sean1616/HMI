using System;
using System.Threading;
using HMI_PermanentForm.Utils;

namespace AM_Kernel.Motor
{
	public abstract class MotorFuncBase
	{
		protected MotorFuncSupplier _MotionFunc;

		protected MotorFuncBase(MotorFuncSupplier src)
		{
			_MotionFunc = src;
		}

		// In the future, there is another motion function extension,
		// we can extract the common parts between the libraries corp. supplied.
		// Todo

	}

	/// <summary>
	/// Use the function AdvMotWrap provided for Logical Combination AM system used. 
	/// </summary>
	public class MotorFunc4PCI1240 : MotorFuncBase
	{
        private static MotorFunc4PCI1240 PCI1240Func = new MotorFunc4PCI1240(new AdvMotWrap());

        private const string errorMsg = "Warning - 01 - Hardware doesn't connect - <ConnectedError> ";
        
        private MotorFuncSupplier MotionFunc
        {
            get { return _MotionFunc; }
            set { _MotionFunc = value; }
        }

		private MotorFunc4PCI1240(MotorFuncSupplier src) : base(src)
		{

		}

        public static MotorFunc4PCI1240 getInstance()
        {
            return PCI1240Func;
        }

		/// <summary>
		/// 使用研華軸卡時，必須先對IPC上所接的裝置作初始化
		/// 初始化完畢後，才能開啟軸卡上各個運動軸
		/// </summary>
		/// <returns>錯誤訊息</returns>
		public string InitialDevice()
		{	
			string Result = "";
			/// Get Available Device Information
			Result = MotionFunc.GetAvailableDevs(DeviceInfo.DevList, ref DeviceInfo.DevCount);
			if (Result != "")
				return "Warning" + " - " + " 06 " + " - " + Result + " - " + "<GetAvailableDevs>";

            if (DeviceInfo.DevCount != 0)
            {
                /// Open Device
                /// DeviceInfo.DeviceCount = 裝置數目
                for (int i = 0; i < DeviceInfo.DevCount; i++)
                {
                    /// DeviceInfo.DevList[i].DeviceNum = Device number needed for Acm_DevOpen.
                    DeviceInfo.DevNum = DeviceInfo.DevList[i].DeviceNum;

                    /// 開啟指定的DeviceInfo.DevList[i].DeviceNum
                    Result = MotionFunc.OpenDevice(DeviceInfo.DevNum, ref DeviceInfo.DevHandle);
                    if (Result != "")
                        return "Warning" + "-" + "06" + "-" + Result + "-" + "<OpenDevice>";
                }

                /// Open Axis
                Result = MotionFunc.OpenAxis(DeviceInfo.DevHandle, 0, ref Recoater.getInstance().AxisHandle);  ///< 取得X軸控點(0)
                if (Result != "")
                    return "Warning" + "-" + "06" + "-" + Result + "-" + "<OpenAxis_X>";
                Result = MotionFunc.OpenAxis(DeviceInfo.DevHandle, 1, ref BuildPlatform.getInstance().AxisHandle);  ///< 取得Z軸控點(1)
                if (Result != "")
                    return "Warning" + "-" + "06" + "-" + Result + "-" + "<OpenAxis_Z>";
                Result = MotionFunc.OpenAxis(DeviceInfo.DevHandle, 2, ref LeftFeeder.getInstance().AxisHandle); ///< 取得U1軸控點(2)
                if (Result != "")
                    return "Warning" + "-" + "06" + "-" + Result + "-" + "<OpenAxis_U1>";
                Result = MotionFunc.OpenAxis(DeviceInfo.DevHandle, 3, ref RightFeeder.getInstance().AxisHandle); ///< 取得U2軸控點(3)
                if (Result != "")
                    return "Warning" + "-" + "06" + "-" + Result + "-" + "<OpenAxis_U2>";

                MotionFunc.is_HardwareConnected = true;
            }
            else
            {
                Result = "No Device.";
                Utils.LogHandler.LogDebug(Utils.LogHandler.Formatting(string.Format("Warning - 06 {0} - <GetAvailableDevs>", Result), ""));
                return string.Format("Warning - 06 - {0} - <GetAvailableDevs>", Result);
            }
			
			return "";
		}

		/// <summary>
		/// Close device
		/// </summary>
		/// <returns>錯誤訊息</returns>
		public string CloseDevice()
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				/// Close Axis
				Result = MotionFunc.CloseAxis(Recoater.getInstance().AxisHandle);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<CloseAxis_X>";
				Result = MotionFunc.CloseAxis(BuildPlatform.getInstance().AxisHandle);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<CloseAxis_Z>";
                Result = MotionFunc.CloseAxis(LeftFeeder.getInstance().AxisHandle);
                if (Result != "")
                    return "Warning" + "-" + "06" + "-" + Result + "-" + "<CloseAxis_U1>";
                Result = MotionFunc.CloseAxis(RightFeeder.getInstance().AxisHandle);
                if (Result != "")
                    return "Warning" + "-" + "06" + "-" + Result + "-" + "<CloseAxis_U2>";

                /// Close Device
                for (int i = 0; i < DeviceInfo.DevCount; i++)
				{
					DeviceInfo.DevNum = DeviceInfo.DevList[i].DeviceNum;

					Result = MotionFunc.CloseDevice(DeviceInfo.DevHandle);
					if (Result != "")
						return "Warning" + "-" + "06" + "-" + Result + "-" + "<CloseDevice>";
				}

				MotionFunc.is_HardwareConnected = false;
			}
            return errorMsg;
		}

		/// <summary>
		/// 取得FT/CFG速度參數
		/// </summary>
		/// <param name="AxisHandle">操作軸控點</param>
		/// <param name="par_Motion">軸控參數</param>
		/// <returns>錯誤訊息</returns>
		public string GetParameter_CFG_Motion(IntPtr AxisHandle, MotorPara.Motion para_Motion)
		{
			string Result = "";                ///< 儲存錯誤訊息
			uint temp_uint = 0;                ///< 暫存
			double temp_double = 0;            ///< 暫存速度、加速度的值

			if (MotionFunc.is_HardwareConnected)
			{
				/// FT_MaxVel
				Result = MotionFunc.Get_FT_MaxVel(AxisHandle, ref temp_double);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get FT MaxVel>";
				else
					para_Motion.FT_MaxVal = temp_double;     ///< 將取得的最大速度賦值給par_Motion.FT_MaxVel

				/// FT_MaxAcc
				Result = MotionFunc.Get_FT_MaxAcc(AxisHandle, ref temp_double);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get FT MaxAcc>";
				else
					para_Motion.FT_MaxAcc = temp_double;    ///< 將取得的最大加速度賦值給par_Motion.FT_MaxACC

				/// FT_MaxDec
				Result = MotionFunc.Get_FT_MaxDec(AxisHandle, ref temp_double);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get FT MaxDec>";
				else
					para_Motion.FT_MaxDec = temp_double;    ///< 將取得的最大減速度賦值給par_Motion.FT_MaxDec

				/// CFG_PPU
				Result = MotionFunc.Get_CFG_PPU(AxisHandle, ref temp_uint);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get CFG PPU>";
				else
					para_Motion.CFG_PPU = temp_uint;        ///< 將取得的每秒多少Pulse賦值給par_Motion.CFG_PPU

				/// CFG_MaxVel
				Result = MotionFunc.Get_CFG_MaxVel(AxisHandle, ref temp_double);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get CFG MaxVel>";
				else
					para_Motion.CFG_MaxVal = temp_double;  ///< 將取得的配置最大速度賦值給par_Motion.CFG_MaxVel

				/// CFG_MaxAcc
				Result = MotionFunc.Get_CFG_MaxAcc(AxisHandle, ref temp_double);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get CFG Acc>";
				else
					para_Motion.CFG_MaxAcc = temp_double;  ///< 將取得的配置最大加速度賦值給par_Motion.CFG_MaxACC

				/// CFG_MaxDec
				Result = MotionFunc.Get_CFG_MaxDec(AxisHandle, ref temp_double);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get CFG Dec>";
				else
					para_Motion.CFG_MaxDec = temp_double;  ///< 將取得的配置最大減速度賦值給par_Motion.CFG_MaxDec
			}
            return errorMsg;
		}

		/// <summary>
		/// 設定硬體參數
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="par_Hardware">硬體參數</param>
		/// <returns>錯誤訊息</returns>
		public string SetParameter_Hardware(IntPtr AxisHandle, MotorPara.Hardware par_Hardware)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				/// Pulse Out
				Result = MotionFunc.SetPulseOut(AxisHandle, par_Hardware.PulseOut);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Pulse Out>";

				/// Pulse In
				Result = MotionFunc.SetPulseIn(AxisHandle, par_Hardware.PulseIn);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Pulse In>";

				/// Limit Logic
				Result = MotionFunc.SetLimit(AxisHandle, par_Hardware.Limit_Logic);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Limit Logic>";

				/// Limit Stop Type
				Result = MotionFunc.SetReactingMode(AxisHandle, par_Hardware.Limit_StopAction);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Limit Stop Action>";

				/// INP Enable
				Result = MotionFunc.SetINPEnable(AxisHandle, par_Hardware.INP_Enable);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set INP Enable>";

				/// INP Logic
				Result = MotionFunc.SetINPLogic(AxisHandle, par_Hardware.INP_Logic);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set INP Logic>";

				/// Alarm Enable
				Result = MotionFunc.SetAlarmEnable(AxisHandle, par_Hardware.Alarm_Enable);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Alarm Enable>";

				/// Alarm Logic
				Result = MotionFunc.SetAlarmLogic(AxisHandle, par_Hardware.Alarm_Logic);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Alarm Logic>";

				/// Do Enable Function
				Result = MotionFunc.SetDOEnable(AxisHandle, 1);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Do Enable>";
			}
            else
                return errorMsg;
            return "";
		}

		/// <summary>
		/// 設定軸控參數
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="par_Motion">軸控參數</param>
		/// <returns>錯誤訊息</returns>
		public string SetParameter_Motion(IntPtr AxisHandle, MotorPara.Motion par_Motion)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				/// T/S Curve
				Result = MotionFunc.SetCurve(AxisHandle, par_Motion.TSCurve);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set T/S Curve>";

				/// Initial Speed
				Result = MotionFunc.SetInitialSpeed(AxisHandle, par_Motion.Speed_Initial);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Initial Speed>";

				/// Driving Speed
				Result = MotionFunc.SetDrivingSpeed(AxisHandle, par_Motion.Speed_Driving);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Driving Speed>";

				/// Acc
				Result = MotionFunc.SetAcceleration(AxisHandle, par_Motion.Acc);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Acc>";

				/// Dec
				Result = MotionFunc.SetDeceleration(AxisHandle, par_Motion.Dec);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Dec>";
			}
            else
                return errorMsg;
            return "";
		}

		// Set CFG Speed Parameter
		/// <summary>
		/// 設定CFG 速度參數
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="par_Motion">軸控參數</param>
		/// <returns>錯誤訊息</returns>
		public string SetParameter_CFG_Motion(IntPtr AxisHandle, MotorPara.Motion par_Motion)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				/// PPU
				Result = MotionFunc.Set_CFG_PPU(AxisHandle, par_Motion.CFG_PPU);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set CFG PPU>";

				/// CFG_MaxVel
				Result = MotionFunc.Set_CFG_MaxVel(AxisHandle, par_Motion.CFG_MaxVal);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set CFG MaxVel>";

				/// CFG_MaxAcc
				Result = MotionFunc.Set_CFG_MaxAcc(AxisHandle, par_Motion.CFG_MaxAcc);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set CFG MaxAcc>";

				/// CFG_MaxDec
				Result = MotionFunc.Set_CFG_MaxDec(AxisHandle, par_Motion.CFG_MaxDec);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set CFG MaxDec>";
			}
            else
                return errorMsg;

            return "";

		}

		// Set Home Search Parameter
		/// <summary>
		/// 設定Home搜索參數
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="par_Home">回home參數</param>
		/// <returns>錯誤訊息</returns>
		public string SetParameter_HomeSearch(IntPtr AxisHandle, MotorPara.Home par_Home)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				/// Home Encoder Reset Mode
				Result = MotionFunc.HomeSetResetEncoder(AxisHandle, par_Home.HomeResetEnable);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Home Reset Encoder>";

				/// Home Position Shift
				Result = MotionFunc.HomeShiftSet(AxisHandle, par_Home.HomeShift);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Home Shift>";
			}
            else
                return errorMsg;
            return "";
		}

		/// <summary>
		/// 設定X軸與Z軸的ServoON
		/// </summary>
		/// <returns>錯誤訊息</returns>
		public string ServoON()
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				if (!Recoater.getServoRDY())     ///< 如果X軸的伺服旗標為False
				{
					/// 設定X軸伺服旗標為true
					Result = MotionFunc.SetServoOn(Recoater.getInstance().AxisHandle, 1);
					if (Result != "")
						return "Warning" + "-" + "06" + "-" + Result + "-" + "<Servo ON X>";
				}
				if (!BuildPlatform.getServoRDY())
				{
					Result = MotionFunc.SetServoOn(BuildPlatform.getInstance().AxisHandle, 1);
					if (Result != "")
						return "Warning" + "-" + "06" + "-" + Result + "-" + "<Servo ON Z>";
				}
			}
            else
                return errorMsg;

            return "";
		}

		/// <summary>
		/// 設定X軸與Z軸的Servo OFF
		/// </summary>
		/// <returns>錯誤資訊</returns>
		public string ServoOFF()
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				if (Recoater.getServoRDY())
				{
					Result = MotionFunc.SetServoOn(Recoater.getInstance().AxisHandle, 0);
					if (Result != "")
						return "Warning" + "-" + "06" + "-" + Result + "-" + "<Servo OFF>";
				}
				if (BuildPlatform.getServoRDY())
				{
					Result = MotionFunc.SetServoOn(BuildPlatform.getInstance().AxisHandle, 0);
					if (Result != "")
						return "Warning" + "-" + "06" + "-" + Result + "-" + "<Servo OFF>";
				}
			}
            else
                return errorMsg;
            return "";
		}

		// Servo RDY Signal
		/// <summary>
		/// 取得與監看之運動軸的ServoRDY狀態
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Is_RDY">判斷是否Ready旗標</param>
		/// <returns>錯誤資訊</returns>
		public string ServoRDY(IntPtr AxisHandle, ref bool Is_RDY)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				int State = 0;
				Result = MotionFunc.GetServoRDY(AxisHandle, ref State);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<RDY Singal>";
				else
				{
					if (State == 0)
						Is_RDY = false;
					else if (State == 1)
						Is_RDY = true;

					return "";
				}
			}

            else
                return errorMsg;

		}

		// =======================================================================
		/// <summary>
		/// 運動軸作絕對位置的移動
		/// </summary>
		/// <param name="AxisHandel">運動軸控點</param>
		/// <param name="Displacement">欲移動到的座標</param>
		/// <returns>錯誤訊息</returns>
		public string MoveABS(IntPtr AxisHandel, double Displacement)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				string State = "";
				/// 取得欲移動之運動軸的狀態，查看其狀態是否為STA_AxReady
				Result = MotionFunc.Get_Axis_State(AxisHandel, ref State);
				if (Result == "")
				{
					/// 如果狀態為STA_AxReady，呼叫研華API作運動軸的移動
					if (State == "STA_AX_READY")
					{
						Result = MotionFunc.MoveABS(AxisHandel, Displacement);
						if (Result != "")
						{
							return "Warning" + "-" + "06" + "-" + Result + "-" + "<Move ABS>";
						}
					}
					else
						return "Alarm" + "-" + "09" + "-" + Result + "-" + "<Move ABS>";
				}
				else
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get Axis State>";
			}
            else
                return errorMsg;
            return "";
		}

		/// <summary>
		/// 運動軸作增量位置的移動
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Displacement">欲移動的距離</param>
		/// <returns>錯誤資訊</returns>
		public string MoveINC(IntPtr AxisHandle, double Displacement)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				string State = "";
				/// 檢查運動軸的狀態
				Result = MotionFunc.Get_Axis_State(AxisHandle, ref State);
				if (Result == "")
				{
					/// 若為STA_AX_READY，呼叫研華API作增量移動
					if (State == "STA_AX_READY")
					{
						Result = MotionFunc.MoveINC(AxisHandle, Displacement);
						if (Result != "")
						{
							return "Warning" + "-" + "06" + "-" + Result + "-" + "<Move INC>";
						}
					}
					else
						return "Alarm" + "-" + "09" + "-" + Result + "-" + "<Move INC>";
				}
				else
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Move INC>";
			}
            else
                return errorMsg;
            return "";
		}

		/// <summary>
		/// 停止運動軸的移動
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <returns>錯誤資訊</returns>
		public string MoveSTOP(IntPtr AxisHandle)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				/// 呼叫研華API停止運動軸移動，停止模式為立即停止(EMC)
				Result = MotionFunc.MoveStop(AxisHandle, 2);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Move STOP>";
			}
            else
                return errorMsg;
            return "";
		}

		/// <summary>
		/// 運動軸作正向JOG移動
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="JOG_Speed">JOG的速度</param>
		/// <returns>錯誤資訊</returns>
		public string JOG_P(IntPtr AxisHandle, double JOG_Speed)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				string State = "";
				/// 檢查運動軸的狀態
				Result = MotionFunc.Get_Axis_State(AxisHandle, ref State);
				if (Result == "")
				{
					/// 若為STA_AX_READY則繼續下命令
					if (State == "STA_AX_READY")
					{
						/// 設定JOG速度
						Result = MotionFunc.SetDrivingSpeed(AxisHandle, JOG_Speed);
						if (Result != "")
							return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set JOG_P Speed>";
						Thread.Sleep(100);   ///< 執行緒暫停0.1秒
											 ///
											 /// 作連續性的運動，方向為positive direction
						Result = MotionFunc.MoveContinuous(AxisHandle, 1);
						if (Result != "")
							return "Warning" + "-" + "06" + "-" + Result + "-" + "<Move Continuous JOG_P>";
					}
					else
						return "Alarm" + "-" + "09" + "-" + Result + "-" + "<Move Continuous JOG_P>";
				}
				else
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get Axis State>";
			}
            else
                return errorMsg;
            return "";
		}

		/// <summary>
		/// 運動軸作負向JOG移動
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="JOG_Speed">JOG的速度</param>
		/// <returns>錯誤資訊</returns>
		public string JOG_N(IntPtr AxisHandle, double JOG_Speed)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				string State = "";
				/// 檢查運動軸的狀態
				Result = MotionFunc.Get_Axis_State(AxisHandle, ref State);
				if (Result == "")
				{
					/// 若運動軸狀態為STA_AX_READY，繼續執行命令
					if (State == "STA_AX_READY")
					{
						/// 設定JOG驅動速度
						Result = MotionFunc.SetDrivingSpeed(AxisHandle, JOG_Speed);
						if (Result != "")
							return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set JOG_N Speed>";
						Thread.Sleep(100);    ///< 執行緒暫停0.1秒

						/// 運動軸作連續運動，方向為Negative direction
						Result = MotionFunc.MoveContinuous(AxisHandle, 2);
						if (Result != "")
							return "Warning" + "-" + "06" + "-" + Result + "-" + "<Move Continuous JOG_N>";
					}
					else
						return "Alarm" + "-" + "09" + "-" + Result + "-" + "<Move Continuous JOG_N>";
				}
				else
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get Axis State>";
			}
            else
                return errorMsg;
            return "";
		}

		/// <summary>
		/// 運動軸JOG模式停止
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Driving_Speed">設定JOG速度</param>
		/// <returns>錯誤資訊</returns>
		public string JOG_Stop(IntPtr AxisHandle, double Driving_Speed)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				/// 命令運動軸緊急停止
				Result = MotionFunc.MoveStop(AxisHandle, 2);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Move Stop _JOG>";
				Thread.Sleep(100);   ///< 執行緒暫停0.1秒

				///設定JOG速度
				Result = MotionFunc.SetDrivingSpeed(AxisHandle, Driving_Speed);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Driving Speed _JOG>";
			}
            else
                return errorMsg;

            return "";
		}

		/// <summary>
		/// 找尋Home的位置並回home
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Mode">歸home模式</param>
		/// <param name="Dir">運動軸旋轉方向</param>
		/// <returns>錯誤資訊</returns>
		public string HomeSearch(IntPtr AxisHandle, int Mode, int Dir)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				string State = "";
				/// 檢查運動軸狀態
				Result = MotionFunc.Get_Axis_State(AxisHandle, ref State);
				if (Result == "")
				{
					/// 若狀態為STA_AX_READY，繼續執行命令
					if (State == "STA_AX_READY")
					{
						/// 呼叫研華API方法執行Home Search
						Result = MotionFunc.HomeSearch(AxisHandle, Mode, Dir);
						if (Result != "")
						{
							return "Warning" + "-" + "06" + "-" + Result + "-" + "<HomeSearch>";
						}
					}
					else
						return "Alarm" + "-" + "09" + "-" + Result + "-" + "<HomeSearch>";
				}
				else
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Axis Get Axis State>";
			}
            else
                return errorMsg;

            return "";
		}
		// =======================================================================

		/// <summary>
		/// 讀取極限狀態
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Is_ELP">正極限判斷旗標</param>
		/// <param name="Is_ELN">負極限判斷旗標</param>
		/// <returns>錯誤資訊</returns>
		public string LimtiStatus(IntPtr AxisHandle, ref bool Is_ELP, ref bool Is_ELN)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				int P_Limit = 0;             ///< 初始化P_Limit = 0
				int N_Limit = 0;             ///< 初始化N_Limit = 0

				/// 呼叫研華API方法取得運動軸正負極限資訊
				Result = MotionFunc.GetLimitStatus(AxisHandle, ref P_Limit, ref N_Limit);
				if (Result != "")
				{
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Read Limit Status>";
				}
				/// 將取得的P_Limit、N_Limit賦值給傳入的正負極限參考
				else
				{
					switch (P_Limit)
					{
						case 0:
							Is_ELP = false;
							break;
						case 1:
							Is_ELP = true;
							break;
					}
					switch (N_Limit)
					{
						case 0:
							Is_ELN = false;
							break;
						case 1:
							Is_ELN = true;
							break;
					}
				}
			}
            else
                return errorMsg;

            return "";
		}

		/// <summary>
		/// 讀取Home極限狀態資訊
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Is_Home">判斷回Home旗標</param>
		/// <returns>錯誤資訊</returns>
		public string HomeStatus(IntPtr AxisHandle, ref bool Is_Home)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				int HomeLMT = 0;            ///< 初始化HomeLMT = 0
											/// 呼叫研華API方法取得原點狀態
				Result = MotionFunc.GetHomeStatus(AxisHandle, ref HomeLMT);
				if (Result != "")
				{
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Read Home Status>";
				}
				/// 將原點狀態資訊賦值給傳入的參考Is_Home
				else
				{
					switch (HomeLMT)
					{
						case 1:
							Is_Home = true;
							break;
						case 0:
							Is_Home = false;
							break;
					}
					return "";
				}
			}
            else
                return errorMsg;

		}

		/// <summary>
		/// 讀取運動軸Encoder資訊
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="CommandEncoder">Command Pulse數量</param>
		/// <param name="ActualEncoder">Actual Pulse數量</param>
		/// <returns>錯誤資訊</returns>
		public string EncoderStatus(IntPtr AxisHandle, ref double CommandEncoder, ref double ActualEncoder)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				/// 取得運動軸Command Encoder的資料
				Result = MotionFunc.GetCommandEncoder(AxisHandle, ref CommandEncoder);
				if (Result != "")
				{
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get Command Encoder>";
				}

				/// 取得運動軸Actual Encoder的資料
				Result = MotionFunc.GetActualEncoder(AxisHandle, ref ActualEncoder);
				if (Result != "")
				{
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get Actual Encoder>";
				}
				return "";
			}
            else
                return errorMsg;
		}

		// Read INP Status
		/// <summary>
		/// 讀取INP的狀態
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Is_INP">判斷INP旗標</param>
		/// <returns>錯誤資訊</returns>
		public string INPStatus(IntPtr AxisHandle, ref bool Is_INP)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				int INP = 0;
				Result = MotionFunc.GetINPStatus(AxisHandle, ref INP);
				if (Result != "")
				{
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get INP Status>";
				}
				else
				{
					switch (INP)
					{
						case 0:
							Is_INP = false;
							break;
						case 1:
							Is_INP = true;
							break;
					}
					return "";
				}
			}
            else 
                return errorMsg;
		}

		// Read ALM Status
		/// <summary>
		/// 讀取Alarm狀態
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Is_ALM">判斷Alarm旗標</param>
		/// <returns>錯誤資訊</returns>
		public string ALMStatus(IntPtr AxisHandle, ref bool Is_ALM)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				int ALM = 0;
				/// 呼叫研華API方法取得Alarm狀態
				Result = MotionFunc.GetAlarmStatus(AxisHandle, ref ALM);
				if (Result != "")
				{
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get Alarm Status>";
				}
				else
				{
					string ReturnMsg = "";
					switch (ALM)
					{
						case 0:
							Is_ALM = false;
							ReturnMsg = "";
							break;
						/// 表示Servo alarm
						case 1:
							Is_ALM = true;
							ReturnMsg = "Alarm" + "-" + "01" + "-" + "Servo Alarm" + "-" + "<Servo Alarm>";
							break;
					}
					return ReturnMsg;
				}
			}
            else
                return errorMsg;

		}

		// Read Driving Status
		/// <summary>
		/// 讀取運動軸驅動狀態
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Is_Driving">判斷是否正在驅動</param>
		/// <returns>錯誤資訊</returns>
		public string DrivingStatus(IntPtr AxisHandle, ref bool Is_Driving)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				int Driving = 0;
				/// 呼叫研華API方法取得馬達驅動狀態
				Result = MotionFunc.GetDrivingState(AxisHandle, ref Driving);
				if (Result != "")
				{
                    
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get Driving State>";
				}
				else
				{
					switch (Driving)
					{
						case 0: ///< No Driving
							Is_Driving = false;
							break;
						case 1: ///< Driving
							Is_Driving = true;
							break;
					}
					return "";
				}
			}
            else
                return errorMsg;
		}

		// Read EMC Status
		/// <summary>
		/// 讀取馬達EMC(緊急)狀態
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Is_EMC">判斷馬達是否處於EMC狀態</param>
		/// <returns>錯誤資訊</returns>
		public string EMCStatus(IntPtr AxisHandle, ref bool Is_EMC)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				int EMC = 0;
				/// 呼叫研華API方法取得EMC狀態
				Result = MotionFunc.GetEMCStatus(AxisHandle, ref EMC);
				if (Result != "")
				{
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get EMC Status>";
				}
				else
				{
					string ReturnMsg = "";
					switch (EMC)
					{
						case 0:
							Is_EMC = false;
							ReturnMsg = "";
							break;
						case 1:
							Is_EMC = true;
							ReturnMsg = "Alarm" + "-" + "08" + "-" + "EMC STOP" + "-" + "<Get EMC Status>";
							break;
					}
					return ReturnMsg;
				}
			}
            else
                return errorMsg;
		}

		// Is Home Search
		/// <summary>
		/// 判斷使否正在搜尋原點位置
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Is_Search">判斷是否正在搜尋的旗標</param>
		/// <returns>錯誤資訊</returns>
		public string IsHomeSearch(IntPtr AxisHandle, ref int Is_Search)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				Result = MotionFunc.IsHomeSearch(AxisHandle, ref Is_Search);
				if (Result != "")
				{
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Is HomeSearch>";
				}
			}
            else
                return errorMsg;
            return "";
		}

		/// <summary>
		/// 重設軸卡錯誤
		/// </summary>
		/// <returns>錯誤訊息</returns>
		public string AxisResetError(MotorBase src)
		{
			string Result = "";      ///< Result -- 紀錄錯誤資訊

			/// Is_PCI1240Open為True，表示有接到PCI1240軸卡
			if (MotionFunc.is_HardwareConnected)
			{
				/// 初始化用以儲存各軸狀態的字串
                string State_Axis = "";

				/// Axis X
                Result = MotionFunc.Get_Axis_State(src.AxisHandle, ref State_Axis);
				if (Result != "")
				{
                    Utils.LogHandler.LogWarning(Utils.LogHandler.Formatting(string.Format("Warning - 06 - {0} - <Get Axis State {1}>", Result, src.ToString()), ""));
					return Result;
				}

				/// 如果X軸狀態為"因為錯誤而停止"，呼叫Axis_ErrorReset方法
				if (State_Axis == "STA_AX_ERROR_STOP")
				{
					Result = MotionFunc.ErrorReset(src.AxisHandle);
					if (Result != "")
					{
                        Utils.LogHandler.LogWarning(Utils.LogHandler.Formatting(string.Format("Warning - 06 - {0} - <Rest Error {1}>", Result, src.ToString()), ""));
                        return Result;
					}
				}
			}
            else
                return errorMsg;
            return "";
		}

		/// <summary>
		/// 運動軸狀態
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="State">儲存操作軸的狀態</param>
		/// <returns>錯誤訊息</returns>
		public string AxisState(IntPtr AxisHandle, ref string State)
		{
			string Result = "";
			if (MotionFunc.is_HardwareConnected)
			{
				string temp = "";
				Result = MotionFunc.Get_Axis_State(AxisHandle, ref temp);
				if (Result != "")
				{
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Get Axis State>";
				}
				else
				{
					State = temp;
					return "";
				}
			}
            else
                return errorMsg;
            
		}

		/// <summary>
		/// 設定運動軸最大速度
		/// </summary>
		/// <param name="AxisHandle">運動軸控點</param>
		/// <param name="Speed">欲設定的最大速度</param>
		/// <returns>錯誤訊息</returns>
		public string AxisSetSpeed(IntPtr AxisHandle, double Speed)
		{
			string Result = "";    ///< Result -- 儲存錯誤訊息
			if (MotionFunc.is_HardwareConnected)
			{
				Result = MotionFunc.SetDrivingSpeed(AxisHandle, Speed);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Set Driving Speed>";
				else
					return "";
			}
            else
                return errorMsg;
		}

		/// <summary>
		/// 重設伺服馬達驅動器
		/// DO07腳位接什麼?
		/// </summary>
		/// <param name="AxisHandle">運動軸的控點</param>
		/// <returns>錯誤訊息</returns>
		public string AxisServoReset(IntPtr AxisHandle)
		{
			string Result = "";     ///< Result -- 儲存錯誤訊息
			if (MotionFunc.is_HardwareConnected)
			{
				/// 使操作軸(伺服馬達)Servo ON
				Result = MotionFunc.ResetServoAlarm(AxisHandle, true);
				if (Result != "")
					return "Warning" + "-" + "06" + "-" + Result + "-" + "<Reset Servo Alarm>";
				else
				{
					/// 系統休息0.1秒後，使操作軸(伺服馬達)Servo OFF
					Thread.Sleep(100);
					Result = MotionFunc.ResetServoAlarm(AxisHandle, false);
					if (Result != "")
						return "Warning" + "-" + "06" + "-" + Result + "-" + "<Reset Servo Alarm>";
				}
				return "";
			}
            else
                return errorMsg;
		}

		/// <summary>
		/// 確認供粉軸(X軸)位置
		/// </summary>
		/// <returns>
		/// true  -- 供粉軸位於X的工作範圍內
		/// false -- 供粉軸超出X的工作範圍 
		/// </returns>
		//public static bool Check_AxisXPosition()
		//{
		//	double Encoder_X = Recoater.getInstance().Status.CommandEncoder;
		//	if ((Encoder_X > g_WarningArea.X_WarningZone_Left) & (Encoder_X < g_WarningArea.X_WarningZone_Right))
		//		return true;
		//	else
		//		return false;
		//}

		/// <summary>
		/// 確認工作軸(Z軸)位置
		/// </summary>
		/// <returns>
		/// true  -- 工作軸位置大於程序所設定的初始位置
		/// false -- 工作軸位置於程序設定的初始位置之外 
		/// </returns>
		//public static bool Check_AxisZPosition()
		//{
		//	double Encoder_Z = AxisInfo.AxisZ.par_Status.CommandEncoder;
		//	if (Encoder_Z > g_Procedure.m_ProcedurePara.WorkAxis_Ini_Pos)
		//		return true;
		//	else
		//		return false;
		//}
	}


}



