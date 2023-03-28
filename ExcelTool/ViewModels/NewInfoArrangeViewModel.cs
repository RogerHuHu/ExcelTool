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
    public class NewInfoArrangeViewModel : ViewModelBase
    {
        #region 变量
        private string oldFilePath;
        private string newFilePath;
        TalentDictionary dic = null;
        #endregion

        #region 构造函数
        public NewInfoArrangeViewModel()
        {
            Init();
        }
        #endregion

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
        #endregion

        #region 命令
        public DelegateCommand SelectOldFileCmd { get; private set; }
        public DelegateCommand SelectNewFileCmd { get; private set; }
        public DelegateCommand ConfirmCmd { get; private set; }
        #endregion

        #region 方法
        private void Init()
        {
            SelectOldFileCmd = new DelegateCommand(() => SelectOldFile());
            SelectNewFileCmd = new DelegateCommand(() => SelectNewFile());
            ConfirmCmd = new DelegateCommand(() => Confirm());
        }

        private void ReadExcelContent()
        {
            dic = new TalentDictionary();
            ExcelHelper.ReadTalentInfo(OldFilePath, dic);
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

        private void Confirm()
        {
            if (dic == null) return;

            ExcelPackage package = ExcelHelper.NewExcelPackage(NewFilePath);
            ExcelWorksheet sheet = ExcelHelper.NewWorksheet(package, "Sheet1");
            dic.WriteToExcel(sheet);
            package.Save();
            package.Dispose();
            MessageBox.Success("处理完成");
        }
        #endregion
    }
}
