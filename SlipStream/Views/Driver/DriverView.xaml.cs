using SlipStream.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for DriverView.xaml
    /// </summary>
    public partial class DriverView : UserControl
    {
        // === ViewModel ===
        public DriverViewModel DVM { get => DriverViewModel.GetInstance(); }

        public DriverView()
        {
            InitializeComponent();
            this.DataContext = DVM;

            //this.Leaderboard.Items.IsLiveSorting = true;
            //this.Leaderboard.Items.SortDescriptions.Add(new SortDescription("CarPosition", ListSortDirection.Ascending));
        }
    }
}
