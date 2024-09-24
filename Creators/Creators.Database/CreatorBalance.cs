using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Creators.Creators.Database
{
    public class CreatorBalance
    {
        [Key]
        public string Id_Donates { get; set; }

        public string Id_Creator { get; set; }

        [Column(TypeName = "decimal(9, 2)")]
        public decimal Balance { get; set; }

        public DateTime LastCashout {  get; set; }
        public DateTime LastDeposit { get; set; }

        public CreatorPage CreatorPage { get; set; }
    }
}
