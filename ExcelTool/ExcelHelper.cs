using ExcelTool.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace ExcelTool
{
    public class ExcelHelper
    {
        private static string[] titles = new string[]
        {
            "地方", "学校", "学院", "人才类别", "职位", "姓名",
            "研究方向关键词", "详细研究方向", "备注", "科学院", "职称", "项目", "分组"
        };

        public static void ReadTalentInfo(string filePath, TalentDictionary dic)
        {
            if (dic == null) return;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var sheet = package.Workbook.Worksheets["Sheet1"];
                //int colCount = sheet.Dimension.End.Column;
                int rowCount = sheet.Dimension.End.Row;
                
                for(int r = sheet.Dimension.Start.Row + 1; r <= rowCount; r++)
                {
                    dic.Add(new Talent(GetMergeValue(sheet, r, 1),
                                           GetMergeValue(sheet, r, 2),
                                           GetMergeValue(sheet, r, 3),
                                           GetMergeValue(sheet, r, 4),
                                           GetMergeValue(sheet, r, 5),
                                           sheet.GetValue<string>(r, 6),
                                           sheet.GetValue<string>(r, 7),
                                           sheet.GetValue<string>(r, 8),
                                           sheet.GetValue<string>(r, 9),
                                           sheet.GetValue<string>(r, 10),
                                           sheet.GetValue<string>(r, 11),
                                           GetMergeValue(sheet, r, 12),
                                           GetMergeValue(sheet, r, 13)
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

        public static void WriteTalentInfo(string filePath, TalentDictionary dic)
        {
            if (dic == null) return;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Sheet1");
                package.Save();
            }
        }

        public static void WriteTalentTitle(ExcelWorksheet sheet)
        {
            //sheet.Cells[1, 1].Value = "地方";
            //sheet.Cells[1, 2].Value = "学校";
            //sheet.Cells[1, 3].Value = "学院";
            //sheet.Cells[1, 4].Value = "人才类别";
            //sheet.Cells[1, 5].Value = "职位";
            //sheet.Cells[1, 6].Value = "姓名";
            //sheet.Cells[1, 7].Value = "研究方向关键词";
            //sheet.Cells[1, 8].Value = "详细研究方向";
            //sheet.Cells[1, 9].Value = "备注";
            //sheet.Cells[1, 10].Value = "科学院";
            //sheet.Cells[1, 11].Value = "职称";
            //sheet.Cells[1, 12].Value = "项目";
            //sheet.Cells[1, 13].Value = "分组";

            for(int i = 1; i <= 13; i++)
            {
                sheet.Cells[1, i].Value = titles[i - 1];
                sheet.Cells[1, i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[1, i].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }
        }

        public static void WriteTalentTitle(ExcelWorksheet sheet, ScreenCondition screenCondition)
        {
            int index = 1;
            for(int i = 0; i < screenCondition.ShowTitles.Length; i++)
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

        public static void WriteTalentInfo(ExcelWorksheet sheet, ICollection<Talent> talents)
        {
            int startRow = sheet.Dimension.End.Row + 1;
            foreach(var talent in talents)
            {
                sheet.Cells[startRow, 1].Value = talent.CompetentDepartment;
                sheet.Cells[startRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                sheet.Cells[startRow, 2].Value = talent.School;
                sheet.Cells[startRow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                sheet.Cells[startRow, 3].Value = talent.Institute;
                sheet.Cells[startRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                sheet.Cells[startRow, 4].Value = talent.TalentType;
                sheet.Cells[startRow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                sheet.Cells[startRow, 5].Value = talent.Position;
                sheet.Cells[startRow, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                sheet.Cells[startRow, 6].Value = talent.Name;
                sheet.Cells[startRow, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                sheet.Cells[startRow, 7].Value = talent.ResearchInterestsKeywords;
                sheet.Cells[startRow, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                sheet.Cells[startRow, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                sheet.Cells[startRow, 7].Style.WrapText = true;

                sheet.Cells[startRow, 8].Value = talent.ResearchInterests;
                sheet.Cells[startRow, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                sheet.Cells[startRow, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                sheet.Cells[startRow, 8].Style.WrapText = true;

                sheet.Cells[startRow, 9].Value = talent.Remark;
                sheet.Cells[startRow, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                sheet.Cells[startRow, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                sheet.Cells[startRow, 9].Style.WrapText = true;

                sheet.Cells[startRow, 10].Value = talent.AcademyOfScience;
                sheet.Cells[startRow, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                sheet.Cells[startRow, 11].Value = talent.ProfessionalTitle;
                sheet.Cells[startRow, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                sheet.Cells[startRow, 12].Value = talent.Project;
                sheet.Cells[startRow, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                sheet.Cells[startRow, 13].Value = talent.Group;
                sheet.Cells[startRow, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                startRow++;
            }
        }

        public static void WriteTalentInfo(ExcelWorksheet sheet, ICollection<Talent> talents, ScreenCondition screenCondition)
        {
            int startRow = sheet.Dimension.End.Row + 1;
            foreach (var talent in talents)
            {
                int index = 1;
                for(int i = 0; i < screenCondition.ShowTitles.Length; i++)
                {
                    if (screenCondition.ShowTitles[i])
                    {
                        sheet.Cells[startRow, index].Value = talent[i];
                        sheet.Cells[startRow, index].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheet.Cells[startRow, index].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        if (i == 6 || i == 7 || i == 8)
                            sheet.Cells[startRow, index].Style.WrapText = true;
                        index++;
                    }
                }
                startRow++;
            }
        }

        private static string GetMergeValue(ExcelWorksheet wSheet, int row, int column)
        {
            string range = wSheet.MergedCells[row, column];
            if(range == null)
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