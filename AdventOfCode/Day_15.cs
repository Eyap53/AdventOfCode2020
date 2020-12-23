namespace AdventOfCode
{
    using AdventOfCode.Day_15_Core;
    using AoCHelper;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Day_15 : BaseDay
    {
        string[] _input;
        int[] _startingNumbers;

        public Day_15()
        {
            ParseInput1();
        }

        public override string Solve_1()
        {
            ElvesMemoryGame memoryGame = new ElvesMemoryGame(_startingNumbers);
            memoryGame.PlayTurns(2020);
            return memoryGame.lastNumber.ToString();
        }

        public override string Solve_2()
        {
            ElvesMemoryGame memoryGame = new ElvesMemoryGame(_startingNumbers);
            memoryGame.PlayTurns(30000000);
            return memoryGame.lastNumber.ToString();
        }

        private void ParseInput1()
        {
            _input = File.ReadAllLines(InputFilePath);
            _startingNumbers = _input[0].Split(",").Select(n => Convert.ToInt32(n)).ToArray();
        }

    }
}
