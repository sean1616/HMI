using System.ComponentModel;
using System.Windows.Media;


namespace mainHMI.gaugedata
{
    public class CircleGaugeData : INotifyPropertyChanged
    {
        public CircleGaugeData()   //此處內容待確認，因Title2、Title3、GoalPercentage也沒有在此設定，一樣可用
        {
            Title = "---";
            Percentage = 0.0;
            Unit = "unit";
            Unit2 = "";
            TickNumberAmount = 10;
            Visibility = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // 名稱
        private string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }
        }

        // 名稱2
        private string _Title2;
        public string Title2
        {
            get
            {
                return _Title2;
            }
            set
            {
                _Title2 = value;
                OnPropertyChanged("Title2");
            }
        }

        // 名稱3
        private string _Title3;
        public string Title3
        {
            get
            {
                return _Title3;
            }
            set
            {
                _Title3 = value;
                OnPropertyChanged("Title3");
            }
        }

        // 目前數值
        private double _Percentage;
        public double Percentage
        {
            get
            {
                return _Percentage;
            }
            set
            {
                _Percentage = value;
                OnPropertyChanged("Percentage");
            }
        }

        // 目標數值
        private double _GoalPercentage;
        public double GoalPercentage
        {
            get
            {
                return _GoalPercentage;
            }
            set
            {
                _GoalPercentage = value;
                OnPropertyChanged("GoalPercentage");
            }
        }

        //  單位
        private string _Unit;
        public string Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                _Unit = value;
                OnPropertyChanged("Unit");
            }
        }

        //  單位(上標)
        private string _Unit2;
        public string Unit2
        {
            get
            {
                return _Unit2;
            }
            set
            {
                _Unit2 = value;
                OnPropertyChanged("Unit2");
            }
        }

        // 刻度數值個數
        private int _TickNumberAmount;
        public int TickNumberAmount
        {
            get
            {
                return _TickNumberAmount;
            }
            set
            {
                _TickNumberAmount = value;
                OnPropertyChanged("TickNumberAmount");
            }
        }

        // 第一段顏色
        private Brush _Color1;
        public Brush Color1
        {
            get
            {
                return _Color1;
            }
            set
            {
                _Color1 = value;
                OnPropertyChanged("Color1");
            }
        }

        // 第二段顏色
        private Brush _Color2;
        public Brush Color2
        {
            get
            {
                return _Color2;
            }
            set
            {
                _Color2 = value;
                OnPropertyChanged("Color2");
            }
        }

        // 第三段顏色
        private Brush _Color3;
        public Brush Color3
        {
            get
            {
                return _Color3;
            }
            set
            {
                _Color3 = value;
                OnPropertyChanged("Color3");
            }
        }

        // 顏色分隔位置1
        private int _ColorDivideAngle1;
        public int ColorDivideAngle1
        {
            get
            {
                return _ColorDivideAngle1;
            }
            set
            {
                _ColorDivideAngle1 = value;
                OnPropertyChanged("ColorDivideAngle1");
            }
        }

        // 顏色分隔位置2
        private int _ColorDivideAngle2;
        public int ColorDivideAngle2
        {
            get
            {
                return _ColorDivideAngle2;
            }
            set
            {
                _ColorDivideAngle2 = value;
                OnPropertyChanged("ColorDivideAngle2");
            }
        }

        // Visibility
        private bool _Visibility;
        public bool Visibility
        {
            get
            {
                return _Visibility;
            }
            set
            {
                _Visibility = value;
                OnPropertyChanged("Visibility");
            }
        }
    }
}
