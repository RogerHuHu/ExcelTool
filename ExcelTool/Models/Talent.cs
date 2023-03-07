﻿namespace ExcelTool.Models
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

        #endregion 变量

        #region 构造函数

        public Talent()
        {
            isSelected = false;
        }

        public Talent(string competentDepartment, string school, string institute, string talentType, string name,
                      string position, string researchInterestsKeywords, string researchInterests, string remark,
                      string academyOfScience) : this()
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

        #endregion 属性
    }
}