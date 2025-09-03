namespace TariffCalculator.Api.Models
{
    public class UploadModel
    {
        public int UserId { get; set; }
        public IFormFile File { get; set; }
    }
}
