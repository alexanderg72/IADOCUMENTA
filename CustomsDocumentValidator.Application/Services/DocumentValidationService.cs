using System.IO;
using System.Threading.Tasks;
using CustomsDocumentValidator.Application.Interfaces;
using CustomsDocumentValidator.Domain.Models;

namespace CustomsDocumentValidator.Application.Services;

public class DocumentValidationService
{
    private readonly IOcrService _ocr;

    public DocumentValidationService(IOcrService ocr)
    {
        _ocr = ocr;
    }

    public async Task<ValidationResult> ValidateAsync(Stream ducaStream, Stream facturaStream)
    {
        var duca = await _ocr.ExtractDucaAsync(ducaStream);
        var factura = await _ocr.ExtractFacturaAsync(facturaStream);

        return new ValidationResult
        {
            Duca = duca,
            Factura = factura
        };
    }
}