namespace AdventOfCode
{
    using AoCHelper;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;

    public class Day_23 : BaseDay
    {
        private List<int> _cups;

        public override string Solve_1()
        {
            ParseInput();
            /// With previous algo
            // CrabShuffle(_cups, 100, 9);
            // return DisplayCups(_cups);

            /// With improved algo for part 2
            int[] cupsMap = new int[_cups.Count + 1];
            for (int i = 0; i < _cups.Count - 1; i++)
            {
                cupsMap[_cups[i]] = _cups[i + 1];
            }
            cupsMap[_cups[_cups.Count - 1]] = _cups[0];
            CrabShuffleImproved(cupsMap, _cups[0], 100, 9);
            return DisplayCupsMap(cupsMap, 1);
        }

        public override string Solve_2()
        {
            int totalCups = 1000000;
            ParseInput();
            int[] cupsMap = new int[totalCups + 1];
            for (int i = 0; i < _cups.Count - 1; i++)
            {
                cupsMap[_cups[i]] = _cups[i + 1];
            }
            cupsMap[_cups[_cups.Count - 1]] = _cups.Count + 1;
            for (int i = _cups.Count + 1; i < totalCups; i++)
            {
                cupsMap[i] = i + 1;
            }
            cupsMap[totalCups] = _cups[0];
            CrabShuffleImproved(cupsMap, _cups[0], 10000000, totalCups);
            return ((long)cupsMap[1] * cupsMap[cupsMap[1]]).ToString();
        }

        private void ParseInput()
        {
            string[] input = File.ReadAllLines(InputFilePath);
            _cups = new List<int>();
            foreach (char c in input[0])
            {
                _cups.Add(int.Parse(c.ToString()));
            }
        }

        /// <summary>
        /// Play a game of crab cups shuffle.
        /// </summary>
        /// <param name="cups"></param>
        /// <returns></returns>
        private void CrabShuffle(List<int> cups, int totalMoves, int maxCup)
        {
            int currentCupIndex = 0;
            int minCup = 1; // Already known. cups.Min();
            // int maxCup = cups.Max(); // already known.

            int movesCount = 0;

            //var caching
            List<int> pickedCups;
            int destinationCupValue;
            int currentCupValue = cups[currentCupIndex];
            int destinationIndex;
            do
            {
                // 1
                // Console.WriteLine("cups: " + String.Join(" ", cups));

                pickedCups = new List<int>();
                for (int i = 0; i < 3; i++)
                {
                    if (currentCupIndex + 1 < cups.Count)
                    {
                        pickedCups.Add(cups[currentCupIndex + 1]);
                        cups.RemoveAt(currentCupIndex + 1);
                    }
                    else
                    {
                        pickedCups.Add(cups[0]);
                        cups.RemoveAt(0);
                        currentCupIndex--;
                    }
                }
                // Console.WriteLine("cups: " + String.Join(" ", pickedCups));

                //2
                destinationCupValue = currentCupValue - 1;

                while (pickedCups.IndexOf(destinationCupValue) != -1)
                {
                    destinationCupValue--;
                    if (destinationCupValue < minCup)
                    {
                        destinationCupValue = maxCup;
                    }
                }
                destinationIndex = cups.IndexOf(destinationCupValue);

                // Console.WriteLine("DestinationCupValue: " + destinationCupValue);
                //3
                if (destinationIndex + 1 <= currentCupIndex)
                {
                    currentCupIndex += 3;
                }
                cups.InsertRange(destinationIndex + 1, pickedCups);
                //4
                currentCupIndex++;
                if (currentCupIndex >= cups.Count)
                {
                    currentCupIndex = 0;
                }
                currentCupValue = cups[currentCupIndex];

                movesCount++;
            } while (movesCount < totalMoves);
        }

        private void CrabShuffleImproved(int[] cupsMap, int firstCup, int totalMoves, int maxCup)
        {
            int minCup = 1; // Already known. cups.Min();
            // int maxCup = cups.Max(); // already known.

            int movesCount = 0;

            //var caching
            int destinationCupValue;
            int currentCupValue = firstCup;
            int firstPicked;
            int secondPicked;
            int thirdPicked;
            int next;
            do
            {
                // // 1
                // Console.WriteLine("cups: " + DisplayCupsMap(cupsMap, 1, currentCupValue));
                // Console.WriteLine("cups: " + cupsMap[currentCupValue] + " " + cupsMap[cupsMap[currentCupValue]] + " " + cupsMap[cupsMap[cupsMap[currentCupValue]]]);

                firstPicked = cupsMap[currentCupValue];
                secondPicked = cupsMap[firstPicked];
                thirdPicked = cupsMap[secondPicked];
                next = cupsMap[thirdPicked];
                //2
                destinationCupValue = currentCupValue;

                do
                {
                    destinationCupValue--;
                    if (destinationCupValue < minCup)
                    {
                        destinationCupValue = maxCup;
                    }
                } while (destinationCupValue == firstPicked || destinationCupValue == secondPicked || destinationCupValue == thirdPicked);
                // re-maping 
                cupsMap[currentCupValue] = next; // current to next
                cupsMap[thirdPicked] = cupsMap[destinationCupValue]; // third to after-dest
                cupsMap[destinationCupValue] = firstPicked; // Destination to first-picked

                // Console.WriteLine("DestinationCupValue: " + destinationCupValue);

                //4
                currentCupValue = next;

                movesCount++;
            } while (movesCount < totalMoves);
        }
        private string DisplayCups(List<int> cups)
        {
            int index1 = cups.IndexOf(1);
            string result = "";
            for (int i = index1 + 1; i < cups.Count; i++)
            {
                result += cups[i].ToString();
            }
            for (int i = 0; i < index1; i++)
            {
                result += cups[i].ToString();
            }
            return result;
        }

        private string DisplayCupsMap(int[] cupsMap, int start)
        {
            int prev = start;
            string result = "";
            while (cupsMap[prev] != start)
            {
                prev = cupsMap[prev];
                result += prev.ToString();
            }
            return result;
        }

        private string DisplayCupsMap(int[] cupsMap, int start, int highlighted)
        {
            int prev = start;
            string result = "";
            while (cupsMap[prev] != start)
            {
                prev = cupsMap[prev];
                result += prev == highlighted ? "(" + prev.ToString() + ")" : prev.ToString();
            }
            return result;
        }
    }
}
