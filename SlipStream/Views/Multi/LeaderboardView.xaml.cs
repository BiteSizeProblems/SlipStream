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

        public static async Task ModelToExcel()
        {
            var DataVM = DataViewModel.GetInstance();
            var driver = DataVM.Driver;

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var file = new FileInfo(@"C:\Users\alexa\OneDrive\Documents\SlipstreamExport.xlsx");

            await SaveExcelFile(driver, file);
        }

        private static async Task SaveExcelFile(ObservableCollection<DriverModel> driver, FileInfo file)
        {
            DeleteIfExists(file);

            using var package = new ExcelPackage(file);

            // Create a new worksheet
            var ws = package.Workbook.Worksheets.Add("FinalClassification");

            // Format Column Data Types
            //ws.Column(0).Style.Numberformat = 

            // Select & Filter
            var range = ws.Cells["A1"].LoadFromCollection(driver, true);

            ws.DeleteColumn(71, 9);
            ws.DeleteColumn(68);
            ws.DeleteColumn(61);
            ws.DeleteColumn(58, 2);
            ws.DeleteColumn(54, 3);
            ws.DeleteColumn(52);
            ws.DeleteColumn(48, 2);
            ws.DeleteColumn(37, 6);
            ws.DeleteColumn(19, 8);
            ws.DeleteColumn(16);
            ws.DeleteColumn(12, 3);
            ws.DeleteColumn(3,3);

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

            range.AutoFilter = true;
            var colPosition = ws.AutoFilter.Columns.AddValueFilterColumn(0);
            
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
