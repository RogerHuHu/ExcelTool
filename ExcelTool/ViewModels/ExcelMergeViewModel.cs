using ExcelTool.Models;
using HandyControl.Controls;
using HandyControl.Data;
using HIIUtils.Commands;
using HIIUtils.HIIFile;
using HIIUtils.MVVM;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ExcelTool.ViewModels
{
    public class ExcelMergeViewModel : ViewModelBase
    {
        #region 变量

        private ObservableCollection<ExcelFile> excelFiles;
        private ExcelFile selectedExcelFile;
        private string newFilePath;

        #endregion 变量

        #region 构造函数

        public ExcelMergeViewModel()
        {
            Init();
        }

        #endregion 构造函数

        #region 属性

        public ObservableCollection<ExcelFile> ExcelFiles
        {
            get
            {
                if (excelFiles == null)
                    excelFiles = new ObservableCollection<ExcelFile>();
                return excelFiles;
            }
        }

        public ExcelFile SelectedExcelFile
        {
            get => selectedExcelFile;
            set => SetProperty(ref selectedExcelFile, value);
        }

        public string NewFilePath
        {
            get => newFilePath;
            set => SetProperty(ref newFilePath, value);
        }

        #endregion 属性

        #region 命令

        public DelegateCommand AddFileCmd { get; private set; }
        public DelegateCommand DeleteFileCmd { get; private set; }
        public DelegateCommand MergeFileCmd { get; private set; }
        public DelegateCommand SelectNewFilePathCmd { get; private set; }

        #endregion 命令

        #region 方法

        private void Init()
        {
            AddFileCmd = new DelegateCommand(() => SelectFile());
            DeleteFileCmd = new DelegateCommand(() => DeleteFile());
            MergeFileCmd = new DelegateCommand(() => MergeFile());
            SelectNewFilePathCmd = new DelegateCommand(() => SelectNewFilePath());
        }

        private void SelectFile()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Excel文件 (*.xlsx)|*.xlsx;*.xls",
                RestoreDirectory = true,
                CheckFileExists = true,
                Multiselect = true,
                Title = "选择 Excel 文件"
            };

            if (dialog.ShowDialog() == true)
            {
                ExcelFiles.Clear();
                for (int i = 0; i < dialog.SafeFileNames.Length; i++)
                    ExcelFiles.Add(new ExcelFile(dialog.SafeFileNames[i], dialog.FileNames[i]));
                NewFilePath = Path.Combine(DirectoryHelper.GetDirectoryName(ExcelFiles.LastOrDefault().FilePath), "新文档.xlsx");
            }
        }

        private void DeleteFile()
        {
            ExcelFiles.Remove(SelectedExcelFile);
        }

        private void MergeFile()
        {
            TalentDictionary dic = new TalentDictionary();
            foreach(var excelFile in ExcelFiles)
                ExcelHelper.ReadTalentInfo(excelFile.FilePath, dic);

            ExcelPackage package = ExcelHelper.NewExcelPackage(NewFilePath);
            ExcelWorksheet sheet = ExcelHelper.NewWorksheet(package, "Sheet1");
            dic.WriteToExcel(sheet);
            package.Save();
            package.Dispose();
            MessageBox.Success("合并完成");
        }

        private void SelectNewFilePath()
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

        #endregion 方法
    }
}