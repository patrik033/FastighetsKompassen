namespace FastighetsKompassen.API.Features.RealEstate.Query.GetRealEstateById
{
    public class GetRealEstateByIdDTO
    {
        public string? Street { get; set; }
        public string? PropertyType { get; set; }
        public int? BuildYear { get; set; }
        public string? OwnerShipType { get; set; }
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
        public string? Broker { get; set; }
        public DateTime? SoldAt { get; set; }
        
    }
}
