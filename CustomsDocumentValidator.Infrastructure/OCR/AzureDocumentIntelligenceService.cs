using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using CustomsDocumentValidator.Application.Interfaces;
using CustomsDocumentValidator.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace CustomsDocumentValidator.Infrastructure.OCR
{
    public class AzureDocumentIntelligenceService : IOcrService
    {
        private readonly DocumentAnalysisClient _ducaClient;
        private readonly DocumentAnalysisClient _facturaClient;

        public AzureDocumentIntelligenceService(IConfiguration configuration)
        {
            var ducaEndpoint = configuration["AzureDocumentIntelligence:DUCA:Endpoint"];
            var ducaKey = configuration["AzureDocumentIntelligence:DUCA:Key"];

            var facturaEndpoint = configuration["AzureDocumentIntelligence:FACTURA:Endpoint"];
            var facturaKey = configuration["AzureDocumentIntelligence:FACTURA:Key"];

            _ducaClient = new DocumentAnalysisClient(
                new Uri(ducaEndpoint),
                new AzureKeyCredential(ducaKey)
            );

            _facturaClient = new DocumentAnalysisClient(
                new Uri(facturaEndpoint),
                new AzureKeyCredential(facturaKey)
            );
        }

        // ===== DUCA =====
        public async Task<DucaData> ExtractDucaAsync(Stream pdfStream)
        {
            var result = (await _ducaClient.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "CONINTERCOM",
                pdfStream)).Value;

            var doc = result.Documents.FirstOrDefault();

            if (doc == null)
                return new DucaData();

            return new DucaData
            {
                NumeroFactura = Get(doc, "NumeroFactura"),
                Proveedor = Get(doc, "Proveedor"),
                Nit = Get(doc, "Nit"),
                ValorTransaccion = GetDecimal(doc, "ValorTransaccion"),
                PesoBrutoTotal = GetDecimal(doc, "PesoBrutoTotal"),
                ValorAduana = GetDecimal(doc, "ValorAduana"),
                LiquidacionGeneral = GetDecimal(doc, "LiquidacionGeneral"),
                Bultos = (int)GetDecimal(doc, "Bultos"),
                Intercom = Get(doc, "Intercom")
            };
        }

        // ===== FACTURA =====
        public async Task<FacturaData> ExtractFacturaAsync(Stream pdfStream)
        {
            var result = (await _facturaClient.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "factura6",
                pdfStream)).Value;

            var doc = result.Documents.FirstOrDefault();

            if (doc == null)
                return new FacturaData();

            return new FacturaData
            {
                NumeroFactura = Get(doc, "NumeroFactura"),
                Proveedor = Get(doc, "Proveedor"),
                Carrier = Get(doc, "Carrier"),
                Total = GetDecimal(doc, "Total"),
                PesoKg = GetDecimal(doc, "PesoKg"),
                Bultos = (int)GetDecimal(doc, "Bultos"),
                Totalsinseg = GetDecimal(doc, "Totalsinseg"), 
                Intercom = Get(doc, "Intercom"),
            };
        }

        private string Get(AnalyzedDocument doc, string field)
        {
            return doc.Fields.ContainsKey(field) ? doc.Fields[field].Content : null;
        }

        private decimal GetDecimal(AnalyzedDocument doc, string field)
        {
            if (!doc.Fields.ContainsKey(field))
                return 0;

            var value = doc.Fields[field].Content;

            if (string.IsNullOrEmpty(value))
                return 0;

            value = value.Replace(",", "").Replace(" ", "");

            decimal.TryParse(value, out decimal result);

            return result;
        }
    }
}