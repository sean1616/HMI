using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using HMI_PermanentForm.Utils;
using Emgu.CV.Structure;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.IO;

namespace HMI_PermanentForm.WebCam
{
    public class CamCapturer
    {
        private Capture camStram = new Capture();

        private static System.Timers.Timer captureTimer = new System.Timers.Timer();

        public CamCapturer()
        {
            captureTimer.Elapsed += OnCaptureTimerElapsed;
            captureTimer.AutoReset = true;
        }

        public void WebCamTrigger(bool RunJudge)
        {
            if(RunJudge)
            {
                try
                {
                    captureTimer.Interval = 60;
                    captureTimer.Enabled = RunJudge;
                }
                catch(Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                try
                {
                    captureTimer.Enabled = RunJudge;
                }
                catch(Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        public void OnCaptureTimerElapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            Image<Bgr, Byte> frame;
            try
            {
                frame = camStram.QueryFrame().ToImage<Bgr, Byte>();
                GlobalChannel.ImageVM.CameraImage = BitmapSourceConvert.ToBitmapSource(frame);
                GlobalChannel.ImageVM.CameraImage.Freeze();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public static class BitmapSourceConvert
        {
            [DllImport("gdi32")]
            private static extern int DeleteObject(IntPtr o);

            public static BitmapSource ToBitmapSource(IImage image)
            {
                using (System.Drawing.Bitmap source = image.Bitmap)
                {
                    IntPtr ptr = source.GetHbitmap();

                    BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        ptr,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                    DeleteObject(ptr);
                    return bs;
                }
            }
        }


    }
}
