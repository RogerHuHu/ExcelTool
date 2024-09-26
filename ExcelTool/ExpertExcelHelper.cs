using ExcelTool.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace ExcelTool
{
    public class ExpertExcelHelper
    {
        private static string[] titles = new string[]
        {
            "省份", "单位", "学院", "姓名", "职称", "职务",
            "研究方向", "奖项", "备注", "是否报奖"
        };

        public static void ReadExpertInfo(string filePath, ExpertDictionary dic)
        {
            if (dic == null) return;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var sheet = package.Workbook.Worksheets["Sheet1"];
                //int colCount = sheet.Dimension.End.Column;
                int rowCount = sheet.Dimension.End.Row;

                for (int r = sheet.Dimension.Start.Row + 1; r <= rowCount; r++)
                {
                    dic.Add(new Expert(GetMergeValue(sheet, r, 1),
                                       GetMergeValue(sheet, r, 2),
                                       GetMergeValue(sheet, r, 3),
                                       sheet.GetValue<string>(r, 4),
                                       sheet.GetValue<string>(r, 5),
                                       sheet.GetValue<string>(r, 6),
                                       sheet.GetValue<string>(r, 7),
                                       sheet.GetValue<string>(r, 8),
                                       sheet.GetValue<string>(r, 9),
                                       sheet.GetValue<string>(r, 10)
                                       ));
                }
            }
        }

        public static ExcelPackage NewExcelPackage(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage(new FileStream(filePath, FileMode.Create, FileAccess.Write));
            return package;
        }

        public static ExcelWorksheet NewWorksheet(ExcelPackage package, string worksheetName)
        {
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add(worksheetName);
            return sheet;
        }

        public static void WriteExpertInfo(string filePath, ExpertDictionary dic)
        {
            if (dic == null) return;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Sheet1");
                package.Save();
            }
        }

        public static void WriteExpertTitle(ExcelWorksheet sheet)
        {
            for (int i = 1; i <= 10; i++)
            {
                sheet.Cells[1, i].Value = titles[i - 1];
                sheet.Cells[1, i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[1, i].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }
        }

        public static void WriteExpertTitle(ExcelWorksheet sheet, ScreenCondition screenCondition)
        {
            int index = 1;
            for (int i = 0; i < screenCondition.ShowTitles.Length; i++)
            {
                if (screenCondition.ShowTitles[i])
                {
                    sheet.Cells[1, index].Value = titles[i];
                    sheet.Cells[1, index].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[1, index].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    index++;
                }
            }
        }

        public static void WriteExpertInfo(ExcelWorksheet sheet, ICollection<Expert> experts)
        {
            int startRow = sheet.Dimension.End.Row + 1;
            foreach (var expert in experts)
            {
                int index = 1;
                for (int i = 0; i < titles.Length; i++)
                {
                    sheet.Cells[startRow, index].Value = expert[i];
                    sheet.Cells[startRow, index].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[startRow, index].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    index++;
                }
                startRow++;
            }
        }

        public static void WriteTalentInfo(ExcelWorksheet sheet, ICollection<Talent> talents, ScreenCondition screenCondition)
        {
            int startRow = sheet.Dimension.End.Row + 1;
            foreach (var talent in talents)
            {
                int index = 1;
                for (int i = 0; i < screenCondition.ShowTitles.Length; i++)
                {
                    if (screenCondition.ShowTitles[i])
                    {
                        sheet.Cells[startRow, index].Value = talent[i];
                        sheet.Cells[startRow, index].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheet.Cells[startRow, index].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        index++;
                    }
                }
                startRow++;
            }
        }

        private static string GetMergeValue(ExcelWorksheet wSheet, int row, int column)
        {
            string range = wSheet.MergedCells[row, column];
            if (range == null)
            {
                if (wSheet.Cells[row, column].Value != null)
                    return wSheet.Cells[row, column].Value.ToString();
                else
                    return "";
            }

            object value = wSheet.Cells[(new ExcelAddress(range)).Start.Row, (new ExcelAddress(range)).Start.Column].Value;
            if (value != null)
                return value.ToString();
            else
                return "";
        }
    }
}