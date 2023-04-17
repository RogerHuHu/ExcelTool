using HIIUtils.String;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

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
            ICollection<Talent> talents = new List<Talent>();
            foreach (var item in dic.Values)
            {
                if (item.TalentType == null) continue;
                List<string> talentTypes = StringHelper.Split(item.TalentType, new string[] { "、" });
                bool valid = false;
                foreach (var talentType in talentTypes)
                {
                    if (screenCondition.TalentTypes.Count == 0 || screenCondition.TalentTypes.Contains(talentType))
                    {
                        valid = true;
                        break;
                    }
                }

                if (valid)
                {
                    if (screenCondition.Statuses.Count > 0 && screenCondition.Statuses.Contains(item.Status)) continue;

                    if((screenCondition.CADs.Count == 0 && screenCondition.ComputerSoftwares.Count == 0
                        && screenCondition.ComputerApplications.Count == 0 && screenCondition.DevelopEnvironments.Count == 0
                        && screenCondition.DevelopTechnologies.Count == 0 && screenCondition.SoftwareDevelopments.Count == 0)
                       || Contain(screenCondition.CADs, item.CAD)
                       || Contain(screenCondition.ComputerSoftwares, item.ComputerSoftware)
                       || Contain(screenCondition.ComputerApplications, item.ComputerApplication)
                       || Contain(screenCondition.DevelopEnvironments, item.DevelopEnvironment)
                       || Contain(screenCondition.DevelopTechnologies, item.DevelopTechnology)
                       || Contain(screenCondition.SoftwareDevelopments, item.SoftwareDevelopment))
                        talents.Add(item);
                }
            }

            ExcelHelper.WriteTalentInfo(sheet, talents);
        }

        public List<string> GetTalentTypes()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
            {
                if(item.TalentType == null) continue;
                result.AddRange(StringHelper.Split(item.TalentType, new string[] { "、" }));
            }
            return result;
        }

        public List<string> GetStatuses()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.Add(item.Status);
            return result;
        }

        public List<string> GetCADs()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.Add(item.CAD);
            return result;
        }

        public List<string> GetComputerSoftwares()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.Add(item.ComputerSoftware);
            return result;
        }

        public List<string> GetComputerApplications()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.Add(item.ComputerApplication);
            return result;
        }

        public List<string> GetDevelopEnvironments()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.Add(item.DevelopEnvironment);
            return result;
        }

        public List<string> GetDevelopTechnologies()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.Add(item.DevelopTechnology);
            return result;
        }

        public List<string> GetSoftwareDevelopments()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.Add(item.SoftwareDevelopment);
            return result;
        }

        public Talent GetNextTalent()
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
            //bool write = false;
            foreach (var item in dic)
            {
                //if(screenCondition.Institutes.Count == 0)
                //{
                //    item.Value.WriteToExcel(sheet, screenCondition);
                //    write = true;
                //}
                //else
                //{
                //    foreach(string str in screenCondition.Institutes)
                //    {
                //        if(item.Key.Contains(str))
                //        {
                //            item.Value.WriteToExcel(sheet, screenCondition);
                //            write = true;
                //            break;
                //        }
                //    }
                //}

                if (screenCondition.Institutes.Count > 0 && !screenCondition.Institutes.Contains(item.Key))
                    continue;
                int startRow = sheet.Dimension.End.Row + 1;
                item.Value.WriteToExcel(sheet, screenCondition);
                int endRow = sheet.Dimension.End.Row;
                if (endRow < startRow) continue;
                sheet.Cells[startRow, 2, endRow, 2].Merge = true;
                sheet.Cells[startRow, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                sheet.Cells[startRow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
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

        public List<string> GetStatuses()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetStatuses());
            return result;
        }

        public List<string> GetCADs()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetCADs());
            return result;
        }

        public List<string> GetComputerSoftwares()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetComputerSoftwares());
            return result;
        }

        public List<string> GetComputerApplications()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetComputerApplications());
            return result;
        }

        public List<string> GetDevelopEnvironments()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetDevelopEnvironments());
            return result;
        }

        public List<string> GetDevelopTechnologies()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetDevelopTechnologies());
            return result;
        }

        public List<string> GetSoftwareDevelopments()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetSoftwareDevelopments());
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
                sheet.Cells[startRow, 1, endRow, 1].Merge = true;
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
            foreach(var item in dic.Values)
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

        public List<string> GetStatuses()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetStatuses());
            return result;
        }

        public List<string> GetCADs()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetCADs());
            return result;
        }

        public List<string> GetComputerSoftwares()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetComputerSoftwares());
            return result;
        }

        public List<string> GetComputerApplications()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetComputerApplications());
            return result;
        }

        public List<string> GetDevelopEnvironments()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetDevelopEnvironments());
            return result;
        }

        public List<string> GetDevelopTechnologies()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetDevelopTechnologies());
            return result;
        }

        public List<string> GetSoftwareDevelopments()
        {
            List<string> result = new List<string>();
            foreach (var item in dic.Values)
                result.AddRange(item.GetSoftwareDevelopments());
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
            ExcelHelper.WriteTalentTitle(sheet);
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

        public List<string> GetInstitutes()
        {
            return dic.GetInstitutes();
        }

        public List<string> GetTalentTypes()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetTalentTypes())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetStatuses()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetStatuses())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetCADs()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetCADs())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetComputerSoftwares()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetComputerSoftwares())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetComputerApplications()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetComputerApplications())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetDevelopEnvironments()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetDevelopEnvironments())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetDevelopTechnologies()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetDevelopTechnologies())
                result.Add(item);

            return result.ToList();
        }

        public List<string> GetSoftwareDevelopments()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string item in dic.GetSoftwareDevelopments())
                result.Add(item);

            return result.ToList();
        }

        public Talent GetNextTalent()
        {
            if (dic.Count == 0) return null;
            return dic.GetNextTalent();
        }
    }
}