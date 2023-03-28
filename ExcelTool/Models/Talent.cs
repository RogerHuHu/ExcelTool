namespace ExcelTool.Models
{
    public class Talent
    {
        #region 变量

        private bool isSelected;
        private string school; // 学校
        private string institute; // 学院
        private string talentType; // 人才类别
        private string name; // 姓名
        private string phone; // 电话
        private string status; // 状态
        private string cad; // 辅助设计
        private string computerSoftware; // 计算机软件
        private string computerApplication; // 计算机应用
        private string developmentEnvironment; // 开发环境
        private string developmentTechnology; // 开发技术
        private string softwareDevelopment; // 软件开发
        private string remark; // 备注

        #endregion 变量

        #region 构造函数

        public Talent()
        {
            isSelected = false;
        }

        public Talent(string school, string institute, string talentType,
                      string name, string phone, string status, string cad,
                      string computerSoftware, string computerApplication, string developmentEnvironment,
                      string developmentTechnology, string softwareDevelopment, string remark) : this()
        {
            this.school = school;
            this.institute = institute == null ? "未知" : institute;
            this.talentType = talentType;
            this.name = name;
            this.phone = phone;
            this.status = status;
            this.cad = cad;
            this.computerSoftware = computerSoftware;
            this.computerApplication = computerApplication;
            this.developmentEnvironment = developmentEnvironment;
            this.developmentTechnology = developmentTechnology;
            this.softwareDevelopment = softwareDevelopment;
            this.remark = remark;
        }

        #endregion 构造函数

        #region 属性

        public bool IsSelected
        {
            get => isSelected;
            set => isSelected = value;
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

        public string Phone
        {
            get => phone;
            set => phone = value;
        }

        public string Status
        {
            get => status;
            set => status = value;
        }

        public string CAD
        {
            get => cad;
            set => cad = value;
        }

        public string ComputerSoftware
        {
            get => computerSoftware;
            set => computerSoftware = value;
        }

        public string ComputerApplication
        {
            get => computerApplication;
            set => computerApplication = value;
        }

        public string DevelopmentEnvironment
        {
            get => developmentEnvironment;
            set => developmentEnvironment = value;
        }

        public string DevelopmentTechnology
        {
            get => developmentTechnology;
            set => developmentTechnology = value;
        }

        public string SoftwareDevelopment
        {
            get => softwareDevelopment;
            set => softwareDevelopment = value;
        }

        public string Remark
        {
            get => remark;
            set => remark = value;
        }

        public string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return School;
                    case 1: return Institute;
                    case 2: return TalentType;
                    case 3: return Name;
                    case 4: return Phone;
                    case 5: return Status;
                    case 6: return CAD;
                    case 7: return ComputerSoftware;
                    case 8: return ComputerApplication;
                    case 9: return DevelopmentEnvironment;
                    case 10: return DevelopmentTechnology;
                    case 11: return SoftwareDevelopment;
                    case 12: return Remark;
                    default: return "";
                }
            }
        }

        #endregion 属性
    }
}