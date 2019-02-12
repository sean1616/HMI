using System;
using System.Threading;

namespace AMEntity
{
    public enum AMProcedureDefCorpse
    {
        IN_PROCESSING = -1,
        PROCESSING_PAUSE = 0,

        INIT_LASER_SCANNING_PROCESSING = 1,
        IN_LASER_SCANNING_PROCESSING = 2,

        AUTOPOWDER_PROVIDE = 3,
        AUTOPOWDER_PROVIDE_DONE = 4,

        PODWER_SUPPLY = 5,
        PODWER_SUPPLY_DONE = 6,

        LASER_SCANNING = 7,
        LASER_SCANNING_DONE = 8,

        POWDER_FILLING_INIT = 9,
        POWDER_FILLING_INIT_DONE = 10,

        POWDER_FILLING_END = 11,
        POWDER_FILLING_END_DONE = 12,

        BUILDPLATFORM_DECLINE = 13,
        BUILDPLATFORM_DECLINE_DONE = 14

    }

    public enum AMProcedureDef
    {
        IN_PROCESSING = -1,
        PROCESSING_DONE = 0,

        LASER_SCANNING = 1,
        LASER_SCANNING_DONE = 2,
        BUILDPLATFORM_DECLINE = 3,
        BUILDPLATFORM_DECLINE_DONE = 4,
        POWDER_SUPPLY = 5,
        POWDER_SUPPLY_DONE = 6,
        POWDER_FILLING = 7,
        POWDER_FILLING_DONE = 8,
        AUTOPOWDER_PROVIDE = 9,

    }

    public class AMProcedureUtils
	{
		/// <summary>
		/// Wait the specified targetStatus, maxWaitTime and elapsTime.
		/// The type of target status isn't sure. Need to check the AdvMot.dll. 
		/// </summary>
		/// <param name="targetStatus">Target status.</param>
		/// <param name="maxWaitTime">Max wait time.</param>
		/// <param name="elapsTime">Elaps time.</param>
		public static bool wait(int targetStatus, int maxWaitTime, out int elapsTime)
		{
			DateTime started = DateTime.Now;
			int m_statusDelay = 100;   // ms
			elapsTime = 0;
			bool timeOut = false;
			int status = 0;

			do
			{
                // TODO: Add code for receiving status.
                status = AM_Kernel.Procedure.AMProcedure.GetInstance().Step;

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
	
	}
}

