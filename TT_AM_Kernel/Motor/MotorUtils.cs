using System;
using Advantech.Motion;
using System.Threading;
using System.Threading.Tasks;

namespace AM_Kernel.Motor
{
    enum MotorTarget
    {
        None = 0,
        Driving_OK = 1,
        INP_OK = 2,
        HomeSearch_OK = 3
    }

	// Dev_List尚需建立底層物件
	public interface MotorFuncSupplier
	{
		bool is_HardwareConnected { get; set; } 
		uint Result { get; set; }

		string GetAvailableDevs(DEV_LIST[] List, ref uint Count);
		string GetErrorMsg(uint ErrorCode);

		string OpenDevice(uint DevNum, ref IntPtr DevHandle);
		string CloseDevice(IntPtr DevHandle);

		string OpenAxis(IntPtr DevHandle, ushort Axis, ref IntPtr AxisHandle);
		string CloseAxis(IntPtr AxisHandle);

		string SetPulseOut(IntPtr AxisHandle, int Pulse_OUT_Type);
		string GetPulseOut(IntPtr AxisHandle, ref int Pulse_OUT_Type);

		string SetPulseIn(IntPtr AxisHandle, int Pulse_IN_Type);
		string GetPulseIn(IntPtr AxisHandle, ref int Pulse_IN_Type);

		string SetInitialSpeed(IntPtr AxisHandle, double IniSpeed);
		string GetInitialSpeed(IntPtr AxisHandle, ref double IniSpeed);

		string SetDrivingSpeed(IntPtr AxisHandle, double DrivingSpeed);
		string GetDrivingSpeed(IntPtr AxisHandle, ref double DrivingSpeed);

		string SetAcceleration(IntPtr AxisHandle, double Acc);
		string GetAcceleration(IntPtr AxisHandle, ref double Acc);

		string SetDeceleration(IntPtr AxisHandle, double Dec);
		string GetDeceleration(IntPtr AxisHandle, ref double Dec);

		string SetCurve(IntPtr AxisHandle, int CurveType);
		string GetCurve(IntPtr AxisHandle, ref int CurveType);

		string SetLimit(IntPtr AxisHandle, int ActiveType);
		string GetLimit(IntPtr AxisHandle, ref int ActiveType);

		string SetReactingMode(IntPtr AxisHandle, int ReactingType);
		string GetReactingMode(IntPtr AxisHandle, ref int ReactingType);

		string SetDOEnable(IntPtr AxisHandle, int ActiveType);
		string SetServoOn(IntPtr AxisHandle, int Active);

		string GetServoRDY(IntPtr AxisHandle, ref int Active);
		string SetINPEnable(IntPtr AxisHandle, int EnableType);
		string GetINPEnable(IntPtr AxisHandle, ref int EnableType);

		string SetINPLogic(IntPtr AxisHandle, int ActiveType);
		string GetINPLogic(IntPtr AxisHandle, ref int ActiveType);

		string SetAlarmEnable(IntPtr AxisHandle, int EnableType);
		string GetAlarmEnable(IntPtr AxisHandle, ref int EnableType);
		string SetAlarmLogic(IntPtr AxisHandle, int ActiveType);

		string GetLimitStatus(IntPtr AxisHandle, ref int PLimit, ref int NLimit);
		string GetHomeStatus(IntPtr AxisHandle, ref int HomeLimit);

		string GetINPStatus(IntPtr AxisHandle, ref int Is_INP);
		string GetAlarmStatus(IntPtr AxisHandle, ref int Is_Alarm);

		string GetEMCStatus(IntPtr AxisHandle, ref int Is_EMC);
		string GetDrivingState(IntPtr AxisHandle, ref int Is_Driving);

		string GetDrivingState(IntPtr AxisHandle, ref string state, ref uint Status_num);
		string SetCommandEncoder(IntPtr AxisHandle, double Command_Position);
		string SetActualEncoder(IntPtr AxisHandle, double Actual_Position);

		string GetCommandEncoder(IntPtr AxisHandle, ref double Command_Encoder);
		string GetActualEncoder(IntPtr AxisHandle, ref double Actual_Encoder);

		string MoveABS(IntPtr AxisHandle, double Position);
		string MoveINC(IntPtr AxisHandle, double Distance);
		string MoveStop(IntPtr AxisHandle, int StopType);
		string MoveContinuous(IntPtr AxisHandle, int Dir);
		string Multi_SetAxisGroup(ref IntPtr GpHandle, IntPtr AxisHandle);
		string Multi_CloseGroup(IntPtr GpHandle);

		string Multi_GroupRemoveAxis(IntPtr GpHandle, IntPtr AxisHandle);
		string HomeSearch(IntPtr AxisHandle, int mode, int Dir);
		string HomeSetResetEncoder(IntPtr AxisHandle, int ActiveType);
		string HomeGetResetEncoder(IntPtr AxisHandle, ref int ActiveType);
		string HomeShiftSet(IntPtr AxisHandle, double ShiftValue);
		string HomeShiftGet(IntPtr AxisHandle, double ShiftValue);
		string IsHomeSearch(IntPtr AxisHandle, ref int Is_Search);
		string Get_CFG_PPU(IntPtr AxisHandle, ref uint PPU);
		string Set_CFG_PPU(IntPtr AxisHandle, uint PPU);
		string Get_CFG_MaxVel(IntPtr AxisHandle, ref double MaxVel);
		string Set_CFG_MaxVel(IntPtr AxisHandle, double MaxVel);
		string Get_CFG_MaxAcc(IntPtr AxisHandle, ref double MaxAcc);
		string Set_CFG_MaxAcc(IntPtr AxisHandle, double MaxAcc);
		string Get_CFG_MaxDec(IntPtr AxisHandle, ref double MaxDec);

		string Set_CFG_MaxDec(IntPtr AxisHandle, double MaxDec);
		string Get_FT_MaxVel(IntPtr AxisHandle, ref double FT_MaxVel);
		string Get_FT_MaxAcc(IntPtr AxisHandle, ref double FT_MaxAcc);
		string Get_FT_MaxDec(IntPtr AxisHandle, ref double FT_MaxDec);
		string Get_Axis_State(IntPtr AxisHandle, ref string State);
		string GetHomeLogic(IntPtr AxisHandle, ref int ActiveType);
		string SetHomeLogic(IntPtr AxisHandle, int ActiveType);
		string ErrorReset(IntPtr AxisHandle);

		string ResetServoAlarm(IntPtr AxisHandle, bool Is_ON);

	}

	interface UnitTransformer
	{
		double Pulse2MM(double Pulse);
		double MM2Pulse(double MM);
	}

	public class LinearMechUnitTransform : UnitTransformer
	{
		private MotorBase motor;
		public LinearMechUnitTransform(MotorBase src)
		{
			motor = src;
		}

		public double Pulse2MM(double Pulse)
		{
			MotorPara tmp = (MotorPara)motor.GetConfig();

			return Pulse / (tmp.paraPhysical.Resolution * 
			                tmp.paraPhysical.GearRatio) * tmp.paraPhysical.Pitch;
		}

		public double MM2Pulse(double MM)
		{
			MotorPara tmp = (MotorPara)motor.GetConfig();

			return (MM / tmp.paraPhysical.Pitch) * 
				(tmp.paraPhysical.GearRatio * tmp.paraPhysical.Resolution);
		}
	}

    public class MotorUtils
    {
        public static bool wait(MotorBase target, int targetStatus, int maxWaitTime, out int elapsTime)
        {
            DateTime started = DateTime.Now;
            int m_statusDelay = 100;   // ms
            elapsTime = 0;
            bool timeOut = false;
            int status = 0;

            do
            {
                // TODO: Add code for receiving status.
                status = sort_MotorStatus(targetStatus, target);

                if (elapsTime == 0)
                    Console.WriteLine(String.Format("First Scanner status {0} target {1}", status, targetStatus));

                DateTime finished = DateTime.Now;

                // Tick unit: ms
                elapsTime = (int)new TimeSpan(finished.Ticks - started.Ticks).TotalSeconds;

                if (elapsTime > maxWaitTime)
                    timeOut = true;

                Thread.Sleep(m_statusDelay);

                //sb.AppendLine(String.Format("Current time: {0:O}. Elapse time: {1:D}", DateTime.Now, elapsTime));
                Console.WriteLine(String.Format("Current time: {0:O}. Elapse time: {1:D}", DateTime.Now, elapsTime));

            } while (!(status == targetStatus || timeOut == true));  //< De-morgan law. (Can't use status != targetStatus || ...)


            return timeOut;
        }

        private static int sort_MotorStatus(int type, MotorBase src)
        {
            bool status = false;
            int HomeSearch_mode = 0;
            int ret = 0;

            switch (type)
            {
                case (int)MotorTarget.Driving_OK:
                    MotorFunc4PCI1240.getInstance().DrivingStatus(src.AxisHandle, ref status);
                    if (!status)
                        ret = (int)MotorTarget.Driving_OK;
                    break;
                case (int)MotorTarget.INP_OK:
                    MotorFunc4PCI1240.getInstance().INPStatus(src.AxisHandle, ref status);
                    if (status)
                        ret = (int)MotorTarget.INP_OK;
                    break;
                case (int)MotorTarget.HomeSearch_OK:
                    MotorFunc4PCI1240.getInstance().IsHomeSearch(src.AxisHandle, ref HomeSearch_mode);
                    if (HomeSearch_mode == 0)
                        ret = (int)MotorTarget.HomeSearch_OK;
                    break;
            }

            return ret;

        }

        private static void GoHome(MotorBase src)
        {
            string result = string.Empty;
            int elpaseTime = 0;

            // 動作尚須測試
            if (src.Status.P_Limit)
                {
                    MotorFunc4PCI1240.getInstance().AxisResetError(src);
                    result = MotorFunc4PCI1240.getInstance().MoveINC(src.AxisHandle, -(src.GetConfig() as MotorPara).paraMotion.BackOff_Dis);
                    if (result != "")
                    {
                        MotorFunc4PCI1240.getInstance().MoveSTOP(src.AxisHandle);
                        return;
                    }

                }
                else if (src.Status.N_Limit)
                {
                    MotorFunc4PCI1240.getInstance().AxisResetError(src);
                    result = MotorFunc4PCI1240.getInstance().MoveINC(src.AxisHandle, (src.GetConfig() as MotorPara).paraMotion.BackOff_Dis);
                    if (result != "")
                    {
                        MotorFunc4PCI1240.getInstance().MoveSTOP(src.AxisHandle);
                        return;
                    }
                }

                MotorUtils.wait(src, (int)MotorTarget.Driving_OK, 10000, out elpaseTime);

                if (!src.Status.isDriving)
                {
                    if(src.ToString() == "Recoater" || src.ToString() == "Build Platform")
                        MotorUtils.wait(src, (int)MotorTarget.INP_OK, 10000, out elpaseTime);

                    MotorFunc4PCI1240.getInstance().AxisSetSpeed(src.AxisHandle, (src.GetConfig() as MotorPara).paraHome.HomeSpeed);

                    result = MotorFunc4PCI1240.getInstance().HomeSearch(src.AxisHandle,
                                     (src.GetConfig() as MotorPara).paraHome.HomeMode,
                                     (src.GetConfig() as MotorPara).paraHome.HomeDir);
                    if (result != "")
                    {
                        MotorFunc4PCI1240.getInstance().MoveSTOP(src.AxisHandle);
                        return;
                    }

                    wait(src, (int)MotorTarget.HomeSearch_OK, 10000, out elpaseTime);
                    MotorFunc4PCI1240.getInstance().MoveSTOP(src.AxisHandle);
                
                if(src.Status.P_Limit || src.Status.N_Limit)
                {
                    MotorFunc4PCI1240.getInstance().AxisResetError(src);
                }
            }
                else
                    return;
        }

        public static Task GoHomeAsync(MotorBase src)
        {
            return Task.Run(() => GoHome(src));
        }
    }
}

