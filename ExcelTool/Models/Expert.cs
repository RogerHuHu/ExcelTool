using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelTool.Models
{
    public class Expert
    {
        #region 变量

        private bool isSelected;
        private string province; // 省份
        private string school; // 学校
        private string institute; // 学院
        private string name; // 姓名
        private string title; // 职称
        private string duty; // 职务
        private string research; // 研究方向
        private string award; // 奖项
        private string remark; // 备注
        private string apply; // 是否报奖

        #endregion 变量

        #region 构造函数

        public Expert()
        {
            isSelected = false;
        }

        public Expert(string province, string school, string institute, string name,
                      string title, string duty, string research, string award,
                      string remark, string apply) : this()
        {
            this.province = province;
            this.school = school;
            this.institute = institute == null ? "/" : institute;
            this.name = name;
            this.title = title == null ? "/": title;
            this.duty = duty == null ? "/" : duty;
            this.research = research;
            this.award = award == null ? "/" : award;
            this.remark = remark == null ? "/" : remark;
            this.apply = apply;
        }

        #endregion 构造函数

        #region 属性

        public bool IsSelected
        {
            get => isSelected;
            set => isSelected = value;
        }

        public string Province
        {
            get => province;
            set => province = value;
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

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Title
        {
            get => title;
            set => title = value;
        }

        public string Duty
        {
            get => duty;
            set => duty = value;       
        }

        public string Research
        {
            get => research;
            set => research = value;
        }

        public string Award
        {
            get => award;
            set => award = value;
        }

        public string Remark
        {
            get => remark;
            set => remark = value;
        }

        public string Apply
        {
            get => apply;set => apply = value;
        }

        public string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Province;
                    case 1: return School;
                    case 2: return Institute;
                    case 3: return Name;
                    case 4: return Title;
                    case 5: return Duty;
                    case 6: return Research;
                    case 7: return Award;
                    case 8: return Remark;
                    case 9: return Apply;
                    default: return "";
                }
            }
        }

        #endregion 属性
    }
}
