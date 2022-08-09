using System;

namespace DotnetCraigslist
{
    public class QueryStringValueAttribute : Attribute
    {
        public string Value { get; }

        public QueryStringValueAttribute(string value)
        {
            Value = value;
        }
    }
}