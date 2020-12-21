namespace AdventOfCode
{
    using AdventOfCode.Day_20_Core;
    using AoCHelper;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public class Day_20 : BaseDay
    {
        private readonly List<Tile> _tiles;
        private readonly Image _image;
        private static List<char> _directionIds = new List<char>() { 'N', 'E', 'S', 'W' };

        public Day_20()
        {
            _tiles = new List<Tile>();
            ParseInput();
            _image = new Image(_tiles);
        }

        public override string Solve_1() => _image.MultiplyIdCorners().ToString();

        public override string Solve_2() => _image.DetermineWaterRoughness().ToString();

        private void ParseInput()
        {
            string[] input = File.ReadAllLines(InputFilePath);
            List<string> buffer = new List<string>();
            foreach (string line in input)
            {
                if (line == "")
                {
                    _tiles.Add(new Tile(buffer));
                    buffer = new List<string>();
                }
                else
                {
                    buffer.Add(line);
                }
            }
            if (buffer.Any())
            {
                _tiles.Add(new Tile(buffer));
                buffer = new List<string>();
            }
        }
    }
}
