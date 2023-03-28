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
            if (!string.IsNullOrEmpty(talent.Name))
            {
                string key = talent.Name + talent.Phone;
                if (dic.ContainsKey(key))
                {
                    Talent tmp = dic[key];
                    string[] talentTypes = tmp.TalentType.Split("、".ToArray());
                    if (!talentTypes.Contains(talent.TalentType))
                        tmp.TalentType += "、" + talent.TalentType;
                    return;
                }
                dic.Add(key, talent);
            }
        }

        public void Remove(Talent talent)
        {
            if (!string.IsNullOrEmpty(talent.Name) && dic.ContainsKey(talent.Name + talent.Phone))
                dic.Remove(talent.Name + talent.Phone);
        }

        public void WriteToExcel(ExcelWorksheet sheet)
        {
            ExcelHelper.WriteTalentInfo(sheet, dic.Values);
        }

        public void WriteToExcel(ExcelWorksheet sheet, ScreenCondition screenCondition)
        {
            //ICollection<Talent> talents = new List<Talent>();
            //foreach (var item in dic.Values)
            //{
            //    if (item.ResearchInterestsKeywords == null) continue;
            //    List<string> keywords = StringHelper.Split(item.ResearchInterestsKeywords, new string[] { "、" });
            //    bool valid = false;
            //    foreach (var keyword in keywords)
            //    {
            //        if (screenCondition.ResearchInterestsKeywords.Count == 0 || screenCondition.ResearchInterestsKeywords.Contains(keyword))
            //        {
            //            valid = true;
            //            break;
            //        }
            //    }

            //    if (valid)
            //    {
            //        if (screenCondition.Projects.Count == 0 || screenCondition.Projects.Contains(item.Project))
            //            talents.Add(item);
            //    }
            //}

            //ExcelHelper.WriteTalentInfo(sheet, talents, screenCondition);
        }

        public List<string> GetResearchInterestsKeywords()
        {
            List<string> result = new List<string>();
            //foreach (var item in dic.Values)
            //{
            //    if (item.ResearchInterestsKeywords != null)
            //        result.AddRange(StringHelper.Split(item.ResearchInterestsKeywords, new string[] { "、" }));
            //}
            return result;
        }

        public List<string> GetProjects()
        {
            List<string> result = new List<string>();
            //foreach (var item in dic.Values)
            //{
            //    if(item.Project != null)
            //        result.Add(item.Project);
            //}
            return result;
        }

        public Talent GetNextTalent()
        {
            var item = dic.First();
            dic.Remove(item.Key);
            return item.Value;
        }
    }

    public class InstituteDictionary
    {
        private Dictionary<string, TalentNameDictionary> dic = null;

        public InstituteDictionary()
        {
            dic = new Dictionary<string, TalentNameDictionary>();
        }

        public int Count => dic.Count;

        public void Add(Talent talent)
        {
            TalentNameDictionary tmpDic = null;
            if (dic.ContainsKey(talent.Institute))
            {
                tmpDic = dic[talent.Institute];
            }
            else
            {
                tmpDic = new TalentNameDictionary();
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
                sheet.Cells[startRow, 1, endRow, 1].Merge = true;
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
        private SchoolDictionary dic = null;

        public TalentDictionary()
        {
            dic = new SchoolDictionary();
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
            ExcelHelper.WriteTalentTitle(sheet, screenCondition);
            dic.WriteToExcel(sheet, screenCondition);
        }

        public void Create(List<Talent> talents)
        {
            foreach (var talent in talents)
                dic.Add(talent);
        }

        public List<string> GetSchools()
        {
            return dic.GetSchools();
        }

        public Talent GetNextTalent()
        {
            if (dic.Count == 0) return null;
            return dic.GetNextTalent();
        }
    }
}