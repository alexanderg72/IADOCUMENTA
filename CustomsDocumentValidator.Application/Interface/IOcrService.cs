using System.IO;
using System.Threading.Tasks;
using CustomsDocumentValidator.Domain.Models;

namespace CustomsDocumentValidator.Application.Interfaces;

public interface IOcrService
{
    Task<DucaData> ExtractDucaAsync(Stream pdfStream);
    Task<FacturaData> ExtractFacturaAsync(Stream pdfStream);
}