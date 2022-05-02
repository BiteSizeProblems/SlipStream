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
using static SlipStream.Structs.Appendeces;

namespace SlipStream.Views.Multi
{
    /// <summary>
    /// Interaction logic for LeaderboardView.xaml
    /// </summary>
    public partial class LeaderboardView : UserControl
    {
        public DataViewModel DataVM { get; set; }

        public LeaderboardView()
        {
            DataVM = DataViewModel.GetInstance();

            InitializeComponent();
            this.DataContext = DataVM;

            this.Leaderboard.Items.IsLiveSorting = true;
            this.Leaderboard.Items.SortDescriptions.Add(new SortDescription("CarPosition", ListSortDirection.Ascending));

            if (DataVM.model.CurrentSession == SessionTypes.RACE | DataVM.model.CurrentSession == SessionTypes.RACE_TWO)
            {
                Leaderboard.Columns[8].Visibility = Visibility.Collapsed;
                Leaderboard.Columns[13].Visibility = Visibility.Collapsed;
                Leaderboard.Columns[14].Visibility = Visibility.Visible;
            }
            else
            {
                Leaderboard.Columns[1].Visibility = Visibility.Collapsed;
                Leaderboard.Columns[2].Visibility = Visibility.Collapsed;
                Leaderboard.Columns[8].Visibility = Visibility.Visible;
                Leaderboard.Columns[9].Visibility = Visibility.Collapsed;
                Leaderboard.Columns[10].Visibility = Visibility.Collapsed;
                Leaderboard.Columns[13].Visibility = Visibility.Visible;
                Leaderboard.Columns[14].Visibility = Visibility.Collapsed;

                DeltaButton.Visibility = Visibility.Collapsed;
            }

        }

        private void VisibilityButtonInstance_Click(object sender, RoutedEventArgs e)
        {
            if(Leaderboard.Columns[9].Visibility == Visibility.Collapsed)
            {
                Leaderboard.Columns[10].Visibility = Visibility.Collapsed; // Interval
                Leaderboard.Columns[9].Visibility = Visibility.Visible; // Leader
            }
            else
            {
                Leaderboard.Columns[10].Visibility = Visibility.Visible;
                Leaderboard.Columns[9].Visibility = Visibility.Collapsed;
            }
        }
    }
}
