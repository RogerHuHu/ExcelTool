﻿using ExcelTool.Models;
using HandyControl.Controls;
using HIIUtils.Commands;
using HIIUtils.HIIFile;
using HIIUtils.MVVM;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Media;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelTool.ViewModels
{
    public class SelectionItem : ViewModelBase
    {
        private string name;
        private bool isSelected;

        public SelectionItem(string name)
        {
            this.name = name;
            isSelected = false;
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }
    }

    public class ExcelScreeningViewModel : ViewModelBase
    {
        #region 变量
        private string oldFilePath;
        private string newFilePath;
        TalentDictionary dic = null;

        private bool showtTalentType = true;
        private bool showPosition = true;
        private bool showName = true;
        private bool showResearchInterestsKeywords = true;
        private bool showResearchInterests = true;
        private bool showRemark = true;
        private bool showAcademyOfScience = true;
        private bool showProfessionalTitle = true;
        private bool showProject = true;
        private bool showGroup = true;

        private ObservableCollection<string> competentDepartments = null;
        private ObservableCollection<string> schools = null;
        private ObservableCollection<string> institutes = null;
        private ObservableCollection<string> talentTypes = null;
        private ObservableCollection<string> positions = null;
        private ObservableCollection<string> researchInterestsKeywords = null;
        private ObservableCollection<string> projects = null;

        private string institutesKeywords;
        private ScreenCondition screenCondition;
        #endregion

        #region 构造函数
        public ExcelScreeningViewModel()
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

        public bool ShowTalentType
        {
            get => showtTalentType;
            set
            {
                if (showtTalentType != value)
                {
                    showtTalentType = value;
                    screenCondition.ShowTitles[3] = value;
                }
            }
        }

        public bool ShowPosition
        {
            get => showPosition;
            set
            {
                if (showPosition != value)
                {
                    showPosition = value;
                    screenCondition.ShowTitles[4] = value;
                }
            }
        }

        public bool ShowName
        {
            get => showName;
            set
            {
                if (showName != value)
                {
                    showName = value;
                    screenCondition.ShowTitles[5] = value;
                }
            }
        }

        public bool ShowResearchInterestsKeywords
        {
            get => showResearchInterestsKeywords;
            set
            {
                if (showResearchInterestsKeywords != value)
                {
                    showResearchInterestsKeywords = value;
                    screenCondition.ShowTitles[6] = value;
                }
            }
        }

        public bool ShowResearchInterests
        {
            get => showResearchInterests;
            set
            {
                if (showResearchInterests != value)
                {
                    showResearchInterests = value;
                    screenCondition.ShowTitles[7] = value;
                }
            }
        }

        public bool ShowRemark
        {
            get => showRemark;
            set
            {
                if (showRemark != value)
                {
                    showRemark = value;
                    screenCondition.ShowTitles[8] = value;
                }
            }
        }

        public bool ShowAcademyOfScience
        {
            get => showAcademyOfScience;
            set
            {
                if (showAcademyOfScience != value)
                {
                    showAcademyOfScience = value;
                    screenCondition.ShowTitles[9] = value;
                }
            }
        }

        public bool ShowProfessionalTitle
        {
            get => showProfessionalTitle;
            set
            {
                if (showProfessionalTitle != value)
                {
                    showProfessionalTitle = value;
                    screenCondition.ShowTitles[10] = value;
                }
            }
        }

        public bool ShowProject
        {
            get => showProject;
            set
            {
                if (showProject != value)
                {
                    showProject = value;
                    screenCondition.ShowTitles[11] = value;
                }
            }
        }

        public bool ShowGroup
        {
            get => showGroup;
            set
            {
                if (showGroup != value)
                {
                    showGroup = value;
                    screenCondition.ShowTitles[12] = value;
                }
            }
        }

        public ObservableCollection<string> CompetentDepartments
        {
            get
            {
                if(competentDepartments == null)
                    competentDepartments = new ObservableCollection<string>();
                return competentDepartments;
            }
        }

        public ObservableCollection<string> Schools
        {
            get
            {
                if (schools == null)
                    schools = new ObservableCollection<string>();
                return schools;
            }
        }

        public ObservableCollection<string> Institutes
        {
            get
            {
                if (institutes == null)
                    institutes = new ObservableCollection<string>();
                return institutes;
            }
        }

        public ObservableCollection<string> TalentTypes
        {
            get
            {
                if (talentTypes == null)
                    talentTypes = new ObservableCollection<string>();
                return talentTypes;
            }
        }

        public ObservableCollection<string> Positions
        {
            get
            {
                if (positions == null)
                    positions = new ObservableCollection<string>();
                return positions;
            }
        }

        public ObservableCollection<string> ResearchInterestsKeywords
        {
            get
            {
                if (researchInterestsKeywords == null)
                    researchInterestsKeywords = new ObservableCollection<string>();
                return researchInterestsKeywords;
            }
        }

        public ObservableCollection<string> Projects
        {
            get
            {
                if(projects == null)
                    projects = new ObservableCollection<string>();
                return projects;
            }
        }

        public string InstitutesKeywords
        {
            get => institutesKeywords;
            set
            {
                if(institutesKeywords != value)
                {
                    institutesKeywords = value;
                    RaisePropertyChanged();
                    UpdateInstitutesKeywords();
                }
            }
        }
        #endregion

        #region 命令
        public DelegateCommand SelectOldFileCmd { get; private set; }
        public DelegateCommand SelectNewFileCmd { get; private set; }
        public DelegateCommand<IList<object>> CompetentDepartmentSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> SchoolSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> InstitutSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> TalentTypeSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> PositionSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> ResearchInterestsKeywordsSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> ProjectSelectionChanged { get; private set; }
        public DelegateCommand ConfirmCmd { get; private set; }
        #endregion

        #region 方法
        private void Init()
        {   
            screenCondition = new ScreenCondition();
            SelectOldFileCmd = new DelegateCommand(() => SelectOldFile());
            SelectNewFileCmd = new DelegateCommand(() => SelectNewFile());
            CompetentDepartmentSelectionChanged = new DelegateCommand<IList<object>>(OnCompetentDepartmentSelectionChanged);
            SchoolSelectionChanged = new DelegateCommand<IList<object>>(OnSchoolSelectionChanged);
            InstitutSelectionChanged = new DelegateCommand<IList<object>>(OnInstituteSelectionChanged);
            TalentTypeSelectionChanged = new DelegateCommand<IList<object>>(OnTalentTypeSelectionChanged);
            PositionSelectionChanged = new DelegateCommand<IList<object>>(OnPositionSelectionChanged);
            ResearchInterestsKeywordsSelectionChanged = new DelegateCommand<IList<object>>(OnResearchInterestsKeywordSelectionChanged);
            ProjectSelectionChanged = new DelegateCommand<IList<object>>(OnProjectSelectionChanged);
            ConfirmCmd = new DelegateCommand(() => Confirm());
        }
        
        private void ReadExcelContent()
        {
            dic = new TalentDictionary();
            ExcelHelper.ReadTalentInfo(OldFilePath, dic);

            CompetentDepartments.Clear();
            foreach (var cd in dic.GetCompetentDepartments())
                CompetentDepartments.Add(cd);

            Schools.Clear();
            foreach (var school in dic.GetSchools())
                Schools.Add(school);

            Institutes.Clear();
            foreach (var institute in dic.GetInstitutes())
                Institutes.Add(institute);

            TalentTypes.Clear();
            foreach (var talentType in dic.GetTalentTypes())
                TalentTypes.Add(talentType);

            Positions.Clear();
            foreach(var position in dic.GetPositions())
                Positions.Add(position);

            ResearchInterestsKeywords.Clear();
            foreach (var researchInterestKeyword in dic.GetResearchInterestsKeywords())
                ResearchInterestsKeywords.Add(researchInterestKeyword);

            Projects.Clear();
            foreach(var project in dic.GetProjects())
                Projects.Add(project);
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

        private void OnCompetentDepartmentSelectionChanged(IList<object> competentDepartments)
        {
            screenCondition.CompetentDepartments = competentDepartments;
        }

        private void OnSchoolSelectionChanged(IList<object> schools)
        {
            screenCondition.Schools = schools;
        }

        private void OnInstituteSelectionChanged(IList<object> institutes)
        {
            screenCondition.Institutes = institutes;
        }

        private void OnTalentTypeSelectionChanged(IList<object> talentTypes)
        {
            screenCondition.TalentTypes = talentTypes;
        }

        private void OnPositionSelectionChanged(IList<object> positions)
        {
            screenCondition.Positions = positions;
        }

        private void OnResearchInterestsKeywordSelectionChanged(IList<object> researchInterestsKeywords)
        {
            screenCondition.ResearchInterestsKeywords = researchInterestsKeywords;
        }

        private void OnProjectSelectionChanged(IList<object> projects)
        {
            screenCondition.Projects = projects;
        }

        private void UpdateInstitutesKeywords()
        {
            string[] keywords = InstitutesKeywords.Split(';');
            screenCondition.Institutes = keywords;
        }

        private void Confirm()
        {
            if (dic == null) return;

            ExcelPackage package = ExcelHelper.NewExcelPackage(NewFilePath);
            ExcelWorksheet sheet = ExcelHelper.NewWorksheet(package, "Sheet1");
            dic.WriteToExcel(sheet, screenCondition);
            package.Save();
            package.Dispose();
            MessageBox.Success("筛选完成");
        }
        #endregion
    }
}
