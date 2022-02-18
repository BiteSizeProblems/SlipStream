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
    /// Interaction logic for RaceControlView.xaml
    /// </summary>
    public partial class RaceControlView : UserControl
    {
        // === ViewModel ===
        public RaceControlViewModel RCVM { get => RaceControlViewModel.GetInstance(); }

        public RaceControlView()
        {
            InitializeComponent();
            this.DataContext = RCVM;
        }
    }
}
