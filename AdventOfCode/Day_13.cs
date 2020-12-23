namespace AdventOfCode
{
    using AdventOfCode.Day_13_Core;
    using AoCHelper;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;

    public class Day_13 : BaseDay
    {
        int _arrivalTime;
        List<int> _busesID;
        List<int> _busesA;
        public Day_13()
        {
            ParseInput1();
        }

        public override string Solve_1()
        {
            (int, int) minimal = IndexOfMin(_busesID, x => x - (_arrivalTime % x)); // Works well but not the most efficient
            // Console.WriteLine(_arrivalTime.ToString());
            // Console.WriteLine(minimal.Item1.ToString());
            // Console.WriteLine(_busesID[minimal.Item2].ToString());
            return (minimal.Item1 * _busesID[minimal.Item2]).ToString();
        }

        public override string Solve_2()
        {
            long[] n = _busesID.ConvertAll(i => (long)i).ToArray<long>();
            long[] a = _busesA.ConvertAll(i => (long)i).ToArray<long>();
            return ChineseRemainderTheorem.Solve(n, a).ToString();
        }

        private void ParseInput1()
        {
            string[] input = File.ReadAllLines(InputFilePath);
            _arrivalTime = int.Parse(input[0]);
            _busesID = new List<int>();
            _busesA = new List<int>();
            string[] busesInput = input[1].Split(",");
            for (int i = 0; i < busesInput.Length; i++)
            {
                if (busesInput[i] != "x")
                {
                    int bus = (int.Parse(busesInput[i]));
                    _busesID.Add(bus);
                    _busesA.Add(bus - i);
                }
            }
        }

        public static (int, int) IndexOfMin<TSource>(List<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("self");
            }

            if (source.Count == 0)
            {
                throw new ArgumentException("List is empty.", "self");
            }

            int min = selector(source[0]);
            int minIndex = 0;

            //var caching
            int currentValue;
            for (int i = 1; i < source.Count; ++i)
            {
                currentValue = selector(source[i]);
                if (currentValue < min)
                {
                    min = currentValue;
                    minIndex = i;
                }
            }

            return (min, minIndex);
        }
    }
}
