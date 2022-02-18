using SlipStream.ViewModels;
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
    /// Interaction logic for DriverViewSummary.xaml
    /// </summary>
    public partial class DriverViewSummary : UserControl
    {
        // === ViewModel ===
        public DriverViewModel DVM { get => DriverViewModel.GetInstance(); }

        public DriverViewSummary()
        {
            InitializeComponent();
            this.DataContext = DVM;
        }
    }
}
