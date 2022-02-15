﻿using SlipStream.ViewModels;
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

namespace SlipStream.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        // === ViewModel ===
        public HomeViewModel HVM { get => HomeViewModel.GetInstance(); }

        public HomeView()
        {
            InitializeComponent();
            this.DataContext = HVM;
        }
    }
}
