using System.Collections.Generic;

namespace DotnetCraigslist
{
    public class SearchJobsRequest : SearchRequest
    {
        private const string QP_INTERNSHIP = "is_internship";
        private const string QP_NONPROFIT = "is_nonprofit";
        private const string QP_TELECOMMUTE = "is_telecommuting";
        private const string QP_EMPLOYMENTTYPE = "employment_type";

        public static class Categories
        {
            public const string All = "jjj";
            public const string AccountingFinance = "acc";
            public const string AdminOffice = "ofc";
            public const string ArchitectEngineerCad = "egr";
            public const string ArtMediaDesign = "med";
            public const string BusinessMgmt = "bus";
            public const string CustomerService = "csr";
            public const string EducationTeaching = "edu";
            public const string EtCetera = "etc";
            public const string FoodBeverageHospitality = "fbh";
            public const string GeneralLabor = "lab";
            public const string Government = "gov";
            public const string Healthcare = "hea";
            public const string HumanResource = "hum";
            public const string LegalParalegal = "lgl";
            public const string Manufacturing = "mnu";
            public const string MarketingAdvertisingPr = "mar";
            public const string Nonprofit = "npo";
            public const string RealEstate = "rej";
            public const string RetailWholesale = "ret";
            public const string Sales = "sls";
            public const string SalonSpaFitness = "spa";
            public const string ScienceBiotech = "sci";
            public const string Security = "sec";
            public const string SkilledTradesArtisan = "trd";
            public const string SoftwareQaDbaEtc = "sof";
            public const string SystemsNetworking = "sad";
            public const string TechnicalSupport = "tch";
            public const string Transportation = "trp";
            public const string TvFilmVideoRadio = "tfr";
            public const string WebHtmlInfoDesign = "web";
            public const string WritingEditing = "wri";
        }
        
        public SearchJobsRequest(string site) : base(site, SearchJobsRequest.Categories.All) {}
        public SearchJobsRequest(string site, string category) : base(site, category) {}
        public SearchJobsRequest(string site, string? area, string category) : base(site, area, category) {}
        internal SearchJobsRequest(string site, string? area, string category, IDictionary<string, object> parameterStore) : base(site, area, category, parameterStore) {}

        public enum EmploymentType
        {
            FullTime = 1,
            PartTime = 2,
            Contract = 3,
            EmployeesChoice = 4,
        }

        public bool Internship
        {
            get => GetParameter<bool>(QP_INTERNSHIP);
            set => SetParameter(QP_INTERNSHIP, value);
        }

        public bool Nonprofit
        {
            get => GetParameter<bool>(QP_NONPROFIT);
            set => SetParameter(QP_NONPROFIT, value);
        }

        public bool Telecommute
        {
            get => GetParameter<bool>(QP_TELECOMMUTE);
            set => SetParameter(QP_TELECOMMUTE, value);
        }

        public IEnumerable<EmploymentType>? EmploymentTypes
        {
            get => GetParameter<IEnumerable<EmploymentType>>(QP_EMPLOYMENTTYPE);
            set => SetParameter(QP_EMPLOYMENTTYPE, value);
        }
    }
}