namespace PDFReaderAPI.ValueObjects;

public record PdfTextDataVO (string NaturezaDaOperacao, string ValorTotalDaNota, ICollection<string> DadosAdicionais, string filename);
