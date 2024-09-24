using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Creators.Creators.Database
{
    public class Donates
    {
        [Key]
        public int Id { get; set; }
        public string Id_Donates { get; set; }
        public string Donator {  get; set; }
        public DateTime DateTime {  get; set; }
        public int Count { get; set; }
        public string Currency { get; set; }
        public string PaymentId { get; set; }

        public CreatorPage CreatorPage { get; set; }   
    }
}
