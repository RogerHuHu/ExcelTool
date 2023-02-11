using ExcelTool.Models;
using HandyControl.Controls;
using HIIUtils.Commands;
using HIIUtils.HIIFile;
using HIIUtils.MVVM;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelTool.ViewModels
{
    public class ExcelRemoveInvalidItemViewModel : ViewModelBase
    {
        #region 变量
        private string baseFilePath;
        private string oldFilePath;
        private string newFilePath;

        TalentDictionary dicBase = null;
        TalentDictionary dicOld = null;
        #endregion

        #region 构造函数
        public ExcelRemoveInvalidItemViewModel()
        {
            Init();
        }
        #endregion

        #region 属性
        public string BaseFilePath
        {
            get => baseFilePath;
            set => SetProperty(ref baseFilePath, value);
        }

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
        #endregion

        #region 命令
        public DelegateCommand SelectBaseFileCmd { get; private set; }
        public DelegateCommand SelectOldFileCmd { get; private set; }
        public DelegateCommand SelectNewFileCmd { get; private set; }
        public DelegateCommand ConfirmCmd { get; private set; }
        #endregion

        #region 方法
        private void Init()
        {
            SelectBaseFileCmd = new DelegateCommand(() => SelectBaseFile());
            SelectOldFileCmd = new DelegateCommand(() => SelectOldFile());
            SelectNewFileCmd = new DelegateCommand(() => SelectNewFile());
            ConfirmCmd = new DelegateCommand(() => Confirm());
        }

        private void SelectBaseFile()
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
                BaseFilePath = dialog.FileName;
                
            }
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

        private TalentDictionary ReadExcelContent(string path)
        {
            TalentDictionary dic = new TalentDictionary();
            ExcelHelper.ReadTalentInfo(path, dic);
            return dic;
        }

        private void Confirm()
        {
            dicBase = ReadExcelContent(BaseFilePath);
            dicOld = ReadExcelContent(OldFilePath);
            if (dicBase == null || dicOld == null) return;

            Talent talent = dicBase.GetNextTalent();
            while(talent != null)
            {
                dicOld.Remove(talent);
                talent = dicBase.GetNextTalent();
            }

            ExcelPackage package = ExcelHelper.NewExcelPackage(NewFilePath);
            ExcelWorksheet sheet = ExcelHelper.NewWorksheet(package, "Sheet1");
            dicOld.WriteToExcel(sheet);
            package.Save();
            package.Dispose();
            MessageBox.Success("删除完成");
        }
        #endregion
    }
}
