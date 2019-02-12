using System;

namespace MotorCtrl
{
	public class ErrorHandle
	{
		private static ErrorHandle ErrorMsg = new ErrorHandle();

		private ErrorHandle() {}

		public static ErrorHandle getInstance()
		{
			return ErrorMsg;
		}


	}
}

