namespace Creators.Creators.Models
{
    public class DonateUserForView
    {
        public string Creator { get; set; }
        public DateTime DateTime { get; set; }
        public int Count { get; set; }
        public string Currency { get; set; }

        public DonateUserForView(string creator, DateTime dateTime, int count, string currency)
        {
            Creator = creator;
            DateTime = dateTime;
            Count = count;
            Currency = currency;
        }
    }
}
