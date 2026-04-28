namespace CustomsDocumentValidator.Domain.Models;




//SE ASIGNAN LAS PROPIEDADES DE LA CLASE EXTRACTEDDOCUMENTDATA CON LOS CAMPOS EXTRAIDOS DEL DOCUMENTO ANALIZADO POR AZURE DOCUMENT INTELLIGENCE
//O SEA LAS VARIABLES QUE SE VAN A UTILIZAR
//PARA VALIDAR LOS DOCUMENTOS DE IMPORTACION EN EL PROCESO DE VALIDACION DE DOCUMENTOS DE IMPORTACION
public class ExtractedDocumentData
{
    public string NumeroFactura { get; set; }
    public string Proveedor { get; set; }
    public string Nit { get; set; } 
    public string Carrier { get; set; } 
    public decimal ValorTransaccion { get; set; }
    public decimal PesoBrutoTotal { get; set; }
    public decimal ValorAduana { get; set; }
    public decimal LiquidacionGeneral { get; set; }
    public int Bultos { get; set; }
    public decimal Totalsinseg { get; set; }

    public decimal Total { get; set; }
    public decimal PesoKg { get; set; }
}