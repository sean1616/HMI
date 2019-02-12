using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using AM_Kernel.Utils;

namespace AM_Kernel.Periphery
{
	public interface ICOMFactory
	{
		ICOMProvider CreateCOM();    
	}

	public interface ICOMProvider
	{
		bool isReceiving { set; get; } 
		bool isOpen { set; get; }

        void DataReceiving();
        string byte2Text(byte[] buffer);

    }

	public class ADAM4117Factory : ICOMFactory
	{
		private string _PortName;

		public ADAM4117Factory(string src)
		{
			_PortName = src;
		}

		public ICOMProvider CreateCOM()
		{
			return new EnviroDectector(_PortName);
		}
        
	}

    public class HeaterFactory : ICOMFactory
    {
        private string _PortName;

        public HeaterFactory(string src)
        {
            _PortName = src;
        }

        public ICOMProvider CreateCOM()
        {
            return new HeaterMonitor(_PortName);
        }
    }
	public class EnviroDectector : ICOMProvider
	{
		private static SerialPort ADAM4117;
		private static Thread ADAM_Read;
		private static bool _isReceiving = false;
		private static bool _isOpen = false;
        private string enviorData = "";

        const Int32 S = 62;   // Start byte
        const Int32 E = 13;   // End byte


		public bool isOpen
		{
			get
			{
				return _isOpen;
			}

			set
			{
				_isOpen = value;
			}
		}

		public bool isReceiving
		{
			get { return _isReceiving; }
			set { _isReceiving = value; }
		}

        public string EnviroData
        {
            get { return enviorData; }
            set { enviorData = value; }
        }

		public EnviroDectector(string src)
		{
			try
			{
				ADAM4117 = new SerialPort(src, 9600, Parity.None, 8, StopBits.One);
                ADAM4117.Open();
                isOpen = true;

                ADAM_Read = new Thread(DataReceiving);
                ADAM_Read.IsBackground = true;
                isReceiving = true;
                ADAM_Read.Start();
                
			}
			catch (Exception e)
			{
                LogHandler.LogException(LogHandler.Formatting(e.ToString(), ""));
				Console.WriteLine(e.ToString());
			}
		}

        public string[] ExportEnviroData()
        {
            // unknown
            if (_isOpen)
                ADAM4117.Write("#00" + '\r');

            string[] ret = new string[9];

            if(this.enviorData.Length == 57)
            {
                // 8 channels
                for(int i = 0; i <= 7; i++)
                {
                    // the bits of value at every channel is 7.
                    ret[i] = this.enviorData.Substring(i * 7 + 1, 7);　
                }
            }

            return ret;
        }

        public void DataReceiving()
        {
            List<byte> tmp = new List<byte>();
            while(_isReceiving)
            {
                if(ADAM4117.BytesToRead > 0)
                {
                    Int32 val = ADAM4117.ReadByte();

                    if(val == S)
                    {
                        tmp.Clear();
                    }
                    tmp.Add((byte)val);

                    if (val == E)
                    {
                        enviorData = byte2Text(tmp.ToArray());
                    }
                    
                }

            }
        }

        public  string byte2Text(byte[] buffer)
        {
            string[] hex = BitConverter.ToString(buffer).Split('-');
            string ret = string.Empty;
            int cnt = 0;
            
            while(hex[cnt] != "0D")
            {
                ret += char.ConvertFromUtf32(Convert.ToInt32(hex[cnt], 16));
                cnt++;
            }

            return ret;                     
        }

    }

    public class HeaterMonitor : ICOMProvider
    {
        private static SerialPort HeaterPort;
        private static Thread Heater_Read;
        private string _dataFrame = string.Empty;
        private static bool _isReceiving = false;
        public bool isReading = false;
        private static bool _isOpen = false;
        
        const Int32 S = 58;             // 開頭字元 => ':' 
        const Int32 E1 = 13;            // 結尾字元_1 => CR
        const Int32 E2 = 10;            // 結尾字元_2 => LF

        public string Dataframe
        {
            get { return _dataFrame; }
            set { _dataFrame = value; }
        }

        public List<DTA_Heater> DTA_Heaters = new List<DTA_Heater>()
        {
            new DTA_Heater("01"),
            new DTA_Heater("02"),
            new DTA_Heater("03")
        };

        public HeaterMonitor(string src)
        {
            try
            {
                HeaterPort = new SerialPort(src, 9600, Parity.None, 8, StopBits.One);
                HeaterPort.Open();
                isOpen = true;

                Heater_Read = new Thread(DataReceiving);
                Heater_Read.IsBackground = true;
                isReceiving = true;
                Heater_Read.Start();

            }
            catch (Exception e)
            {
                LogHandler.LogException(LogHandler.Formatting(e.ToString(), ""));
                Console.WriteLine(e.ToString());
            }
        }

        public void DataReceiving()
        {
            List<byte> tmp = new List<byte>();
            while (_isReceiving)
            {
                if(isOpen)
                {
                    if (HeaterPort.BytesToRead > 0)
                    {
                        Int32 val = HeaterPort.ReadByte();

                        if (val == S)
                        {
                            tmp.Clear();
                        }

                        tmp.Add((byte)val);

                        if (val == E2)
                        {
                            Dataframe = byte2Text(tmp.ToArray());
                            allocateDataFrame(Dataframe);
                        }
                        
                    }
                }
            }
        }

        public string byte2Text(byte[] buffer)
        {
            string[] hex = BitConverter.ToString(buffer).Split('-');
            string ret = string.Empty;
            int cnt = 0;

            while (hex[cnt] != "0A")
            {
                ret += char.ConvertFromUtf32(Convert.ToInt32(hex[cnt], 16));
                cnt++;
            }

            return ret;
        }

        private void allocateDataFrame(string data)
        {
            int heater_Address = Convert.ToInt32(data.Substring(1, 2));
            DTA_Heaters[heater_Address].RetDataFrame = data;
        }

        public void SetTempCmd(int heaterNum, double val)
        {
            HeaterPort.Write(DTA_Heaters[heaterNum].CmdSetValue(val));
            isReading = false;
        }

        public void GetTempCmd(int heaterNum)
        {
            HeaterPort.Write(DTA_Heaters[heaterNum].CmdReadValue());
            isReading = false;
        }

        public void GetHeaterVal(int heaterNum)
        {
            isReading = HeaterUtils.HeaterValueHandler(Dataframe, ref DTA_Heaters[heaterNum].PV, ref DTA_Heaters[heaterNum].SV);
        }

        public bool isOpen
        {
            get { return _isOpen; }

            set { _isOpen = value; }
        }

        public bool isReceiving
        {
            get { return _isReceiving; }

            set { _isReceiving = value; }
        }
    }

    public class COMStore
	{
		private ICOMFactory _factory;
		private ICOMProvider _provider;

		public COMStore(ICOMFactory src)
		{
			_factory = src;
			_provider = _factory.CreateCOM();
		}

		public ICOMProvider GetInstance()
		{
			return _provider;
		}

	}

}

