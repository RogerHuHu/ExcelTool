using ExcelTool.Models;
using HandyControl.Controls;
using HIIUtils.Commands;
using HIIUtils.HIIFile;
using HIIUtils.MVVM;
using HIIUtils.String;
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
        private bool showPhone = true;
        private bool showName = true;
        private bool showStatus = true;
        private bool showRemark = true;

        private ObservableCollection<string> schoolBase = null;
        private ObservableCollection<string> schools = null;
        private ObservableCollection<string> instituteBase = null;
        private ObservableCollection<string> institutes = null;
        private ObservableCollection<string> talentTypes = null;
        private ObservableCollection<string> statuses = null;
        private ObservableCollection<string> cads = null;
        private ObservableCollection<string> computerSoftwares = null;
        private ObservableCollection<string> computerApplications = null;
        private ObservableCollection<string> developEnvironments = null;
        private ObservableCollection<string> developTechnologies = null;
        private ObservableCollection<string> softwareDevelopments = null;

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

        public bool ShowPhone
        {
            get => showPhone;
            set
            {
                if (showPhone != value)
                {
                    showPhone = value;
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

        public bool ShowStatus
        {
            get => showStatus;
            set
            {
                if (showStatus != value)
                {
                    showStatus = value;
                    screenCondition.ShowTitles[6] = value;
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

        public ObservableCollection<string> SchoolBase
        {
            get
            {
                if (schoolBase == null)
                    schoolBase = new ObservableCollection<string>();
                return schoolBase;
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

        public ObservableCollection<string> InstituteBase
        {
            get
            {
                if (instituteBase == null)
                    instituteBase = new ObservableCollection<string>();
                return instituteBase;
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

        public ObservableCollection<string> Statuses
        {
            get
            {
                if (statuses == null)
                    statuses = new ObservableCollection<string>();
                return statuses;
            }
        }

        public ObservableCollection<string> CADs
        {
            get
            {
                if (cads == null)
                    cads = new ObservableCollection<string>();
                return cads;
            }
        }

        public ObservableCollection<string> ComputerSoftwares
        {
            get
            {
                if(computerSoftwares == null)
                    computerSoftwares = new ObservableCollection<string>();
                return computerSoftwares;
            }
        }

        public ObservableCollection<string> ComputerApplications
        {
            get
            {
                if (computerApplications == null)
                    computerApplications = new ObservableCollection<string>();
                return computerApplications;
            }
        }

        public ObservableCollection<string> DevelopEnvironments
        {
            get
            {
                if(developEnvironments == null)
                    developEnvironments= new ObservableCollection<string>();
                return developEnvironments;
            }
        }

        public ObservableCollection<string> DevelopTechnologies
        {
            get
            {
                if(developTechnologies == null)
                    developTechnologies= new ObservableCollection<string>();
                return developTechnologies;
            }
        }

        public ObservableCollection<string> SoftwareDevelopments
        {
            get
            {
                if(softwareDevelopments == null)
                    softwareDevelopments = new ObservableCollection<string>();
                return softwareDevelopments;
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
        public DelegateCommand<string> SchoolSearchCmd { get; private set; }
        public DelegateCommand<IList<object>> SchoolSelectionChanged { get; private set; }
        public DelegateCommand<string> InstituteSearchCmd { get; private set; }
        public DelegateCommand<IList<object>> InstituteSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> TalentTypeSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> StatusSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> CADSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> ComputerSoftwareSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> ComputerApplicationSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> DevelopEnvironmentSelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> DevelopTechnologySelectionChanged { get; private set; }
        public DelegateCommand<IList<object>> SoftwareDevelopmentSelectionChanged { get; private set; }
        public DelegateCommand ConfirmCmd { get; private set; }
        #endregion

        #region 方法
        private void Init()
        {   
            screenCondition = new ScreenCondition();
            SelectOldFileCmd = new DelegateCommand(() => SelectOldFile());
            SelectNewFileCmd = new DelegateCommand(() => SelectNewFile());
            SchoolSearchCmd = new DelegateCommand<string>(SearchSchool);
            SchoolSelectionChanged = new DelegateCommand<IList<object>>(OnSchoolSelectionChanged);
            InstituteSearchCmd = new DelegateCommand<string>(SearchInstitute);
            InstituteSelectionChanged = new DelegateCommand<IList<object>>(OnInstituteSelectionChanged);
            TalentTypeSelectionChanged = new DelegateCommand<IList<object>>(OnTalentTypeSelectionChanged);
            StatusSelectionChanged = new DelegateCommand<IList<object>>(OnStatusSelectionChanged);
            CADSelectionChanged = new DelegateCommand<IList<object>>(OnCADSelectionChanged);
            ComputerSoftwareSelectionChanged = new DelegateCommand<IList<object>>(OnComputerSoftwareSelectionChanged);
            ComputerApplicationSelectionChanged = new DelegateCommand<IList<object>>(OnComputerApplicationSelectionChanged);
            DevelopEnvironmentSelectionChanged = new DelegateCommand<IList<object>>(OnDevelopEnvironmentSelectionChanged);
            DevelopTechnologySelectionChanged = new DelegateCommand<IList<object>>(OnDevelopTechnologySelectionChanged);
            SoftwareDevelopmentSelectionChanged = new DelegateCommand<IList<object>>(OnSoftwareDevelopmentSelectionChanged);
            ConfirmCmd = new DelegateCommand(() => Confirm());
        }
        
        private void ReadExcelContent()
        {
            dic = new TalentDictionary();
            ExcelHelper.ReadTalentInfo(OldFilePath, dic);

            SchoolBase.Clear();
            Schools.Clear();
            foreach (var school in dic.GetSchools())
            {
                SchoolBase.Add(school);
                Schools.Add(school);
            }

            InstituteBase.Clear();
            Institutes.Clear();
            foreach (var institute in dic.GetInstitutes())
            {
                InstituteBase.Add(institute);
                Institutes.Add(institute);
            }

            TalentTypes.Clear();
            foreach (var talentType in dic.GetTalentTypes())
                TalentTypes.Add(talentType);

            Statuses.Clear();
            foreach (var status in dic.GetStatuses())
                Statuses.Add(status);

            CADs.Clear();
            foreach (var cad in dic.GetCADs())
                CADs.Add(cad);

            ComputerSoftwares.Clear();
            foreach (var software in dic.GetComputerSoftwares())
                ComputerSoftwares.Add(software);

            ComputerApplications.Clear();
            foreach (var app in dic.GetComputerApplications())
                ComputerApplications.Add(app);

            DevelopEnvironments.Clear();
            foreach (var env in dic.GetDevelopEnvironments())
                DevelopEnvironments.Add(env);

            DevelopTechnologies.Clear();
            foreach (var tech in dic.GetDevelopTechnologies())
                DevelopTechnologies.Add(tech);

            SoftwareDevelopments.Clear();
            foreach (var dev in dic.GetSoftwareDevelopments())
                SoftwareDevelopments.Add(dev);
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

        private void SearchSchool(string keyword)
        {
            List<string> keywords = StringHelper.Split(keyword, new string[] { ";" });
            Schools.Clear();
            foreach(string str in SchoolBase)
            {
                foreach(string key in keywords)
                {
                    if(str.Contains(key))
                        Schools.Add(str);
                }
            }
        }

        private void OnSchoolSelectionChanged(IList<object> schools)
        {
            screenCondition.Schools = schools;
        }

        private void SearchInstitute(string keyword)
        {
            List<string> keywords = StringHelper.Split(keyword, new string[] { ";" });
            Institutes.Clear();
            foreach (string str in InstituteBase)
            {
                foreach (string key in keywords)
                {
                    if (str.Contains(key))
                        Institutes.Add(str);
                }
            }
        }

        private void OnInstituteSelectionChanged(IList<object> institutes)
        {
            screenCondition.Institutes = institutes;
        }

        private void OnTalentTypeSelectionChanged(IList<object> talentTypes)
        {
            screenCondition.TalentTypes = talentTypes;
        }

        private void OnStatusSelectionChanged(IList<object> statuses)
        {
            screenCondition.Statuses = statuses;
        }

        private void OnCADSelectionChanged(IList<object> cads)
        {
            screenCondition.CADs = cads;
        }

        private void OnComputerSoftwareSelectionChanged(IList<object> computerSoftwares)
        {
            screenCondition.ComputerSoftwares = computerSoftwares;
        }

        private void OnComputerApplicationSelectionChanged(IList<object> computerApplications)
        {
            screenCondition.ComputerApplications = computerApplications;
        }

        private void OnDevelopEnvironmentSelectionChanged(IList<object> developEnvironments)
        {
            screenCondition.DevelopEnvironments = developEnvironments;
        }

        private void OnDevelopTechnologySelectionChanged(IList<object> developTechnologies)
        {
            screenCondition.DevelopTechnologies = developTechnologies;
        }

        private void OnSoftwareDevelopmentSelectionChanged(IList<object> softwareDevelopments)
        {
            screenCondition.SoftwareDevelopments = softwareDevelopments;
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
