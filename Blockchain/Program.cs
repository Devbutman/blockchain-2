using System;
using System.Collections.Generic;
using System.Linq;

namespace Blockchain
{
    internal class Program
    {
        private static void Main()
        {
            var Blockchain = new BlockChain(proofOfWorkDifficulty: 5, miningReward: 10);
            Console.WriteLine($"Is Blockchain valid: {Blockchain.IsValidChain()}\n");

            var user1Address = "A";
            var user2Address = "B";

            Console.WriteLine("Add transaction..");
            Blockchain.AddTransaction(new Transaction(user1Address, user2Address, 200));
            Console.WriteLine("Add transaction..");
            Blockchain.AddTransaction(new Transaction(user2Address, user1Address, 10));


            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var transactions = Blockchain.PendingTransactions;
            var previousHash = Blockchain.Chain.Last().Hash;
            var uniqueCode = 0;

            var rawData = previousHash + timestamp + transactions.GetSequenceHashCode() + uniqueCode;

            var block1 = new Block(timestamp, transactions, previousHash, HashGenerator.ComputeHash(rawData), uniqueCode);
            Console.WriteLine("\tAdd block..");

            Blockchain.AddBlock(block1);

            Console.WriteLine("Add transaction..");
            Blockchain.AddTransaction(new Transaction(user1Address, user2Address, 5));

            timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            transactions = Blockchain.PendingTransactions;
            previousHash = Blockchain.Chain.Last().Hash;
            uniqueCode = 0;

            rawData = previousHash + timestamp + transactions.GetSequenceHashCode() + uniqueCode;

            var block2 = new Block(timestamp, transactions, previousHash, HashGenerator.ComputeHash(rawData), uniqueCode);

            Blockchain.AddBlock(block2);
            Console.WriteLine("\tAdd block..");

            Console.WriteLine($"\nIs Blockchain valid: {Blockchain.IsValidChain()}\n");

            PrintChain(Blockchain);

            var hackerAddress = "hacker";
            Console.WriteLine("Hacking the Blockchain...");

            Blockchain.Chain[1].Transactions = new List<Transaction> {
                new Transaction(user1Address, hackerAddress, 150)};

            Console.WriteLine($"Is Blockchain valid: {Blockchain.IsValidChain()}\n");
            Console.ReadKey();
        }


        private static void PrintChain(BlockChain Blockchain)
        {
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Print Blockchain\n");
            for (var i = 0; i < Blockchain.Chain.Count; i++)
            {
                var block = Blockchain.Chain[i];
                Console.WriteLine($"Block - index {i}");
                Console.WriteLine($"Hash: {block.Hash}");
                Console.WriteLine($"Previous Hash: {block.PreviousHash}\n");

                Console.WriteLine("\tTransactions");
                foreach (var transaction in block.Transactions)
                {
                    Console.WriteLine($"\tFrom: {transaction.From} To {transaction.To} Amount {transaction.Amount}");
                }
            }

            Console.WriteLine("----------------------------------------------\n");
        }

    }
}
