using System.Collections.Generic;

namespace ExcelTool.Models
{
    public class ScreenCondition
    {
        private IList<object> competentDepartments = null;
        private IList<object> schools = null;
        private IList<object> institutes = null;
        private IList<object> talentTypes = null;
        private IList<object> positions = null;
        private IList<object> researchInterestsKeywords = null;
        private IList<object> projects = null;

        public IList<object> CompetentDepartments
        {
            get => competentDepartments ?? (competentDepartments = new List<object>());
            set => competentDepartments = value;
        }

        public IList<object> Schools
        {
            get => schools ?? (schools = new List<object>());
            set => schools = value;
        }

        public IList<object> Institutes
        {
            get => institutes ?? (institutes = new List<object>());
            set => institutes = value;
        }

        public IList<object> TalentTypes
        {
            get => talentTypes ?? (talentTypes = new List<object>());
            set => talentTypes = value;
        }

        public IList<object> Positions
        {
            get => positions ?? (positions = new List<object>());
            set => positions = value;
        }

        public IList<object> ResearchInterestsKeywords
        {
            get => researchInterestsKeywords ?? (researchInterestsKeywords = new List<object>());
            set => researchInterestsKeywords = value;
        }

        public IList<object> Projects
        {
            get => projects ?? (projects = new List<object>());
            set => projects = value;
        }
    }
}