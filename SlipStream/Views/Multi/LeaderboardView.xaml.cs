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

namespace SlipStream.Views.Multi
{
    /// <summary>
    /// Interaction logic for LeaderboardView.xaml
    /// </summary>
    public partial class LeaderboardView : UserControl
    {
        //public DataViewModel DVM { get; set; }

        public LeaderboardView()
        {
            InitializeComponent();

            this.Leaderboard.Items.IsLiveSorting = true;
            this.Leaderboard.Items.SortDescriptions.Add(new SortDescription("CarPosition", ListSortDirection.Ascending));
        }
    }
}
