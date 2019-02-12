using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using HMI_PermanentForm.Utils;

namespace HMI_PermanentForm.E_folder.Converter
{
    class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (GlobalChannel.SlicingVM.NumOfLayers == 0)
                return 0;
            else
            {
                int tmp = (int)((double)value / (double)GlobalChannel.SlicingVM.NumOfLayers * 100);
                return tmp.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
