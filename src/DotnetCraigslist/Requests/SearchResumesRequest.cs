using System.Collections.Generic;

namespace DotnetCraigslist
{
    public class SearchResumesRequest : SearchRequest
    {
        private const string QP_AVAILABLEMORNING = "resumes_available_morning";
        private const string QP_AVAILABLEAFTERNOON = "resumes_available_afternoon";
        private const string QP_AVAILABLEEVENING = "resumes_available_evening";
        private const string QP_AVAILABLEOVERNIGHT = "resumes_available_overnight";
        private const string QP_AVAILABLEWEEKDAYS = "resumes_available_weekdays";
        private const string QP_AVAILABLEWEEKENDS = "resumes_available_weekends";
        private const string QP_EDUCATIONLEVEL = "education_level_completed";

        public static class Categories
        {
            public const string All = "rrr";
        }

        public SearchResumesRequest(string site) : base(site, SearchResumesRequest.Categories.All) {}
        public SearchResumesRequest(string site, string? area) : base(site, area, SearchResumesRequest.Categories.All) {}
        internal SearchResumesRequest(string site, string? area, IDictionary<string, object> parameterStore) : base(site, area, SearchResumesRequest.Categories.All, parameterStore) {}
        
        public enum EducationCompleted
        {
            LessThanHighSchool = 1,
            HighSchoolGED = 2,
            SomeCollege = 3,
            Associates = 4,
            Bachelors = 5,
            Masters = 6,
            Doctoral = 7,
        }

        public bool AvailableMorning
        {
            get => GetParameter<bool>(QP_AVAILABLEMORNING);
            set => SetParameter(QP_AVAILABLEMORNING, value);
        }

        public bool AvailableAfternoon
        {
            get => GetParameter<bool>(QP_AVAILABLEAFTERNOON);
            set => SetParameter(QP_AVAILABLEAFTERNOON, value);
        }

        public bool AvailableEvening
        {
            get => GetParameter<bool>(QP_AVAILABLEEVENING);
            set => SetParameter(QP_AVAILABLEEVENING, value);
        }

        public bool AvailableOvernight
        {
            get => GetParameter<bool>(QP_AVAILABLEOVERNIGHT);
            set => SetParameter(QP_AVAILABLEOVERNIGHT, value);
        }

        public bool AvailableWeekdays
        {
            get => GetParameter<bool>(QP_AVAILABLEWEEKDAYS);
            set => SetParameter(QP_AVAILABLEWEEKDAYS, value);
        }

        public bool AvailableWeekends
        {
            get => GetParameter<bool>(QP_AVAILABLEWEEKENDS);
            set => SetParameter(QP_AVAILABLEWEEKENDS, value);
        }

        public IEnumerable<EducationCompleted>? EducationLevels
        {
            get => GetParameter<IEnumerable<EducationCompleted>>(QP_EDUCATIONLEVEL);
            set => SetParameter(QP_EDUCATIONLEVEL, value);
        }
    }
}