using HIIUtils.String;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExcelTool.Models
{
    public class ExpertNameDictionary
    {
        private Dictionary<string, Expert> dic = null;

        public ExpertNameDictionary()
        {
            dic = new Dictionary<string, Expert>();
        }

        public int Count => dic.Count;

        public void Add(Expert expert)
        {
            if (!string.IsNullOrEmpty(expert.Name))
            {
                string key = expert.School + expert.Name;
                if (dic.ContainsKey(key))
                {
                    return;
                }
                dic.Add(key, expert);
            }
        }

        public void Remove(Expert expert)
        {
            if (!string.IsNullOrEmpty(expert.Name) && dic.ContainsKey(expert.School + expert.Name))
                dic.Remove(expert.School + expert.Name);
        }

        public void WriteToExcel(ExcelWorksheet sheet)
        {
            ExpertExcelHelper.WriteExpertInfo(sheet, dic.Values);
        }

        public List<string> GetTitles()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
            {
                result.Add(item.Title);
            }
            return result;
        }

        public List<string> GetDuties()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.Add(item.Duty);
            return result;
        }

        public List<string> GetResearches()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.Add(item.Research);
            return result;
        }

        public List<string> GetAwards()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.Add(item.Award);
            return result;
        }

        public List<string> GetRemarks()
        {
            List<string> result = new List<string>();
            foreach(var item in dic.Values)
                result.Add(item.Remark);
            return result;
        }

        public List<string> GetApplys()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.Add(item.Apply);
            return result;
        }

        public Expert GetNextExpert()
        {
            var item = dic.First();
            dic.Remove(item.Key);
            return item.Value;
        }

        private bool Contain(IList<object> list, object item)
        {
            return list.Count > 0 && list.Contains(item);
        }
    }

    public class ExpertInstituteDictionary
    {
        private Dictionary<string, ExpertNameDictionary> dic = null;

        public ExpertInstituteDictionary()
        {
            dic = new Dictionary<string, ExpertNameDictionary>();
        }

        public int Count => dic.Count;

        public void Add(Expert expert)
        {
            ExpertNameDictionary tmpDic = null;
            if (dic.ContainsKey(expert.Institute))
            {
                tmpDic = dic[expert.Institute];
            }
            else
            {
                tmpDic = new ExpertNameDictionary();
                dic.Add(expert.Institute, tmpDic);
            }

            tmpDic.Add(expert);
        }

        public void Remove(Expert expert)
        {
            if (dic.ContainsKey(expert.Institute))
            {
                dic[expert.Institute].Remove(expert);
                if (dic[expert.Institute].Count == 0)
                    dic.Remove(expert.Institute);
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
                sheet.Cells[startRow, 3, endRow, 3].Merge = true;
            }
        }

        public List<string> GetInstitutes()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Keys)
                result.Add(item);
            return result;
        }

        public List<string> GetTitles()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetTitles());
            return result;
        }

        public List<string> GetDuties()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetDuties());
            return result;
        }

        public List<string> GetResearches()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetResearches());
            return result;
        }

        public List<string> GetAwards()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetAwards());
            return result;
        }

        public List<string> GetRemarks()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetRemarks());
            return result;
        }

        public List<string> GetApplys()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetApplys());
            return result;
        }

        public Expert GetNextExpert()
        {
            Expert expert = null;
            while (dic.Count > 0)
            {
                var item = dic.ElementAt(0);
                if (item.Value.Count == 0)
                {
                    dic.Remove(item.Key);
                }
                else
                {
                    expert = dic.ElementAt(0).Value.GetNextExpert();
                    if (expert == null) continue;
                    break;
                }
            }

            return expert;
        }
    }

    public class ExpertSchoolDictionary
    {
        private Dictionary<string, ExpertInstituteDictionary> dic = null;

        public ExpertSchoolDictionary()
        {
            dic = new Dictionary<string, ExpertInstituteDictionary>();
        }

        public int Count => dic.Count;

        public void Add(Expert expert)
        {
            ExpertInstituteDictionary tmpDic = null;
            if (dic.ContainsKey(expert.School))
            {
                tmpDic = dic[expert.School];
            }
            else
            {
                tmpDic = new ExpertInstituteDictionary();
                dic.Add(expert.School, tmpDic);
            }

            tmpDic.Add(expert);
        }

        public void Remove(Expert expert)
        {
            if (dic.ContainsKey(expert.School))
            {
                dic[expert.School].Remove(expert);
                if (dic[expert.School].Count == 0)
                    dic.Remove(expert.School);
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

        public List<string> GetTitles()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetTitles());
            return result;
        }

        public List<string> GetDuties()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetDuties());
            return result;
        }

        public List<string> GetResearches()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetResearches());
            return result;
        }

        public List<string> GetAwards()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetAwards());
            return result;
        }

        public List<string> GetRemarks()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetRemarks());
            return result;
        }

        public List<string> GetApplys()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetApplys());
            return result;
        }

        public Expert GetNextExpert()
        {
            Expert expert = null;
            while (dic.Count > 0)
            {
                var item = dic.ElementAt(0);
                if (item.Value.Count == 0)
                {
                    dic.Remove(item.Key);
                }
                else
                {
                    expert = dic.ElementAt(0).Value.GetNextExpert();
                    if (expert == null) continue;
                    break;
                }
            }

            return expert;
        }
    }

    public class ExpertProvinceDictionary
    {
        private Dictionary<string, ExpertSchoolDictionary> dic = null;

        public ExpertProvinceDictionary()
        {
            dic = new Dictionary<string, ExpertSchoolDictionary>();
        }

        public int Count => dic.Count;

        public void Add(Expert expert)
        {
            ExpertSchoolDictionary tmpDic = null;
            if (dic.ContainsKey(expert.Province))
            {
                tmpDic = dic[expert.Province];
            }
            else
            {
                tmpDic = new ExpertSchoolDictionary();
                dic.Add(expert.Province, tmpDic);
            }

            tmpDic.Add(expert);
        }

        public void Remove(Expert expert)
        {
            if (dic.ContainsKey(expert.Province))
            {
                dic[expert.Province].Remove(expert);
                if (dic[expert.Province].Count == 0)
                    dic.Remove(expert.Province);
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

        public List<string> GetProvinces()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Keys)
                result.Add(item);
            return result;
        }

        public List<string> GetSchools()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetSchools());
            return result;
        }

        public List<string> GetInstitutes()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetInstitutes());
            return result;
        }

        public List<string> GetTitles()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetTitles());
            return result;
        }

        public List<string> GetDuties()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetDuties());
            return result;
        }

        public List<string> GetResearches()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetResearches());
            return result;
        }

        public List<string> GetAwards()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetAwards());
            return result;
        }

        public List<string> GetRemarks()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetRemarks());
            return result;
        }

        public List<string> GetApplys()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetApplys());
            return result;
        }

        public Expert GetNextExpert()
        {
            Expert expert = null;
            while (dic.Count > 0)
            {
                var item = dic.ElementAt(0);
                if (item.Value.Count == 0)
                {
                    dic.Remove(item.Key);
                }
                else
                {
                    expert = dic.ElementAt(0).Value.GetNextExpert();
                    if (expert == null) continue;
                    break;
                }
            }

            return expert;
        }
    }

    public class ExpertDictionary
    {
        private ExpertProvinceDictionary dic = null;

        public ExpertDictionary()
        {
            dic = new ExpertProvinceDictionary();
        }

        public void Add(Expert expert)
        {
            dic.Add(expert);
        }

        public void Remove(Expert expert)
        {
            dic.Remove(expert);
        }

        public void WriteToExcel(ExcelWorksheet sheet)
        {
            ExpertExcelHelper.WriteExpertTitle(sheet);
            dic.WriteToExcel(sheet);
        }

        public void Create(List<Expert> experts)
        {
            foreach (var expert in experts)
                dic.Add(expert);
        }

        public List<string> GetProvinces()
        {
            return dic.GetProvinces();
        }

        public List<string> GetSchools()
        {
            return dic.GetSchools();
        }

        public List<string> GetInstitutes()
        {
            return dic.GetInstitutes();
        }

        public List<string> GetTitles()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetTitles())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetDuties()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetDuties())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetResearches()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetResearches())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetAwards()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetAwards())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetRemarks()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetRemarks())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetApplys()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetApplys())
                result.Add(item);

            return result.ToList();
        }

        public Expert GetNextExpert()
        {
            if (dic.Count == 0) return null;
            return dic.GetNextExpert();
        }
    }
}
