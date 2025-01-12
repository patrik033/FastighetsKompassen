using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Features.RealEstate.Query.GetRealEstateById
{
    public class GetRealEstateByIdHandler : IRequestHandler<GetRealEstateByIdQuery, Result<GetRealEstateByIdDTO>>
    {

        private readonly AppDbContext _context;

        public GetRealEstateByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetRealEstateByIdDTO>> Handle(GetRealEstateByIdQuery request, CancellationToken cancellationToken)
        {

            var result = await _context.RealEstateData.FirstOrDefaultAsync(x => x.Id == request.RealEstateId);
            if (result == null)
            {
                return Result<GetRealEstateByIdDTO>.Failure("Fastighetsobjektet hittades inte.");
            }

            var resultFromEstate = new GetRealEstateByIdDTO
            {
                Street = result.Street,
                PropertyType = result.PropertyType,
                BuildYear = result.BuildYear,
                OwnerShipType = result.OwnershipType,
                HousingForm = result.HousingForm,
                LivingArea = result.LivingArea,
                LandArea = result.LandArea,
                County = result.County,
                Area = result.Area,
                Price = result.Price,
                WantedPrice = result.WantedPrice,
                Latitude = result.Latitude,
                Longitude = result.Longitude,
                Fee = result.Fee,
                OperatingCost = result.OperatingCost,
                Rooms = result.Rooms,
                Balcony = result.Balcony,
                Broker = result.Broker,
                SoldAt = result.SoldAt
            };

            return Result<GetRealEstateByIdDTO>.Success(resultFromEstate);
        }
    }
}
