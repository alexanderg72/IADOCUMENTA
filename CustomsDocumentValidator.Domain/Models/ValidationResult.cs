using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CustomsDocumentValidator.Domain.Models;

public class ValidationResult
{
    public DucaData Duca { get; set; }
    public FacturaData Factura { get; set; }
}