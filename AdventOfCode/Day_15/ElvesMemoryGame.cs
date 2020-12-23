namespace AdventOfCode.Day_15_Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ElvesMemoryGame
    {
        int[] _startingNumbers;

        /// <summary>
        /// Contains the memory of the game, but NOT the most recent number.
        /// </summary>
        Dictionary<int, int> _memory;

        public int lastNumber { get; private set; }
        public int turnIndex { get; private set; }

        public ElvesMemoryGame(int[] startingNumbers)
        {
            _startingNumbers = startingNumbers;
            _memory = new Dictionary<int, int>();
            turnIndex = 0;
            lastNumber = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count">The number of turns to play</param>
        public void PlayTurns(int count)
        {
            for (int i = 0; i < count; i++)
            {
                PlayTurn();
            }
        }

        public void PlayTurn()
        {
            turnIndex++;
            if (turnIndex <= _startingNumbers.Length)
            {
                PlayNumber(_startingNumbers[turnIndex - 1]);
            }
            else
            {
                if (_memory.ContainsKey(lastNumber)) 
                {
                    PlayNumber(turnIndex - 1 - _memory[lastNumber]);
                }
                else
                {
                    PlayNumber(0);
                }
            }
        }

        private void PlayNumber(int number)
        {
            if (turnIndex > 1)
            {
                _memory[lastNumber] = turnIndex - 1;
            }
            lastNumber = number;
        }
    }
}
