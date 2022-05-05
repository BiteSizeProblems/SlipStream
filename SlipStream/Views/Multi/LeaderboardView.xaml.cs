using SlipStream.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using static SlipStream.Structs.Appendeces;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using SlipStream.Models;
using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.Drawing;

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
                Leaderboard.Columns[8].Visibility = Visibility.Collapsed; // Best Lap Time Delta
                Leaderboard.Columns[14].Visibility = Visibility.Collapsed; // Laps Completed
                Leaderboard.Columns[15].Visibility = Visibility.Visible; // # of Stops
                Leaderboard.Columns[19].Visibility = Visibility.Visible; // PTS

                Leaderboard.Columns[13].Width = 85;
                Leaderboard.Columns[14].Width = 85;
                Leaderboard.Columns[15].Width = 85;
                Leaderboard.Columns[16].Width = 100; // Tire Column Width
                Leaderboard.Columns[17].Width = 100;
            }
            else
            {
                Leaderboard.Columns[1].Visibility = Visibility.Collapsed; // Position Gain Icon
                Leaderboard.Columns[2].Visibility = Visibility.Collapsed; // Position Gain #
                Leaderboard.Columns[8].Visibility = Visibility.Visible; // Best Lap Time Delta
                Leaderboard.Columns[9].Visibility = Visibility.Collapsed; // Race Interval
                Leaderboard.Columns[10].Visibility = Visibility.Collapsed; // Race Interval LEADER
                Leaderboard.Columns[14].Visibility = Visibility.Visible; // Laps Completed
                Leaderboard.Columns[15].Visibility = Visibility.Collapsed; // # of Stops
                Leaderboard.Columns[19].Visibility = Visibility.Collapsed; // PTS

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

        public static async Task ModelToExcel()
        {
            var DataVM = DataViewModel.GetInstance();
            var driver = DataVM.Driver;
            var session = DataVM.SessionModel;

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var file = new FileInfo(@"C:\Users\alexa\OneDrive\Documents\SlipstreamExport.xlsx");

            await SaveExcelFile(driver, session, file);
        }

        private static async Task SaveExcelFile(ObservableCollection<DriverModel> driver, ObservableCollection<SessionModel> session, FileInfo file)
        {
            DeleteIfExists(file);

            using var package = new ExcelPackage(file);

            // Create new worksheets
            var ws = package.Workbook.Worksheets.Add("SessionData");
            //var ws_session = package.Workbook.Worksheets.Add("SessionInfo");

            // Format Column Data Types
            //ws.Column(0).Style.Numberformat = 

            // Select & Filter
            var range = ws.Cells["A1"].LoadFromCollection(driver, true);
            //var range_session = ws_session.Cells["A1"].LoadFromCollection(session, true);

            //ws.DeleteColumn(71, 9);
            //ws.DeleteColumn(68);
            //ws.DeleteColumn(61);
            //ws.DeleteColumn(58, 2);
            //ws.DeleteColumn(54, 3);
            //ws.DeleteColumn(52);
            //ws.DeleteColumn(48, 2);
            //ws.DeleteColumn(37, 6);
            //ws.DeleteColumn(19, 8);
            //ws.DeleteColumn(16);
            //ws.DeleteColumn(12, 3);
            //ws.DeleteColumn(3,3);

            //ws.DeleteColumn(3, 3);
            //ws.DeleteColumn(12, 3);
            //ws.DeleteColumn(16);
            //ws.DeleteColumn(19, 8);
            //ws.DeleteColumn(37, 6);
            //ws.DeleteColumn(48, 2);
            //ws.DeleteColumn(52);
            //ws.DeleteColumn(54, 3);
            //ws.DeleteColumn(58, 2);
            //ws.DeleteColumn(61);
            //ws.DeleteColumn(68);
            //ws.DeleteColumn(71, 9);

            //ws.Column(9).Style.Numberformat.Format = "21";

            range.AutoFilter = true;
            var colPosition = ws.AutoFilter.Columns.AddValueFilterColumn(0);

            //ws.Columns.AutoFit();

            // Style & Format all cells
            ws.Cells["A2:CF23"].Sort(x => x.SortBy.Column(0));

            ws.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            ws.Cells["D2:D25"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

            ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            //ws.Columns.BestFit = true;

            ws.Cells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);

            // Style Header
            ws.Row(1).Style.Font.Bold = true;
            ws.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

            // Format Session Sheet

            await package.SaveAsync();
        }

        private static void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }

        private void ModelToExcel(object sender, RoutedEventArgs e)
        {
            ModelToExcel();
        }
    }
}
