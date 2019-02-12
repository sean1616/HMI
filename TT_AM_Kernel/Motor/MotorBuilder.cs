using System;
using System.IO;
using Newtonsoft.Json;
using Advantech.Motion;
using AM_Kernel.Utils;

namespace AM_Kernel.Motor
{
	class DeviceInfo
	{
		public static DEV_LIST[] DevList = 	new DEV_LIST[Motion.MAX_DEVICES];
		public static uint DevCount = 0;
		public static uint DevNum = 0;

		public static IntPtr DevHandle = IntPtr.Zero;
	}

	public class MotorPara {
		
		public MotorPara.Hardware paraHardware = new MotorPara.Hardware();
		public MotorPara.Motion paraMotion = new MotorPara.Motion();
		public MotorPara.Home paraHome = new MotorPara.Home();
		public MotorPara.PhysicalPara paraPhysical = new MotorPara.PhysicalPara();
		public MotorPara.LimitPostion paramLimitPos = new MotorPara.LimitPostion();

		public struct Hardware {
			public int PulseOut;
			public int PulseIn;
			public int Limit_Logic;
			public int Limit_StopAction;
			public int INP_Enable;
			public int INP_Logic;
			public int Alarm_Enable;
			public int Alarm_Logic;
		}

		public struct Motion {
			#region FT
			public double FT_MaxVal;
			public double FT_MaxAcc;
			public double FT_MaxDec;
			#endregion

			#region CFG
			private uint _CFG_PPU;
			private double _CFG_MaxVal;
			private double _CFG_MaxAcc;
			private double _CFG_MaxDec;

			public uint CFG_PPU {
				get{ return _CFG_PPU; }
				set 
				{
					if (value < 1)
						value = 1;
					else if (value > 500)
						value = 500;
					_CFG_PPU = value;
				}
			}

			public double CFG_MaxVal
			{
				get { return _CFG_MaxVal; }
				set {
					if (value > (FT_MaxVal / CFG_PPU))
						value = FT_MaxVal / CFG_PPU;
					else if (value < (8000 / CFG_PPU))
						value = 8000 / CFG_PPU;
					_CFG_MaxVal = value;
				}
			}

			public double CFG_MaxAcc
			{
				get { return _CFG_MaxAcc; }
				set {
					if (value > Math.Min ((FT_MaxAcc / CFG_PPU), (125 * CFG_MaxVal)))
						value = Math.Min ((FT_MaxAcc / CFG_PPU), (125 * CFG_MaxVal));
					else if (value < (125 / CFG_PPU))
						value = 125 / CFG_PPU;
					_CFG_MaxAcc = value;
				}
			}
			public double CFG_MaxDec
			{
				get { return _CFG_MaxDec; }
				set {
					if (value > Math.Min ((FT_MaxDec / CFG_PPU), (125 * CFG_MaxVal)))
						value = Math.Min ((FT_MaxDec / CFG_PPU), (125 * CFG_MaxVal));
					else if (value < (125 / CFG_PPU))
						value = 125 / CFG_PPU;
					_CFG_MaxDec = value;
				}
			}
			#endregion

			public int TSCurve;

			#region PAR
			private double _Speed_Initial;
			private double _Speed_Driving;
			private double _Acc;
			private double _Dec;

            public double Speed_Driving
            {
                get { return _Speed_Driving; }
                set
                {
                    if (value > CFG_MaxVal)
                        value = CFG_MaxVal;
                    else if (value < Speed_Initial)
                        value = Speed_Initial;
                    _Speed_Driving = value;
                }
            }

            public double Speed_Initial {
				get { return _Speed_Initial; }
				set {
					if (value > Speed_Driving)
						value = Speed_Driving;
					else if (value < (CFG_MaxVal / 8000))
						value = CFG_MaxVal / 8000;
					_Speed_Initial = value;
				}
			}

			public double Acc
			{
				get { return _Acc; }
				set {
					if (value > CFG_MaxAcc)
						value = CFG_MaxAcc;
					else if (value < (CFG_MaxVal / 64))
						value = CFG_MaxVal / 64;
					_Acc = value;
				}
			}

			public double Dec
			{
				get { return _Dec; }
				set {
					if (value > CFG_MaxDec)
						value = CFG_MaxDec;
					else if(value < (CFG_MaxVal / 64))
						value = CFG_MaxVal / 64;
					_Dec = value;
				}
			}
			#endregion

			public double Speed_Jog;

            public double BackOff_Dis;
		}

		#region Home
		public struct Home
		{
			public int HomeMode;
			public int HomeResetEnable;
			public double HomeShift;
			public int HomeDir;
            public double HomeSpeed;
		}
		#endregion

		#region Default
		public struct PhysicalPara
		{
			public int Pitch;
			public int Resolution;
			public int GearRatio;
			public double Gain;
		}
		#endregion

		public struct LimitPostion
		{
			public int P_LimitPos;
			public int N_LimitPos;
		}
	}

	#region Status
	public struct MotorStatus
	{
		public bool P_Limit;
		public bool N_Limit;
		public bool H_Limit;
		public bool isAlarm;
		public bool isINP;
		public bool isDriving;
		public bool isEMC;

		public double CommandEncoder;
		public double ActualEncoder;
	}
	#endregion

	public abstract class MotorBase : IConfig {
		public IntPtr AxisHandle;
		public MotorStatus Status;
		private MotorPara parameters;
		private double _gain;

		public object GetConfig()
		{
            // For Debug use.
            if (parameters == null)
                parameters = new MotorPara();

			return parameters;
		}

		public void ReadConfig(string filePath)
		{
            if (parameters != null)
                parameters = null;
            parameters = JsonConvert.DeserializeObject<MotorPara>(System.IO.File.ReadAllText(filePath));
        }

    }

	// Singleton pattern
	public class Recoater : MotorBase, IConfig
	{
		private static Recoater xAxis = new Recoater();
		private static bool isServoRDY = false;

		public static LinearMechUnitTransform UnitTransform = new LinearMechUnitTransform(xAxis);
		private Recoater() { }

		public static Recoater getInstance()
		{
			return xAxis;
		}

		public static void setServoRDY(bool servoOn)
		{
			isServoRDY = servoOn;	
		}

		public static bool getServoRDY() {
			return isServoRDY;
		}

        public override string ToString()
        {
            return "Recoater";
        }

    }

	public class BuildPlatform : MotorBase, IConfig
	{
		private static BuildPlatform zAxis = new BuildPlatform();
		private static bool isServoRDY = false;

		public static LinearMechUnitTransform UnitTransform = new LinearMechUnitTransform(zAxis);

		private BuildPlatform() {
		}

		public static BuildPlatform getInstance() {
			return zAxis;
		}

		public static bool getServoRDY() {
			return isServoRDY;
		}

        public override string ToString()
        {
            return "Build Platform";
        }
	}


    public class LeftFeeder : MotorBase, IConfig
    {
        private static LeftFeeder U1_Axis = new LeftFeeder();
        //private static bool isServoRDY = false;

        public static LinearMechUnitTransform UnitTransform = new LinearMechUnitTransform(U1_Axis);

        private LeftFeeder()
        {
        }

        public static LeftFeeder getInstance()
        {
            return U1_Axis;
        }

        public override string ToString()
        {
            return "Left Feeder";
        }
    }

    public class RightFeeder : MotorBase, IConfig
    {
        private static RightFeeder U2_Axis = new RightFeeder();
        //private static bool isServoRDY = false;

        public static LinearMechUnitTransform UnitTransform = new LinearMechUnitTransform(U2_Axis);

        private RightFeeder()
        {
        }

        public static RightFeeder getInstance()
        {
            return U2_Axis;
        }

        public override string ToString()
        {
            return "Right Feeder";
        }
    }
}

