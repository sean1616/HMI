using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AM_Kernel.Utils
{
    public class LogHandler
    {
        static string LogPath_Status = @"../Debug/Logs/Log_Status.txt";
        static string LogPath_Error = @"../Debug/Logs/Log_Error.txt";
        static string LogPath_Debug = @"../Debug/Logs/Log_Debug.txt";
        static string LogPath_Warning = @"../Debug/Logs/Log_Warning.txt";
        static string LogPath_Exception = @"../Debug/Logs/Log_Exception.txt";
        static string LogPath_Journal = @"../Debug/Logs/Log_Journal.txt";

        public static void LogDebug(params string[] msg)
        {
            AppendText2Log(LogPath_Debug, msg);
        }

        public static void LogStatus(params string[] msg)
        {
            AppendText2Log(LogPath_Status, msg);
        }

        public static void LogWarning(params string[] msg)
        {
            AppendText2Log(LogPath_Warning, msg);
        }

        public static void LogError(params string[] msg)
        {
            AppendText2Log(LogPath_Error, msg);
        }

        public static void LogException(params string[] msg)
        {
            AppendText2Log(LogPath_Exception, msg);
        }

        public static void LogJournal(params string[] msg)
        {
            AppendText2Log(LogPath_Journal, msg);
        }

        private static void AppendText2Log(string path, params string[] msg)
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    Parallel.ForEach(msg, str => sw.WriteLine(str));
                    //foreach (string str in msg)
                    //{
                    //    sw.WriteLine(str);
                    //}
                }
            }

            using (StreamWriter sw = File.AppendText(path))
            {
                Parallel.ForEach(msg, str => sw.WriteLine(str));
                //foreach(string str in msg)
                //{
                //    sw.WriteLine(str);
                //}
            }
        }

        public static string Formatting(string Error, string Description)
        {
            return String.Format("[{0}] [{1}] [{2}]", System.DateTime.Now.ToString("o"), Error, Description);
        }
    }
}
