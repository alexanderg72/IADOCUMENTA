using CustomsDocumentValidator.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomsDocumentValidator.API.Controllers;

[ApiController]
[Route("api/document")]
public class DocumentController : ControllerBase
{
    private readonly DocumentValidationService _service;

    public DocumentController(DocumentValidationService service)
    {
        _service = service;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate(IFormFile duca, IFormFile factura)
    {
        if (duca == null || factura == null)
            return BadRequest("Sube ambos archivos");

        using var ducaStream = duca.OpenReadStream();
        using var facturaStream = factura.OpenReadStream();

        var result = await _service.ValidateAsync(ducaStream, facturaStream);

        return Ok(result);
    }
}