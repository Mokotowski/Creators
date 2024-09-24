namespace Creators.Creators.Models
{
    public class DonateCreatorForView
    {
        public string Donator { get; set; }
        public DateTime DateTime { get; set; }
        public int Count { get; set; }
        public string Currency { get; set; }

        public DonateCreatorForView(string donator, DateTime dateTime, int count, string currency)
        {
            Donator = donator;
            DateTime = dateTime;
            Count = count;
            Currency = currency;
        }
    }
}
