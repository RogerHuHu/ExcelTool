using System.Collections.Generic;

namespace ExcelTool.Models
{
    public class ScreenCondition
    {
        private bool[] showTitles = null;

        private IList<object> competentDepartments = null;
        private IList<object> schools = null;
        private IList<object> institutes = null;
        private IList<object> talentTypes = null;
        private IList<object> statuses = null;
        private IList<object> cads = null;
        private IList<object> computerSoftwares = null;
        private IList<object> computerApplications = null;
        private IList<object> developEnvironments = null;
        private IList<object> developTechnologies = null;
        private IList<object> softwareDevelopments = null;

        public bool[] ShowTitles
        {
            get
            {
                if (showTitles == null)
                {
                    showTitles = new bool[13];
                    for(int i = 0; i < showTitles.Length; i++)
                        showTitles[i] = true;
                }
                return showTitles;
            }

            private set { }
        }

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

        public IList<object> Statuses
        {
            get => statuses ?? (statuses = new List<object>());
            set => statuses = value;
        }
        
        public IList<object> CADs
        {
            get => cads ?? (cads = new List<object>());
            set => cads = value;
        }

        public IList<object> ComputerSoftwares
        {
            get => computerSoftwares ?? (computerSoftwares = new List<object>());
            set => computerSoftwares = value;
        }

        public IList<object> ComputerApplications
        {
            get => computerApplications ?? (computerApplications = new List<object>());
            set => computerApplications = value;
        }

        public IList<object> DevelopEnvironments
        {
            get => developEnvironments ?? (developEnvironments = new List<object>());
            set => developEnvironments = value;
        }

        public IList<object> DevelopTechnologies
        {
            get => developTechnologies ?? (developTechnologies = new List<object>());
            set => developTechnologies = value;
        }

        public IList<object> SoftwareDevelopments
        {
            get => softwareDevelopments ?? (softwareDevelopments = new List<object>());
            set => softwareDevelopments = value;
        }
    }
}