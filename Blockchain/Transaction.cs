namespace Blockchain
{
    public class Transaction
    {
        public string From { get; }
        public string To { get; set; }
        public double Amount { get; }
        public Transaction(string from, string to, double amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }
    }
}