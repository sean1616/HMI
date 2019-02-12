//using AffIO;
using RTC5Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Media.Media3D;
using LinearAlgebraAPI;
using AM_Kernel.Interpretation;

namespace AM_Kernel.LaserCtrl
{
    class ScanSystem
    {
        const uint DefaultCard = 1;
        const uint LaserCtrl = 0x8;
        public static int LaserPower = 70;
        public static int LaserSpeed = 100;
        const float LaserPower_Native = 500;
        static uint layerCnt = 0;

        // Command code with optional parameters.
        static readonly uint SendStatus = 0x0500;
        static readonly uint SendGalvanometerTemp = 0x0514;
        static readonly uint SendServoBoardTemp = 0x0515;
        static readonly uint SendRealPos = 0x0501;
        static readonly uint SendSetPos = 0x0502;
        static readonly uint SendActZPos = 0x0512;
        static readonly uint SendPosError = 0x0503;

        // the index of periphery, i.e. scan head, laser etc.
        static readonly uint HeadA = 1;
        static readonly uint HeadB = 2;
        static readonly uint XAxis = 1;
        static readonly uint YAxis = 2;
        static readonly uint ZAxis = 1;

        // there are introductions in "set_trigger"
        static readonly uint StatusAX = 1;
        static readonly uint StatusAY = 2;
        static readonly uint StatusBX = 4;

        //RTC5狀態值
        public static uint RTC5_Busy = 0;
        public static uint RTC5_Pos = 0;

        static Int32 K_xy = 3120;
        static Int32 K_z = 195;

        static double coef_A = 0;
        static double coef_B = 0;
        static double coef_C = 0;

        static bool Z_Mode_enabled = true;

        static volatile Layer preloadLayer = null;
        static volatile List<ScanPath> preloadPaths = null;
        static volatile PenLibrary preloadLaserPenLibrary = null;

        static bool isRTC5Ready = false;       //< a flag that identify whether RTC5 initialize or not.

        public static bool isScanPattern = false;

        public static string correctionFilePath = "../../../TT_AM_Kernel/Correction/160704 1386 test12.ct5";

        public static string InitialSystem()
        {
            uint ErrorCode;

            #region initRTC5
            try
            {
                ErrorCode = RTC5Wrap.init_rtc5_dll();
                if (ErrorCode != 0)
                {
                    uint RTC5CountCards = RTC5Wrap.rtc5_count_cards();

                    if (RTC5CountCards != 0)
                    {
                        uint AccError = 0;

                        // Error analysis in detail
                        for (uint i = 1; i <= RTC5CountCards; i++)
                        {
                            uint Error = RTC5Wrap.n_get_last_error(i);

                            if (Error != 0)
                            {
                                AccError |= Error;
                                MessageBox.Show("Card No. " + i + ": Error " + Error + " dectected.");
                                RTC5Wrap.n_reset_error(i, Error);
                            }
                        }

                        if (AccError != 0)
                        {
                            CloseSystem();
                            return "AccError";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Initializing the DLL: Error " + ErrorCode + " detected.");
                        CloseSystem();
                        return ErrorCode.ToString();
                    }
                }
                else
                {
                    RTC5Wrap.reset_error(0);
                    RTC5Wrap.set_rtc5_mode();

                    if (DefaultCard != RTC5Wrap.select_rtc(DefaultCard))
                    {
                        ErrorCode = RTC5Wrap.n_get_last_error(DefaultCard);

                        if ((ErrorCode & 256) != 0)
                        {

                            ErrorCode = RTC5Wrap.n_load_program_file(DefaultCard, correctionFilePath);
                        }
                        else
                        {
                            CloseSystem();
                            return "No acces to Card no. " + DefaultCard;
                        }

                        if (ErrorCode != 0)
                        {
                            CloseSystem();
                            return "No access to Card no. " + DefaultCard;
                        }
                        else
                        {
                            RTC5Wrap.select_rtc(DefaultCard);
                        }
                    }
                }

                RTC5Wrap.stop_execution();
                

                isRTC5Ready = true;

            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.ToString());
                return Err.ToString();
            }
            #endregion

            return "0";

        }

        public static string LoadCorrectionFile(string filePath)
        {
            uint ErrorCode;

            if (isRTC5Ready)
            {
                try
                {
                    if (Z_Mode_enabled == true)
                    {
                        //3D 模式
                        ErrorCode = RTC5Wrap.load_correction_file(filePath, 1, 3);
                        coef_A = RTC5Wrap.get_table_para(1, 5);
                        coef_B = RTC5Wrap.get_table_para(1, 6);
                        coef_C = RTC5Wrap.get_table_para(1, 7);
                        ErrorCode = RTC5Wrap.load_z_table(coef_A, coef_B, coef_C);
                        
                    }
                    else
                    {
                        //2D 模式
                        //Select table#1 and make 2D and 3D correction files stored as 2D correction table.
                        ErrorCode = RTC5Wrap.load_correction_file(filePath, 1, 2);
                        coef_A = RTC5Wrap.get_table_para(1, 5);
                        coef_B = RTC5Wrap.get_table_para(1, 6);
                        coef_C = RTC5Wrap.get_table_para(1, 7);
                        ErrorCode = RTC5Wrap.load_z_table(coef_A, coef_B, coef_C);
                    }

                    if (ErrorCode != 0)
                    {
                        CloseSystem();
                        return "load correction file error. Error Code:" + ErrorCode.ToString();
                    }
                    else
                    {
                        if (Z_Mode_enabled == true)
                        {
                            RTC5Wrap.select_cor_table(1, 2);
                        }
                        else
                        {
                            RTC5Wrap.select_cor_table(1, 1);
                        }
                    }

                    ErrorCode = RTC5Wrap.load_program_file(null);

                    if (ErrorCode != 0)
                    {
                        CloseSystem();
                        return "Program file loading error: " + ErrorCode;
                    }


                    // Laser Setting
                    RTC5Wrap.set_laser_mode(2U);
                    RTC5Wrap.set_laser_control(LaserCtrl);
                    RTC5Wrap.enable_laser();

                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.ToString());
                }
            }
            else
            {
                return "1";
            }

            return "0";
        }

        public static void CloseSystem()
        {
            if(isRTC5Ready)
            {
                try
                {
                    RTC5Wrap.reset_error(0);
                    RTC5Wrap.stop_execution();
                    RTC5Wrap.free_rtc5_dll();
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.ToString());
                }
            }
 
            GC.Collect();
        }

        public static string PreloadLayer(Layer specifyLayer, uint Cnt)
        {
            if (specifyLayer == null)
                return "1";
            else
            {
                preloadLayer = null;
                preloadLayer = specifyLayer;
                layerCnt = Cnt;
            }
            return "0";
        }

        public static string PreloadPattern(List<ScanPath> specifyPattern)
        {
            if (specifyPattern == null)
                return "1";
            else
            {
                preloadPaths = null;
                preloadPaths = specifyPattern;
            }
            return "0";
        }

        public static string PreloadPenLibrary(PenLibrary speficyLibrary)
        {
            if (speficyLibrary == null)
                return "1";
            else
            {
                preloadLaserPenLibrary = null;
                preloadLaserPenLibrary = speficyLibrary;
            }
            return "0";
        }

        private static string ScanLayer()
        {
            if (isRTC5Ready)
            {
                if (preloadLayer == null)
                    return "Not loaded the layer";
                else
                {
                    try
                    {
                            uint StandbyHalfPeriod = 6400;
                            uint StandbyPulseWidth = 0;
                            RTC5Wrap.set_standby(StandbyHalfPeriod, StandbyPulseWidth);

                            uint JumpDelay = 64;
                            uint MarkDelay = 32;
                            uint PolygonDelay = 16;

                            double Laser_Jump_Speed = 6886 * K_xy / 1000;

                            uint LaserHalfPeriod = 6400;
                            uint LaserPulseWidth = 12864;

                            RTC5Wrap.config_list(50000U, 50000U);

                            int LaserONDelay = 192;
                            uint LaserOFFDelay = 768;
                            
                            RTC5Wrap.set_start_list(1U);
                            RTC5Wrap.long_delay(100000U);
                                RTC5Wrap.set_laser_pulses(LaserHalfPeriod, LaserPulseWidth);
                                RTC5Wrap.set_scanner_delays(JumpDelay, MarkDelay, PolygonDelay);
                                RTC5Wrap.set_laser_delays(LaserONDelay, LaserOFFDelay);
                                RTC5Wrap.set_jump_speed(Laser_Jump_Speed);
                            RTC5Wrap.set_end_of_list();

                            RTC5Wrap.execute_list(1);

                            // for change list
                            uint list = 2U;
                            uint ListLevel = 0U;
                            bool ListStart = true;

                            RTC5Wrap.write_io_port(0);
                            RTC5Wrap.set_angle(HeadA, 90.0, 1);
                            //RTC5Wrap.set_offset_xyz(HeadA, 125 * K_xy, -125 * K_xy, 0, 1);
                            //RTC5Wrap.set_defocus(Convert.ToInt32( * K_z));

                            do { RTC5Wrap.get_status(out RTC5_Busy, out RTC5_Pos); } while (RTC5_Busy != 0U);
                            AM_Kernel.Interpretation.LaserPen workPen = null;
                        int index = 0;    

                        #region mark hatch
                                    
                                for (int i = 0; i != preloadLayer.getPaths().getPaths().Count; i++)
                                {
                                    index = (int)preloadLaserPenLibrary.getIndex()[(int)layerCnt][i];
                                    workPen = preloadLaserPenLibrary.getLibrary()[index];
                                    
                                    RTC5Wrap.write_da_1((uint)Math.Round((4095 / LaserPower_Native) * workPen.LaserPower));
                                    RTC5Wrap.long_delay(50U);

                                    for (int j = 0; j < preloadLayer.getPaths().getPaths()[i].getPoints().Count; j++)
                                    {
                                        bool wait;
                                        do
                                        {
                                            wait = false;
                                            RTC5Wrap.get_status(out RTC5_Busy, out RTC5_Pos);
                                            if(RTC5_Busy != 0U)
                                            {
                                                // even, odd
                                                if ((list & 0x1U) == 0x0U)
                                                {
                                                    if (RTC5_Pos >= 5000U)
                                                    {
                                                        wait = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (RTC5_Pos < 5000U)
                                                        wait = true;
                                                }
                                            }
                                        } while(wait);

                                        if (ListStart)
                                        {
                                            RTC5Wrap.set_start_list(list);
                                            RTC5Wrap.set_mark_speed(workPen.LaserSpeed * K_xy / 1000);

                                            ListLevel = 0U;
                                            ListLevel++;
                                            ListStart = false;
                                        }

                                        ListLevel++;
                                        ListLevel++;
                                        if(preloadLayer.getPaths().getPaths()[i].getScanType() == (uint)ScanPathType.Hatch)
                                        {
                                            if (j % 2 == 0)
                                            {
                                                RTC5Wrap.jump_abs((int)(preloadLayer.getPaths().getPaths()[i].getPoints()[j].X * K_xy),
                                                                  (int)(preloadLayer.getPaths().getPaths()[i].getPoints()[j].Y * K_xy));
                                            }
                                            else
                                            {
                                                RTC5Wrap.mark_abs((int)(preloadLayer.getPaths().getPaths()[i].getPoints()[j].X * K_xy),
                                                                  (int)(preloadLayer.getPaths().getPaths()[i].getPoints()[j].Y * K_xy));
                                            }
                                        }
                                        else if(preloadLayer.getPaths().getPaths()[i].getScanType() == (uint)ScanPathType.Polygon)
                                        {
                                            if (j == 0)
                                            {
                                                RTC5Wrap.jump_abs((int)(preloadLayer.getPaths().getPaths()[i].getPoints()[j].X * K_xy),
                                                                  (int)(preloadLayer.getPaths().getPaths()[i].getPoints()[j].Y * K_xy));
                                            }
                                            else
                                            {
                                                RTC5Wrap.mark_abs((int)(preloadLayer.getPaths().getPaths()[i].getPoints()[j].X * K_xy),
                                                                  (int)(preloadLayer.getPaths().getPaths()[i].getPoints()[j].Y * K_xy));
                                            }
                                        }

                                        if (ListLevel < 5000U - 1U)
                                        {
                                            if (j >= preloadLayer.getPaths().getPaths()[i].getPoints().Count - 1)
                                            {
                                            	
                                            }
                                            else
                                                continue;
                                        }
                                        //RTC5Wrap.jump_abs(0, 0);
                                        RTC5Wrap.set_end_of_list();
                                        ListStart = true;

                                        RTC5Wrap.auto_change();
                                        list++;
                                    }

                                }
                        #endregion

                        do { RTC5Wrap.get_status(out RTC5_Busy, out RTC5_Pos); } while (Convert.ToBoolean(RTC5_Busy));      //確認前段雷射掃描是否結束

                        /// set DO1 high.
                        RTC5Wrap.set_start_list(1);
                        RTC5Wrap.write_io_port_list(1);

                        RTC5Wrap.jump_abs(0, 0);
                        RTC5Wrap.set_end_of_list();
                        RTC5Wrap.execute_list(1);

                        layerCnt = 0;
                        preloadLayer = null;

                    }
                    catch(Exception e)
                    {
                        return e.Message;
                    }
                    
                }
            }
            else
                return "RTC5 isn't ready";

            return "0";
        }

        public static Task<string> ScanLayerAsync()
        {
            return Task.Run(() => { return ScanLayer(); });
        }

        //public static string ScanPattern()
        //{
        //    if (isRTC5Ready)
        //    {
        //        if (preloadPaths == null)
        //            return "Not loaded the layer";
        //        else
        //        {
        //            try
        //            {
        //                uint StandbyHalfPeriod = 6400;
        //                uint StandbyPulseWidth = 0;
        //                RTC5Wrap.set_standby(StandbyHalfPeriod, StandbyPulseWidth);

        //                uint JumpDelay = 64;
        //                uint MarkDelay = 32;
        //                uint PolygonDelay = 16;

        //                double Laser_Jump_Speed = 6886 * K_xy / 1000;

        //                uint LaserHalfPeriod = 6400;
        //                uint LaserPulseWidth = 12864;

        //                RTC5Wrap.config_list(50000U, 50000U);

        //                int LaserONDelay = 192;
        //                uint LaserOFFDelay = 768;

        //                RTC5Wrap.set_start_list(1U);
        //                RTC5Wrap.long_delay(100000U);
        //                RTC5Wrap.set_laser_pulses(LaserHalfPeriod, LaserPulseWidth);
        //                RTC5Wrap.set_scanner_delays(JumpDelay, MarkDelay, PolygonDelay);
        //                RTC5Wrap.set_laser_delays(LaserONDelay, LaserOFFDelay);
        //                RTC5Wrap.set_jump_speed(Laser_Jump_Speed);
        //                RTC5Wrap.set_end_of_list();

        //                RTC5Wrap.execute_list(1);

        //                RTC5Wrap.write_io_port(0);
        //                RTC5Wrap.set_angle(HeadA, 90.0, 1);

        //                do { RTC5Wrap.get_status(out RTC5_Busy, out RTC5_Pos); } while (RTC5_Busy != 0U);

        //                Task.Factory.StartNew(() =>
        //                {
        //                    #region mark hatch
        //                    foreach (ScanPath sp in preloadPaths)
        //                    {
        //                        do { RTC5Wrap.get_status(out RTC5_Busy, out RTC5_Pos); } while (RTC5_Busy != 0U);

        //                        RTC5Wrap.write_da_1((uint)Math.Round((4095 / LaserPower_Native) * sp.pen.Power));
        //                        RTC5Wrap.long_delay(20U);

        //                        RTC5Wrap.set_start_list(1U);
        //                        RTC5Wrap.set_mark_speed(sp.pen.Speed * K_xy / 1000);
        //                        Console.WriteLine(sp.pen.Speed);
        //                        foreach (LineSegment seg in sp.segments)
        //                        {

        //                            RTC5Wrap.jump_abs((int)(seg.startPt.X * K_xy),
        //                                                      (int)(seg.startPt.Y * K_xy));

        //                            RTC5Wrap.mark_abs((int)(seg.endPt.X * K_xy),
        //                                                      (int)(seg.endPt.Y * K_xy));

        //                        }
        //                        RTC5Wrap.set_end_of_list();
        //                        RTC5Wrap.execute_list(1U);
        //                    }
        //                    #endregion
        //                }).ContinueWith(task =>
        //                {
        //                    do { RTC5Wrap.get_status(out RTC5_Busy, out RTC5_Pos); } while (Convert.ToBoolean(RTC5_Busy));      //確認前段雷射掃描是否結束

        //                    /// set DO1 high.
        //                    RTC5Wrap.set_start_list(1);
        //                    RTC5Wrap.write_io_port_list(1);
        //                    //RTC5Wrap.jump_abs(125 * K_xy, 125 * K_xy);
        //                    RTC5Wrap.jump_abs(0, 0);
        //                    RTC5Wrap.set_end_of_list();
        //                    RTC5Wrap.execute_list(1);

        //                    preloadPaths = null;
        //                });

        //            }
        //            catch (Exception e)
        //            {
        //                return e.Message;
        //            }

        //        }
        //    }
        //    else
        //        return "RTC5 isn't ready";

        //    return "0";
        //}

        #region Single Action
        public static string SetLaserOn(bool flag)
        {
            if (isRTC5Ready)
            {
                RTC5Wrap.write_da_1((uint)((4096 / LaserPower_Native) * LaserPower));

                if (flag)
                {
                    //RTC5Wrap.enable_laser();
                    RTC5Wrap.set_laser_control(16U);
                }
                else
                {
                    RTC5Wrap.set_laser_control(8U);
                }
                return "0";
            }
            else
                return "RTC5 isn't initialized.";
        }

        public static string MoveScanner(double x, double y, double z)
        {
            if (isRTC5Ready)
            {
                Int32 CtrlVal_X = (int)(x * K_xy);
                Int32 CtrlVal_Y = (int)(y * K_xy);
                Int32 CtrlVal_Z = (int)(z * K_z);

                RTC5Wrap.goto_xy(CtrlVal_X, CtrlVal_Y);
                RTC5Wrap.set_defocus(CtrlVal_Z);

                return "0";
            }
            else
                return "RTC5 isn't initialized.";
        }
        #endregion

        public static bool GetRTC5Status()
        {
            return isRTC5Ready;
        }

        public static double[] FeedbackScanner_Status()
        {
            if (isRTC5Ready)
            {
                double[] ret = new double[3];

                RTC5Wrap.control_command(HeadA, XAxis, SendRealPos);
                ret[0] = ((double)(RTC5Wrap.get_value(StatusAX) >> 4) /*/ (double)K_xy**/);

                RTC5Wrap.control_command(HeadA, YAxis, SendRealPos);
                ret[1] = ((double)(RTC5Wrap.get_value(StatusAY)) /*/ (double)K_xy*/);

                /// Shift 4 bit, why?
                RTC5Wrap.control_command(HeadB, ZAxis, SendRealPos);
                ret[2] = ((double)(RTC5Wrap.get_value(StatusBX) >> 4) /*/ (double)K_z*/);

                return ret;
            }
            else
                return new double[]{-1, -1, -1};

        }

        public static string BuildInfo(out int actLayer, out int numOfLayer)
        {
            if (SlicingObj.getLayers() == null)
            {
                actLayer = 0;
                numOfLayer = 0;
                return "1";
            }
            else
            {
                if (preloadLayer == null)
                {
                    actLayer = 0;
                    numOfLayer = 0;
                    return "1";
                }
                else
                {
                    actLayer = (int)layerCnt;
                    numOfLayer = SlicingObj.getLayers().getLayers().Count;
                }
            }

            return "0";
        }

        public static bool Status()
        {
            //string result = "";

            RTC5Wrap.get_status(out RTC5_Busy, out RTC5_Pos);

            return Convert.ToBoolean(RTC5_Busy);
        }

        
    }
}
