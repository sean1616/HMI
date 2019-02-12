using System;
using System.Windows;
using System.Globalization;
using System.Windows.Media;

namespace HMI_PermanentForm
{
    public class Elem_A6_1_2 : FrameworkElement
    {
        public static readonly DependencyProperty MinimumProperty =
          DependencyProperty.Register("Minimum", typeof(double), typeof(Elem_A6_1_2),
          new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
           DependencyProperty.Register("Maximum", typeof(double), typeof(Elem_A6_1_2),
           new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty TickUnitLongProperty =
            DependencyProperty.Register("TickUnitLong", typeof(double), typeof(Elem_A6_1_2),
            new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double TickUnitLong
        {
            get { return (double)GetValue(TickUnitLongProperty); }
            set { SetValue(TickUnitLongProperty, value); }
        }

        public static readonly DependencyProperty MinimumPositionProperty =
            DependencyProperty.Register("MinimumPosition", typeof(double), typeof(Elem_A6_1_2),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double MinimumPosition
        {
            get { return (double)GetValue(MinimumPositionProperty); }
            set { SetValue(MinimumPositionProperty, value); }
        }

        public static readonly DependencyProperty MaximumPositionProperty =
            DependencyProperty.Register("MaximumPosition", typeof(double), typeof(Elem_A6_1_2),
            new FrameworkPropertyMetadata(800.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double MaximumPosition
        {
            get { return (double)GetValue(MaximumPositionProperty); }
            set { SetValue(MaximumPositionProperty, value); }
        }

        public static readonly DependencyProperty ThresholdOneProperty =
            DependencyProperty.Register("ThresholdOne", typeof(double), typeof(Elem_A6_1_2),
            new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double ThresholdOne
        {
            get { return (double)GetValue(ThresholdOneProperty); }
            set { SetValue(ThresholdOneProperty, value); }
        }

        public static readonly DependencyProperty ThresholdTwoProperty =
    DependencyProperty.Register("ThresholdTwo", typeof(double), typeof(Elem_A6_1_2),
    new FrameworkPropertyMetadata(3.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double ThresholdTwo
        {
            get { return (double)GetValue(ThresholdTwoProperty); }
            set { SetValue(ThresholdTwoProperty, value); }
        }

        public static readonly DependencyProperty CurrentValueProperty =
   DependencyProperty.Register("CurrentValue", typeof(double), typeof(Elem_A6_1_2),
   new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double CurrentValue
        {
            get { return (double)GetValue(CurrentValueProperty); }
            set { SetValue(CurrentValueProperty, value); }
        }

        public static readonly DependencyProperty TargetValueProperty =
  DependencyProperty.Register("TargetValue", typeof(double), typeof(Elem_A6_1_2),
  new FrameworkPropertyMetadata(450.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double TargetValue
        {
            get { return (double)GetValue(TargetValueProperty); }
            set { SetValue(TargetValueProperty, value); }
        }

        public static readonly DependencyProperty LimitValueProperty =
  DependencyProperty.Register("LimitValue", typeof(double), typeof(Elem_A6_1_2),
  new FrameworkPropertyMetadata(520.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double LimitValue
        {
            get { return (double)GetValue(LimitValueProperty); }
            set { SetValue(LimitValueProperty, value); }
        }

        public static readonly DependencyProperty ScaleFormatProperty =
           DependencyProperty.Register("ScaleFormat", typeof(string), typeof(Elem_A6_1_2),
           new FrameworkPropertyMetadata("G", FrameworkPropertyMetadataOptions.AffectsRender));
        public string ScaleFormat
        {
            get { return (string)GetValue(ScaleFormatProperty); }
            set { SetValue(ScaleFormatProperty, value); }
        }

        public static readonly DependencyProperty UnitFormatProperty =
          DependencyProperty.Register("UnitFormat", typeof(string), typeof(Elem_A6_1_2),
          new FrameworkPropertyMetadata("°C", FrameworkPropertyMetadataOptions.AffectsRender));
        public string UnitFormat
        {
            get { return (string)GetValue(UnitFormatProperty); }
            set { SetValue(UnitFormatProperty, value); }
        }


        // Action
        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            Pen pen1 = new Pen(Brushes.White, 2);
            double ShowAmount = 10;                                   // 預計顯示個數
            double IntervalLocation = this.ActualWidth / ShowAmount;  // 數字與刻度的間隔，在此設定畫10個
            double tickFirstPositionY = 60;                           // 刻度y方向開始位置
            double tickSecondPositionY = 75;                          // 刻度y方向結束位置

            #region 畫刻度與數字
            //畫刻度
            for (int i = 0; i < ShowAmount + 1; ++i)
            {
                var location = IntervalLocation * i;
                var point1 = new Point(location, tickFirstPositionY);
                var point2 = new Point(location, tickSecondPositionY);
                drawingContext.DrawLine(pen1, point1, point2);
            }
            for (int i = 0; i < ShowAmount; ++i)
            {
                var location = IntervalLocation * i + IntervalLocation / 2;
                var point1 = new Point(location, tickFirstPositionY);
                var point2 = new Point(location, tickSecondPositionY + 5);
                drawingContext.DrawLine(pen1, point1, point2);
            }

            //畫數字
            for (int i = 0; i < ShowAmount + 1; ++i)
            {
                var value = (Maximum - Minimum) / ShowAmount * i + Minimum;
                var location = (MaximumPosition - MinimumPosition) / ShowAmount * i - 10;         // -10 是數字shift往左位移，對齊刻度用                    
                var ft = new FormattedText(Math.Round(value, 0).ToString(ScaleFormat),
                     CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight,
                     new Typeface("Arial"), 15, Brushes.White);
                Point point = new Point(location, this.ActualHeight + 30);
                drawingContext.DrawText(ft, point);
            }
            #endregion


            #region 畫progressbar

            //Progressbar處理
            var pointStart = new Point(0, this.ActualHeight / 2);
            var pointT1 = new Point(0, this.ActualHeight / 2);
            var pointT2 = new Point(0, this.ActualHeight / 2);
            var pointT3 = new Point(0, this.ActualHeight / 2);

            double SizeValue = this.ActualWidth / ((Maximum - CurrentValue) / (CurrentValue - Minimum) + 1);                  // 目前數值轉換到Border位置   -- 內插轉換
            double TargetSizeValue = this.ActualWidth / ((Maximum - TargetValue) / (TargetValue - Minimum) + 1);    // 目標數值轉換到Border位置   -- 內插轉換
            double LimitSizeValue = this.ActualWidth / ((Maximum - LimitValue) / (LimitValue - Minimum) + 1);       // 極限數值轉換到Border位置   -- 內插轉換
            double T1 = this.ActualWidth / ((Maximum - ThresholdOne) / (ThresholdOne - Minimum) + 1);               // 第一個閥值轉換到Border位置 -- 內插轉換
            double T2 = this.ActualWidth / ((Maximum - ThresholdTwo) / (ThresholdTwo - Minimum) + 1);               // 第二個閥值轉換到Border位置 -- 內插轉換

            Pen BarPen1 = new Pen((SolidColorBrush)(new BrushConverter().ConvertFrom("#28559c")), this.ActualHeight);  //第一個閥值內的顏色
            Pen BarPen2 = new Pen((SolidColorBrush)(new BrushConverter().ConvertFrom("#eaab34")), this.ActualHeight);  //第二個閥值內的顏色
            Pen BarPen3 = new Pen((SolidColorBrush)(new BrushConverter().ConvertFrom("#c25746")), this.ActualHeight);  //第三個閥值內的顏色

            if (SizeValue <= T1)
            {
                pointT1 = new Point(SizeValue, this.ActualHeight / 2);
                drawingContext.DrawLine(BarPen1, pointStart, pointT1);
            }
            else if (SizeValue > T1 && SizeValue <= T2)
            {
                pointT1 = new Point(T1, this.ActualHeight / 2);
                pointT2 = new Point(SizeValue, this.ActualHeight / 2);
                drawingContext.DrawLine(BarPen1, pointStart, pointT1);
                drawingContext.DrawLine(BarPen2, pointT1, pointT2);
            }
            else if (SizeValue > T2 && SizeValue <= this.ActualWidth)
            {
                pointT1 = new Point(T1, this.ActualHeight / 2);
                pointT2 = new Point(T2, this.ActualHeight / 2);
                pointT3 = new Point(SizeValue, this.ActualHeight / 2);
                drawingContext.DrawLine(BarPen1, pointStart, pointT1);
                drawingContext.DrawLine(BarPen2, pointT1, pointT2);
                drawingContext.DrawLine(BarPen3, pointT2, pointT3);
            }
            else
            {
                pointT1 = new Point(0, this.ActualHeight / 2);
                pointT2 = new Point(0, this.ActualHeight / 2);
                pointT3 = new Point(0, this.ActualHeight / 2);
                drawingContext.DrawLine(BarPen1, pointStart, pointT1);
                drawingContext.DrawLine(BarPen2, pointT1, pointT2);
                drawingContext.DrawLine(BarPen3, pointT2, pointT3);
            }

            // 現在溫度
            //Pen BarPenCurrentValue = new Pen(Brushes.White, this.ActualHeight + 12);
            //drawingContext.DrawLine(BarPenCurrentValue, new Point(SizeValue - 3, this.ActualHeight / 2 - 6), new Point(SizeValue, this.ActualHeight / 2 - 6));

            //var NowState = new FormattedText("目前溫度:" + Math.Round(CurrentValue, 0).ToString(ScaleFormat),
            //        CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight,
            //        new Typeface("Arial"), 15, Brushes.White);

            //drawingContext.DrawText(NowState, new Point(SizeValue - 50, -30));


            ////目標溫度
            //Pen BarPenTargetValue = new Pen((SolidColorBrush)(new BrushConverter().ConvertFrom("#c25746")), this.ActualHeight + 12);
            //drawingContext.DrawLine(BarPenTargetValue, new Point(TargetSizeValue, this.ActualHeight / 2 - 6), new Point(TargetSizeValue + 3, this.ActualHeight / 2 - 6));

            //var TargetState = new FormattedText("目標溫度:" + Math.Round(TargetValue, 0).ToString(ScaleFormat),
            //        CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight,
            //        new Typeface("Arial"), 15, (SolidColorBrush)(new BrushConverter().ConvertFrom("#c25746")));
            //drawingContext.DrawText(TargetState, new Point(TargetSizeValue - 50, -30));


            //預設值
            Pen BarPenLimitValue = new Pen((SolidColorBrush)(new BrushConverter().ConvertFrom("#929193")), 2);
            BarPenLimitValue.DashStyle = DashStyles.Dash;
            drawingContext.DrawLine(BarPenLimitValue, new Point(LimitSizeValue, -10), new Point(LimitSizeValue, this.ActualHeight));

            var LimitState = new FormattedText("預設值:" + Math.Round(LimitValue, 0).ToString(ScaleFormat),
                    CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight,
                    new Typeface("Arial"), 15, (SolidColorBrush)(new BrushConverter().ConvertFrom("#929193")));
            drawingContext.DrawText(LimitState, new Point(LimitSizeValue - 50, -30));

            //單位
            //var UnitState = new FormattedText(UnitFormat,
            //   CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight,
            //   new Typeface("Arial"), 30, (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF")));
            //drawingContext.DrawText(UnitState, new Point(this.ActualWidth + 15, this.ActualHeight + 10));

            #endregion
        }
    }
}
