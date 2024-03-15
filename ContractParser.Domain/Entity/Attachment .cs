using System.ComponentModel.DataAnnotations;

namespace ContractParser.Domain.Entity
{
    public class Attachment
    {
        [Key]
        public int id { get; set; }
        public int contractId { get; set; } 
        public string url { get; set; } 
        public string fileName { get; set; }

        public virtual Contract Contract { get; set; } 
    }
}
