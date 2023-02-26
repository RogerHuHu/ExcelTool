using ExcelTool.Models;
using HandyControl.Controls;
using HIIUtils.Commands;
using HIIUtils.HIIFile;
using HIIUtils.MVVM;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace ExcelTool.ViewModels
{
    public class DeduplicationByNameViewModel : ViewModelBase
    {
        #region 变量

        private string oldFilePath;
        private string newFilePath;
        private List<Talent> duplicateNames;
        private bool isDeduplicationBySchool;
        private TalentDictionary dic = null;
        private TalentDictionary dic1 = null;

        private Dictionary<string, List<Talent>> talentsDic; 

        #endregion 变量

        #region 构造函数

        public DeduplicationByNameViewModel()
        {
            duplicateNames = new List<Talent>();
            isDeduplicationBySchool = false;
            talentsDic = new Dictionary<string, List<Talent>>();
            Init();
        }

        #endregion 构造函数

        #region 属性

        public string OldFilePath
        {
            get => oldFilePath;
            set => SetProperty(ref oldFilePath, value);
        }

        public string NewFilePath
        {
            get => newFilePath;
            set => SetProperty(ref newFilePath, value);
        }

        public List<Talent> DuplicateNames
        {
            get => duplicateNames;
            set => SetProperty(ref duplicateNames, value);
        }

        public bool IsDeduplicationBySchool
        {
            get => isDeduplicationBySchool;
            set => SetProperty(ref isDeduplicationBySchool, value);
        }

        #endregion 属性

        #region 命令

        public DelegateCommand SelectOldFileCmd { get; private set; }
        public DelegateCommand SelectNewFileCmd { get; private set; }
        public DelegateCommand AnalyzeCmd { get; private set; }
        public DelegateCommand DeduplicateCmd { get; private set; }

        #endregion 命令

        #region 方法

        private void Init()
        {
            SelectOldFileCmd = new DelegateCommand(() => SelectOldFile());
            SelectNewFileCmd = new DelegateCommand(() => SelectNewFile());
            AnalyzeCmd = new DelegateCommand(() => Analyze());
            DeduplicateCmd = new DelegateCommand(() => Deduplicate());
        }

        private void SelectOldFile()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Excel文件 (*.xlsx)|*.xlsx;*.xls",
                RestoreDirectory = true,
                CheckFileExists = true,
                Multiselect = false,
                Title = "选择 Excel 文件"
            };

            if (dialog.ShowDialog() == true)
            {
                OldFilePath = dialog.FileName;
                NewFilePath = Path.Combine(DirectoryHelper.GetDirectoryName(OldFilePath), "新文档.xlsx");
                ReadExcelContent();
            }
        }

        private void SelectNewFile()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Excel文件 (*.xlsx)|*.xlsx;*.xls",
                RestoreDirectory = true,
                CheckFileExists = false,
                Multiselect = false,
                Title = "选择 Excel 文件"
            };

            if (dialog.ShowDialog() == true)
            {
                NewFilePath = dialog.FileName;
            }
        }

        /// <summary>
        /// 找出姓名相同的人才信息
        /// </summary>
        private void Analyze()
        {
            Talent talent = dic1.GetNextTalent();
            while (talent != null)
            {
                if (!talentsDic.ContainsKey(talent.Name))
                {
                    talent.IsSelected = true;
                    List<Talent> talents = new List<Talent>() { talent };
                    talentsDic.Add(talent.Name, talents);
                }
                else
                {
                    if(!IsDeduplicationBySchool)
                        talentsDic[talent.Name].Add(talent);
                    else
                    {
                        if(talent.School != talentsDic[talent.Name][0].School)
                            talentsDic[talent.Name].Add(talent);
                    }
                }

                dic1.Remove(talent);
                talent = dic1.GetNextTalent();
            }

            // 获得存在姓名重复的人才信息
            List<Talent> tmp = new List<Talent>();
            foreach (var item in talentsDic.Values)
            {
                if (item.Count <= 1) continue;

                tmp.AddRange(item);
            }

            DuplicateNames = tmp;
        }

        /// <summary>
        /// 去除重复的人
        /// </summary>
        private void Deduplicate()
        {
            foreach(var talent in DuplicateNames)
            {
                if (talent.IsSelected) continue;
                dic.Remove(talent);
            }

            ExcelPackage package = ExcelHelper.NewExcelPackage(NewFilePath);
            ExcelWorksheet sheet = ExcelHelper.NewWorksheet(package, "Sheet1");
            dic.WriteToExcel(sheet);
            package.Save();
            package.Dispose();
            MessageBox.Success("去重完成");
        }

        private void ReadExcelContent()
        {
            dic = new TalentDictionary();
            ExcelHelper.ReadTalentInfo(OldFilePath, dic);

            dic1 = new TalentDictionary();
            ExcelHelper.ReadTalentInfo(OldFilePath, dic1);
        }

        #endregion 方法
    }
}