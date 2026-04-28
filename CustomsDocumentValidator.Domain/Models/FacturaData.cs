using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsDocumentValidator.Domain.Models;

public class FacturaData
{
    public string NumeroFactura { get; set; }
    public string Proveedor { get; set; }
    public string Carrier { get; set; }
    public string Intercom { get; set; }
    public decimal Total { get; set; }
    public decimal Totalsinseg { get; set; }
    public decimal PesoKg { get; set; }
    public int Bultos { get; set; }
}