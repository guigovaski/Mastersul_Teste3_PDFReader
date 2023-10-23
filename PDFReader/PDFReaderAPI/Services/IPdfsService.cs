using PDFReaderAPI.ValueObjects;

namespace PDFReaderAPI.Services;

public interface IPdfsService
{
    Task UploadPdf(IFormFile file);
    Task<List<PdfTextDataVO>> ReadAllPdfs();
    Task<PdfTextDataVO> ReadPdf(string path);
}
