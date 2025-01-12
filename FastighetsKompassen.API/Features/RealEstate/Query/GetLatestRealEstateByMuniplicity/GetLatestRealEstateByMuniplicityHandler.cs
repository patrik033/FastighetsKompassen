using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Features.RealEstate.Query.GetLatestRealEstateByMuniplicity
{
    public class GetLatestRealEstateByMuniplicityHandler : IRequestHandler<GetLatestRealEstateByMuniplicityQuery, Result<List<GetLatestRealEstatateByMuniplicityDTO>>>
    {

        private readonly AppDbContext _context;

        public GetLatestRealEstateByMuniplicityHandler(AppDbContext context)
        {
            _context = context;
        }

      
        public async Task<Result<List<GetLatestRealEstatateByMuniplicityDTO>>> Handle(GetLatestRealEstateByMuniplicityQuery request, CancellationToken cancellationToken)
        {
            var kommun = await _context.RealEstateData
             .FirstOrDefaultAsync(x => x.Kommun.Kommun == request.KommunId);

            if (kommun == null)
            {
                return Result<List<GetLatestRealEstatateByMuniplicityDTO>>.Failure("Kommun finns inte");
            }

            var realEstateData = await _context.RealEstateData
                .AsNoTracking()
                .Where(x => x.Kommun.Kommun == request.KommunId)
                .OrderByDescending(x => x.SoldAt)
                .Select(g => new GetLatestRealEstatateByMuniplicityDTO
                {
                    Id = g.Id,
                    Street = g.Street,
                    SoldDate = g.SoldAt,
                    Latitude = g.Latitude,
                    Longitude = g.Longitude
                })
                .Take(25)
                .ToListAsync(cancellationToken);

            return Result<List<GetLatestRealEstatateByMuniplicityDTO>>.Success(realEstateData);
        }
    }
}
