using System.Collections.Generic;

namespace Blockchain
{
    public class Block
    {
        public long Timestamp { get; }
        public IList<Transaction> Transactions { get; set; }
        public string PreviousHash { get; }
        public string Hash { get; }
        public int UniqueCode { get; }

        public Block(long timestamp, IList<Transaction> transactions, string previousHash, string hash, int uniqueCode)
        {
            Timestamp = timestamp;
            Transactions = transactions;
            PreviousHash = previousHash;
            Hash = hash;
            UniqueCode = uniqueCode;
        }
    }
}