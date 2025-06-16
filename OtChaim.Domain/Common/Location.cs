using System.Collections.Generic;

namespace OtChaim.Domain.Common
{
    public class Location : ValueObject
    {
        public double Latitude { get; }
        public double Longitude { get; }
        public string Description { get; }

        public Location(double latitude, double longitude, string description)
        {
            Latitude = latitude;
            Longitude = longitude;
            Description = description;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Latitude;
            yield return Longitude;
        }
    }
}