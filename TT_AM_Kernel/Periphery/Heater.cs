using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM_Kernel.Periphery
{
    public class HeaterBase
    {
        private string _heaterAddress = string.Empty;

        public string HeaterAddress { get { return _heaterAddress; } set { _heaterAddress = value; } }

        public HeaterBase(string address)
        {
            address = _heaterAddress;
        }
    }

    public class DTA_Heater : HeaterBase, IHeater
    {
        private string _retDataFrame = string.Empty;
        public string RetDataFrame { get { return _retDataFrame; } set { _retDataFrame = value; } }

        public string PV, SV;

        public DTA_Heater(string address) : base(address)
        {
        }

        public string CmdReadValue()
        {
            string data = String.Format("{0}{1}{2}{3}", HeaterAddress,
                HeaterUtils.ReadFuncCode, HeaterUtils.ReadFuncAddress, 0002);
            string LRC = HeaterUtils.ComplementOf2(data);
            return string.Format(":{0}{1}\r\n", data, LRC);
        }

        public string CmdSetValue(double val)
        {
            string temp2Hex = Convert.ToString((int)(val * 10), 16).PadLeft(4, '0').ToUpper();
            string data = String.Format("{0}{1}{2}{3}", HeaterAddress,
            HeaterUtils.WriteFuncCode, HeaterUtils.WriteFuncAddress, temp2Hex);
            string LRC = HeaterUtils.ComplementOf2(data);
            return string.Format(":{0}{1}\r\n", data, LRC);
        }

    }

    class HeaterUtils
    {
        private static string _readFuncCode = "03";
        public static string ReadFuncCode { get { return _readFuncCode; }}

        private static string _writeFuncCode = "06";
        public static string WriteFuncCode { get { return _writeFuncCode; }}

        private static string _readFunc_address = "4700";
        public static string ReadFuncAddress { get { return _readFunc_address; }}

        private static string _writeFunc_address = "4701";
        public static string WriteFuncAddress { get { return _writeFunc_address; }}

        public static string ComplementOf2(string commandstr)
        {
            string ret = string.Empty;
            int HexCoupleNum = commandstr.Length / 2;
            int[] num = new int[HexCoupleNum];
            for(int i = HexCoupleNum - 1; i >= 0; i--)
            {
                num[i] = Convert.ToInt32(commandstr.Substring(i * 2, 2), 16);
            }

            int sum = 0;
            for(int i = HexCoupleNum - 1; i >= 0; i--)
            {
                sum += num[i];
            }

            string binary = Convert.ToString(~sum, 2).PadLeft(8, '0');

            int outputTemp = Convert.ToInt32(binary, 2);

            return Convert.ToString(outputTemp + 1, 16).PadLeft(2, '0').ToUpper();

        }

        public static bool HeaterValueHandler(string data, ref string PV, ref string SV)
        {
            if(data.Length > 13)
            {
                string cmd = data.Substring(3, 2);

                if (cmd == "03")
                {
                    PV = (Convert.ToInt32(data.Substring(7, 4), 16) * 0.1).ToString("0.0");
                    SV = (Convert.ToInt32(data.Substring(11, 4), 16) * 0.1).ToString("0.0");
                }
                else if (cmd == "06")
                {
                    SV = (Convert.ToInt32(data.Substring(9, 4), 16) * 0.1).ToString("0.0");
                }
                else
                    return false;

                return true;

            }
            return false;
        }

    }
}
