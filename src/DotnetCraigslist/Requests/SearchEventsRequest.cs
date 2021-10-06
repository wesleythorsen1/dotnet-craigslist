using System;
using System.Collections.Generic;

namespace DotnetCraigslist
{
    public class SearchEventsRequest : SearchRequest
    {
        private const string QP_ONDATE = "sale_date";
        private const string QP_ARTFILM = "event_art";
        private const string QP_CAREER = "event_career";
        private const string QP_CHARITABLE = "event_fundraiser_vol";
        private const string QP_COMPETITION = "event_athletics";
        private const string QP_DANCE = "event_dance";
        private const string QP_FESTFAIR = "event_festival";
        private const string QP_FITNESSHEALTH = "event_fitness_wellness";
        private const string QP_FOODDRINK = "event_food";
        private const string QP_FREE = "event_free";
        private const string QP_KIDFRIENDLY = "event_kidfriendly";
        private const string QP_LITERARY = "event_literary";
        private const string QP_MUSIC = "event_music";
        private const string QP_OUTDOOR = "event_outdoor";
        private const string QP_SALE = "event_sale";
        private const string QP_SINGLES = "event_singles";
        private const string QP_TECH = "event_geek";

        public static class Categories
        {
            public const string All = "eee";
            public const string Classes = "cls";
            public const string Events = "eve";
        }
        
        public SearchEventsRequest(string site) : base(site, SearchEventsRequest.Categories.All) {}
        public SearchEventsRequest(string site, string category) : base(site, category) {}
        public SearchEventsRequest(string site, string? area, string category) : base(site, area, category) {}
        internal SearchEventsRequest(string site, string? area, string category, IDictionary<string, object> parameterStore) : base(site, area, category, parameterStore) {}

        public enum LostOrFound
        {
            Lost = 1,
            Found = 2,
        }

        public enum RideshareType
        {
            RideOffered = 1,
            RideWanted = 2,
        }

        public DateTime? OnDate
        {
            get => GetParameter<DateTime?>(QP_ONDATE);
            set => SetParameter(QP_ONDATE, value);
        }

        public bool ArtFilm
        {
            get => GetParameter<bool>(QP_ARTFILM);
            set => SetParameter(QP_ARTFILM, value);
        }

        public bool Career
        {
            get => GetParameter<bool>(QP_CAREER);
            set => SetParameter(QP_CAREER, value);
        }

        public bool Charitable
        {
            get => GetParameter<bool>(QP_CHARITABLE);
            set => SetParameter(QP_CHARITABLE, value);
        }

        public bool Competition
        {
            get => GetParameter<bool>(QP_COMPETITION);
            set => SetParameter(QP_COMPETITION, value);
        }

        public bool Dance
        {
            get => GetParameter<bool>(QP_DANCE);
            set => SetParameter(QP_DANCE, value);
        }

        public bool FestFair
        {
            get => GetParameter<bool>(QP_FESTFAIR);
            set => SetParameter(QP_FESTFAIR, value);
        }

        public bool FitnessHealth
        {
            get => GetParameter<bool>(QP_FITNESSHEALTH);
            set => SetParameter(QP_FITNESSHEALTH, value);
        }

        public bool FoodDrink
        {
            get => GetParameter<bool>(QP_FOODDRINK);
            set => SetParameter(QP_FOODDRINK, value);
        }

        public bool Free
        {
            get => GetParameter<bool>(QP_FREE);
            set => SetParameter(QP_FREE, value);
        }

        public bool KidFriendly
        {
            get => GetParameter<bool>(QP_KIDFRIENDLY);
            set => SetParameter(QP_KIDFRIENDLY, value);
        }

        public bool Literary
        {
            get => GetParameter<bool>(QP_LITERARY);
            set => SetParameter(QP_LITERARY, value);
        }

        public bool Music
        {
            get => GetParameter<bool>(QP_MUSIC);
            set => SetParameter(QP_MUSIC, value);
        }

        public bool Outdoor
        {
            get => GetParameter<bool>(QP_OUTDOOR);
            set => SetParameter(QP_OUTDOOR, value);
        }

        public bool Sale
        {
            get => GetParameter<bool>(QP_SALE);
            set => SetParameter(QP_SALE, value);
        }

        public bool Singles
        {
            get => GetParameter<bool>(QP_SINGLES);
            set => SetParameter(QP_SINGLES, value);
        }

        public bool Tech
        {
            get => GetParameter<bool>(QP_TECH);
            set => SetParameter(QP_TECH, value);
        }
    }
}