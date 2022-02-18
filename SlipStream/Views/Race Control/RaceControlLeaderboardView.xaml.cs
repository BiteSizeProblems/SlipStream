using OfficeOpenXml;
using OfficeOpenXml.Style;
using SlipStream.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace SlipStream.Views
{
    /// <summary>
    /// Interaction logic for RaceControlLeaderboardView.xaml
    /// </summary>
    public partial class RaceControlLeaderboardView : UserControl
    {
        public RaceControlLeaderboardView()
        {
            InitializeComponent();
        }

        private void LeaderboardExportClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
