using Microsoft.AspNetCore.Mvc;
using PDFReaderAPI.Services;
using PDFReaderAPI.ValueObjects;

namespace PDFReaderAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PdfsController : ControllerBase
{
    private readonly IPdfsService _pdfsService;

    public PdfsController(IPdfsService pdfsService)
    {
        _pdfsService = pdfsService;
    }

    [HttpGet("read-pdfs")]
    public async Task<ActionResult<PdfTextDataVO>> ReadPdfs()
    {
        try
        {
            var pdfList = await _pdfsService.ReadAllPdfs();
            return Ok(pdfList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }        
    }

    [HttpPost("upload-pdf")]
    public async Task<IActionResult> UploadPdf ([FromForm] IFormFile file)
    {
        try
        {
            await _pdfsService.UploadPdf(file);
            return Ok(new { message = "Arquivo carregado com sucesso!" });
        }
        catch (Exception ex)
        {           
            return StatusCode(500, ex.Message);
        }
    }   
}
