using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsDocumentValidator.Domain.Models;

public class DucaData
{
    public string NumeroFactura { get; set; }
    public string Proveedor { get; set; }
    public string Nit { get; set; }

    public decimal ValorTransaccion { get; set; }
    public decimal PesoBrutoTotal { get; set; }
    public decimal ValorAduana { get; set; }
    public decimal LiquidacionGeneral { get; set; }
    public int Bultos { get; set; }
}