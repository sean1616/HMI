using System;
using System.IO;
using Newtonsoft.Json;
using AM_Kernel.Utils;

namespace AM_Kernel.Periphery
{
	public class PeripherySetting : IConfig
	{
		private static PeripherySetting device = new PeripherySetting();
		private static int[] _IO_Card;

		public int[] IO_Card
		{
		    set { _IO_Card = value; }
			get { return _IO_Card;}
		}

		private static string[] _COM_Port;

		public string[] COM_Port
		{
		    set { _COM_Port = value; }
			get { return _COM_Port; }
		}

		// Set default value. Currently We don't set default.
		private PeripherySetting()
		{
			
		}

		public static PeripherySetting GetInstance()
		{
			return device;
		}

		// use for output .json file.
		protected void OutputJson()
		{
			using (StreamWriter sw = new StreamWriter(@"PeripheryConfig.json"))
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				new JsonSerializer().Serialize(writer, device);
			}
		}

		public void ReadConfig(string filePath)
		{
			string json = System.IO.File.ReadAllText(filePath);
			device = JsonConvert.DeserializeObject<PeripherySetting>(json);
		}

        public object GetConfig()
        {
            return device;
        }
	}
}

