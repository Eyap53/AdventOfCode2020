namespace AdventOfCode
{
    using AoCHelper;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day_01 : BaseDay
    {
        string[] _input;

        public Day_01()
        {
            ParseInput();
        }

        public override string Solve_1() => throw new NotImplementedException();

        public override string Solve_2() => throw new NotImplementedException();

        private void ParseInput()
        {
            _input = File.ReadAllLines(InputFilePath);
            // TODO
        }

    }
}
