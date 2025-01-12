using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Features.Muniplicity.Command.DeleteMuniplicityById
{
    public class DeleteMuniplicityByIdHandler : IRequestHandler<DeleteMuniplicityByIdQuery, Result>
    {
        private readonly AppDbContext _context;

        public DeleteMuniplicityByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteMuniplicityByIdQuery request, CancellationToken cancellationToken)
        {
            var kommun = await _context.Kommuner
                .FirstOrDefaultAsync(x => x.Id == request.KommunId);

            if (kommun == null)
                return Result.Failure("Kommunen finns ej.");

            _context.Kommuner.Remove(kommun);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
