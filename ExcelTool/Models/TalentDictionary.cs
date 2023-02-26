using HIIUtils.String;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;

namespace ExcelTool.Models
{
    public class TalentNameDictionary
    {
        private Dictionary<string, Talent> dic = null;

        public TalentNameDictionary()
        {
            dic = new Dictionary<string, Talent>();
        }

        public int Count => dic.Count;

        public void Add(Talent talent)
        {
            if (!string.IsNullOrEmpty(talent.Name) && !dic.ContainsKey(talent.Name))
                dic.Add(talent.Name, talent);
        }

        public void Remove(Talent talent)
        {
            if (!string.IsNullOrEmpty(talent.Name) && dic.ContainsKey(talent.Name))
                dic.Remove(talent.Name);
        }

        public void WriteToExcel(ExcelWorksheet sheet)
        {
            ExcelHelper.WriteTalentInfo(sheet, dic.Values);
        }

        public void WriteToExcel(ExcelWorksheet sheet, ScreenCondition screenCondition)
        {
            ICollection<Talent> talents = new List<Talent>();
            foreach (var item in dic.Values)
            {
                if (item.ResearchInterestsKeywords == null) continue;
                List<string> keywords = StringHelper.Split(item.ResearchInterestsKeywords, new string[] { "、" });
                bool valid = false;
                foreach (var keyword in keywords)
                {
                    if (screenCondition.ResearchInterestsKeywords.Count == 0 || screenCondition.ResearchInterestsKeywords.Contains(keyword))
                    {
                        valid = true;
                        break;
                    }
                }

                if (valid)
                    talents.Add(item);
            }

            ExcelHelper.WriteTalentInfo(sheet, talents);
        }

        public List<string> GetResearchInterestsKeywords()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
            {
                if (item.ResearchInterestsKeywords != null)
                    result.AddRange(StringHelper.Split(item.ResearchInterestsKeywords, new string[] { "、" }));
            }
            return result;
        }

        public Talent GetNextTalent()
        {
            var item = dic.First();
            dic.Remove(item.Key);
            return item.Value;
        }
    }

    public class TalentTypeDictionary
    {
        private Dictionary<string, TalentNameDictionary> dic = null;

        public TalentTypeDictionary()
        {
            dic = new Dictionary<string, TalentNameDictionary>();
        }

        public int Count => dic.Count;

        public void Add(Talent talent)
        {
            TalentNameDictionary tmpDic = null;
            if (dic.ContainsKey(talent.TalentType))
            {
                tmpDic = dic[talent.TalentType];
            }
            else
            {
                tmpDic = new TalentNameDictionary();
                dic.Add(talent.TalentType, tmpDic);
            }

            tmpDic.Add(talent);
        }

        public void Remove(Talent talent)
        {
            if (dic.ContainsKey(talent.TalentType))
            {
                dic[talent.TalentType].Remove(talent);
                if (dic[talent.TalentType].Count == 0)
                    dic.Remove(talent.TalentType);
            }
        }

        public void WriteToExcel(ExcelWorksheet sheet)
        {
            foreach (var item in dic.Values)
            {
                int startRow = sheet.Dimension.End.Row + 1;
                item.WriteToExcel(sheet);
                int endRow = sheet.Dimension.End.Row;
                if (endRow < startRow) continue;
                sheet.Cells[startRow, 4, endRow, 4].Merge = true;
            }
        }

        public void WriteToExcel(ExcelWorksheet sheet, ScreenCondition screenCondition)
        {
            foreach (var item in dic)
            {
                int startRow = sheet.Dimension.End.Row + 1;
                if (screenCondition.TalentTypes.Count > 0 && !screenCondition.TalentTypes.Contains(item.Key))
                    continue;
                item.Value.WriteToExcel(sheet, screenCondition);
                int endRow = sheet.Dimension.End.Row;
                if (endRow < startRow) continue;
                sheet.Cells[startRow, 4, endRow, 4].Merge = true;
                sheet.Cells[startRow, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                sheet.Cells[startRow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }
        }

        public List<string> GetTalentTypes()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Keys)
                result.Add(item);
            return result;
        }

        public List<string> GetResearchInterestsKeywords()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetResearchInterestsKeywords());
            return result;
        }

        public Talent GetNextTalent()
        {
            Talent talent = null;
            while(dic.Count > 0)
            {
                var item = dic.ElementAt(0);
                if (item.Value.Count == 0)
                {
                    dic.Remove(item.Key);
                }
                else
                {
                    talent = dic.ElementAt(0).Value.GetNextTalent();
                    if (talent == null) continue;
                    break;
                }
            }

            return talent;
        }
    }

    public class InstituteDictionary
    {
        private Dictionary<string, TalentTypeDictionary> dic = null;

        public InstituteDictionary()
        {
            dic = new Dictionary<string, TalentTypeDictionary>();
        }

        public int Count => dic.Count;

        public void Add(Talent talent)
        {
            TalentTypeDictionary tmpDic = null;
            if (dic.ContainsKey(talent.Institute))
            {
                tmpDic = dic[talent.Institute];
            }
            else
            {
                tmpDic = new TalentTypeDictionary();
                dic.Add(talent.Institute, tmpDic);
            }

            tmpDic.Add(talent);
        }

        public void Remove(Talent talent)
        {
            if (dic.ContainsKey(talent.Institute))
            {
                dic[talent.Institute].Remove(talent);
                if (dic[talent.Institute].Count == 0)
                    dic.Remove(talent.Institute);
            }
        }

        public void WriteToExcel(ExcelWorksheet sheet)
        {
            foreach (var item in dic.Values)
                item.WriteToExcel(sheet);
        }

        public void WriteToExcel(ExcelWorksheet sheet, ScreenCondition screenCondition)
        {
            foreach (var item in dic)
            {
                if (screenCondition.Institutes.Count > 0 && !screenCondition.Institutes.Contains(item.Key))
                    continue;
                item.Value.WriteToExcel(sheet, screenCondition);
            }
        }

        public List<string> GetInstitutes()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Keys)
                result.Add(item);
            return result;
        }

        public List<string> GetTalentTypes()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetTalentTypes());
            return result;
        }

        public List<string> GetResearchInterestsKeywords()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetResearchInterestsKeywords());
            return result;
        }

        public Talent GetNextTalent()
        {
            Talent talent = null;
            while (dic.Count > 0)
            {
                var item = dic.ElementAt(0);
                if (item.Value.Count == 0)
                {
                    dic.Remove(item.Key);
                }
                else
                {
                    talent = dic.ElementAt(0).Value.GetNextTalent();
                    if (talent == null) continue;
                    break;
                }
            }

            return talent;
        }
    }

    public class SchoolDictionary
    {
        private Dictionary<string, InstituteDictionary> dic = null;

        public SchoolDictionary()
        {
            dic = new Dictionary<string, InstituteDictionary>();
        }

        public int Count => dic.Count;

        public void Add(Talent talent)
        {
            InstituteDictionary tmpDic = null;
            if (dic.ContainsKey(talent.School))
            {
                tmpDic = dic[talent.School];
            }
            else
            {
                tmpDic = new InstituteDictionary();
                dic.Add(talent.School, tmpDic);
            }

            tmpDic.Add(talent);
        }

        public void Remove(Talent talent)
        {
            if (dic.ContainsKey(talent.School))
            {
                dic[talent.School].Remove(talent);
                if (dic[talent.School].Count == 0)
                    dic.Remove(talent.School);
            }
        }

        public void WriteToExcel(ExcelWorksheet sheet)
        {
            foreach (var item in dic.Values)
            {
                int startRow = sheet.Dimension.End.Row + 1;
                item.WriteToExcel(sheet);
                int endRow = sheet.Dimension.End.Row;
                if (endRow < startRow) continue;
                sheet.Cells[startRow, 2, endRow, 2].Merge = true;
            }
        }

        public void WriteToExcel(ExcelWorksheet sheet, ScreenCondition screenCondition)
        {
            foreach (var item in dic)
            {
                int startRow = sheet.Dimension.End.Row + 1;
                if (screenCondition.Schools.Count > 0 && !screenCondition.Schools.Contains(item.Key))
                    continue;
                item.Value.WriteToExcel(sheet, screenCondition);
                int endRow = sheet.Dimension.End.Row;
                if (endRow < startRow) continue;
                sheet.Cells[startRow, 2, endRow, 2].Merge = true;
                sheet.Cells[startRow, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                sheet.Cells[startRow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }
        }

        public List<string> GetSchools()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Keys)
                result.Add(item);
            return result;
        }

        public List<string> GetInstitutes()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetInstitutes());
            return result;
        }

        public List<string> GetTalentTypes()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetTalentTypes());
            return result;
        }

        public List<string> GetResearchInterestsKeywords()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetResearchInterestsKeywords());
            return result;
        }

        public Talent GetNextTalent()
        {
            Talent talent = null;
            while (dic.Count > 0)
            {
                var item = dic.ElementAt(0);
                if (item.Value.Count == 0)
                {
                    dic.Remove(item.Key);
                }
                else
                {
                    talent = dic.ElementAt(0).Value.GetNextTalent();
                    if (talent == null) continue;
                    break;
                }
            }

            return talent;
        }
    }

    public class CompetentDepartmentDictionary
    {
        private Dictionary<string, SchoolDictionary> dic = null;

        public CompetentDepartmentDictionary()
        {
            dic = new Dictionary<string, SchoolDictionary>();
        }

        public int Count => dic.Count;

        public void Add(Talent talent)
        {
            SchoolDictionary tmpDic = null;
            if (dic.ContainsKey(talent.CompetentDepartment))
            {
                tmpDic = dic[talent.CompetentDepartment];
            }
            else
            {
                tmpDic = new SchoolDictionary();
                dic.Add(talent.CompetentDepartment, tmpDic);
            }

            tmpDic.Add(talent);
        }

        public void Remove(Talent talent)
        {
            if (dic.ContainsKey(talent.CompetentDepartment))
            {
                dic[talent.CompetentDepartment].Remove(talent);
                if (dic[talent.CompetentDepartment].Count == 0)
                    dic.Remove(talent.CompetentDepartment);
            }
        }

        public void WriteToExcel(ExcelWorksheet sheet)
        {
            foreach (var item in dic.Values)
            {
                int startRow = sheet.Dimension.End.Row + 1;
                item.WriteToExcel(sheet);
                int endRow = sheet.Dimension.End.Row;
                if (endRow < startRow) continue;
                sheet.Cells[startRow, 1, endRow, 1].Merge = true;
                sheet.Cells[startRow, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                sheet.Cells[startRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }
        }

        public void WriteToExcel(ExcelWorksheet sheet, ScreenCondition screenCondition)
        {
            foreach (var item in dic)
            {
                int startRow = sheet.Dimension.End.Row + 1;
                if (screenCondition.CompetentDepartments.Count > 0 && !screenCondition.CompetentDepartments.Contains(item.Key))
                    continue;
                item.Value.WriteToExcel(sheet, screenCondition);
                int endRow = sheet.Dimension.End.Row;
                if (endRow < startRow) continue;
                sheet.Cells[startRow, 1, endRow, 1].Merge = true;
                sheet.Cells[startRow, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                sheet.Cells[startRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }
        }

        public List<string> GetCompetentDepartments()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Keys)
                result.Add(item);
            return result;
        }

        public List<string> GetSchools()
        {
            HashSet<string> set = new HashSet<string>();
            foreach (var value in dic.Values)
            {
                foreach (var item in value.GetSchools())
                    set.Add(item);
            }
            return set.ToList();
        }

        public List<string> GetInstitutes()
        {
            HashSet<string> set = new HashSet<string>();
            foreach (var value in dic.Values)
            {
                foreach (var item in value.GetInstitutes())
                    set.Add(item);
            }
            return set.ToList();
        }

        public List<string> GetTalentTypes()
        {
            HashSet<string> set = new HashSet<string>();
            foreach (var value in dic.Values)
            {
                foreach (var item in value.GetTalentTypes())
                    set.Add(item);
            }
            return set.ToList();
        }

        public List<string> GetResearchInterestsKeywords()
        {
            HashSet<string> set = new HashSet<string>();
            foreach (var value in dic.Values)
            {
                foreach (var item in value.GetResearchInterestsKeywords())
                    set.Add(item);
            }
            return set.ToList();
        }

        public Talent GetNextTalent()
        {
            Talent talent = null;
            while (dic.Count > 0)
            {
                var item = dic.ElementAt(0);
                if (item.Value.Count == 0)
                {
                    dic.Remove(item.Key);
                }
                else
                {
                    talent = dic.ElementAt(0).Value.GetNextTalent();
                    if (talent == null) continue;
                    break;
                }
            }

            return talent;
        }
    }

    public class TalentDictionary
    {
        private CompetentDepartmentDictionary dic = null;

        public TalentDictionary()
        {
            dic = new CompetentDepartmentDictionary();
        }

        public void Add(Talent talent)
        {
            dic.Add(talent);
        }

        public void Remove(Talent talent)
        {
            dic.Remove(talent);
        }

        public void WriteToExcel(ExcelWorksheet sheet)
        {
            ExcelHelper.WriteTalentTitle(sheet);
            dic.WriteToExcel(sheet);
        }

        public void WriteToExcel(ExcelWorksheet sheet, ScreenCondition screenCondition)
        {
            ExcelHelper.WriteTalentTitle(sheet);
            dic.WriteToExcel(sheet, screenCondition);
        }

        public void Create(List<Talent> talents)
        {
            foreach (var talent in talents)
                dic.Add(talent);
        }

        public List<string> GetCompetentDepartments()
        {
            return dic.GetCompetentDepartments();
        }

        public List<string> GetSchools()
        {
            return dic.GetSchools();
        }

        public List<string> GetInstitutes()
        {
            return dic.GetInstitutes();
        }

        public List<string> GetTalentTypes()
        {
            return dic.GetTalentTypes();
        }

        public List<string> GetResearchInterestsKeywords()
        {
            return dic.GetResearchInterestsKeywords();
        }

        public Talent GetNextTalent()
        {
            if (dic.Count == 0) return null;
            return dic.GetNextTalent();
        }
    }
}