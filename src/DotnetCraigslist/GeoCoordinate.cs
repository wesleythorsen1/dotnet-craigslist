namespace DotnetCraigslist
{
    readonly public struct GeoCoordinate
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public int? Accuracy { get; init; }
    }
}