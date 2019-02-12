﻿using System;
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

namespace HMI_PermanentForm.Utils
{
    /// <summary>
    /// View3D.xaml 的互動邏輯
    /// </summary>
    public partial class View3D : Page
    {
        public View3D()
        {
            InitializeComponent();
            
            ViewPort.DataContext = GlobalChannel.SlicingVM;
        }
    }
}
