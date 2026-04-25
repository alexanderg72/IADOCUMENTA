using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsDocumentValidator.Domain.Models;

public class DocumentField
{
    public string Name { get; set; }
    public string Value { get; set; }
    public float Confidence { get; set; }
}