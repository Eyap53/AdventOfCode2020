namespace AdventOfCode.Day_11_Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Seating system to solve day 11.
    /// A seat could use its own class, but not added for simplicity (over re-usability).
    /// </summary>
    public class SeatingSystem
    {
        private SeatStatus[,] _seats;

        public SeatingSystem(string[] input)
        {
            _seats = Parse(input);
        }

        public int ApplySeatingRound(int maxNeighborsTolerated, bool useSight)
        {
            SeatStatus[,] _nextIterationSeats = new SeatStatus[_seats.GetLength(0), _seats.GetLength(1)];
            int changesCount = 0;

            // var caching
            int neighborsCount;

            for (int i = 0; i < _nextIterationSeats.GetLength(0); i++)
            {
                for (int j = 0; j < _nextIterationSeats.GetLength(1); j++)
                {
                    if (_seats[i, j] == SeatStatus.Floor) // Prevent neighbors count. Could be removed for clarity (over micro perf.)
                    {
                        _nextIterationSeats[i, j] = SeatStatus.Floor;
                    }
                    else
                    {
                        neighborsCount = useSight ? CountOccupiedInSight(i, j) : CountNeighbors(i, j);
                        if (_seats[i, j] == SeatStatus.Empty && neighborsCount == 0)
                        {
                            _nextIterationSeats[i, j] = SeatStatus.Occupied;
                            changesCount++;
                        }
                        else if (_seats[i, j] == SeatStatus.Occupied && neighborsCount > maxNeighborsTolerated)
                        {
                            _nextIterationSeats[i, j] = SeatStatus.Empty;
                            changesCount++;
                        }
                        else
                        {
                            _nextIterationSeats[i, j] = _seats[i, j];
                        }
                    }
                }
            }
            _seats = _nextIterationSeats;
            return changesCount;
        }

        public int CountOccupiedSeats()
        {
            int occupiedSeatsCount = 0;
            foreach (SeatStatus seat in _seats)
            {
                if (seat == SeatStatus.Occupied)
                {
                    occupiedSeatsCount++;
                }
            }
            return occupiedSeatsCount;
        }

        #region Parsing methods

        public static SeatStatus[,] Parse(string[] input)
        {
            SeatStatus[,] seats = new SeatStatus[input.Length, input[0].Length];

            for (int i = 0; i < seats.GetLength(0); i++)
            {
                for (int j = 0; j < seats.GetLength(1); j++)
                {
                    seats[i, j] = ParseSeat(input[i][j]);
                }
            }
            return seats;
        }

        public override string ToString()
        {
            string output = "";

            for (int i = 0; i < _seats.GetLength(0); i++)
            {
                for (int j = 0; j < _seats.GetLength(0); j++)
                {
                    output += SeatToString(_seats[i, j]);
                }
                output += '\n';
            }
            return output;
        }

        public static SeatStatus ParseSeat(char seatString)
        {
            switch (seatString)
            {
                case '.':
                    return SeatStatus.Floor;
                case 'L':
                    return SeatStatus.Empty;
                case '#':
                    return SeatStatus.Occupied;
                default:
                    throw new ArgumentException();
            }
        }

        public static char SeatToString(SeatStatus seat)
        {
            switch (seat)
            {
                case SeatStatus.Floor:
                    return '.';
                case SeatStatus.Empty:
                    return 'L';
                case SeatStatus.Occupied:
                    return '#';
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        /// <summary>
        /// Count the number of adjacent occupied seats.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>The number of adjacent occupied seats.</returns>
        private int CountNeighbors(int i, int j)
        {
            int count = 0;
            for (int di = (i > 0 ? -1 : 0); di <= (i < _seats.GetLength(0) - 1 ? 1 : 0); di++)
            {
                for (int dj = (j > 0 ? -1 : 0); dj <= (j < _seats.GetLength(1) - 1 ? 1 : 0); dj++)
                {
                    if (di != 0 || dj != 0)
                    {
                        if (_seats[i + di, j + dj] == SeatStatus.Occupied)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Count the number of occupied seats in sight.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>The number of occupied seats in sight.</returns>
        private int CountOccupiedInSight(int i, int j)
        {
            int count = 0;
            List<(int, int)> directions = new List<(int, int)>(){
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1),
                (1, 1),
                (1, -1),
                (-1, 1),
                (-1, -1),
            };

            // var caching
            int increment;
            (int, int) directionIncremented;
            foreach ((int, int) direction in directions)
            {
                increment = 1;
                directionIncremented = direction;
                while (i + directionIncremented.Item1 >= 0 && i + directionIncremented.Item1 < _seats.GetLength(0) && j + directionIncremented.Item2 >= 0 && j + directionIncremented.Item2 < _seats.GetLength(1))
                {
                    if (_seats[i + directionIncremented.Item1, j + directionIncremented.Item2] != SeatStatus.Floor)
                    {
                        if (_seats[i + directionIncremented.Item1, j + directionIncremented.Item2] == SeatStatus.Occupied)
                        {
                            count++;
                        }
                        break;
                    }
                    // Increment
                    increment++;
                    directionIncremented = (direction.Item1 * increment, direction.Item2 * increment);
                }
            }
            return count;
        }
    }
}
