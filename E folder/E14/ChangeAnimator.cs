using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HMI_PermanentForm
{
    public class ChangeAnimator : FrameworkElement
    {
        public static readonly DependencyProperty SourceValueProperty =
            DependencyProperty.Register("SourceValue", typeof(double), typeof(ChangeAnimator),
            new PropertyMetadata(0.0,
                (s, e) =>
                {
                    ChangeAnimator me = (ChangeAnimator)s;
                    if (me.SourceMin == me.SourceMax) return;

                    double toValue = ((((double)e.NewValue - me.SourceMin) / (me.SourceMax - me.SourceMin)) * (me.TargetMax - me.TargetMin)) + me.TargetMin;
                    me.BeginAnimation(TargetValueProperty,
                        new DoubleAnimation(
                            toValue,
                            new Duration(TimeSpan.FromSeconds(0.25))), HandoffBehavior.SnapshotAndReplace);

                    Color toColor;
                    if ((double)e.NewValue < 100.0)
                        toColor = Color.FromRgb(133, 178, 0);
                    else if ((double)e.NewValue >= 100.0 && (double)e.NewValue < 150.0)
                        toColor = Color.FromRgb(255, 255, 84);
                    else
                        toColor = Color.FromRgb(255, 35, 35);

                    me.BeginAnimation(TargetColorProperty,
                        new ColorAnimation(
                            toColor,
                            new Duration(TimeSpan.FromSeconds(0.5))));
                }));
        public double SourceValue
        {
            get { return (double)GetValue(SourceValueProperty); }
            set { SetValue(SourceValueProperty, value); }
        }

        public static readonly DependencyProperty SourceMinProperty =
            DependencyProperty.Register("SourceMin", typeof(double), typeof(ChangeAnimator));
        public double SourceMin
        {
            get { return (double)GetValue(SourceMinProperty); }
            set { SetValue(SourceMinProperty, value); }
        }

        public static readonly DependencyProperty SourceMaxProperty =
            DependencyProperty.Register("SourceMax", typeof(double), typeof(ChangeAnimator),
            new PropertyMetadata(100.0));
        public double SourceMax
        {
            get { return (double)GetValue(SourceMaxProperty); }
            set { SetValue(SourceMaxProperty, value); }
        }

        public static readonly DependencyProperty TargetValueProperty =
            DependencyProperty.Register("TargetValue", typeof(double), typeof(ChangeAnimator),
            new PropertyMetadata(-110.0));
        public double TargetValue
        {
            get { return (double)GetValue(TargetValueProperty); }
            set { SetValue(TargetValueProperty, value); }
        }

        public static readonly DependencyProperty TargetMinProperty =
            DependencyProperty.Register("TargetMin", typeof(double), typeof(ChangeAnimator));
        public double TargetMin
        {
            get { return (double)GetValue(TargetMinProperty); }
            set { SetValue(TargetMinProperty, value); }
        }

        public static readonly DependencyProperty TargetMaxProperty =
            DependencyProperty.Register("TargetMax", typeof(double), typeof(ChangeAnimator));
        public double TargetMax
        {
            get { return (double)GetValue(TargetMaxProperty); }
            set { SetValue(TargetMaxProperty, value); }
        }

        public static readonly DependencyProperty TargetColorProperty =
            DependencyProperty.Register("TargetColor", typeof(Color), typeof(ChangeAnimator));
        public Color TargetColor
        {
            get { return (Color)GetValue(TargetColorProperty); }
            set { SetValue(TargetColorProperty, value); }
        }
    }
}
