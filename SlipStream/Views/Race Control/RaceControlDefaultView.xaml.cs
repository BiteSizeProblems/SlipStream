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
    /// Interaction logic for RaceControlDefaultView.xaml
    /// </summary>
    public partial class RaceControlDefaultView : UserControl
    {

        public RaceControlDefaultView()
        {
            InitializeComponent();
        }

        private void CompressedLeaderboard_Sorting(object sender, DataGridSortingEventArgs e)
        {
            this.CompressedLeaderboard.Items.IsLiveSorting = true;
            this.CompressedLeaderboard.Items.SortDescriptions.Add(new SortDescription("DriverName", ListSortDirection.Ascending));
        }
    }
}
