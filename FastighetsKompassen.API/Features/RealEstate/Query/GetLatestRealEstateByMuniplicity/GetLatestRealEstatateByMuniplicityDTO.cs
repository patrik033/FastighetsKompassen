namespace FastighetsKompassen.API.Features.RealEstate.Query.GetLatestRealEstateByMuniplicity
{
    public class GetLatestRealEstatateByMuniplicityDTO
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public DateTime? SoldDate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
