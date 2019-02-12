using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HMI_PermanentForm
{
    class Lang
    {
        ResourceDictionary Lang_ResourceDictionary = new ResourceDictionary();
        ResourceDictionary Lang_ResourceDictionary_B = new ResourceDictionary();

        public void En_to_Ch()
        {          
            Lang_ResourceDictionary.Clear();
            Lang_ResourceDictionary.Source = new Uri("C folder/Language/Lang_C.xaml", UriKind.RelativeOrAbsolute);
            Lang_ResourceDictionary_B.Source = new Uri("B folder/Language/Lang_B.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(Lang_ResourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(Lang_ResourceDictionary_B);
        }

        public void Ch_to_En()
        {
            Lang_ResourceDictionary.Clear();
            Lang_ResourceDictionary.Source = new Uri("C folder/Language/Lang_Eng_C.xaml", UriKind.RelativeOrAbsolute);
            Lang_ResourceDictionary_B.Source = new Uri("B folder/Language/Lang_En_B.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(Lang_ResourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(Lang_ResourceDictionary_B);
        }
    }
}
