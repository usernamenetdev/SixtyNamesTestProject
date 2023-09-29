using System;

namespace TestProject.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public int PersonID { get; set; }
        public decimal ContractAmount { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
