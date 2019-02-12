using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AM_Kernel.IO;

namespace HMI_PermanentForm.Utils
{
    public class TimerHandler
    {
       public static DIO_Timer m_DIO_Timer = new DIO_Timer();

       public class DIO_Timer
       {
           protected static Timer m_timer = new Timer(300);

           public DIO_Timer()
           {
               DO_Read_Beginning();
               m_timer.Elapsed += new System.Timers.ElapsedEventHandler(DIO_Tick);
               
           }

           private void DO_Read_Beginning()
           {
               DIOUtility.DIORead(DIO_DevType.Device1, DigitalType.DO);
               DIOUtility.DIORead(DIO_DevType.Device2, DigitalType.DO);

               foreach(var io in DO_Set.employees)
               {
                   // capacity is 32 port/card.
                   // because the name of DIO is DOXX, so substring(2) can ignore first two character. 
                   if(Convert.ToUInt16(io.Key.Substring(2)) < 32)
                       DIOUtility.DOSignalIn(DIO_DevType.Device1, ref io.Value.IsOn, io.Value);
                   else
                       DIOUtility.DOSignalIn(DIO_DevType.Device2, ref io.Value.IsOn, io.Value);
               }
           }

           private void DIO_Tick(object source, System.Timers.ElapsedEventArgs e)
           {
                //DIOUtility.DIORead(DIO_DevType.Device1, DigitalType.DO);
                DIOUtility.DIORead(DIO_DevType.Device1, DigitalType.DI);

                //DIOUtility.DIORead(DIO_DevType.Device2, DigitalType.DO);
                DIOUtility.DIORead(DIO_DevType.Device2, DigitalType.DI);

                foreach (var io in DI_Set.employees)
                {
                    // capacity is 32 port/card.
                    // because the name of DIO is DOXX, so substring(2) can ignore first two character. 
                    if (Convert.ToUInt16(io.Key.Substring(2)) < 32)
                        DIOUtility.DISignalIn(DIO_DevType.Device1, ref io.Value.IsOn, io.Value);
                    else
                        DIOUtility.DISignalIn(DIO_DevType.Device2, ref io.Value.IsOn, io.Value);
                }
           }

           public Timer GetTimer()
           {
               return m_timer;
           }

 
       }
    }
}
