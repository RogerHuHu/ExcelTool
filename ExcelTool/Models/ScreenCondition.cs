using System.Collections.Generic;

namespace ExcelTool.Models
{
    public class ScreenCondition
    {
        private IList<object> competentDepartments = null;
        private IList<object> schools = null;
        private IList<object> institutes = null;
        private IList<object> talentTypes = null;
        private IList<object> researchInterestsKeywords = null;

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

        public IList<object> ResearchInterestsKeywords
        {
            get => researchInterestsKeywords ?? (researchInterestsKeywords = new List<object>());
            set => researchInterestsKeywords = value;
        }
    }
}