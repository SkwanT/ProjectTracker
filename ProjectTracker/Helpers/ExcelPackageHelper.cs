using OfficeOpenXml;
using OfficeOpenXml.Style;
using ProjectTracker.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectTracker.Helpers
{
    public static class ExcelPackageHelper
    {
        public static ExcelPackage GenerateExcelFile(List<Script> datasource)
        {
            ExcelPackage pck = new ExcelPackage();

            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            ws.Cells[1, 1].Value = "Entry Date";
            ws.Cells[1, 1].Style.Numberformat.Format = "d.M.yyyy";
            ws.Cells[1, 2].Value = "Script Name";
            ws.Cells[1, 3].Value = "Type";
            ws.Cells[1, 4].Value = "Author";
            ws.Cells[1, 5].Value = "Project Number";
            ws.Cells[1, 6].Value = "Project Name";
            ws.Cells[1, 7].Value = "Status";
            ws.Cells[1, 8].Value = "Location";
            ws.Cells[1, 9].Value = "Comments";


            for (int i = 0; i < datasource.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = datasource.ElementAt(i).EntryDate;
                ws.Cells[i + 2, 1].Style.Numberformat.Format = "d.M.yyyy";
                ws.Cells[i + 2, 2].Value = datasource.ElementAt(i).ScriptName;
                ws.Cells[i + 2, 3].Value = datasource.ElementAt(i).ScriptType.Type;
                ws.Cells[i + 2, 4].Value = datasource.ElementAt(i).Author.FullName;
                ws.Cells[i + 2, 5].Value = ExtractProjectNumbering(datasource.ElementAt(i).ProjectName);
                ws.Cells[i + 2, 6].Value = datasource.ElementAt(i).ProjectName;
                ws.Cells[i + 2, 7].Value = datasource.ElementAt(i).ProjectStatus;
                ws.Cells[i + 2, 8].Value = datasource.ElementAt(i).ProjectLocation;
                ws.Cells[i + 2, 9].Value = datasource.ElementAt(i).Comments;
            }

            using (ExcelRange rng = ws.Cells["A1:I1"])
            {

                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                rng.Style.Font.Color.SetColor(Color.Black);
            }

            ws.Cells.AutoFitColumns();
            return pck;
        }


        private static string ExtractProjectNumbering(string input)
        {
            string[] patterns = { @"\d{2}-\d{6}-\d{2}-\d{2}", @"\d{2}-\d{6}-\d{2}", @"\d{2}-\d{6}" };

            foreach (var pattern in patterns)
            {
                Match match = Regex.Match(input, pattern);
                if (match.Success)
                {
                    return match.Value;
                }
            }
            return input;
        }

        public static ExcelPackage GenerateExcelFile(List<Report> datasource)
        {

            ExcelPackage pck = new ExcelPackage();

            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            ws.Cells[1, 1].Value = "Complexity";
            ws.Cells[1, 2].Value = "Points";
            ws.Cells[1, 3].Value = "Country";
            ws.Cells[1, 4].Value = "Script";
            ws.Cells[1, 5].Value = "Type";
            ws.Cells[1, 6].Value = "Author";
            ws.Cells[1, 7].Value = "Project";
            ws.Cells[1, 8].Value = "Task Date";
            ws.Cells[1, 8].Style.Numberformat.Format = "d.M.yyyy";
            ws.Cells[1, 9].Value = "Start Date";
            ws.Cells[1, 9].Style.Numberformat.Format = "d.M.yyyy";
            ws.Cells[1, 10].Value = "End Date";
            ws.Cells[1, 10].Style.Numberformat.Format = "d.M.yyyy";
            ws.Cells[1, 11].Value = "Status";
            ws.Cells[1, 12].Value = "Estimated Hours";
            ws.Cells[1, 13].Value = "Actual Hours";
            ws.Cells[1, 14].Value = "Test Hours";
            ws.Cells[1, 15].Value = "Test Errors";
            ws.Cells[1, 16].Value = "Field Errors";
            ws.Cells[1, 17].Value = "Comments";

            for (int i = 0; i < datasource.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = datasource.ElementAt(i).ComplexityID;
                ws.Cells[i + 2, 2].Value = datasource.ElementAt(i).Points;
                ws.Cells[i + 2, 3].Value = datasource.ElementAt(i).Country.Code;
                ws.Cells[i + 2, 4].Value = datasource.ElementAt(i).Script.ScriptName;
                ws.Cells[i + 2, 5].Value = datasource.ElementAt(i).Script.ScriptType.Type;
                ws.Cells[i + 2, 6].Value = datasource.ElementAt(i).Script.Author.FullName;
                ws.Cells[i + 2, 7].Value = datasource.ElementAt(i).Script.ProjectName;
                ws.Cells[i + 2, 8].Value = datasource.ElementAt(i).TaskSentDate;
                ws.Cells[i + 2, 8].Style.Numberformat.Format = "d.M.yyyy";
                ws.Cells[i + 2, 9].Value = datasource.ElementAt(i).ScriptEntryDate;
                ws.Cells[i + 2, 9].Style.Numberformat.Format = "d.M.yyyy";
                ws.Cells[i + 2, 10].Value = datasource.ElementAt(i).ScriptDoneDate;
                ws.Cells[i + 2, 10].Style.Numberformat.Format = "d.M.yyyy";
                ws.Cells[i + 2, 11].Value = datasource.ElementAt(i).ScriptStatus;
                ws.Cells[i + 2, 12].Value = datasource.ElementAt(i).EstimatedScriptingHours;
                ws.Cells[i + 2, 13].Value = datasource.ElementAt(i).ActualScriptingHours;
                ws.Cells[i + 2, 14].Value = datasource.ElementAt(i).ActualTestingHours;
                ws.Cells[i + 2, 15].Value = datasource.ElementAt(i).ScriptInTestErrors;
                ws.Cells[i + 2, 16].Value = datasource.ElementAt(i).ScriptInFieldErrors;
                ws.Cells[i + 2, 17].Value = datasource.ElementAt(i).ScriptComments;
            }

            using (ExcelRange rng = ws.Cells["A1:Q1"])
            {

                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                rng.Style.Font.Color.SetColor(Color.Black);
            }

            ws.Cells.AutoFitColumns();
            return pck;
        }
    }
}