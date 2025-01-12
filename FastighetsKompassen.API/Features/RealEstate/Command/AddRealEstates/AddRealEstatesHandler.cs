using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;

namespace FastighetsKompassen.API.Features.RealEstate.Command.AddRealEstates
{
    public class AddRealEstatesHandler : IRequestHandler<AddRealEstatesCommand, Result>
    {

        private readonly AppDbContext _context;

        public AddRealEstatesHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(AddRealEstatesCommand request, CancellationToken cancellationToken)
        {
            if (request.RealEstateData == null)
                return Result.Failure("Data saknas, eller är tom");

            await _context.RealEstateData.AddRangeAsync(request.RealEstateData,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
