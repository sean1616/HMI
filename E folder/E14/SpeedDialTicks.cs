using System;
using System.Windows;
using System.Windows.Media;
using System.Globalization;

namespace CircleGauge.SpeedDial
{
    public class SpeedDialTicks : FrameworkElement
    {
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty StepsProperty =
            DependencyProperty.Register("Steps", typeof(int), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata(10, FrameworkPropertyMetadataOptions.AffectsRender));
        public int Steps
        {
            get { return (int)GetValue(StepsProperty); }
            set { SetValue(StepsProperty, value); }
        }

        public static readonly DependencyProperty ScaleFormatProperty =
            DependencyProperty.Register("ScaleFormat", typeof(string), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata("G", FrameworkPropertyMetadataOptions.AffectsRender));
        public string ScaleFormat
        {
            get { return (string)GetValue(ScaleFormatProperty); }
            set { SetValue(ScaleFormatProperty, value); }
        }

        public static readonly DependencyProperty MinimumAngleProperty =
            DependencyProperty.Register("MinimumAngle", typeof(double), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double MinimumAngle
        {
            get { return (double)GetValue(MinimumAngleProperty); }
            set { SetValue(MinimumAngleProperty, value); }
        }

        public static readonly DependencyProperty MaximumAngleProperty =
            DependencyProperty.Register("MaximumAngle", typeof(double), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double MaximumAngle
        {
            get { return (double)GetValue(MaximumAngleProperty); }
            set { SetValue(MaximumAngleProperty, value); }
        }

        public static readonly DependencyProperty TickUnitProperty =
            DependencyProperty.Register("TickUnit", typeof(double), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double TickUnit
        {
            get { return (double)GetValue(TickUnitProperty); }
            set { SetValue(TickUnitProperty, value); }
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            double tickAmount = (Maximum - Minimum) / TickUnit;

            // 畫數字
            for (int i = 0; i < Steps + 1; ++i)
            {
                BrushConverter bc = new BrushConverter();  
                var value = (Maximum - Minimum) / Steps * i + Minimum;
                var location = (MaximumAngle - MinimumAngle) / Steps * i + MinimumAngle;
                var rad = (90 - location) * Math.PI / 180;
                while (rad > Math.PI * 2) rad -= Math.PI * 2;
                while (rad < 0) rad += Math.PI * 2;
                var point = new Point(
                    ActualWidth / 2 + ActualWidth / 2 * Math.Cos(rad),
                    ActualHeight / 2 - ActualHeight / 2 * Math.Sin(rad));
                // drawingContext.DrawRectangle(Brushes.White, null, new Rect(point.X - 2, point.Y - 2, 4, 4));
                var point1 = new Point(
                    point.X - 20 * Math.Cos(rad),                             // 修改刻度長短
                    point.Y + 20 * Math.Sin(rad));                            // 修改刻度長短
                Pen pen1 = new Pen((Brush)bc.ConvertFrom("#FF838383"), 4);  // (色碼修改處,刻度寬度)
                drawingContext.DrawLine(pen1, point, point1);

                var ft = new FormattedText(value.ToString(ScaleFormat),
                        CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight,
                        new Typeface("Arial"), 15, (Brush)bc.ConvertFrom("#FF838383"));                        //色碼修改處
                var ft1 = new FormattedText(value.ToString(ScaleFormat),
                        CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight,
                        new Typeface("Arial"), 15, (Brush)bc.ConvertFrom("#FF838383"));                         //色碼修改處
                if (Math.Abs(point1.X - ActualWidth / 2) < 1)
                {
                    point1 = new Point(point1.X - ft.Width / 2, point1.Y);
                }
                else if (point1.X > ActualWidth / 2)
                {
                    point1 = new Point(point1.X - ft.Width, point1.Y);
                }
                Point point2 = new Point(point1.X + 0.5, point1.Y + 0.5);
                drawingContext.DrawText(ft1, point2);
                drawingContext.DrawText(ft, point1);

            }
        }
    }
}
