using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blockchain
{
    public class BlockChain
    {
        private readonly int _proofOfWorkDifficulty;
        private readonly double _miningReward;

        public List<Transaction> PendingTransactions { get; private set; }

        public IList<Block> Chain { get; }

        public BlockChain(int proofOfWorkDifficulty, int miningReward)
        {
            _proofOfWorkDifficulty = proofOfWorkDifficulty;
            _miningReward = miningReward;
            PendingTransactions = new List<Transaction>();
            Chain = new List<Block> { CreateGenesisBlock() };
        }

        public void AddTransaction(Transaction transaction) => PendingTransactions.Add(transaction);

        public void AddBlock(Block block)
        {
            if (IsValid(currentBlock: block, previousBlock: Chain.Last()))
                return;

            Chain.Add(block);
            PendingTransactions = new List<Transaction>(PendingTransactions.Skip(block.Transactions.Count() - 1));
        }

        public double GetBalance(string address)
        {
            var balance = 0d;
            Parallel.ForEach(Chain.SelectMany(block => block.Transactions),
                transaction =>
                {
                    if (transaction.From == address)
                        balance -= transaction.Amount;
                    else if (transaction.To == address)
                        balance += transaction.Amount;
                });
            return balance;
        }

        public bool IsValidChain() =>
            Parallel.For(1, Chain.Count, (i, state) =>
            {
                if (IsValid(currentBlock: Chain[i], previousBlock: Chain[i - 1]))
                    state.Stop();
            }).IsCompleted;

        private static bool IsValid(Block currentBlock, Block previousBlock) =>
            currentBlock.Hash != HashGenerator.ComputeHash(BlockToRowData(currentBlock))
            || currentBlock.PreviousHash != previousBlock.Hash;

        private static string BlockToRowData(Block block) =>
            block.PreviousHash + block.Timestamp + block.Transactions.GetSequenceHashCode() + block.UniqueCode;

        private static Block CreateGenesisBlock()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var transactions = new List<Transaction>();
            var previousHash = "0";
            var uniqueCode = 0;

            var rawData = previousHash + timestamp + transactions.GetSequenceHashCode() + uniqueCode;

            return new Block(timestamp, transactions, previousHash,
                HashGenerator.ComputeHash(rawData), uniqueCode);
        }
    }
}