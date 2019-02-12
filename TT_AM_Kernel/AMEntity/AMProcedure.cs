using System;
using Newtonsoft.Json;
using AM_Kernel.Motor;
using AM_Kernel.Utils;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using AM_Kernel.LaserCtrl;
using AM_Kernel.Interpretation;
using AM_Kernel.IO;

namespace AM_Kernel.Procedure
{
	public enum PowderSupplyDir
	{
		Postive = 1,
		Negative = 2,
		HomeReturn = 3
	}

	public enum PowderFilledMode
	{
		GotoFilledPosMode = 1,
		GotoRecoaterInitPosMode = 2
	}

	public class ProcessParam
	{
		// the initial position of recoater.
		public double RecoaterInitialPos;
		// the distance that recoater travel by to feed the powder.
		public double PowderRouteDist;
		// speed of feeding.
		public double PowderFeedingSpeed;
		// the initial postion of build platform.
		public double BuildPlatformInitialPos;
		// the inch movement of build platform.
		public double BuildPlatformInchDist;
		// filled position of powder.
		public double PowderFilledPos;
		// speed of filling powder.
		public double PowderFilledSpeed;
		// the duration of fiilling powder.
		public double PowderFilledDuration;
        // the filling way of powder
        public int PowderFillWay;
        // the concentration of Oxygen.
        public double OxygenConcentration;
		// the allowable range of oxygen.
		public double OxygenRange;
		// the circulatory flow rate.
		public double CirculatoryFlowRate;
		// the allowable circulatory flow rate.
		public double CirculatoryRange;
		// the temperature of plate.
		public double PlateTemp;
		// the allowable temperature of plate.
		public double PlateTempRange;

        public int PowderCntToFill;
	}

	public class AMProcedure : IConfig
	{
		private static ProcessParam param = new ProcessParam();

		private static AMProcedure process = new AMProcedure();
        
		private int _Step = (int)AMEntity.AMProcedureDef.PROCESSING_DONE;
        private int _currentScanLayerIdx = 0;
        public int CurrentScanLayerIdx
        {
            get { return _currentScanLayerIdx; }
            set { _currentScanLayerIdx = value; }
        }

		public int Step
		{
			get { return _Step; }
			set { _Step = value; }
		}

		private AMProcedure() { }

		public static AMProcedure GetInstance()
		{
			return process;
		}

		public object GetConfig()
		{
            return param;
		}

        public void SaveConfig(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                new JsonSerializer().Serialize(writer, param);
            }
        }

		public void ReadConfig(string filePath)
		{
			param = JsonConvert.DeserializeObject<ProcessParam>(System.IO.File.ReadAllText(filePath));
		}

		private string PowderSupply()
		{
			string Result = "";

			// distance between current postion of recoater and Positive limit. 
			double DistanceFrPLimit = Math.Abs(((MotorPara)Recoater.getInstance().GetConfig()).paramLimitPos.P_LimitPos 
			                                   - Recoater.getInstance().Status.CommandEncoder);
			
			// distance between current postion of recoater and Negative limit.
			double DistanceFrNLimit = Math.Abs(Recoater.getInstance().Status.CommandEncoder - 
			                                   ((MotorPara)Recoater.getInstance().GetConfig()).paramLimitPos.N_LimitPos);


			double PowderRouteDist = Recoater.UnitTransform.MM2Pulse(param.PowderRouteDist);

			int SuppliedMode = 0;

			#region identify the direction of powder supply.
			if ((DistanceFrPLimit > PowderRouteDist) & DistanceFrNLimit > PowderRouteDist)
			{
				if (DistanceFrPLimit > DistanceFrNLimit)
					SuppliedMode = (int)PowderSupplyDir.Postive;
				else
					SuppliedMode = (int)PowderSupplyDir.Negative;
			}

			if ((DistanceFrNLimit > PowderRouteDist) & (DistanceFrPLimit <= PowderRouteDist))
				SuppliedMode = (int)PowderSupplyDir.Negative;
			else if ((DistanceFrPLimit > PowderRouteDist) & (DistanceFrNLimit <= PowderRouteDist))
				SuppliedMode = (int)PowderSupplyDir.Postive;
			else if ((DistanceFrNLimit <= PowderRouteDist) & (DistanceFrPLimit <= PowderRouteDist))
				SuppliedMode = (int)PowderSupplyDir.HomeReturn;

            #endregion
            double speed = Recoater.UnitTransform.MM2Pulse(param.PowderFeedingSpeed);

            Result = MotorFunc4PCI1240.getInstance().AxisSetSpeed(Recoater.getInstance().AxisHandle, speed);
			if (Result != "")
				ErrorInfo(Result, "供粉速度設定");

			switch (SuppliedMode)
			{
				case (int)PowderSupplyDir.Postive:
					if (Recoater.getInstance().Status.CommandEncoder > 0)
					{
						Console.WriteLine("Please make recoater homing.");
						break;
					}
					Result = MotorFunc4PCI1240.getInstance().MoveINC(Recoater.getInstance().AxisHandle, PowderRouteDist);
					if (Result != "")
						ErrorInfo(Result, "正方向供粉");
					break;
					
				case (int)PowderSupplyDir.Negative:
					if (Recoater.getInstance().Status.CommandEncoder - PowderRouteDist > 5)
					{
						Console.WriteLine("Please make recoater homing.");
						break;
					}
					Result = MotorFunc4PCI1240.getInstance().MoveINC(Recoater.getInstance().AxisHandle, -PowderRouteDist);
					if (Result != "")
						ErrorInfo(Result, "負方向供粉");
					break;

				case (int)PowderSupplyDir.HomeReturn:
					Console.WriteLine("Please make recoater homing.");
					break;
			}
            int elapseTime = 0;
            MotorUtils.wait(Recoater.getInstance(), (int)MotorTarget.Driving_OK, 10000, out elapseTime);


            return Result;
		}

		private string BuildPlatformDecline()
		{
			string Result = "";

            double InchPos = BuildPlatform.UnitTransform.MM2Pulse(param.BuildPlatformInchDist);

            Result = MotorFunc4PCI1240.getInstance().MoveINC(BuildPlatform.getInstance().AxisHandle, -InchPos);
			if (Result != "")
				ErrorInfo(Result, "工作軸下降");

            int elapseTime = 0;
            MotorUtils.wait(BuildPlatform.getInstance(), (int)MotorTarget.Driving_OK, 10000, out elapseTime);

            return Result;
		}

		private string PowderFilled()
		{
			string Result = "";
            double speed = Recoater.UnitTransform.MM2Pulse(param.PowderFilledSpeed);

            Result = MotorFunc4PCI1240.getInstance().AxisSetSpeed(Recoater.getInstance().AxisHandle, speed);
			if (Result != "")
				ErrorInfo(Result, "補粉速度設定");

            //if (Recoater.getInstance().Status.CommandEncoder != 0.0)
            //    return "-1";

            double powderFilledPos = Recoater.UnitTransform.MM2Pulse(param.PowderFilledPos);
            Result = MotorFunc4PCI1240.getInstance().MoveABS(Recoater.getInstance().AxisHandle, powderFilledPos);
			ErrorInfo(Result, "補粉位置");

            int elapsT = 0;
            Motor.MotorUtils.wait(Recoater.getInstance(), (int)Motor.MotorTarget.Driving_OK, 100000, out elapsT);

            AutoPowderProvide();

            double initialPos = Recoater.UnitTransform.MM2Pulse(param.RecoaterInitialPos);
            Result = MotorFunc4PCI1240.getInstance().MoveABS(Recoater.getInstance().AxisHandle, initialPos);
	        ErrorInfo(Result, "移動至供粉軸初始位置");

            int elapseTime = 0;
            MotorUtils.wait(Recoater.getInstance(), (int)MotorTarget.Driving_OK, 10000, out elapseTime);

            return Result;	
		}

        private string AutoPowderProvide()
        {
            DIOUtility.DOSignalOut(DIO_DevType.Device2, true, DO_Set.employees["DO60"]);
            Thread.Sleep(1000);
            DIOUtility.DOSignalOut(DIO_DevType.Device2, false, DO_Set.employees["DO60"]);

            return "";
        }

		// Maybe can extract to global error handle function.
		private static string ErrorInfo(string Error, string Description)
		{
			return String.Format("{0}, {1}", Error, Description);
		}

        public Task<string> PowderSupplyAsync()
        {
            LogHandler.LogJournal(String.Format("{0:o} Powder Supply. ", DateTime.Now));
            return Task.Run(() => { return PowderSupply(); });
        }

        public Task<String> BuildPlatformDeclineAsync()
        {
            LogHandler.LogJournal(String.Format("{0:o} Build Platform Decline. ", DateTime.Now));
            return Task.Run(() => { return BuildPlatformDecline(); });
        }

        public Task<String> PowderFilledAsync()
        {
            //LogHandler.LogJournal(String.Format("{0:o} Powder Fill. ", DateTime.Now));
            return Task.Run(() => { return PowderFilled(); });
        }

        public Task<string> LaserScanAsync()
        {
            LogHandler.LogJournal(String.Format("{0:o} Laser Scanning. ", DateTime.Now));
            if (SlicingObj.getLayers().getLayers() == null)
                return Task.Run(() => { return "Null object"; });

            ScanSystem.PreloadLayer(SlicingObj.getLayers().getLayers()[_currentScanLayerIdx], (uint)_currentScanLayerIdx);
            return ScanSystem.ScanLayerAsync();
        }

        #region Flow test
        //public Task<string> PowderSupplyAsync()
        //{
        //    return Task.Run(() => {
        //        Thread.Sleep(2000);
        //        LogHandler.LogJournal(String.Format("{0:o} Powder supply ", DateTime.Now)); return ""; });
        //}

        //public Task<String> BuildPlatformDeclineAsync()
        //{
        //    return Task.Run(() => {
        //        Thread.Sleep(500);
        //        LogHandler.LogJournal(String.Format("{0:o} BuildPlatform Decline", DateTime.Now)); return ""; });
        //}

        //public Task<String> PowderFilledAsync()
        //{
        //    return Task.Run(() => {
        //        Thread.Sleep(300);
        //        LogHandler.LogJournal(String.Format("{0:o} Powder filled", DateTime.Now)); 
        //        return ""; 
        //    });
        //}

        //public Task<string> LaserScanAsync()
        //{
        //    return Task.Run(() => 
        //    {
        //        Thread.Sleep(3000);
        //        LogHandler.LogJournal(string.Format("{0:o} Laser scanning", DateTime.Now));
        //        return "";
        //    });
        //}
        #endregion

    }

    public class PowderSupplyCommand : IAMCommand
    {
        private AMProcedure process;

        public PowderSupplyCommand(AMProcedure src)
        {
            this.process = src;
        }

        public void Execute()
        {
            process.Step = (int)AMEntity.AMProcedureDef.IN_PROCESSING;
            if (process.PowderSupplyAsync().Result == "")
                process.Step = (int)AMEntity.AMProcedureDef.POWDER_SUPPLY_DONE;
        }

        public override string ToString()
        {
            return "PowderSupply";
        }
    }

    public class PowderFilledCommand : IAMCommand
    {
        private AMProcedure process;

        public PowderFilledCommand(AMProcedure src)
        {
            this.process = src;
        }

        public void Execute()
        {
            process.Step = (int)AMEntity.AMProcedureDef.IN_PROCESSING;
            if (process.PowderFilledAsync().Result == "")
                process.Step = (int)AMEntity.AMProcedureDef.POWDER_FILLING_DONE;
        }

        public override string ToString()
        {
            return "PowderFilled";
        }
    }

    public class BuildPlatformDeclineCommand : IAMCommand
    {
        private AMProcedure process;

        public BuildPlatformDeclineCommand(AMProcedure src)
        {
            this.process = src;
        }

        public void Execute()
        {
            process.Step = (int)AMEntity.AMProcedureDef.IN_PROCESSING;
            if (process.BuildPlatformDeclineAsync().Result == "")
                process.Step = (int)AMEntity.AMProcedureDef.BUILDPLATFORM_DECLINE_DONE;
        }

        public override string ToString()
        {
            return "BuildPlatformDecline";
        }
    }

    public class LaserScanCommand : IAMCommand
    {
        private AMProcedure process;

        public LaserScanCommand(AMProcedure src)
        {
            this.process = src;
        }

        public async void Execute()
        {
            await Task.Run(() => 
            {
                process.Step = (int)AMEntity.AMProcedureDef.IN_PROCESSING;
                if (process.LaserScanAsync().Result == "0")
                    process.Step = (int)AMEntity.AMProcedureDef.LASER_SCANNING_DONE;
            }); 
        }

        public override string ToString()
        {
            return "LaserScan";
        }
    }

    public class AM_CycleWorker
    {
        private Dictionary<int, IAMCommand> orders = new Dictionary<int, IAMCommand>();
        private int Cnt = 0;

        private List<int> General_Process_Seq = new List<int>()
        {
            (int)AMEntity.AMProcedureDef.LASER_SCANNING,
            (int)AMEntity.AMProcedureDef.BUILDPLATFORM_DECLINE,
            (int)AMEntity.AMProcedureDef.POWDER_SUPPLY,
        };

        private bool processTrigger = false;

        BackgroundWorker worker = new BackgroundWorker();

        private int _currentState;
        public int CurrentState { get { return _currentState; } set { _currentState = value; } }

        private int currentScanLayerIndex = -1;
        public int CurrentScanLayerIndex
        {
            get { return currentScanLayerIndex; }
            set { currentScanLayerIndex = value; }
        }

        // Default command
        public AM_CycleWorker()
        {
            addOrder(new BuildPlatformDeclineCommand(AMProcedure.GetInstance()), (int)AMEntity.AMProcedureDef.BUILDPLATFORM_DECLINE);
            addOrder(new PowderFilledCommand(AMProcedure.GetInstance()), (int)AMEntity.AMProcedureDef.POWDER_FILLING);
            addOrder(new PowderSupplyCommand(AMProcedure.GetInstance()), (int)AMEntity.AMProcedureDef.POWDER_SUPPLY);
            addOrder(new LaserScanCommand(AMProcedure.GetInstance()), (int)AMEntity.AMProcedureDef.LASER_SCANNING);

            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerSupportsCancellation = true;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int state = -1;
            Cnt = 0;
            if(_currentState != (int)AMEntity.AMProcedureDef.PROCESSING_DONE)
            {
                if (General_Process_Seq.Contains(_currentState))
                    state = General_Process_Seq.FindIndex(x => x == _currentState);
                else
                    state = General_Process_Seq.FindIndex(x => x == (int)AMEntity.AMProcedureDef.BUILDPLATFORM_DECLINE);
            }

            while (processTrigger)
            {
                if (state % General_Process_Seq.Count == General_Process_Seq.Count - 1)
                {
                    state = -1;
                }

                state++;

                AMProcedure.GetInstance().CurrentScanLayerIdx = currentScanLayerIndex;

                orders[General_Process_Seq[state]].Execute();

                int elapsT = 0;
                AMEntity.AMProcedureUtils.wait((General_Process_Seq[state])+1, Int32.MaxValue, out elapsT);

                _currentState = General_Process_Seq[state];

                if(_currentState == (int)AMEntity.AMProcedureDef.LASER_SCANNING)
                {
                    currentScanLayerIndex++;
                }    

                if (General_Process_Seq[state] == (int)AMEntity.AMProcedureDef.POWDER_SUPPLY)
                {
                    Cnt++;
                }

                if (Cnt % 2 == 0 && Cnt != 0/*(AMProcedure.GetInstance().GetConfig() as ProcessParam).PowderCntToFill*/)
                {
                    Task.Run(() => 
                    {
                        Cnt = 0;
                        orders[(int)AMEntity.AMProcedureDef.POWDER_FILLING].Execute();
                    });
                }

            }
        }

        public void addOrder(IAMCommand command, int cmd_name)
        {
            orders.Add(cmd_name, command);
        }

        public void ExecuteSpecifiedOrders(int command_Name)
        {
            IAMCommand cmd = orders[command_Name];
            _currentState = command_Name;
            cmd.Execute();
        }
       
        public void Trigger()
        {
            processTrigger = true;
            if (worker.IsBusy)
                return;
            worker.RunWorkerAsync();
        }

        public void PauseTrigger()
        {
            processTrigger = false;
            worker.CancelAsync();
        }

        public void ClearTrigger()
        {
            processTrigger = false;
            _currentState = 0;
            worker.CancelAsync();
        }
    }
}

