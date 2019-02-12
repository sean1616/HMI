using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AM_Kernel.IO
{
    class DIO_Core
    {
        public bool IsOn;
        private int _portNum;
        private int _bits;
        private string _name;

        public DIO_Core(int port, int bits, string name)
        {
            _portNum = port;
            _bits = bits;
            _name = name;
        }

        public int PortNum { get { return _portNum; } private set { _portNum = value; } }
        public int Bits { get { return _bits; } private set { _bits = value; } }
        public string Name { get { return _name; } private set { _name = value; } }

		public override string ToString()
		{
			return String.Format("{0} in Port: {1}, Bits: {2}", _name, _portNum, _bits);
		}
    }

    enum DigitalType
    {
        DI = 0,
        DO = 1
    }

    interface IDIOFactory
    {
        Dictionary<string, DIO_Core> Produce();
    }

    class DOEntity : IDIOFactory
    {
		private readonly Dictionary<string, DIO_Core> employees = new Dictionary<string, DIO_Core>();

        public Dictionary<string, DIO_Core> Produce()
		{
            // Can add the IO in advance. The key of dictionary is the name of DO used for.
            #region device#1
            employees.Add("DO00", new DIO_Core(0, 0, "DO00"));
            employees.Add("DO01", new DIO_Core(0, 1, "DO01"));
            employees.Add("DO02", new DIO_Core(0, 2, "DO02"));
            employees.Add("DO03", new DIO_Core(0, 3, "DO03"));
            employees.Add("DO04", new DIO_Core(0, 4, "DO04"));
            employees.Add("DO05", new DIO_Core(0, 5, "DO05"));
            employees.Add("DO06", new DIO_Core(0, 6, "DO06"));
            employees.Add("DO07", new DIO_Core(0, 7, "DO07"));

            employees.Add("DO08", new DIO_Core(1, 0, "DO08"));
            employees.Add("DO09", new DIO_Core(1, 1, "DO09"));
            employees.Add("DO10", new DIO_Core(1, 2, "DO10"));
            employees.Add("DO11", new DIO_Core(1, 3, "DO11"));
            employees.Add("DO12", new DIO_Core(1, 4, "DO12"));
            employees.Add("DO13", new DIO_Core(1, 5, "DO13"));
            employees.Add("DO14", new DIO_Core(1, 6, "DO14"));
            employees.Add("DO15", new DIO_Core(1, 7, "DO15"));

            employees.Add("DO16", new DIO_Core(2, 0, "DO16"));
            employees.Add("DO17", new DIO_Core(2, 1, "DO17"));
            employees.Add("DO18", new DIO_Core(2, 2, "DO18"));
            employees.Add("DO19", new DIO_Core(2, 3, "DO19"));
            employees.Add("DO20", new DIO_Core(2, 4, "DO20"));
            employees.Add("DO21", new DIO_Core(2, 5, "DO21"));
            employees.Add("DO22", new DIO_Core(2, 6, "DO22"));
            employees.Add("DO23", new DIO_Core(2, 7, "DO23"));

            employees.Add("DO24", new DIO_Core(3, 0, "DO24"));
            employees.Add("DO25", new DIO_Core(3, 1, "DO25"));
            employees.Add("DO26", new DIO_Core(3, 2, "DO26"));
            employees.Add("DO27", new DIO_Core(3, 3, "DO27"));
            employees.Add("DO28", new DIO_Core(3, 4, "DO28"));
            employees.Add("DO29", new DIO_Core(3, 5, "DO29"));
            employees.Add("DO30", new DIO_Core(3, 6, "DO30"));
            employees.Add("DO31", new DIO_Core(3, 7, "DO31"));
            #endregion

            #region device#2
            employees.Add("DO32", new DIO_Core(0, 0, "DO32"));
            employees.Add("DO33", new DIO_Core(0, 1, "DO33"));
            employees.Add("DO34", new DIO_Core(0, 2, "DO34"));
            employees.Add("DO35", new DIO_Core(0, 3, "DO35"));
            employees.Add("DO36", new DIO_Core(0, 4, "DO36"));
            employees.Add("DO37", new DIO_Core(0, 5, "DO37"));
            employees.Add("DO38", new DIO_Core(0, 6, "DO38"));
            employees.Add("DO39", new DIO_Core(0, 7, "DO39"));

            employees.Add("DO40", new DIO_Core(1, 0, "DO40"));
            employees.Add("DO41", new DIO_Core(1, 1, "DO41"));
            employees.Add("DO42", new DIO_Core(1, 2, "DO42"));
            employees.Add("DO43", new DIO_Core(1, 3, "DO43"));
            employees.Add("DO44", new DIO_Core(1, 4, "DO44"));
            employees.Add("DO45", new DIO_Core(1, 5, "DO45"));
            employees.Add("DO46", new DIO_Core(1, 6, "DO46"));
            employees.Add("DO47", new DIO_Core(1, 7, "DO47"));

            employees.Add("DO48", new DIO_Core(2, 0, "DO48"));
            employees.Add("DO49", new DIO_Core(2, 1, "DO49"));
            employees.Add("DO50", new DIO_Core(2, 2, "DO50"));
            employees.Add("DO51", new DIO_Core(2, 3, "DO51"));
            employees.Add("DO52", new DIO_Core(2, 4, "DO52"));
            employees.Add("DO53", new DIO_Core(2, 5, "DO53"));
            employees.Add("DO54", new DIO_Core(2, 6, "DO54"));
            employees.Add("DO55", new DIO_Core(2, 7, "DO55"));

            employees.Add("DO56", new DIO_Core(3, 0, "DO56"));
            employees.Add("DO57", new DIO_Core(3, 1, "DO57"));
            employees.Add("DO58", new DIO_Core(3, 2, "DO58"));
            employees.Add("DO59", new DIO_Core(3, 3, "DO59"));
            employees.Add("DO60", new DIO_Core(3, 4, "DO60"));
            employees.Add("DO61", new DIO_Core(3, 5, "DO61"));
            employees.Add("DO62", new DIO_Core(3, 6, "DO62"));
            employees.Add("DO63", new DIO_Core(3, 7, "DO63"));
            #endregion
            return employees;
        }

		public void Add(string IO_Name ,DIO_Core src)
		{
			employees.Add(IO_Name, src);
		}

		public bool Remove(string IO_Name)
		{
			return employees.Remove(IO_Name);
		}
    }

    class DIEntity : IDIOFactory
    {
		private readonly Dictionary<string, DIO_Core> employees = new Dictionary<string, DIO_Core>();

		public Dictionary<string, DIO_Core> Produce()
		{
            #region device#1
            employees.Add("DI00", new DIO_Core(0, 0, "DI00"));
            employees.Add("DI01", new DIO_Core(0, 1, "DI01"));
            employees.Add("DI02", new DIO_Core(0, 2, "DI02"));
            employees.Add("DI03", new DIO_Core(0, 3, "DI03"));
            employees.Add("DI04", new DIO_Core(0, 4, "DI04"));
            employees.Add("DI05", new DIO_Core(0, 5, "DI05"));
            employees.Add("DI06", new DIO_Core(0, 6, "DI06"));
            employees.Add("DI07", new DIO_Core(0, 7, "DI07"));

            employees.Add("DI08", new DIO_Core(1, 0, "DI08"));
            employees.Add("DI09", new DIO_Core(1, 1, "DI09"));
            employees.Add("DI10", new DIO_Core(1, 2, "DI10"));
            employees.Add("DI11", new DIO_Core(1, 3, "DI11"));
            employees.Add("DI12", new DIO_Core(1, 4, "DI12"));
            employees.Add("DI13", new DIO_Core(1, 5, "DI13"));
            employees.Add("DI14", new DIO_Core(1, 6, "DI14"));
            employees.Add("DI15", new DIO_Core(1, 7, "DI15"));

            employees.Add("DI16", new DIO_Core(2, 0, "DI16"));
            employees.Add("DI17", new DIO_Core(2, 1, "DI17"));
            employees.Add("DI18", new DIO_Core(2, 2, "DI18"));
            employees.Add("DI19", new DIO_Core(2, 3, "DI19"));
            employees.Add("DI20", new DIO_Core(2, 4, "DI20"));
            employees.Add("DI21", new DIO_Core(2, 5, "DI21"));
            employees.Add("DI22", new DIO_Core(2, 6, "DI22"));
            employees.Add("DI23", new DIO_Core(2, 7, "DI23"));

            employees.Add("DI24", new DIO_Core(3, 0, "DI24"));
            employees.Add("DI25", new DIO_Core(3, 1, "DI25"));
            employees.Add("DI26", new DIO_Core(3, 2, "DI26"));
            employees.Add("DI27", new DIO_Core(3, 3, "DI27"));
            employees.Add("DI28", new DIO_Core(3, 4, "DI28"));
            employees.Add("DI29", new DIO_Core(3, 5, "DI29"));
            employees.Add("DI30", new DIO_Core(3, 6, "DI30"));
            employees.Add("DI31", new DIO_Core(3, 7, "DI31"));
            #endregion

            #region device#2
            employees.Add("DI32", new DIO_Core(0, 0, "DI32"));
            employees.Add("DI33", new DIO_Core(0, 1, "DI33"));
            employees.Add("DI34", new DIO_Core(0, 2, "DI34"));
            employees.Add("DI35", new DIO_Core(0, 3, "DI35"));
            employees.Add("DI36", new DIO_Core(0, 4, "DI36"));
            employees.Add("DI37", new DIO_Core(0, 5, "DI37"));
            employees.Add("DI38", new DIO_Core(0, 6, "DI38"));
            employees.Add("DI39", new DIO_Core(0, 7, "DI39"));

            employees.Add("DI40", new DIO_Core(1, 0, "DI40"));
            employees.Add("DI41", new DIO_Core(1, 1, "DI41"));
            employees.Add("DI42", new DIO_Core(1, 2, "DI42"));
            employees.Add("DI43", new DIO_Core(1, 3, "DI43"));
            employees.Add("DI44", new DIO_Core(1, 4, "DI44"));
            employees.Add("DI45", new DIO_Core(1, 5, "DI45"));
            employees.Add("DI46", new DIO_Core(1, 6, "DI46"));
            employees.Add("DI47", new DIO_Core(1, 7, "DI47"));

            employees.Add("DI48", new DIO_Core(2, 0, "DI48"));
            employees.Add("DI49", new DIO_Core(2, 1, "DI49"));
            employees.Add("DI50", new DIO_Core(2, 2, "DI50"));
            employees.Add("DI51", new DIO_Core(2, 3, "DI51"));
            employees.Add("DI52", new DIO_Core(2, 4, "DI52"));
            employees.Add("DI53", new DIO_Core(2, 5, "DI53"));
            employees.Add("DI54", new DIO_Core(2, 6, "DI54"));
            employees.Add("DI55", new DIO_Core(2, 7, "DI55"));

            employees.Add("DI56", new DIO_Core(3, 0, "DI56"));
            employees.Add("DI57", new DIO_Core(3, 1, "DI57"));
            employees.Add("DI58", new DIO_Core(3, 2, "DI58"));
            employees.Add("DI59", new DIO_Core(3, 3, "DI59"));
            employees.Add("DI60", new DIO_Core(3, 4, "DI60"));
            employees.Add("DI61", new DIO_Core(3, 5, "DI61"));
            employees.Add("DI62", new DIO_Core(3, 6, "DI62"));
            employees.Add("DI63", new DIO_Core(3, 7, "DI63"));
            #endregion
			return employees;
        }

		public void Add(string IO_Name, DIO_Core src)
		{
			employees.Add(IO_Name, src);
		}

		public bool Remove(string IO_Name)
		{
			return employees.Remove(IO_Name);
		}
    }
    
    /// <summary>
    /// Manufacture the specify digital I/O that is determined by DigitalType.
    /// </summary>
    class DIOFactory
    {
        public Dictionary<string, DIO_Core> GetProduct(DigitalType DIOType)
        {
            switch(DIOType)
            {
                case DigitalType.DO:
                    return new DOEntity().Produce();
                case DigitalType.DI:
                    return new DIEntity().Produce();
                default:
                    return null;
            }
        }
    }


}
