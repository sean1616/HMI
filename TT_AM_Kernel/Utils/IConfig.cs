using System;
using Newtonsoft.Json;
using System.IO;

namespace AM_Kernel.Utils
{
	public interface IConfig
	{
		void ReadConfig(string filePath);
		object GetConfig();
	}
    
    public class Utility
    {
        public static void OutputLogs(string str)
        {
            int count = 0;
            string filePath = @"logs.txt";
            while(File.Exists(filePath))
            {
                count++;
                filePath = string.Format(@"logs{0}.txt", count); ;
            }

            StreamWriter sw = new StreamWriter(filePath);
            sw.Write(str);
            sw.Close();
        }

        public static void OuputPolygonData(string filePath, string str)
        {
            StreamWriter sw = new StreamWriter(filePath);
            sw.Write(str);
            sw.Close();
        }
    }
}

