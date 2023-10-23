using Newtonsoft.Json;
using PDFReaderAPI.ValueObjects;

namespace PDFReaderAPI.Services;

public class PdfsService : IPdfsService
{    
    private const string _PDFS_DIRECTORY = "Pdfs";
    private readonly IHttpClientFactory _httpClientFactory;

    public PdfsService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;       
    }

    public async Task UploadPdf(IFormFile file)
    {
        if (file.ContentType != "application/pdf")
        {
            throw new Exception("Tipo de arquivo não suportado");
        }

        string fileNameWithoutUnderline = file.FileName.Split(".")[0].Replace("_", "-");

        string filename = $"{fileNameWithoutUnderline}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var pdfsDirectoryPath = Path.Combine(GetRootPath(), _PDFS_DIRECTORY);

        if (!Directory.Exists(pdfsDirectoryPath))
        {
            Directory.CreateDirectory(pdfsDirectoryPath);
        }        

        var filePath = Path.Combine(pdfsDirectoryPath, filename);

        using var fileStream = new FileStream(filePath, FileMode.Create);

        await file.OpenReadStream().CopyToAsync(fileStream);
    }

    public async Task<List<PdfTextDataVO>> ReadAllPdfs()
    {
        var pdfList = new List<PdfTextDataVO>();

        var pdfsDirectoryPath = Path.Combine(GetRootPath(), _PDFS_DIRECTORY);

        var files = Directory.GetFiles(pdfsDirectoryPath);

        if (!Directory.Exists(pdfsDirectoryPath) || files.Length == 0)
        {
            throw new Exception("Nenhum arquivo encontrado");
        }

        foreach (var file in files)
        {
            var data = await ReadPdf(file);
            pdfList.Add(data);
        }

        return pdfList;
    }

    public async Task<PdfTextDataVO> ReadPdf(string path)
    {
        using var httpClient = _httpClientFactory.CreateClient("OCRApi");

        try
        {
            MultipartFormDataContent form = new()
            {
                { new StringContent("por"), "language" },
                { new StringContent("PDF"), "filetype" },
                { new StringContent(GetBase64DataFromPdf(path)), "base64image" },
                { new StringContent("true"), "isOverlayRequired" },
                { new StringContent("false"), "IsCreateSearchablePDF" },
                { new StringContent("true"), "isSearchablePdfHideTextLayer" },
                { new StringContent("true"), "detectOrientation" },
                { new StringContent("false"), "isTable" },
                { new StringContent("true"), "scale" },
                { new StringContent("1"), "OCREngine" },
                { new StringContent("false"), "detectCheckbox" },
                { new StringContent("0"), "checkboxTemplate" },
            };

            var response = await httpClient.PostAsync("/parse/image", form);

            var dataString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<dynamic>(dataString);

            var lines = result?["ParsedResults"][0]["TextOverlay"]["Lines"];

            string operationKind = string.Empty;
            string invoiceTotalValue = string.Empty;
            var additionalData = new List<string>();

            for (int i = 0; i < lines?.Count; i++)
            {
                if (lines[i]["LineText"].Value.ToLower().Contains("natureza da operação"))
                {
                    operationKind = lines[i + 1]["LineText"];
                }
                else if (lines[i]["LineText"].Value.ToLower().Contains("valor total da nota"))
                {
                    invoiceTotalValue = lines[i + 1]["LineText"];
                }
                else if (lines[i]["LineText"].Value.ToLower().StartsWith("-"))
                {
                    additionalData.Add(lines[i]["LineText"].ToString());
                }
            }

            return new PdfTextDataVO(operationKind, invoiceTotalValue, additionalData, GetFilenameFromPath(path));
        }
        catch (Exception)
        {
            throw;
        }
    }

    private string GetBase64DataFromPdf(string path)
    {
        if (!File.Exists(path))
        {
            throw new Exception("Arquivo não encontrado");
        }
        
        var fileBytes = File.ReadAllBytes(path);
        var base64Data = Convert.ToBase64String(fileBytes);
        return $"data:application/pdf;base64,{base64Data}";
    }

    private string GetFilenameFromPath(string path)
    {
        var filename = Path.GetFileName(path);
        var separatorIndex = filename.IndexOf('_');

        if (separatorIndex == -1)
        {
            return filename;
        }

        var filenameTreated = filename[..separatorIndex].Replace("-", "_");

        return filenameTreated;
    }

    private string GetRootPath() => Path.GetFullPath(".\\");
}
