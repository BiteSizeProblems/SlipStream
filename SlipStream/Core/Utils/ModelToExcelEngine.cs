using System.Data;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlipStream.Models;
using System.IO;
using OfficeOpenXml;

namespace SlipStream.Core.Utils
{
    public class ModelToExcelEngine
    {
        public static async Task ModelToExcel(string[] args)
        {
            var driver2 = new List<DriverModel>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var file = new FileInfo(@"C:\Users\alexa\OneDrive\Documents\TestFile.xlsx");

            await SaveExcelFile(driver2, file);
        }

        private static async Task SaveExcelFile(List<DriverModel> driver2, FileInfo file)
        {
            DeleteIfExists(file);

            using var package = new ExcelPackage(file);

            var ws = package.Workbook.Worksheets.Add("FinalClassification");

            var range = ws.Cells["A1"].LoadFromCollection(driver2, true);
            range.AutoFitColumns();

            await package.SaveAsync();
        }

        private static void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}
