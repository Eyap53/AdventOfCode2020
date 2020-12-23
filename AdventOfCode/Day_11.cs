namespace AdventOfCode
{
    using AdventOfCode.Day_11_Core;
    using AoCHelper;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public class Day_11 : BaseDay
    {
        private readonly int maxRoundCount = 1000;
        private static SeatingSystem _seatingSystem;

        public Day_11()
        {
        }

        public override string Solve_1()
        {
            ParseInput();
            int roundCount = 0;
            int lastChangesCount;
            do
            {
                roundCount++;

                lastChangesCount = _seatingSystem.ApplySeatingRound(3, false);
            } while (roundCount <= maxRoundCount && lastChangesCount != 0);
            return _seatingSystem.CountOccupiedSeats().ToString();
        }

        public override string Solve_2()
        {
            ParseInput();
            int roundCount = 0;
            int lastChangesCount;
            do
            {
                roundCount++;

                lastChangesCount = _seatingSystem.ApplySeatingRound(4, true);
            } while (roundCount <= maxRoundCount && lastChangesCount != 0);
            return _seatingSystem.CountOccupiedSeats().ToString();
        }

        private void ParseInput()
        {
            string[] input = File.ReadAllLines(InputFilePath);
            _seatingSystem = new SeatingSystem(input);
        }
    }
}
