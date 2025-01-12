using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;

namespace FastighetsKompassen.API.Features.Upload.Command.UploadKommunFromFolder
{
    public record UploadKommunFromFolderCommand(string Letter) : IRequest<Result<List<FileProcessingResult>>>;

}
