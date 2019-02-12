using System;
using System.ComponentModel;
using System.IO;

namespace HMI_PermanentForm.Utils
{
    class MainUtility
    {
        public static void SavaRecord(string str)
        {
			int count = 0;
            string filePath = @"E:/AM250_New_Solution/logs/logs_LCS.txt";
            
			while (File.Exists(filePath))
            {
                count++;
                filePath = string.Format(@"E:/AM250_New_Solution/logs/logs_LCS({0}).txt", count);
            }

			#region save the file to specify file path.
			StreamWriter sw = new StreamWriter(filePath);
			sw.Write(str);
			sw.Close();
            #endregion
        }
    }
}
