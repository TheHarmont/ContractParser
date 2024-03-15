using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ContractParser.Domain.Entity
{
    public class Contract
    {
        [Key]
        public int id { get; set; }
        public string number { get; set; }
        public DateTime signDate { get; set; }
        public string? shortName { get; set; }
        public string? singularName { get; set; }
        public string? mailingAddress { get; set; }
        public string? fullName { get; set; }
        public string? purchaseCode { get; set; }
        public string contractSubject { get; set; }
        public string? contactEMail { get; set; }
        public string? INN { get; set; }
        public string? KPP { get; set; }
        public string? regNum { get; set; }
        public virtual ICollection<Attachment>? Attachments { get; set; }
        public string? address { get; set; }
        public string? counterpartyName { get; set; }
        public string href { get; set; }
        public string sourceHash { get; set; }
        public long sourceSize { get; set; }
        public DateTime uploadDate { get; set; }

        public Contract()
        {
            Attachments = new List<Attachment>();
        }
    }
}
