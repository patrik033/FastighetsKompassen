using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastighetsKompassen.Shared.Models.RealEstate
{
    public class RealEstateData
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public string? PropertyType { get; set; }
        public int? BuildYear { get; set; }
        public string? OwnershipType { get; set; }
        public string? HousingForm { get; set; }
        public double? LivingArea { get; set; }
        public double? LandArea { get; set; }
        public string? County { get; set; }
        public string? Area { get; set; }
        public decimal? Price { get; set; }
        public decimal? WantedPrice { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Fee { get; set; }
        public int? OperatingCost { get; set; }
        public int? Rooms { get; set; }
        public bool? Balcony { get; set; }
        public string? Association { get; set; }
        public string? Broker { get; set; }
        public DateTime? SoldAt { get; set; }
        public string? Url { get; set; }
        public PriceChangeInfo? PriceChangeInfo { get; set; }
        public string? Story { get; set; }

        public int KommunDataId { get; set; }
        public KommunData? Kommun { get; set; }
    }
}
