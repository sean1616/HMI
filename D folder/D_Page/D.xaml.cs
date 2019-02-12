using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace HMI_PermanentForm
{
    /// <summary>
    /// Dict_History_Page.xaml 的互動邏輯
    /// </summary>
    public partial class D : Page
    {
        public D()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem itm = new ListBoxItem();
            itm.Content = "yayayaya";
            listbox.Items.Add(itm);
        }
                
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (f.ShowDialog() == true)
            {
                using (StreamReader r = new StreamReader(f.OpenFile()))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        ListBoxItem itm = new ListBoxItem();
                        itm.Content = line;
                        listbox.Items.Add(itm);
                    }
                }
            }
        }
    }
}
