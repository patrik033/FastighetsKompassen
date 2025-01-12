namespace FastighetsKompassen.API.Features.Upload.Command.UploadKommunFromFolder
{
    public class FileProcessingResult
    {
        public string FileName { get; }
        public string Status { get; }

        public FileProcessingResult(string fileName, string status)
        {
            FileName = fileName;
            Status = status;
        }
    }
}
