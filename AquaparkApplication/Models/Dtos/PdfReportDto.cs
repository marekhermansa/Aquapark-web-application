namespace AquaparkApplication.Models.Dtos
{
    public class PdfReportDto
    {
        public string Status { get; set; }
        public bool Success { get; set; }
        public byte[] PdfData { get; set; }
    }
}
