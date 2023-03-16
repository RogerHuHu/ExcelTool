namespace ExcelTool.Models
{
    public class Talent
    {
        #region 变量

        private bool isSelected;
        private string competentDepartment; // 主管部门
        private string school; // 学校
        private string institute; // 学院
        private string talentType; // 人才类别
        private string name; // 姓名
        private string position; // 职位
        private string researchInterestsKeywords; // 研究方向关键词
        private string researchInterests; // 详细研究方向
        private string remark; // 备注
        private string academyOfScience; // 科学院
        private string professionalTitle; // 职称
        private string project; // 国家重点研发项目
        private string group; // 国家重点研发项目分组

        #endregion 变量

        #region 构造函数

        public Talent()
        {
            isSelected = false;
        }

        public Talent(string competentDepartment, string school, string institute, string talentType, string position,
                      string name, string researchInterestsKeywords, string researchInterests, string remark,
                      string academyOfScience, string professionalTitle, string project, string group) : this()
        {
            this.competentDepartment = competentDepartment;
            this.school = school;
            this.institute = institute == null ? "未知" : institute;
            this.talentType = talentType;
            this.name = name;
            this.position = position;
            this.researchInterestsKeywords = researchInterestsKeywords;
            this.researchInterests = researchInterests;
            this.remark = remark;
            this.academyOfScience = academyOfScience;
            this.professionalTitle = professionalTitle;
            this.project = project;
            this.group = group;
        }

        #endregion 构造函数

        #region 属性

        public bool IsSelected
        {
            get => isSelected;
            set => isSelected = value;
        }

        public string CompetentDepartment
        {
            get => competentDepartment;
            set => competentDepartment = value;
        }

        public string School
        {
            get => school;
            set => school = value;
        }

        public string Institute
        {
            get => institute;
            set => institute = value;
        }

        public string TalentType
        {
            get => talentType;
            set => talentType = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Position
        {
            get => position;
            set => position = value;
        }

        public string ResearchInterestsKeywords
        {
            get => researchInterestsKeywords;
            set => researchInterestsKeywords = value;
        }

        public string ResearchInterests
        {
            get => researchInterests;
            set => researchInterests = value;
        }

        public string Remark
        {
            get => remark;
            set => remark = value;
        }

        public string AcademyOfScience
        {
            get => academyOfScience;
            set => academyOfScience = value;
        }

        public string ProfessionalTitle
        {
            get => professionalTitle;
            set => professionalTitle = value;
        }

        public string Project
        {
            get => project;
            set => project = value;
        }

        public string Group
        {
            get => group;
            set => group = value;
        }

        public string this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0: return CompetentDepartment;
                    case 1: return School;
                    case 2: return Institute;
                    case 3: return TalentType;
                    case 4: return Position;
                    case 5: return Name;
                    case 6: return ResearchInterestsKeywords;
                    case 7: return ResearchInterests;
                    case 8: return Remark;
                    case 9: return AcademyOfScience;
                    case 10: return ProfessionalTitle;
                    case 11: return Project;
                    case 12: return Group;
                    default: return "";
                }
            }
        }

        #endregion 属性
    }
}