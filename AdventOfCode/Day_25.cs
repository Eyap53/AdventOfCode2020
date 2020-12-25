namespace AdventOfCode
{
    using AoCHelper;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day_25 : BaseDay
    {
        string[] _input;
        int _cardPublicKey;
        int _doorPublicKey;


        public Day_25()
        {
            ParseInput();
        }

        public override string Solve_1()
        {
            int secretNumber = 7;
            int iterationCount = 0;
            long value = 1;
            do
            {
                iterationCount++;
                value = EncryptLoop(secretNumber, value);
            } while (value != _cardPublicKey && value != _doorPublicKey);

            int publicKey = value == _cardPublicKey ? _doorPublicKey : _cardPublicKey;
            value = 1;
            for (int i = 0; i < iterationCount; i++)
            {
                value = EncryptLoop(publicKey, value);
            }
            return value.ToString();
        }

        public override string Solve_2() => throw new NotImplementedException();

        private void ParseInput()
        {
            _input = File.ReadAllLines(InputFilePath);
            _cardPublicKey = int.Parse(_input[0]);
            _doorPublicKey = int.Parse(_input[1]);
        }

        public long EncryptLoop(int secretNumber, long value)
        {
            value *= secretNumber;
            return value % 20201227;
        }
    }
}
